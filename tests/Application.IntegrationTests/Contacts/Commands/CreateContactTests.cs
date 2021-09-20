using FluentAssertions;
using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Exceptions;
using FusionIT.TimeFusion.Application.Contacts.Commands.CreateContact;
using FusionIT.TimeFusion.Application.Contacts.Dtos;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Testing;

namespace FusionIT.TimeFusion.Application.IntegrationTests.Contacts.Commands
{
    public class CreateContactTests : TestBase
    {
        public object FluentAccions { get; private set; }

        [Test]
        public void Creation_RequestWithEmpatyName_Fails()
        {
            var testContact = new ContactDto();
            var command = new CreateContactCommand();
            command.Contact = testContact;
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task Creation_ReqeuestWithExistingName_Fails()
        {

            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "Test", ContactList = new List<ContactDto>() { new ContactDto() { Name = "Testing" } } }
            });

            var command = new CreateContactCommand();

            ContactDto newContact = new ContactDto()
            {
                Name = "Testing"
            };

            command.ClientId = clientResult.Id;
            command.Contact = newContact;

            FluentActions.Invoking(() => SendAsync(command)).Should().Equals(CreateContactResult.Error_NameExists);
        }


        [Test]
        public async Task ShouldCreateContact()
        {
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto { Name = "Test", ContactList = new List<ContactDto>() { new ContactDto() { Name = "Testing" } } }
            });

            var command = new CreateContactCommand();

            ContactDto newContact = new ContactDto()
            {
                Name = "Testing Two"
            };

            command.Contact = newContact;
            command.ClientId = clientResult.Id;

            FluentActions.Invoking(() => SendAsync(command)).Should().Equals(DeleteContactResult.Error);
        }

    }
}
