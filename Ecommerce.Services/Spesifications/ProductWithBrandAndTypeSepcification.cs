using Ecommerce.Domain.Entities.Products;
using Ecommerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Spesifications
{
    public class ProductWithBrandAndTypeSepcification:BaseSpecification<Product, int>
    {
        public ProductWithBrandAndTypeSepcification(ProductQueryParams queryParams):base(ProductSpecificationHelper.GetProductCriteria(queryParams))
        {
            AddInclude(P => P.ProductBrands);
            AddInclude(P => P.ProductTypes);

            switch (queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescinding(P => P.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescinding(P => P.Price);
                    break;
                default:
                    AddOrderBy(P => P.Id);
                    break;
            }


            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public ProductWithBrandAndTypeSepcification(int id):base(p => p.Id == id)
        {
            AddInclude(P => P.ProductBrands);
            AddInclude(P => P.ProductTypes);
        }
    }
}
