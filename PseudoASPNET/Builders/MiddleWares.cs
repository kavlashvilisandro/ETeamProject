using PseudoASPNET.Errors;

namespace PseudoASPNET
{


    /*
     if RequestDelegate returns true, that means that next delegate will be called
    but if RequestDelegate returns false, that means that next delegate will not be
    called and RequestPipeline will break;
    */
    public delegate bool RequestDelegate(RequestContext context);
    internal class MiddleWares
    {
        private List<RequestDelegate> requestDelegates;

        public MiddleWares()
        {
            requestDelegates = new List<RequestDelegate>();
        }

        public void AddMiddleware(RequestDelegate newMiddleware)
        {
            requestDelegates.Add(newMiddleware);
        }

        public void StartRequestPipeline(RequestContext context)
        {
            for(int i = 0; i < requestDelegates.Count; i++)
            {
                if (!requestDelegates[i].Invoke(context))
                {
                    throw new RequestProcessingHasStopped();
                }
            }
        }
    }
}
