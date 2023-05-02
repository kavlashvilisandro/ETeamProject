using ETeamProjectPersistence.Contexts;
using ETeamProjectPersistence.Entities;
using Microsoft.Extensions.DependencyInjection;
using PseudoEntityFramework.Errors;

namespace ETeamProjectServices
{
    public class ProductService : IProductService
    {
        private ETeamProjectDbContext _context;
        public ProductService(ETeamProjectDbContext context)
        {
            this._context = context;
        }
        public string AddProduct(string productID, string productName, decimal productPrice)
        {
            Product product = new Product(productID, productName, productPrice);
            try
            {
                _context.AddItem(product);
                return product.ID;
            }
            catch(IDAlreadyExists ex)
            {
                _context.Update(productID, (Product p) =>
                {
                    p.ProductPrice = product.ProductPrice;
                    p.ProductName = product.ProductName;
                });
                return product.ID;
            }
            catch(BaseException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
                return "";
            }
        }

        //Returns operations status. if it is false, that means that purchase did't occured,
        //otherwise returns true
        public string Purchase(string productID, int amount, decimal unitPrice)
        {
            try
            {
                Purchase purchase = new Purchase(productID, amount, unitPrice);
                if (_context.Exists<Product>(productID))
                {
                    _context.AddItem(purchase);
                    return purchase.ID;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is not product with this id");
                Console.ForegroundColor = ConsoleColor.Gray;
                return "";

            }
            catch (BaseException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Gray;
                return "";
            }
        }

        public int ProductCounter(string productID)
        {
            if (!_context.Exists<Product>(productID))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is not product with this id");
                Console.ForegroundColor = ConsoleColor.Gray;
                return -1;
            }
            int productPurchasesAmount = _context.GetTable<Purchase>()
                .Where(p => p.ProductID.Equals(productID)).Sum(p => p.Amount);

            int orderedAmount = _context.GetTable<Order>()
                .Where(o => o.ProductID.Equals(productID)).Sum(o => o.Amount);
            return productPurchasesAmount - orderedAmount;

        }

        public string OrderProduct(string productID, int amount)
        {
            int productAmount = ProductCounter(productID);
            if (productAmount == -1) return "";
            if(productAmount < amount)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is not enough products");
                Console.ForegroundColor = ConsoleColor.Gray;
                return "";
            }
            Product product = _context.Get<Product>(prod => prod.ID.Equals(productID));
            Order order = new Order(productID, amount, product.ProductPrice);
            _context.AddItem<Order>(order);
            return order.ID;
        }

        public decimal AvaragePrice(string productID)
        {
            if (!_context.Exists<Product>(productID))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There is not product with this id");
                Console.ForegroundColor = ConsoleColor.Gray;
                return -1;
            }
            List<Purchase> purchases = _context.GetTable<Purchase>()
                .Where(p => p.ProductID.Equals(productID)).ToList();
            if(purchases.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are not purchases on this product");
                Console.ForegroundColor = ConsoleColor.Gray;
                return -1;
            }
            return purchases.Sum(p => p.FullPrice) / purchases.Sum(p => p.Amount);
        }


        public string GetFewestProduct()
        {
            List<Product> products = _context.GetTable<Product>();
            if(products.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are not products");
                Console.ForegroundColor = ConsoleColor.Gray;
                return "";
            }
            List<int> ProductAmounts = products.Select(p => ProductCounter(p.ID)).ToList();
            int SmallestIndex = ProductAmounts.IndexOf(ProductAmounts.Min());
            return products[SmallestIndex].ProductName;
        }

        public decimal CalculateProfit(string productID)
        {
            decimal avaragePurchasePrice = AvaragePrice(productID);
            if(avaragePurchasePrice == -1)
            {
                return -1;
            }
            List<Order> orders = _context.GetTable<Order>()
                .Where(o => o.ProductID.Equals(productID)).ToList();
            if(orders.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are not orders on this product");
                Console.ForegroundColor = ConsoleColor.Gray;
                return -1;
            }
            int orderAmount = orders.Sum(o => o.Amount);
            decimal orderAvarage = orders.Sum(o => o.FullPrice)/orderAmount;
            return (orderAvarage - avaragePurchasePrice) * orderAmount;
        }

        public string GetMostPopular()
        {
            List<Product> products = _context.GetTable<Product>();
            if (products.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are not products");
                Console.ForegroundColor = ConsoleColor.Gray;
                return "";
            }
            List<int> ProductAmounts = products.Select(p => ProductCounter(p.ID)).ToList();
            int BiggestIndex = ProductAmounts.IndexOf(ProductAmounts.Max());
            return products[BiggestIndex].ProductName;
        }
        public void GenerateReport()
        {
            List<Order> Orders = _context.GetTable<Order>();
            if(Orders.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("There are not Orders");
                Console.ForegroundColor = ConsoleColor.Gray;
                return;
            }
            string res = "";
            foreach(Order order in Orders)
            {
                res += $"[Product id: {order.ProductID}] - [Amount: {order.Amount}] - " +
                    $"[Price of per unit: {order.PriceOfOneUnit}] - [Full cost: {order.FullPrice}] - " +
                    $"[Product name: {_context.Get<Product>(p => p.ID.Equals(order.ProductID)).ProductName}]" +
                    $" - [COGS: {AvaragePrice(order.ProductID) * order.Amount}]\n";
            }
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine(res);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    
    
        
    }

    public static class ServiceExtensiosnForContext
    {
        public static void AddContexts(this IServiceCollection services, string DataBasePath)
        {
            services.AddSingleton<ETeamProjectDbContext>((x) =>
            {
                return new ETeamProjectDbContext(DataBasePath, "ETeamProjectDB");
            });
        }

    }


}
