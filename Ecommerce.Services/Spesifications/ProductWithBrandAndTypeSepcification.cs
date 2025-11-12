using Ecommerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Spesifications
{
    public class ProductWithBrandAndTypeSepcification:BaseSpecification<Product, int>
    {
        public ProductWithBrandAndTypeSepcification(int? brandId, int? typeId):base(P=> (!brandId.HasValue || P.BrandId == brandId.Value) && (!typeId.HasValue || P.TypeId == typeId.Value) )
        {
            AddInclude(P => P.ProductBrands);
            AddInclude(P => P.ProductTypes);
        }

        public ProductWithBrandAndTypeSepcification(int id):base(p => p.Id == id)
        {
            AddInclude(P => P.ProductBrands);
            AddInclude(P => P.ProductTypes);
        }
    }
}
