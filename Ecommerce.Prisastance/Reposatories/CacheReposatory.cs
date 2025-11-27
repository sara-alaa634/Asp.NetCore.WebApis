using Ecommerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Prisastance.Reposatories
{
    public class CacheReposatory : ICacheReposatory
    {
        private readonly IDatabase _database;

        // Connection To Radis
        public CacheReposatory(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        public async Task<string?> GetAsync(string CacheKey)
        {
           var CahceValue =await  _database.StringGetAsync(CacheKey);
           if(CahceValue.IsNullOrEmpty) return null;
           return CahceValue.ToString();
        }

        public async Task SetAsync(string CacheKey, string CacheValue, TimeSpan TimeToLive)
        {
            await _database.StringSetAsync(CacheKey, CacheValue, TimeToLive);
        }
    }
}
