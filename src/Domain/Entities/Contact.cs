using FusionIT.TimeFusion.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Domain.Entities
{
    public class Contact : AuditableEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int ClientId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool Active { get; set; }
    }
}
