
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.IdentityModule;
using Ecommerce.Prisastance.Data.DataSeed;
using Ecommerce.Prisastance.Data.DbContexts;
using Ecommerce.Prisastance.IdentityData.DbContexts;
using Ecommerce.Prisastance.Reposatories;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Services;
using Ecommerce.Services.MappingProfiles;
using ECommerce.Web.CustomMiddlewares;
using ECommerce.Web.Extensions;
using ECommerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

            builder.Services.AddKeyedScoped<IDataIntilizer, DataIntilizer>("Default");
            builder.Services.AddKeyedScoped<IDataIntilizer, IdenttiyDataIntailizer>("Identity");


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductsService>();

            builder.Services.AddAutoMapper(X => X.AddProfile<ProductProfile>());

            builder.Services.AddAutoMapper(X => X.AddProfile<BasketProfile>());

            builder.Services.AddAutoMapper(X => X.AddProfile<OrderProfile>());
            builder.Services.AddSingleton<IConnectionMultiplexer>(O =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);

            });

            builder.Services.AddScoped<IBasketRepo, BasketReposatory>();
           
            builder.Services.AddScoped<IBasketService, BasketService>();

            builder.Services.AddScoped<ICacheReposatory, CacheReposatory>();

            builder.Services.AddScoped<ICacheService, CacheService>();
            builder.Services.AddScoped<IOrderService, OrderServcie>();


            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CreateApiValidationResponse;
              
            });

            // Dependency Ijection
            builder.Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

                //Add-Migration "IdentityTableCreate" -OutputDir "Identity/Migrations" -Context "StoreIdentityDbContext"
            });

            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<StoreIdentityDbContext>();


            //new way to add identity core this equal the above but this faster
        builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();


            builder.Services.AddScoped<IAuthService, AuthenticationService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  // Auth
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // unAuth
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters() { 
                     ValidateIssuer=true,
                     ValidateAudience=true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]!))

                };

            });


            var app = builder.Build();

            #region Data Seed

           await app.MigrateDbAsunc();  // check migration first then seed data 
            await app.MigrateIdentityDbAsunc();
            await app.SeedDbAsync();
            await app.SeedIdentityDbAsync();


            #endregion

             

            // Configure the HTTP request pipeline.


            //Exceptions Here
            app.UseMiddleware<ExceptionHandlerMiddleware>();




            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
