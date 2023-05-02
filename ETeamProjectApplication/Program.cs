using PseudoASPNET;
using System.Reflection;
using ETeamProjectApplication.Infrastructure.Extensions;
using ETeamProjectServices;

namespace ETeamProjectApplication;

//Request form: {path} {parameter 1} {parameter 2} {parameter 3} .. {parameter N}
public class Program
{
    public static AppBuilder Startup()
    {
    /*
     DataBasePath is path where you want to create database folder called ETeamProjectDB
     */
        string DataBasePath = Directory.GetCurrentDirectory() + '\\';



        //App Confiugration Starts here
        AppBuilder builder = new AppBuilder(Assembly.GetExecutingAssembly());


        //Adding Services
        builder.services.AddServices();//Adding custom services
        builder.services.AddContexts(DataBasePath);

        builder.UpdateServiceProvider();


        //Adding Middlewares
        

        return builder;
    }


    public static void Main()
    {
        //Set up
        AppBuilder builder = Startup();
        
        
        //Application
        string Command;
        RequestContext RequestContext;

        while (true)
        {
            try
            {
                Print("   o   ", ConsoleColor.Green);
                Command = Console.ReadLine();
                if (Command.Equals("exit")) return;
                RequestContext = builder.BuildRequestContext(Command);
                builder.HandleRequest(RequestContext);
            }
            catch(Exception ex)
            {
                
            }
        }

    }

    public static void PrintLine(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
    public static void Print(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ForegroundColor = ConsoleColor.Gray;
    }
}