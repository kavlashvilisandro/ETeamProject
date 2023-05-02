using Microsoft.Extensions.DependencyInjection;
using PseudoASPNET.Errors;


namespace PseudoASPNET
{
    public class RequestContext
    {
        //first element in args is path(Command), other elements are arguments
        private string[] args;

        public IServiceProvider serviceProvider { get; private set; }


        internal RequestContext(string request, IServiceProvider provider)
        {
            this.serviceProvider = provider;
            if(request == null || request.Length == 0)
            {
                throw new RequestIsEmptyException();
            }
            args = request.Split(' ');
        }

        public string GetPath()
        {
            return args[0];
        }

        public string GetArg(int index)
        {
            return args[index + 1];
        }

        public int GetArgsCount()
        {
            return args.Length - 1;
        }
    }
}
