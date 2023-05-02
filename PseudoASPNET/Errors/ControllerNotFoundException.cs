
namespace PseudoASPNET.Errors
{
    public class ControllerNotFoundException : Exception
    {
        public ControllerNotFoundException()
            : base("There is not controller with this path")
        {
            
        }
    }
}
