using FluentAssertions;
using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
using FusionIT.TimeFusion.Application.Clients.Commands.UpdateClient;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Currencies.Dtos;
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

            var command = new UpdateClientCommand();


            ClientDto updateClient = new ClientDto
            {
                Id = clientResult.Id,
                Name = "Testing",
                Address = "New address",
                Status = ClientStatus.Active,
                Currency = new CurrencyDto() { Alpha3Code = "USD"}
            };
            command.Client = updateClient;

            var client = await FindAsync<Client>(clientResult.Id);

            await SendAsync(command);

            
            client.Should().NotBeNull();
            client.Name.Should().NotMatch(command.Client.Name);
            command.Client.Name.Should().NotBeNullOrEmpty();
            client.Status.Should().Equals(command.Client.Status);
        }
    }
}