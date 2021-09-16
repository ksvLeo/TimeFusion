using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Contacts.Commands.DeleteContact
{
    public class DeleteContactCommand : IRequest<DeleteContactResult>
    {
        public int ClientId { get; set; }

        public int ContactId { get; set; }
    }

    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, DeleteContactResult>
    {
        private readonly IApplicationDbContext _context;

        public DeleteContactCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteContactResult> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            Contact contact = await _context.Contacts.FirstOrDefaultAsync(c => c.ClientId == request.ClientId && c.Id == request.ContactId);

            if (contact == null)
            {
                return DeleteContactResult.Error_NotFound;
                //throw new ArgumentException("No contact was found with ID #" + request.ContactId);
            }

            if (contact.Active == false)
            {
                throw new ArgumentException($"Contact with ID # {request.ContactId} is already inactive");
            }

            contact.Active = false;

            await _context.SaveChangesAsync(cancellationToken);
            
            return DeleteContactResult.Success;
        }
    }
}
