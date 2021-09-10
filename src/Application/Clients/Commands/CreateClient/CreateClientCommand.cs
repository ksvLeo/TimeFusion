using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Contacts.Dtos;
using FusionIT.TimeFusion.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Commands.CreateClient
{
    public class CreateClientCommand : IRequest<int>
    {
        public ClientDto newClient { get; set; }
        public ContactDto newContact { get; set; }
    }

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateClientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
             if (string.IsNullOrEmpty(request.newClient.Name))
             {
                throw new ArgumentException("Name field can't be null.");
             }

            Client client = _context.Clients.FirstOrDefault(c => c.Name == request.newClient.Name);

            if (client != null)
            {
                throw new ArgumentException($"Client with name: '{ request.newClient.Name }' already exist.");
            }

            Currency currency = _context.Currencies.FirstOrDefault(c => c.Id == request.newClient.Currency.Id);

            if (currency == null)
            {
                throw new ArgumentException($"Unable to find currency with ID: #{ request.newClient.Currency.Id }.");
            }
            
            Client entity = new Client
            {
                Name = request.newClient.Name,
                Address = request.newClient.Address,
                Currency = currency,
                Active = true
            };

            _context.Clients.Add(entity);

            Contact contact = new Contact
            {
                ClientId = entity.Id,
                Title = request.newContact.Email,
                Name = request.newContact.Name,
                Email = request.newContact.Email,
                PhoneNumber = request.newContact.PhoneNumber,
                Active = true
            };

            _context.Contacts.Add(contact);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
