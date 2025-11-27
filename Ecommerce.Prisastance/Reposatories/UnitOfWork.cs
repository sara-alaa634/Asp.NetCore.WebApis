using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Ecommerce.Prisastance.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Prisastance.Reposatories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly Dictionary<Type, object> _reposatories = [];


        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IGenaricReposatory<TEntity, TKey> GetReposatory<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var EntityType = typeof(TEntity);
            if (_reposatories.TryGetValue(EntityType,out var reposatory))
            {
                return (IGenaricReposatory<TEntity, TKey>) reposatory;
            }

            var NewReposatory = new GenaricReposatory<TEntity, TKey>(_dbContext);
            _reposatories.Add(EntityType, NewReposatory);
            return NewReposatory;
        }

        public async Task<int> SaveChangesAsync() => await _dbContext.SaveChangesAsync();


    }
}
