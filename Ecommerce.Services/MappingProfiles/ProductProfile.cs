using AutoMapper;
using Ecommerce.Domain.Entities.Products;
using Ecommerce.Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductBrand, BrandDTO>();
            CreateMap<ProductType, TypeDTO>();
            CreateMap<Product, ProductDTO>().ForMember(desc => desc.ProductType, opt => opt.MapFrom(src => src.ProductTypes.Name)).ForMember(desc => desc.ProductBrand, opt => opt.MapFrom(src => src.ProductBrands.Name));
        }
    }
}
