using Ecommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Spesifications
{
    public class OrderSpesifications :BaseSpecification<Order , Guid>
    {
        public OrderSpesifications(string Email):base(o=> o.UserEmail==Email)
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
            AddOrderByDescinding(o => o.OrderDate);

        }

        public OrderSpesifications(Guid id, string Email):base(O=> O.Id==id && (string.IsNullOrEmpty(Email) ||O.UserEmail.ToLower() == Email.ToLower()))
        {
            AddInclude(o => o.DeliveryMethod);
            AddInclude(o => o.Items);
        }

    }
}
