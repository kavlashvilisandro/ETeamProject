using PseudoEntityFramework.Models;

namespace ETeamProjectPersistence.Entities
{
    public class Purchase : BaseEntity
    {
        public string ProductID { get; set; }
        public int Amount { get; set; }
        public decimal PriceOfUnit { get; set; }
        public decimal FullPrice { get; set; }
        public Purchase(string productID, int amount, decimal priceOfUnit) : base(Guid.NewGuid().ToString())
        {
            this.ProductID = productID;
            this.Amount = amount;
            this.PriceOfUnit = priceOfUnit;
            this.FullPrice = priceOfUnit * amount;
        }
        public Purchase() { }
    }
}
