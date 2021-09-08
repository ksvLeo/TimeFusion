using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Common.Dtos
{
    public class PaginatedListDto<T>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public PaginationOrder Order { get; set; }
        public string OrderField { get; set; }
        public int TotalItems { get; set; }
        public List<T> Items { get; set; }
        
        public PaginatedListDto(int pageSize, int pageNumber, PaginationOrder order, string orderField, int totalItems, List<T> items)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            Order = order;
            OrderField = orderField;
            TotalItems = totalItems;
            Items = items;
        }
    }

    public enum PaginationOrder
    {
        ASD = 1,
        DESC = 2
    }
}
