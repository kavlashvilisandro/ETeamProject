using PseudoASPNET;
using ETeamProjectServices;

namespace ETeamProjectApplication.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;
        public ShopController(IProductService productService)
        {
            this._productService = productService;
        }


        [Path("save_product")]
        public void SaveProduct(string id, string name, decimal price)
        {
            if (price <= 0)
            {
                Program.PrintLine("Cannot be negative or equal to 0", ConsoleColor.Red);
                return;
            } 
            string ID = _productService.AddProduct(id, name, price);
            if(ID.Length != 0) Program.PrintLine($"Product added [{ID}]", ConsoleColor.Green);
        }

        //როცა მაღაზია ყიდულობს დისტრიბუციისგან პროდუქტს
        [Path("purchase_product")]
        public void PurchaseProduct(string id, int amount, decimal price)
        {
            if(amount <= 0 || price <= 0)
            {
                Program.PrintLine("amount and price should be positive numbers",ConsoleColor.Red);
                return;
            }
            string PurchaseID = _productService.Purchase(id, amount, price);
            if (PurchaseID.Length != 0)
            {
                Program.PrintLine($"Purchase occured! ID[{PurchaseID}]", ConsoleColor.Green);
            }
        }

        [Path("get_quantity_of_product")]
        public void GetQuantityProduct(string id)
        {
            int Amount = _productService.ProductCounter(id);
            if(Amount != -1) Program.PrintLine($"amount of [{id}] is: {Amount}", ConsoleColor.Green);
        }


        //მომხმარებელი უკვეთავს პროდუქტს
        [Path("order_product")]
        public void OrderProduct(string id, int amount)
        {
            if(amount <= 0)
            {
                Program.PrintLine("amount should be more than 0", ConsoleColor.Red);
                return;
            }
            string OrderID = _productService.OrderProduct(id, amount);
            if (OrderID.Length != 0) Program.PrintLine("Order added", ConsoleColor.Green);
        }


        [Path("get_average_price")]
        public void GetAvaragePrice(string productID)
        {
            decimal avaragePrice = _productService.AvaragePrice(productID);
            if (avaragePrice != -1) Program.PrintLine($"Avarage price is: {avaragePrice}", ConsoleColor.Green);
        }

        [Path("get_fewest_product")]
        public void GetFewestProduct()
        {
            string FewestProductName = _productService.GetFewestProduct();
            if (FewestProductName.Length != 0) Program.PrintLine(FewestProductName, ConsoleColor.Green);
        }

        [Path("get_product_profit")]
        public void GetProductProfit(string productID)
        {
            decimal Profit = _productService.CalculateProfit(productID);
            if (Profit != -1) Program.PrintLine($"profit: {Profit}", ConsoleColor.Green);
        }

        [Path("get_most_popular_product")]
        public void GetMostPopular()
        {
            string MostPopularProduct = _productService.GetMostPopular();
            if(MostPopularProduct.Length != 0) Program.PrintLine($"Most popular product: {MostPopularProduct}", ConsoleColor.Green);
        }

        [Path("get_orders_report")]
        public void Report()
        {
            _productService.GenerateReport();
        }
    
    
        
    
    }
}
