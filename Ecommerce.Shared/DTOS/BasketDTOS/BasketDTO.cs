using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOS.BasketDTOS
{
    public record BasketDTO(string Id,
        ICollection<BasketItemDTO> Items
        );

}
