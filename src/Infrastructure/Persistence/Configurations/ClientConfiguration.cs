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
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired();

            builder.HasMany(c => c.ContactList).WithOne().HasForeignKey(c => c.ClientId);

            builder.HasOne(c => c.Currency).WithMany();

            builder.HasMany(c => c.Projects).WithOne().HasForeignKey(c => c.ClientId);
        }
    }
}