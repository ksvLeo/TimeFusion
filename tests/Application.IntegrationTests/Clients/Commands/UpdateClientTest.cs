using FluentAssertions;
using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
using FusionIT.TimeFusion.Application.Clients.Commands.UpdateClient;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Testing;

namespace FusionIT.TimeFusion.Application.IntegrationTests.Clients.Commands
{
    public class UpdateClientTest : TestBase
    {
        [Test]
        public void Update_RequestWithEmptyName_Fails()
        {
            var client = new ClientDto();
            var command = new UpdateClientCommand();
            command.Client = client;
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Equals(UpdateClientResult.EmptyName);
        }

        [Test]
        public async Task ShouldUpdateClient()
        {
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "test" }
            });

            var command = new UpdateClientCommand
            {
                Client = new ClientDto
                {
                    Id = clientResult.Id,
                    Name = "New name",
                    Address = "New address",
                    Status = ClientStatus.Active,
                    Currency = new Currencies.Dtos.CurrencyDto { Id = 1}
                }
            };

            await SendAsync(command);

            var client = await FindAsync<Client>(clientResult.Id);

            client.Name.Should().Be(command.Client.Name);
            client.Address.Should().Be(command.Client.Address);
            client.Status.Should().Be(command.Client.Status);
        }
    }
}