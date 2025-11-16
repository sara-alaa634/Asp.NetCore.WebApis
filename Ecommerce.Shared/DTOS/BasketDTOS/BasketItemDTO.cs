using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Shared.DTOS.BasketDTOS
{
    public record BasketItemDTO(
        int Id,
        string ProductName,
        string PictureUrl,
        [Range(1, double.MaxValue)]
        decimal Price,
        int Quantity
        );

}
