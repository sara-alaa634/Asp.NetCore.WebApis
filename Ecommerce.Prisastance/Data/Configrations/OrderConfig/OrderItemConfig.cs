using Ecommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Prisastance.Data.Configrations.OrderConfig
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(D => D.Price)
                .HasColumnType("decimal(18,2)");

            builder.OwnsOne(OI => OI.Product);


        }
    }
}
