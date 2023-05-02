using Microsoft.Extensions.DependencyInjection;
using PseudoASPNET.Errors;
using System.Reflection;

namespace PseudoASPNET
{
    public class AppBuilder
    {
        private MiddleWares middleware;
        public IServiceCollection services;
        private Type[] Controllers;
        private IServiceProvider provider;
        
        public AppBuilder(Assembly ControllersAssembly)
        {
            services = new ServiceCollection();
            middleware = new MiddleWares();
            Controllers = GetAllControllers(ControllersAssembly);
            for(int i = 0; i < Controllers.Length; i++)
            {
                services.AddTransient(Controllers[i]);
            }
        }


        public void HandleRequest(RequestContext context)
        {
            try
            {
                middleware.StartRequestPipeline(context);
                MapRequestToController(context);
            }catch(ControllerNotFoundException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is not command with this name");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (RequestProcessingHasStopped ex)
            {
                //Nothing happens
            }catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        private void MapRequestToController(RequestContext context)
        {
            for (int i = 0; i < Controllers.Length; i++)
            {
                MethodInfo? ActionMethod = Controllers[i].GetMethods()
                    .FirstOrDefault((MethodInfo m) =>
                    {
                        PathAttribute? attribute = m.GetCustomAttribute<PathAttribute>();
                        if(attribute != null && attribute.path.Equals(context.GetPath()))
                        {
                            return true;
                        }
                        return false;
                    });


                if (ActionMethod == null) throw new ControllerNotFoundException();
                Type[] argTypes = ActionMethod.GetParameters().Select(p => p.ParameterType).ToArray();
                object[] arguments = new object[context.GetArgsCount()];
                if(argTypes.Length != arguments.Length)
                {
                    throw new TargetParameterCountException();
                }
                for(int j = 0; j < arguments.Length; j++)
                {
                    arguments[j] = Convert.ChangeType(context.GetArg(j), argTypes[j]);
                }
                ActionMethod.Invoke(provider.GetService(Controllers[i]),arguments);
            }
        }

        public void AddMiddleware(RequestDelegate del)
        {
            middleware.AddMiddleware(del);
        }

        public RequestContext BuildRequestContext(string querry)
        {
            return new RequestContext(querry, provider);
        }

        private static Type[] GetAllControllers(Assembly assembly)
        {
            Type parentType = typeof(Controller);


            Type[] types = assembly.GetTypes();//Getting all class types

            //Getting all classes which are childs of Controller
            Type[] Entites = types.Where(t => parentType.IsAssignableFrom(t) && t != parentType).ToArray();
            return Entites;
        }

        public void UpdateServiceProvider()
        {
            this.provider = services.BuildServiceProvider();
        }

        
    }
}
