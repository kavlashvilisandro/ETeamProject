using ETeamProjectServices;
using Microsoft.Extensions.DependencyInjection;

namespace ETeamProjectApplication.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IProductService, ProductService>();
        }
    }
}
