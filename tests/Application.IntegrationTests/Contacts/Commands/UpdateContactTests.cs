using FluentAssertions;
using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Exceptions;
using FusionIT.TimeFusion.Application.Contacts.Commands.CreateContact;
using FusionIT.TimeFusion.Application.Contacts.Commands.UpdateContact;
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
    public class UpdateContactTests : TestBase
    {
        [Test]
        public void Update_RequestEmptyName_Fails()
        {
            var command = new UpdateContactCommand
            {
                ClientId = 1,
                newContact = new ContactDto { 
                    Id = 2,
                    ClientId = 1
                }
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("newContact.Name"))
                    .And.Errors["newContact.Name"].Should().Contain("Name is required.");

        }

        [Test]
        public async Task Update_NameExist_Fails()
        {
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto
                {
                    Name = "test"
                }
            });

            var contact1 = await SendAsync(new CreateContactCommand
            {
                ClientId = clientResult.Id,
                Contact = new ContactDto
                {
                    Name = "Contact",
                    ClientId = clientResult.Id
                }
            });

            var contact2 = await SendAsync(new CreateContactCommand
            {
                ClientId = clientResult.Id,
                Contact = new ContactDto
                {
                    Name = "Contact 2",
                    ClientId = clientResult.Id
                }
            });

            var command = new UpdateContactCommand
            {
                ClientId = clientResult.Id,
                newContact = new ContactDto
                {
                    Id = contact2.Id,
                    Name = "Contact",
                    ClientId = clientResult.Id
                }
            };

            FluentActions.Invoking(() =>
                SendAsync(command))
                    .Should().Throw<ValidationException>().Where(ex => ex.Errors.ContainsKey("newContact.Name"))
                    .And.Errors["newContact.Name"].Should().Contain("The specified name already exists in other contact.");
        }

        [Test]
        public async Task Update_Contacts_Success()
        {
            var clientResult = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto
                {
                    Name = "test"
                }
            });

            var clientResult2 = await SendAsync(new CreateClientCommand
            {
                NewClient = new ClientDto
                {
                    Name = "Client 2"
                }
            });

            var contact1 = await SendAsync(new CreateContactCommand
            {
                ClientId = clientResult.Id,
                Contact = new ContactDto
                {
                    Name = "Contact",
                    ClientId = clientResult.Id
                }
            });

            var contact2 = await SendAsync(new CreateContactCommand
            {
                ClientId = clientResult2.Id,
                Contact = new ContactDto
                {
                    Name = "Contact 2",
                    ClientId = clientResult2.Id
                }
            });

            var command = new UpdateContactCommand
            {
                ClientId = clientResult2.Id,
                newContact = new ContactDto
                {
                    Id = contact2.Id,
                    Name = "Contact",
                    ClientId = clientResult2.Id
                }
            };

            FluentActions.Invoking(() => SendAsync(command))
                .Should().Equals(UpdateContactResult.Success);
        }

    }
}
