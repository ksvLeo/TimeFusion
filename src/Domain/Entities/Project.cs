using FusionIT.TimeFusion.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public int Id { get; set; }

        public string ReferenceCode { get; set; }

        public int ClientId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string  Notes { get; set; }

        //public IList<string> Tags { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public ProjectStatus ProjectStatus { get; set; }

        public ProjectType ProjectType { get; set; }

        public BudgetType BudgetType { get; set; }

        public int WorkTimeTicks { get; set; }
    }
}
