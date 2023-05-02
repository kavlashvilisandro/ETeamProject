using ETeamProjectPersistence.Entities;
using PseudoEntityFramework.Models;

namespace ETeamProjectPersistence.Entities
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public Product(string ID, string Name, decimal Price) : base(ID)
        {
            this.ProductName = Name;
            this.ProductPrice = Price;
        }
        public Product() { }
    }
}
