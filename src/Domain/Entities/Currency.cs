using FusionIT.TimeFusion.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Domain.Entities
{
    public class Currency : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Alpha3Code { get; set; }

        public string Symbol { get; set; }
    }
}
