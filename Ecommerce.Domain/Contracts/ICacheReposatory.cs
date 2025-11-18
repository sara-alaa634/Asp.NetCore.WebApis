using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface ICacheReposatory
    {
        Task<string?> GetAsync(string CacheKey);
        Task SetAsync(string CacheKey, string CacheValue, TimeSpan TimeToLive);


    }
}
