using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.BasketModule;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Prisastance.Reposatories
{
    public class BasketReposatory : IBasketRepo
    {
        private readonly IDatabase _database;

        //connect form radius => IConnectionMultiplexer
        public BasketReposatory(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
            //Register for radis serice in contianer (program)
        }
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);  // Json
            var IsCreatedOrUdated = await _database.StringSetAsync(basket.Id, JsonBasket, (timeToLive == default)? TimeSpan.FromDays(7):timeToLive);

            if (IsCreatedOrUdated)
            {
                var Basket = await _database.StringGetAsync(basket.Id);
                return JsonSerializer.Deserialize<CustomerBasket>(Basket!);
            }
            else
            {
                return null;
            }


        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var Basket= await _database.StringGetAsync(basketId);
            if (Basket.IsNullOrEmpty)
            {
                return null;
            } else
            {
                return JsonSerializer.Deserialize<CustomerBasket>(Basket!);
            }
        }
    }
}
