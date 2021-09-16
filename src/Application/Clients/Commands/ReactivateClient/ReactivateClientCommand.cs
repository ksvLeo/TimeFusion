using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
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
    public class ReactivateClientCommand : IRequest<ReactivateClientResult>
    {
        public int ClientId { get; set; }
    }

    public class ReactivateCustomerCommandHandler : IRequestHandler<ReactivateClientCommand, ReactivateClientResult>
    {
        private readonly IApplicationDbContext _context;

        public ReactivateCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReactivateClientResult> Handle(ReactivateClientCommand request, CancellationToken cancellationToken)
        {
            Client client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

            if (client == null)
            {
               return ReactivateClientResult.Error_NotFound;
            }

            if (client.Status == ClientStatus.Active)
            {
                return ReactivateClientResult.Error_AlreadyActive;
            }

            client.Status = ClientStatus.Active;

            await _context.SaveChangesAsync(cancellationToken);

            return ReactivateClientResult.Success;
        }
    }
}
