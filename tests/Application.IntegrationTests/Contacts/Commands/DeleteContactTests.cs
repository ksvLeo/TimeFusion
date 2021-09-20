using FluentAssertions;
using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
using FusionIT.TimeFusion.Application.Clients.Commands.DeleteCustomer;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Contacts.Commands.CreateContact;
using FusionIT.TimeFusion.Application.Contacts.Commands.DeleteContact;
using FusionIT.TimeFusion.Application.Contacts.Dtos;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Testing;


namespace FusionIT.TimeFusion.Application.IntegrationTests.Contacts.Commands
{
    public class DeleteContactTests : TestBase
    {
        [Test]
        public async Task Delete_ClientWithActiveProjects_Fails()
        {

            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "Test" }
            });

            var contactResult = await SendAsync(new CreateContactCommand
            {
                ClientId = clientResult.Id,
                Contact = new ContactDto() { Name = "Testing" }
            });

            DeleteContactCommand common = new DeleteContactCommand();
            common.ContactId = contactResult.Id;
            common.ClientId = 5;

            FluentActions.Invoking(() => SendAsync(common)).Should().Equals(DeleteContactResult.Error_NotFound);
        }

        [Test]
        public async Task Delete_ContactAlreadyInactive_Fails()
        {
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "Test" }
            });

            var contactResult = await SendAsync(new CreateContactCommand
            {
                ClientId = clientResult.Id,
                Contact = new ContactDto() { Name = "Testing", Active = false}
            });

            DeleteContactCommand common = new DeleteContactCommand();
            common.ContactId = contactResult.Id;
            common.ClientId = clientResult.Id;

            FluentActions.Invoking(() =>
                SendAsync(new DeleteClientCommand { ClientId = clientResult.Id })).Should().Equals(DeleteContactResult.Error);
        }

        [Test]
        public async Task ShouldDeleteContact()
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

            FluentActions.Invoking(() =>
                SendAsync(new DeleteContactCommand { ClientId = clientResult.Id })).Should().Equals(DeleteContactResult.Success);

            DeleteContactCommand common = new DeleteContactCommand();
            common.ContactId = contactResult.Id;
            common.ClientId = clientResult.Id;

            var deletedContact = await FindAsync<Contact>(contactResult.Id);

            deletedContact.Name.Should().NotBeNullOrEmpty();
            deletedContact.Active.Should().Equals(false);
        }
    }
}

