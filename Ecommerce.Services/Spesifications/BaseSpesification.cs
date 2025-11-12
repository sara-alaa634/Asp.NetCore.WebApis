using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Spesifications
{
    public class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        #region Where

        public Expression<Func<TEntity, bool>> Criteria { get; }

        public BaseSpecification(Expression<Func<TEntity, bool>> CriteriaExp)
        {
            Criteria = CriteriaExp;
        }


        #endregion

        #region Includes

        //Method to add includes to property
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];

        protected void AddInclude(Expression<Func<TEntity, object>> includeExp)
        {
            IncludeExpression.Add(includeExp);
        }
        #endregion


    }
}
