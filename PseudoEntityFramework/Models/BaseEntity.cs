using PseudoEntityFramework.Errors;

namespace PseudoEntityFramework.Models
{
    public abstract class BaseEntity
    {
        public string ID { get; set; }
        public BaseEntity(string ID)
        {
            if (ID == null || ID == "") throw new IDNullException();
            this.ID = ID;
        }
        public BaseEntity() { }
    }
}
