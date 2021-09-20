using FluentAssertions;
using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
using FusionIT.TimeFusion.Application.Clients.Commands.DeleteCustomer;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Contacts.Dtos;
using FusionIT.TimeFusion.Application.Projects.Commands.CreateProject;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using NUnit.Framework;
using System.Threading.Tasks;
using static Testing;

namespace FusionIT.TimeFusion.Application.IntegrationTests.Clients.Commands
{
    public class DeleteClientTests : TestBase
    {
        [Test]
        public async Task Delete_ClientWithActiveProjects_Fails()
        {
          
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "test"}
            });

            await SendAsync(new CreateProjectCommand
            {
                NewProject = new Project { ProjectStatus = new ProjectStatus { Description = "Active" }, ClientId = clientResult.Id }
            });

            FluentActions.Invoking(() =>
                SendAsync(new DeleteClientCommand { ClientId = clientResult.Id })).Should().Equals(DeleteClientResult.Error_ActiveProjects);

        }

        [Test]
        public async Task Delete_ClientAlreadyInactive_Fails()
        {
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "test", Status = ClientStatus.Inactive }
            });

            FluentActions.Invoking(() =>
                SendAsync(new DeleteClientCommand { ClientId = clientResult.Id })).Should().Equals(DeleteClientResult.Error);
        }

        [Test]
        public async Task ShouldDeleteClient()
        {
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "test", Status = ClientStatus.Active }
            });

            await SendAsync(new DeleteClientCommand { ClientId = clientResult.Id });

            var deletedClient = await FindAsync<Client>(clientResult.Id);

            deletedClient.Name.Should().NotBeNullOrEmpty();
            deletedClient.Status.Should().Be(ClientStatus.Inactive);
        }

    }
}
