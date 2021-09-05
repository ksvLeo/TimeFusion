using FusionIT.TimeFusion.Application.Common.Mappings;
using FusionIT.TimeFusion.Domain.Entities;
using System.Collections.Generic;

namespace FusionIT.TimeFusion.Application.TodoLists.Queries.GetTodos
{
    public class TodoListDto : IMapFrom<TodoList>
    {
        public TodoListDto()
        {
            Items = new List<TodoItemDto>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Colour { get; set; }

        public IList<TodoItemDto> Items { get; set; }
    }
}
