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
        public void Intilize()
        {
            try
            {
                var HasProducts=_dbContext.Products.Any();
                var HasProductBrands=_dbContext.ProductBrands.Any();
                var HasProductTypes=_dbContext.ProductTypes.Any();
                if(HasProducts && HasProductBrands && HasProductTypes )
                {
                    return;
                }

               
                if (!HasProductBrands)
                {
                    SeedDataFromJson<ProductBrand,int>("brands.json", _dbContext.ProductBrands);
                }
                if (!HasProductTypes)
                {
                    SeedDataFromJson<ProductType, int>("types.json", _dbContext.ProductTypes);
                }
                _dbContext.SaveChanges();
                // 34an y3ml save lel brands wel types abl ma y3ml save lel products 34an fe 3ala2a benhom
                if (!HasProducts)
                {
                    SeedDataFromJson<Product, int>("products.json", _dbContext.Products);
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Data Seeding Failed : {ex}");
            }   
        }

        private void SeedDataFromJson<T,TKey>(string fileName , DbSet<T> dbset) where T:BaseEntity<TKey>
        {
            // FilePath
            //D:\Courses\Route Asp.net\videos\Web Api Asp.Net Core\ECommerce.Web\Ecommerce.Prisastance\Data\DataSeed\JsonFiles\
            var filePath = @"..\ECommerce.Web\Ecommerce.Prisastance\Data\DataSeed\JsonFiles"+fileName;

            if(!File.Exists(filePath)) throw new FileNotFoundException($"The File {fileName} Not Found in Path {filePath}");

            try
            {
                using var DataStream=File.OpenRead(filePath);
                var Data = JsonSerializer.Deserialize<List<T>>(DataStream);
                if (Data is not null)
                {
                    dbset.AddRange(Data);
                }   


            }
            catch (Exception ex)
            {

                Console.WriteLine($"Failed to Read Data From Json : {ex}");            }
        }
    }
}
