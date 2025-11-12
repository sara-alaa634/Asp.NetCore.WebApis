using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Prisastance
{// helper class to evaluate specifications
    public class SpecificationEvaluator
    {
        // Method to create query

        public static IQueryable<TEntity> CreateQuery<TEntity,TKey>(IQueryable<TEntity> EntryPoint , ISpecification<TEntity, TKey> specification) where TEntity :BaseEntity<TKey>
        {
            //EntryPoint=_dbcontect.Products
            var Query = EntryPoint;

            if (specification is not null)
            {
                if (specification.IncludeExpression is not null && specification.IncludeExpression.Any())
                {
                    //// Inject for Include
                    //foreach (var includeExp in specification.IncludeExpression)
                    //{
                    //    Query = Query.Include(includeExp);
                    //    //_dbcontect.Products.include(......)

                    //}

                    // can use aggregate 
                    Query = specification.IncludeExpression.Aggregate(Query, (current, includeExp) => current.Include(includeExp));
                }

              
            }

            return Query;


        }
    }
}
