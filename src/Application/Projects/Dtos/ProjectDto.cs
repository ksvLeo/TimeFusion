using FusionIT.TimeFusion.Application.Common.Mappings;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Projects.Dtos
{
    public class ProjectDto : IMapFrom<Project>
    {
        public int Id { get; set; }

        public string ReferenceCode { get; set; }

        public int ClientId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }

        public IList Tags { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime FinishDate { get; set; }

        public ProjectType ProjectType { get; set; }

        public BudgetType BudgetType { get; set; }

        public int WorkTimeTicks { get; set; }

    }
}
