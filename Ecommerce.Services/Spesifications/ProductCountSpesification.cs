using Ecommerce.Domain.Entities.Products;
using Ecommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Spesifications
{
    public class ProductCountSpesification : BaseSpecification<Product,int>
    {
        public ProductCountSpesification(ProductQueryParams queryParams):base(P => (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId.Value) && (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId.Value) && (string.IsNullOrEmpty(queryParams.Search) || P.Name.ToLower().Contains(queryParams.Search.ToLower())))
        {
            
        }
    }
}
