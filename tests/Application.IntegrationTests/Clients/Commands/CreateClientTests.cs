using FluentAssertions;
using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Exceptions;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using static Testing;

namespace FusionIT.TimeFusion.Application.IntegrationTests.Clients.Commands
{

    public class CreateClientTests : TestBase
    {
        [Test]
        public void Creation_RequestWithEmptyName_Fails()
        {
            var testClient = new ClientDto();
            var command = new CreateClientCommand();
            command.NewClient = testClient;
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Creation_RequestWithExistingName_Fails()
        {
            var testClient = new ClientDto()
            {
                Name = "test"
            };

            var command = new CreateClientCommand();
            command.NewClient = testClient;
            
            await SendAsync(command);

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Equals(CreateClientResult.Error_NameExists);
        }

        [Test]
        public async Task ShouldCreateClient()
        {
            var userId = await RunAsDefaultUserAsync();
            
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "test" }
            });
            
            var client = await FindAsync<Client>(clientResult.Id);

            client.Should().NotBeNull();
            client.Name.Should().NotBeNull();
        }
    }
}
