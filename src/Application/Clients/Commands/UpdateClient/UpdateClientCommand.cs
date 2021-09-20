﻿using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientCommand : IRequest<UpdateClientResult>
    {
        public ClientDto Client { get; set; }
    }

    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, UpdateClientResult>
    {
        private readonly IApplicationDbContext _context;

        public UpdateClientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateClientResult> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            Client client = await _context.Clients.Include(c => c.ContactList).FirstOrDefaultAsync(c => c.Id == request.Client.Id);

            if (client == null)
            {
                return UpdateClientResult.Error;
                //throw new ArgumentException($"Unable to find client with ID #{ request.Client.Id }");
            }

            if (string.IsNullOrEmpty(request.Client.Name))
            {
                return UpdateClientResult.EmptyName;
                //throw new ArgumentException("Name field can't be null.");
            }

            Client nameClientExist = await _context.Clients
                .Where(c => c.Id != client.Id && c.Name == request.Client.Name)
                .FirstOrDefaultAsync();

            if(nameClientExist != null)
            {
                return UpdateClientResult.Error_NameExists;
            }

            Currency currency = await _context.Currencies
                .FirstOrDefaultAsync(c => c.Alpha3Code == request.Client.Currency.Alpha3Code);

            if (currency == null)
            {
                return UpdateClientResult.Error;
            }

            // Map customer update
            client.Name = request.Client.Name;
            client.Address = request.Client.Address;
            client.Currency = currency;
            client.ContactList = client.ContactList;

            await _context.SaveChangesAsync(cancellationToken);

            return UpdateClientResult.Success;
        }
    }
}
