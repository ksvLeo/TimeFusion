using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Common.Dtos
{
    public class PaginatedListDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public PaginationOrder Order { get; set; }
        public string OrderField { get; set; }
        public int TotalItems { get; set; }
        public List<Object> Items { get; set; }
    }

    public enum PaginationOrder
    {
        ASD = 1,
        DESC = 2
    }
}
