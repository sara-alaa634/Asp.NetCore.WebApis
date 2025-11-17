using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
        IGenaricReposatory<TEntity, TKey> GetReposatory<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

    }
}
