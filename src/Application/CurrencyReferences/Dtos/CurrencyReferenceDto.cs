using FusionIT.TimeFusion.Application.Common.Mappings;
using FusionIT.TimeFusion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.CurrencyReferences.Dtos
{
    public class CurrencyReferenceDto : IMapFrom<CurrencyReference>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Alpha3Code { get; set; }

        public string Symbol { get; set; }
    }
}
