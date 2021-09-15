
namespace FusionIT.TimeFusion.Application.IntegrationTests.Clients.Commands
{
    using FluentAssertions;
    using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
    using FusionIT.TimeFusion.Domain.Entities;
    using NUnit.Framework;
    using System;
    using static Testing;

    public class CreateClientTests : TestBase
    {
        [Test]
        public void Creation_RequestWithEmptyName_Fails()
        {
            var testClient = new Client();
            var command = new CreateClientCommand();
            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NullReferenceException>();
        }

        //[Test]
        //public async Task ShouldCreateClient()
        //{
        //    var client
        //}
    }
}
