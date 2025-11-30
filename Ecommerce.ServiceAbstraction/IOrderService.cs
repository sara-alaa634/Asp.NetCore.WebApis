using Ecommerce.Shared.CommanResult;
using Ecommerce.Shared.DTOS.OrderDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction
{
    public interface IOrderService
    {
        //Create Order
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email);

        Task<Result<IEnumerable<OrderToReturnDTO>>> GetAllOrdersAsync(string Email);

        Task<Result<IEnumerable<DeliveryMethodDTO>>> GetDeliveryMethods();

        Task<Result<OrderToReturnDTO>> GetOrderByIdAsync(Guid orderId, string Email);

    }
}
