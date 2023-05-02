
namespace PseudoASPNET.Errors
{
    internal class RequestIsEmptyException : Exception
    {
        public RequestIsEmptyException() : base("Request is empty")
        { }
    }
}
