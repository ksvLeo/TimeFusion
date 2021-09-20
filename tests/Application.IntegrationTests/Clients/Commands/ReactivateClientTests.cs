using FluentAssertions;
using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
using FusionIT.TimeFusion.Application.Clients.Commands.ReactivateClient;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Exceptions;
using FusionIT.TimeFusion.Application.Projects.Commands.CreateProject;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using NUnit.Framework;
using System.Threading.Tasks;
using static Testing;

namespace FusionIT.TimeFusion.Application.IntegrationTests.Clients.Commands
{
    public class ReactivateClientTests : TestBase
    {
        [Test]
        public void Update_NonExistentClientId_Fails()
        {
            FluentActions.Invoking(() =>
                SendAsync(new ReactivateClientCommand { ClientId = -1 })).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Update_ClientAlreadyActive_Fails()
        {
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "test", Status = ClientStatus.Active }
            });

            FluentActions.Invoking(() =>
                SendAsync(new ReactivateClientCommand { ClientId = clientResult.Id })).Should().Equals(DeleteClientResult.Error_ActiveProjects);
        }

        [Test]
        public async Task ShouldReactivateClient()
        {
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "test", Status = ClientStatus.Inactive }
            });

            await SendAsync(new ReactivateClientCommand { ClientId = clientResult.Id });

            var client = await FindAsync<Client>(clientResult.Id);

            clientResult.Result.Should().Be(ReactivateClientResult.Success);
            client.Name.Should().NotBeNullOrEmpty();
            client.Status.Should().Be(ClientStatus.Active);
        }
    }
}
