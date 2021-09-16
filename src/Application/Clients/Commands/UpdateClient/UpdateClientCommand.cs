using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientCommand : IRequest<bool>
    {
        public ClientDto Client { get; set; }
    }

    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateClientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            Client client = await _context.Clients.Include(c => c.ContactList).FirstOrDefaultAsync(c => c.Id == request.Client.Id);

            if (client == null)
            {
                throw new ArgumentException($"Unable to find client with ID #{ request.Client.Id }");
            }

            if (string.IsNullOrEmpty(request.Client.Name))
            {
                throw new ArgumentException("Name field can't be null.");
            }

            Client nameClientExist = await _context.Clients
                .Where(c => c.Id != client.Id && c.Name == request.Client.Name)
                .FirstOrDefaultAsync();

            if(nameClientExist != null)
            {
                throw new ArgumentException("Client with name: " + request.Client.Name + "already exists.");
            }

            Currency currency = await _context.Currencies
                .FirstOrDefaultAsync(c => c.Id == request.Client.Currency.Id);

            if (currency == null)
            {
                throw new ArgumentException($"Unable to find currency with ID #{request.Client.Currency.Id}.");
            }

            List<Contact> contacts = new List<Contact>();

            request.Client.ContactList.ForEach(c =>
            {
                Contact referrer =  _context.Contacts
                    .FirstOrDefault(r => r.Id == c.Id);

                if(referrer == null)
                {
                    referrer = new Contact();
                }

                // Map referrer update
                referrer.Title = c.Title;
                referrer.Name = c.Name;
                referrer.PhoneNumber = c.PhoneNumber;
                referrer.Email = c.Email;
                referrer.Active = c.Active;

                // Add to list referrers
                contacts.Add(referrer);
            });

            // Map customer update
            client.Name = request.Client.Name;
            client.Address = request.Client.Address;
            client.Currency = currency;
            client.ContactList = contacts;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
