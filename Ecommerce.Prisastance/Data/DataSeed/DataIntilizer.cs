using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Entities.Products;
using Ecommerce.Prisastance.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace Ecommerce.Prisastance.Data.DataSeed
{
    public class DataIntilizer : IDataIntilizer
    {
        private readonly StoreDbContext _dbContext;

        public DataIntilizer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task IntilizeAsync()
        {
            try
            {
                var HasProducts=await _dbContext.Products.AnyAsync();
                var HasProductBrands=await _dbContext.ProductBrands.AnyAsync();
                var HasProductTypes=await _dbContext.ProductTypes.AnyAsync();

                if(HasProducts && HasProductBrands && HasProductTypes )
                {
                    return;
                }
               
                if (!HasProductBrands)
                {
                    await SeedDataFromJsonAsync<ProductBrand,int>("brands.json", _dbContext.ProductBrands);
                }
                if (!HasProductTypes)
                {
                    await SeedDataFromJsonAsync<ProductType, int>("types.json", _dbContext.ProductTypes);
                }
                _dbContext.SaveChanges();
                // 34an y3ml save lel brands wel types abl ma y3ml save lel products 34an fe 3ala2a benhom
                if (!HasProducts)
                {
                    await SeedDataFromJsonAsync<Product, int>("products.json", _dbContext.Products);
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Data Seeding Failed : {ex}");
            }   
        }

        private async Task SeedDataFromJsonAsync<T,TKey>(string fileName , DbSet<T> dbset) where T:BaseEntity<TKey>
        {
            // FilePath
            //D:\Courses\Route Asp.net\videos\Web Api Asp.Net Core\ECommerce.Web\Ecommerce.Prisastance\Data\DataSeed\JsonFiles\
            var filePath = @"..\Ecommerce.Prisastance\Data\DataSeed\JsonFiles\"+fileName;
            if (!File.Exists(filePath)) throw new FileNotFoundException($"The File {fileName} Not Found in Path {filePath}");

            try
            {
                using var DataStream=File.OpenRead(filePath);
                var Data = JsonSerializer.Deserialize<List<T>>(DataStream);
                if (Data is not null)
                {
                   await dbset.AddRangeAsync(Data);
                }   


            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed to Read Data From Json : {ex}");            }
        }
    }
}
