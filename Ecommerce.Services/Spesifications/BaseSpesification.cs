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
        #region Pagination

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public bool IsPaginated { get; set; }

        protected void ApplyPagination(int pageSize, int PageIndex)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = pageSize * (PageIndex - 1);
        }
        #endregion


        #region Sorting

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TEntity, object>> OrderByDescinding { get; private set; }
       
        protected void AddOrderBy(Expression<Func<TEntity,object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;
        }

        protected void AddOrderByDescinding(Expression<Func<TEntity, object>> OrderByDescendingExpression)
        {
            OrderByDescinding = OrderByDescendingExpression;
        }
        #endregion

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
