using FusionIT.TimeFusion.Domain.Common;
using FusionIT.TimeFusion.Domain.Entities;

namespace FusionIT.TimeFusion.Domain.Events
{
    public class TodoItemCreatedEvent : DomainEvent
    {
        public TodoItemCreatedEvent(TodoItem item)
        {
            Item = item;
        }

        public TodoItem Item { get; }
    }
}
