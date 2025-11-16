using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.BasketModule;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.DTOS.BasketDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class BasketService : IBasketService
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepo _basketRepo;

        public BasketService(IBasketRepo basketRepo, IMapper mapper)
        {
            _mapper = mapper;
            _basketRepo= basketRepo;

            //Register BasketRepo in contianer (program)
        }
        public async Task<BasketDTO> CreateOrUpdateBasketAsync(BasketDTO basket)
        {
            var CustoemrBasekt = _mapper.Map<CustomerBasket>(basket);

            var CreatedOrUpdatedBasket = await _basketRepo.CreateOrUpdateBasketAsync(CustoemrBasekt);
            return _mapper.Map<BasketDTO>(CreatedOrUpdatedBasket);
        }

        public Task<bool> DeleteBasketAsync(string basketId)
        {
            return _basketRepo.DeleteBasketAsync(basketId);
        }

        public async Task<BasketDTO> GetBasketAsync(string basketId)
        {
            var basket =await  _basketRepo.GetBasketAsync(basketId);
            return _mapper.Map<BasketDTO>(basket);
        }
    }
}
