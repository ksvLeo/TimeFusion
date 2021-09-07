using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.ValueObjects;
using FusionIT.TimeFusion.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Infrastructure.Persistence
{
    public static class FusionTimeDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var administratorRole = new IdentityRole("Administrator");

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "Administrator1!");
                await userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }
        }

        public static async Task SeedSampleDataAsync(FusionTimeDbContext context)
        {
            // Seed, if necessary

            // Currency Reference
            if (!context.Currencies.Any())
            {
                context.Currencies.Add(new Currency { Name = "Peso Uruguayo", Alpha3Code = "UYU", Symbol = "$" });
                context.Currencies.Add(new Currency { Name = "Dolar Estadounidense", Alpha3Code = "USD", Symbol = "U$S" }); 
            }

            if (!context.BudgetTypes.Any())
            {
                context.BudgetTypes.Add(new BudgetType { Description = "On Demand" });
                context.BudgetTypes.Add(new BudgetType { Description = "By The Hour" });
                context.BudgetTypes.Add(new BudgetType { Description = "By The Fee" });
            }

            if (!context.ProjectTypes.Any())
            {
                context.ProjectTypes.Add(new ProjectType { Description = "NonBillable" });
                context.ProjectTypes.Add(new ProjectType { Description = "FixedPrice" });
                context.ProjectTypes.Add(new ProjectType { Description = "TimeAndMaterials" });
            }

            if (!context.RateTypes.Any())
            {
                context.RateTypes.Add(new RateType { Description = "TotalCostRate" });
                context.RateTypes.Add(new RateType { Description = "PerTaskRate" });
            }

            if (!context.TimeDistributions.Any())
            {
                context.TimeDistributions.Add(new TimeDistribution { Description = "PerPerson" });
                context.TimeDistributions.Add(new TimeDistribution { Description = "PerTask" });
                context.TimeDistributions.Add(new TimeDistribution { Description = "TotalWorkTime" });
            }

            if (!context.TodoLists.Any())
            {
                context.TodoLists.Add(new TodoList
                {
                    Title = "Shopping",
                    Colour = Colour.Blue,
                    Items =
                    {
                        new TodoItem { Title = "Apples", Done = true },
                        new TodoItem { Title = "Milk", Done = true },
                        new TodoItem { Title = "Bread", Done = true },
                        new TodoItem { Title = "Toilet paper" },
                        new TodoItem { Title = "Pasta" },
                        new TodoItem { Title = "Tissues" },
                        new TodoItem { Title = "Tuna" },
                        new TodoItem { Title = "Water" }
                    }
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
