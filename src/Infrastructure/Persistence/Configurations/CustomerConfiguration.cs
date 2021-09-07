using FusionIT.TimeFusion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Infrastructure.Persistence.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired();

            builder.HasOne(c => c.Referrer);

            builder.HasOne(c => c.Currency);

            builder.HasMany(c => c.Projects).WithOne().HasForeignKey(p => p.ClientId);
        }
    }
}