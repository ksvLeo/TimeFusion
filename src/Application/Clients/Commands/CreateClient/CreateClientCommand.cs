using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Contacts.Dtos;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Commands.CreateClient
{
    public class CreateClientCommand : IRequest<CreateClientResultDto>
    {
        public ClientDto NewClient { get; set; }
    }

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, CreateClientResultDto>
    {
        private readonly IApplicationDbContext _context;

        public CreateClientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CreateClientResultDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            CreateClientResultDto result = new CreateClientResultDto();

            Client clientNameExist = _context.Clients.FirstOrDefault(c => c.Name == request.NewClient.Name);

            if (clientNameExist != null)
            {
                result.result = CreateClientResult.Error_NameExists;
                return result;
            }

            Currency currency;

            if(request.NewClient.Currency != null)
            {
                currency = _context.Currencies.FirstOrDefault(c => c.Id == request.NewClient.Currency.Id);
            }
            else
            {
                currency = null;
            }

            var contacts = new List<Contact>();

            if (request.NewClient.ContactList != null)
            {
                var contact = new Contact
                {
                    Title = request.NewClient.ContactList[0].Email,
                    Name = request.NewClient.ContactList[0].Name,
                    Email = request.NewClient.ContactList[0].Email,
                    PhoneNumber = request.NewClient.ContactList[0].PhoneNumber,
                    Active = true
                };

                contacts.Add(contact);
            }

            var client = new Client
            {
                Name = request.NewClient.Name,
                Address = request.NewClient.Address,
                Currency = currency,
                ContactList = contacts,
                Status = ClientStatus.Active
            };

            _context.Clients.Add(client);

            result.Id = await _context.SaveChangesAsync(cancellationToken);
            result.result = CreateClientResult.Success;

            return result;
        }
    }
}
