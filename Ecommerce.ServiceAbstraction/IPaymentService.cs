using Ecommerce.Shared.DTOS.BasketDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.ServiceAbstraction
{
    public interface IPaymentService
    {
        // Service [BasketID] => BasketDTO
        Task<BasketDTO> CreateOrUpdatePaymentIntentAsync(string BasketId);
    }
}
