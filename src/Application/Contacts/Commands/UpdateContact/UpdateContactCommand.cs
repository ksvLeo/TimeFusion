using FusionIT.TimeFusion.Application.Contacts.Dtos;
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
using FusionIT.TimeFusion.Domain.Enums;

namespace FusionIT.TimeFusion.Application.Contacts.Commands.UpdateContact
{
    public class UpdateContactCommand : IRequest<UpdateContactResult>
    {
        public ContactDto newContact { get; set; }
        public int ClientId { get; set; }
    }

    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, UpdateContactResult>
    {
        private readonly IApplicationDbContext _context;

        public UpdateContactCommandHandler(IApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task<UpdateContactResult> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            Contact contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == request.newContact.Id);

            if (contact == null)
            {
                return UpdateContactResult.Error;
            }

            contact.Name = request.newContact.Name;
            contact.Title = request.newContact.Title;
            contact.ClientId = request.ClientId;
            contact.Email = request.newContact.Email;
            contact.PhoneNumber = request.newContact.PhoneNumber;

            await _context.SaveChangesAsync(cancellationToken);

            return UpdateContactResult.Success;
        }
    }
}
