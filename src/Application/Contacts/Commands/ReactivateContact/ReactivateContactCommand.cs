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

namespace FusionIT.TimeFusion.Application.Contacts.Commands.UpdateContact
{
    public class ReactivateContactCommand : IRequest<ReactivateContactResult>
    {
        public int ContactId { get; set; }
        public int ClientId { get; set; }
    }

    public class ReactivateContactCommandHandler : IRequestHandler<ReactivateContactCommand, ReactivateContactResult>
    {
        private readonly IApplicationDbContext _context;
        
        public ReactivateContactCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ReactivateContactResult> Handle(ReactivateContactCommand request, CancellationToken cancellationToken)
        {
            Contact contact = await _context.Contacts.FirstOrDefaultAsync(c => c.ClientId == request.ClientId && c.Id == request.ContactId);

            if (contact == null)
            {
                return ReactivateContactResult.Error_NotFound;
                //throw new ArgumentException("No contact was found with ID #" + request.ContactId);
            }

            if (contact.Active == true)
            {
                throw new ArgumentException($"Contact with ID # {request.ContactId} is already active");
            }

            contact.Active = true;

            await _context.SaveChangesAsync(cancellationToken);

            return ReactivateContactResult.Success;
        }
    }


}
