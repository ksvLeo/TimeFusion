using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Customers.Commands.CreateCustomer;
using FusionIT.TimeFusion.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Customers.Commands.UpdateCustomer
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
            Customer client = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.ClientId);

            if (client == null)
            {
                throw new ArgumentException($"Unable to find customer with ID #{ request.ClientId }.");
            }

            if (client.Active == false)
            {
                throw new ArgumentException($"Client with ID #{ request.ClientId } is already unactive.");
            }

            client.Active = false;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
