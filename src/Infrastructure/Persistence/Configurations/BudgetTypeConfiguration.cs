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
    public class BudgetTypeConfiguration : IEntityTypeConfiguration<BudgetType>
    {
        public void Configure(EntityTypeBuilder<BudgetType> builder)
        {
            
        }
    }
}
