
using Ecommerce.Domain.Contracts;
using Ecommerce.Prisastance.Data.DataSeed;
using Ecommerce.Prisastance.Data.DbContexts;
using ECommerce.Web.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(
                options =>
                {
                    //Connection String
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });

            builder.Services.AddScoped<IDataIntilizer, DataIntilizer>();

            var app = builder.Build();

            #region Data Seed

            app.MigrateDb();
            app.SeedDb();


            #endregion

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
