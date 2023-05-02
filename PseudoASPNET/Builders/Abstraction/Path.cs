namespace PseudoASPNET
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PathAttribute : Attribute
    {
        public string path { get; set; }
        public PathAttribute(string path)
        {
            this.path = path;
        }
    }
}
