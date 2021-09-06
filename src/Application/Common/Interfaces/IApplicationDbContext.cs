using FusionIT.TimeFusion.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TodoList> TodoLists { get; set; }

        DbSet<TodoItem> TodoItems { get; set; }

        DbSet<CurrencyReference> CurrencyReferences { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
