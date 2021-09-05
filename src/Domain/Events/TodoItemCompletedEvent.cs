using FusionIT.TimeFusion.Domain.Common;
using FusionIT.TimeFusion.Domain.Entities;

namespace FusionIT.TimeFusion.Domain.Events
{
    public class TodoItemCompletedEvent : DomainEvent
    {
        public TodoItemCompletedEvent(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}
