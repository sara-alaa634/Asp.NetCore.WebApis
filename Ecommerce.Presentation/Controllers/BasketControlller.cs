using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.DTOS.BasketDTOS;
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
    public class BasketControlller:ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketControlller(IBasketService basketService)
        {
            _basketService = basketService;
        }

        #region Get Basket By Id
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket([FromQuery] string basketId)
        {
            var basket = await _basketService.GetBasketAsync(basketId);
            return Ok(basket);
        }

        #endregion

        #region Create or Update Basket

        [HttpPost]
        //Post : BaseURl/api/Basket
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket([FromBody] BasketDTO basket)
        {
            var createdOrUpdatedBasket = await _basketService.CreateOrUpdateBasketAsync(basket);
            return Ok(createdOrUpdatedBasket);
        }

        #endregion

        #region Delete Basket

        [HttpDelete("{id}")]
        // DELETE : BaseURL/api/Basket/{id}
        public async Task<ActionResult<bool>> DeleteBasket( string basketId)
        {
            var isDeleted = await _basketService.DeleteBasketAsync(basketId);
            return Ok(isDeleted);
        }
        #endregion
    }
}
