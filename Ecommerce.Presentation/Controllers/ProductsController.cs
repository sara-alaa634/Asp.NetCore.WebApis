using Ecommerce.Presentation.Attributes;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared;
using Ecommerce.Shared.DTOS.ProductDTOS;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #region Get All Products
        [HttpGet]
        [RadisCache]
        // BaseUrl/api/Products/brandId=1&typeId=2
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            // ? make the ids can come and can not come
            var Products = await _productService.GetAllProductsAsync(queryParams);
            return Ok(Products);
        }
        #endregion

        #region Get Product By Id
        [HttpGet("{id}")]
        //BaseUrl/api/Products/1
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var Product = await _productService.GetProductByIdAsync(id);
            return Ok(Product);
        }
        #endregion

        #region Get All Brands

        [HttpGet("brands")]
        // BaseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var Brands =await _productService.GetAllBrandsAsync();
            return Ok(Brands);
        }
        #endregion

        #region Get All Types

        [HttpGet("types")]
        // BaseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var Types = await _productService.GetAllTypesAsync();
            return Ok(Types);
        }

        #endregion
    }
}
