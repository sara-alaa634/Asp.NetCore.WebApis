
using Ecommerce.Domain.Contracts;
using Ecommerce.Prisastance.Data.DataSeed;
using Ecommerce.Prisastance.Data.DbContexts;
using Ecommerce.Prisastance.Reposatories;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Services;
using Ecommerce.Services.MappingProfiles;
using ECommerce.Web.Extensions;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace ECommerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
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
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductsService>();

            builder.Services.AddAutoMapper(X => X.AddProfile<ProductProfile>());

            builder.Services.AddAutoMapper(X => X.AddProfile<BasketProfile>());
            builder.Services.AddSingleton<IConnectionMultiplexer>(O =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);

            });

            builder.Services.AddScoped<IBasketRepo, BasketReposatory>();
           
            builder.Services.AddScoped<IBasketService, BasketService>();

            var app = builder.Build();

            #region Data Seed

           await app.MigrateDbAsunc();  // check migration first then seed data 
           await app.SeedDbAsync();


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
