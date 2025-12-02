using Ecommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Spesifications
{
    public class OrderWithPaymentIntentSpesification:BaseSpecification<Order, Guid>
    {
        public OrderWithPaymentIntentSpesification(string IntentID):base(O=> O.PaymentIntentId == IntentID)
        {
            
        }
    }
}
