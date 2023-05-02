
using ETeamProjectPersistence.Entities;

namespace ETeamProjectServices
{
    public interface IProductService
    {
        public string AddProduct(string productID, string productName, decimal productPrice);

        public string Purchase(string productID, int amount, decimal unitPrice);

        public string OrderProduct(string productID, int amount);

        public int ProductCounter(string productID);

        public decimal AvaragePrice(string productID);
        public string GetFewestProduct();

        public decimal CalculateProfit(string productID);

        public void GenerateReport();
        public string GetMostPopular();
    }
}
