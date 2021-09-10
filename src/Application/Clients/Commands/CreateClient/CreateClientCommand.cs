﻿using FusionIT.TimeFusion.Application.Clients.Dtos;
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
            
            List<Contact> contacts = new List<Contact>();

            Contact contact = new Contact
            {
                Title = request.newClient.ContactList[0].Email,
                Name = request.newClient.ContactList[0].Name,
                Email = request.newClient.ContactList[0].Email,
                PhoneNumber = request.newClient.ContactList[0].PhoneNumber,
                Active = true
            };

            contacts.Add(contact);

            Client entity = new Client
            {
                Name = request.newClient.Name,
                Address = request.newClient.Address,
                Currency = currency,
                ContactList = contacts,
                Active = true
            };

            _context.Clients.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
