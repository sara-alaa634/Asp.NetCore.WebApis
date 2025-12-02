using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.BasketModule;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Services.Exceptions;
using Ecommerce.Shared.DTOS.BasketDTOS;
using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepo _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PaymentService(IBasketRepo basketRepo, IUnitOfWork unitOfWork , IMapper mapper, IConfiguration configuration)
        {
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            // 1. Configration Stripe
            // Download Package Stripe.net
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"]; // Secret Key

            // 2. Get Basket Id
           // BasketItemDTO=> BasketRepos => GetById
           var Basket=await _basketRepo.GetBasketAsync(BasketId);
            if(Basket is null)
            {
                throw new BasketNotFoundException(BasketId);
            }
            // 3. Amout
            // ProductRepo
            var Product =_unitOfWork.GetReposatory<Domain.Entities.Products.Product, int>();
            foreach(var item in Basket.Items)
            {
                var productEntity=await Product.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);   

            }
            // DeliveryMethod
            var DeliveryMethod=await _unitOfWork.GetReposatory<DeliveryMethod,int>().GetByIdAsync(Basket.DeliveryMethodId.Value);

            Basket.ShippingPrice=DeliveryMethod.Price;
            var BasketAmout=(long) (Basket.Items.Sum(item=>item.Quantity * item.Price) + DeliveryMethod.Price) * 100;

            // 4. Create PaymentIntent
            var PaymentService = new PaymentIntentService();
            // Create Intent
            if(Basket.PaymendIntentId is null)
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = BasketAmout,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var Paumentintent = await PaymentService.CreateAsync(options);
                Basket.PaymendIntentId=Paumentintent.Id;
                Basket.ClientSecret=Paumentintent.ClientSecret;
            }
            // Update Intent
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = BasketAmout,              
                };
                await PaymentService.UpdateAsync(Basket.PaymendIntentId, options);

            }

            // return => BasketDTO
            await _basketRepo.CreateOrUpdateBasketAsync(Basket);
            return _mapper.Map<CustomerBasket, BasketDTO>(Basket);
        }
    }
}
