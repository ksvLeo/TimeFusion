using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Commands.UpdateCustomer
{
    public class ReactivateClientCommand : IRequest<bool>
    {
        public int ClientId { get; set; }
    }

    public class ReactivateCustomerCommandHandler : IRequestHandler<ReactivateClientCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public ReactivateCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ReactivateClientCommand request, CancellationToken cancellationToken)
        {
            Client client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId);

            if (client == null)
            {
                throw new ArgumentException($"Unable to find customer with ID #{ request.ClientId }.");
            }

            if (client.Active == true)
            {
                throw new ArgumentException($"Client with ID #{ request.ClientId } is already active.");
            }

            client.Active = true;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
