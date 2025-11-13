using Ecommerce.Domain.Contracts;
using Ecommerce.Prisastance.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Web.Extensions
{
    public static class WebAppRegistration 
    {
        // this => as i told him go to webapplicaton class and add this method to it
        public static  async Task<WebApplication> MigrateDbAsunc(this WebApplication app)
        {
            await using var Scope = app.Services.CreateAsyncScope();
            var dbContextService = Scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            // Check for pending migrations and apply them if any before seeding data to avoid conflicts
           var PendingMigrations= await dbContextService.Database.GetPendingMigrationsAsync();
            if(PendingMigrations.Any())
            {
                await dbContextService.Database.MigrateAsync();
            }
            return app;
        }

        public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
           await using var Scope = app.Services.CreateAsyncScope();

            var DataIntilizerService = Scope.ServiceProvider.GetRequiredService<IDataIntilizer>();
            await DataIntilizerService.IntilizeAsync();
            return app;
        }
    }



}
