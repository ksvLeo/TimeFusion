using FusionIT.TimeFusion.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace FusionIT.TimeFusion.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
