using AutoMapper;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.Products;
using Ecommerce.ServiceAbstraction;
using Ecommerce.Shared.CommanResult;
using Ecommerce.Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services
{
    public class OrderServcie : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepo _basketRepo;

        public OrderServcie(IMapper mapper , IUnitOfWork unitOfWork , IBasketRepo basketRepo)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email)
        {
            var orderAddress = _mapper.Map<OrederAddress>(orderDTO.Address);
            var basket = await _basketRepo.GetBasketAsync(orderDTO.BasketId);
            if (basket == null) return Error.NotFound("Basket not found!");
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.GetReposatory<Product, int>().GetByIdAsync(item.Id);
                if (product == null) return Error.NotFound($"Product with id {item.Id} not found!");

                orderItems.Add(CreateOrderItem(item, product));
            }

            var DeliveryMethod = await _unitOfWork.GetReposatory<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if (DeliveryMethod == null) return Error.NotFound($"Delivery Method with id {orderDTO.DeliveryMethodId} not found!");

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);
            var order = new Order()
            {
                UserEmail = Email,
                Address = orderAddress,
                DeliveryMethod = DeliveryMethod,
                Items = orderItems,
                SubTotal = subtotal
            };
            await _unitOfWork.GetReposatory<Order, Guid>().AddAsync(order);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0) return Error.Failure("Failed to create order!");
            return _mapper.Map<OrderToReturnDTO>(order);


        }

        private static OrderItem CreateOrderItem(Domain.Entities.BasketModule.BasketItem item, Product product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrderd()
                {
                    ProductID = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl
                },
                Price = product.Price,
                Quantity = item.Quantity

            };
        }
    }
}
