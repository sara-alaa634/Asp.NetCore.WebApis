using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Orders
{
    public enum OrderStatus
    {
        Peinding = 0,
        PaymentRecived = 1,
        PaymentFailed = 2,
    }
}
