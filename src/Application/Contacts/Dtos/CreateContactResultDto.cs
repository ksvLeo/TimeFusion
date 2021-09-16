using FusionIT.TimeFusion.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Contacts.Dtos
{
    public class CreateContactResultDto
    {
        public int Id { get; set; }
        public CreateContactResult Result { get; set; }
    }
}
