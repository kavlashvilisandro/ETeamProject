namespace PseudoASPNET.Errors
{
    internal class RequestProcessingHasStopped : Exception
    {
        public RequestProcessingHasStopped() 
            : base("Request processing has stopped during RequestPipeLine")
        {

        }
    }
}
