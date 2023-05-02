namespace PseudoEntityFramework.Errors
{
    public class IDAlreadyExists : BaseException
    {
        public IDAlreadyExists() : base("Element with this ID already exists")
        {

        }
    }
}
