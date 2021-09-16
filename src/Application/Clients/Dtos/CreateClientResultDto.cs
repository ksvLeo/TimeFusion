using FusionIT.TimeFusion.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Dtos
{
    public class CreateClientResultDto
    {
        public int Id { get; set; }
        public CreateClientResult result { get; set; }
    }
}
