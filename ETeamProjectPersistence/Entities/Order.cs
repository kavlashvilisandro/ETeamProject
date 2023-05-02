
using PseudoEntityFramework.Models;

namespace ETeamProjectPersistence.Entities
{
    public class Order : BaseEntity
    {
        public string ProductID { get; set; }
        public int Amount { get; set; }
        public decimal PriceOfOneUnit { get; set; }
        public decimal FullPrice { get; set; }
        public Order(string productID, int amount, decimal priceOfOneUnit) : base(Guid.NewGuid().ToString())
        {
            this.ProductID = productID;
            this.Amount = amount;
            this.PriceOfOneUnit = priceOfOneUnit;
            this.FullPrice = priceOfOneUnit * amount;
        }
        public Order() { }
    }
}
