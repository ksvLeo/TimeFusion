using FusionIT.TimeFusion.Domain.Common;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Common.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent domainEvent);
    }
}
