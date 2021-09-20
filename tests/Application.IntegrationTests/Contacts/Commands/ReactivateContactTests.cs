using FluentAssertions;
using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
using FusionIT.TimeFusion.Application.Clients.Commands.ReactivateClient;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Exceptions;
using FusionIT.TimeFusion.Application.Contacts.Commands.CreateContact;
using FusionIT.TimeFusion.Application.Contacts.Commands.DeleteContact;
using FusionIT.TimeFusion.Application.Contacts.Commands.UpdateContact;
using FusionIT.TimeFusion.Application.Contacts.Dtos;
using FusionIT.TimeFusion.Application.Projects.Commands.CreateProject;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using NUnit.Framework;
using System.Threading.Tasks;
using static Testing;


namespace FusionIT.TimeFusion.Application.IntegrationTests.Contacts.Commands
{
    public class ReactivateContactTests : TestBase
    {
        [Test]

        public void ReactvateContact_NoExistContact_fails()
        {
            FluentActions.Invoking(() => SendAsync(new ReactivateClientCommand { ClientId = 20 })).Should().Equals(ReactivateContactResult.Error_NotFound);
        }

        [Test]
        public async Task Reactive_ContactAlreadyActive_Fails()
        {
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "Test" }
            });

            var contactResult = await SendAsync(new CreateContactCommand
            {
                ClientId = clientResult.Id,
                Contact = new ContactDto() { Name = "Testing", Active = true }
            });


            FluentActions.Invoking(() =>
              SendAsync(new ReactivateContactCommand { ClientId = clientResult.Id })).Should().Equals(ReactivateContactResult.Error_NotFound);
        }

        [Test]
        public async Task ShouldDeleteContacto()
        {
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "Test" }
            });

            var contactResult = await SendAsync(new CreateContactCommand
            {
                ClientId = clientResult.Id,
                Contact = new ContactDto() { Name = "Testing", Active = false }
            });

            FluentActions.Invoking(() => SendAsync(new ReactivateContactCommand() { ClientId = clientResult.Id, ContactId = contactResult.Id }).Should().Equals(ReactivateContactResult.Success));
        }
    }
}
