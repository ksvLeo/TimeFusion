using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Mappings;
using FusionIT.TimeFusion.Application.Currencies.Dtos;
using FusionIT.TimeFusion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Clients.Dtos
{
    public class ClientDto : IMapFrom<Client>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public CurrencyDto Currency { get; set; }

        public List<ReferrerDto> Referrer { get; set; }

        public bool Active { get; set; }
    }
}
