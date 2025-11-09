using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Products;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.DTOS.ProductDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class ProductsService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var Brands=await _unitOfWork.GetReposatory<ProductBrand,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDTO>>(Brands);

            // do not forget to create profile for mapping

        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
           var Products= await _unitOfWork.GetReposatory<Product,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(Products);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types=  await _unitOfWork.GetReposatory<ProductType,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(Types);
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var Product=  await _unitOfWork.GetReposatory<Product,int>().GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(Product);
        }
    }
}
