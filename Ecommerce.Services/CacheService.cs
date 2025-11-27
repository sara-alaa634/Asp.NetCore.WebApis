using Ecommerce.Domain.Contracts;
using Ecommerce.ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheReposatory _cacheReposatory;

        public CacheService(ICacheReposatory cacheReposatory)
        {
            _cacheReposatory = cacheReposatory;
        }
        public Task<string?> GetAsync(string cacheKey)
        {
            return _cacheReposatory.GetAsync(cacheKey);
        }

        public async Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive)
        {
            var Value = JsonSerializer.Serialize(cacheValue);

            await _cacheReposatory.SetAsync(cacheKey, Value, timeToLive);
        }
    }
}
