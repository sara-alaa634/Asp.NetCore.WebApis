using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Orders
{
    public class ProductItemOrderd
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = null!;
        public string PictureUrl { get; set; } = null!;

    }
}
