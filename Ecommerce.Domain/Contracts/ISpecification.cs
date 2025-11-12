using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface ISpecification<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        //include
        public ICollection<Expression<Func<TEntity,object>>> IncludeExpression { get; }

        // Where
        public Expression<Func<TEntity,bool>> Criteria { get; }


    }
}
