using Ecommerce.Domain.Contracts;
using Ecommerce.Prisastance.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Web.Extensions
{
    public static class WebAppRegistration 
    {
        // this => as i told him go to webapplicaton class and add this method to it
        public static WebApplication MigrateDb(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();
            var dbContextService = Scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            // Check for pending migrations and apply them if any before seeding data to avoid conflicts
            if (dbContextService.Database.GetPendingMigrations().Any())
            {
                dbContextService.Database.Migrate();
            }
            return app;
        }

        public static WebApplication SeedDb(this WebApplication app)
        {
            using var Scope = app.Services.CreateScope();

            var DataIntilizerService = Scope.ServiceProvider.GetRequiredService<IDataIntilizer>();
            DataIntilizerService.Intilize();
            return app;
        }
    }



}
