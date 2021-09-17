using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using FusionIT.TimeFusion.Domain.ValueObjects;
using FusionIT.TimeFusion.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
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

            Currency USD = new Currency { Name = "US Dollar", Alpha3Code = "USD", Symbol = "U$S" };
            Currency UYU = new Currency { Name = "Peso Uruguayo", Alpha3Code = "UYU", Symbol = "$" };

            if (!context.Currencies.Any())
            {
                context.Currencies.Add(USD);
                context.Currencies.Add(UYU); 
            }

            List<Contact> testContacts = new List<Contact>();
            Contact testContact = new Contact { Name = "Romualdo Rodríguez", Email = "roro1969@hotmail.com", Active = true, Title = "Dpto. Técnico", PhoneNumber = "099699420" };
            testContacts.Add(testContact);
            testContact = new Contact { Name = "Julieta Veganas", Email = "juve3500@gmail.com", Active = true, Title = "Dpto. Ventas", PhoneNumber = "08888888" };
            testContacts.Add(testContact);
            testContact = new Contact { Name = "Rubén Vazquez Santana", Email = "ruben2003@realemail.com", Active = false, Title = "Asistente al Proyecto", PhoneNumber = "09483456" };
            testContacts.Add(testContact);
            testContact = new Contact { Name = "Jimena Martin Perez", Email = "jime.marp@protonmail.com", Active = true, Title = "Project Manager", PhoneNumber = "094563486" };
            testContacts.Add(testContact);
            testContact = new Contact { Name = "José Gomez", Email = "josegomez@gmail.com", Active = true, Title = "Recursos Humanos", PhoneNumber = "0945386" };
            testContacts.Add(testContact);

            if (!context.Clients.Any())
            {
                context.Clients.Add(new Client { Name = "Santander Uruguay", Address = "Calle Falsa 123", Currency = USD, Status = ClientStatus.Inactive});
                context.Clients.Add(new Client { Name = "Fusion IT Uruguay", Address = "Calle Falsa 123", Currency = USD, Status = ClientStatus.Active });
                context.Clients.Add(new Client { Name = "BHCU", Address = "8 de Octubre 3239 bis", Currency = USD, Status = ClientStatus.Active, ContactList = testContacts });
                context.Clients.Add(new Client { Name = "Consultorio Odontológico de enfrente", Address = "Calle Falsa 123", Currency = USD, Status = ClientStatus.Inactive });
                context.Clients.Add(new Client { Name = "Santander Chile", Address = "Calle Falsa 123", Currency = USD, Status = ClientStatus.Active });
            }

            if (!context.BudgetTypes.Any())
            {
                context.BudgetTypes.Add(new BudgetType { Description = "On Demand" });
                context.BudgetTypes.Add(new BudgetType { Description = "By The Hour" });
                context.BudgetTypes.Add(new BudgetType { Description = "By The Fee" });
            }

            if (!context.ProjectStatuses.Any())
            {
                context.ProjectStatuses.Add(new ProjectStatus { Description = "Inactive" });
                context.ProjectStatuses.Add(new ProjectStatus { Description = "Active" });
                //context.ProjectStatuses.Add(new ProjectStatus { Description = "On hold" });
                context.ProjectStatuses.Add(new ProjectStatus { Description = "Finished" });
            }

            if (!context.ProjectTypes.Any())
            {
                context.ProjectTypes.Add(new ProjectType { Description = "Non Billable" });
                context.ProjectTypes.Add(new ProjectType { Description = "Fixed Price" });
                context.ProjectTypes.Add(new ProjectType { Description = "Time And Materials" }); //No encontré cómo se llamaría este tipo de proyectos, por ahora va con ese nombre
            }

            if (!context.RateTypes.Any())
            {
                context.RateTypes.Add(new RateType { Description = "Total Cost Rate" });
                context.RateTypes.Add(new RateType { Description = "Per Task Rate" });
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

            }
            await context.SaveChangesAsync();
        }
    }
}
