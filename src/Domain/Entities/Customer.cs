using FusionIT.TimeFusion.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Domain.Entities
{
    public class Customer : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public Currency Currency { get; set; }

        public Referrer Referrer { get; set; }

        public bool Active { get; set; }

        public IList<Project> Projects { get; set; }
    }
}
