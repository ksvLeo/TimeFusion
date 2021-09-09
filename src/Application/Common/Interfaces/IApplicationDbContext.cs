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

        DbSet<Currency> Currencies { get; set; }

        DbSet<Contact> Contacts { get; set; }

        DbSet<Client> Clients { get; set; }

        DbSet<Project> Projects { get; set; }

        DbSet<BudgetType> BudgetTypes { get; set; }

        DbSet<ProjectStatus> ProjectStatuses { get; set; }

        DbSet<ProjectType> ProjectTypes { get; set; }

        DbSet<RateType> RateTypes { get; set; }

        DbSet<TimeDistribution> TimeDistributions { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
