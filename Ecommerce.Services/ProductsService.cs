using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Products;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Services.Exceptions;
using Ecommerce.Services.Spesifications;
using Ecommerce.Shared;
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

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Spec=new ProductWithBrandAndTypeSepcification(queryParams);

            var Products= await _unitOfWork.GetReposatory<Product,int>().GetAllAsync(Spec);

            var DataToReturn= _mapper.Map<IEnumerable<ProductDTO>>(Products);
            // count of page size
            var CountOfReturendData = DataToReturn.Count();
            var CountSpec=new ProductCountSpesification(queryParams);
            var CountOfProducts = await _unitOfWork.GetReposatory<Product, int>().CountAsync(CountSpec);
            return new PaginatedResult<ProductDTO>(queryParams.PageIndex,CountOfReturendData, CountOfProducts, DataToReturn);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types=  await _unitOfWork.GetReposatory<ProductType,int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(Types);
        }

        public async Task<ProductDTO?> GetProductByIdAsync(int id)
        {
            var Spec=new ProductWithBrandAndTypeSepcification(id);
            var Product=  await _unitOfWork.GetReposatory<Product,int>().GetByIdAsync(Spec);
            if (Product is null)
                throw new ProductNotFoundException(id);
            return _mapper.Map<ProductDTO>(Product);
        }
    }
}
