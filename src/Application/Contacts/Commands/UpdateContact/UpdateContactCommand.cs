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

namespace FusionIT.TimeFusion.Application.Contacts.Commands.UpdateContact
{
    public class UpdateContactCommand : IRequest<int>
    {
        public ContactDto newContact { get; set; }
        public int ClientId { get; set; }
    }

    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateContactCommandHandler(IApplicationDbContext context)
        {
            _context = context; 
        }
        public async Task<int> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrEmpty(request.newContact.Name))
            {
                throw new ArgumentException("Contact name can't be null.");
            }

            bool nameExists = await _context.Contacts.AnyAsync(c => c.ClientId == request.ClientId &&
                                                                    c.Id != request.newContact.Id &&
                                                                    c.Name == request.newContact.Name);

            if (nameExists)
            {
                throw new ArgumentException("Contact attached to Client: " + request.ClientId + " already exists with that name.");
            }

            Contact contact = new Contact
            {
                Name = request.newContact.Name,
                Title = request.newContact.Title,
                ClientId = request.ClientId,
                Email = request.newContact.Email,
                PhoneNumber = request.newContact.PhoneNumber,
                Active = true
            };

            await _context.SaveChangesAsync(cancellationToken);

            return contact.Id;
    
        }
    }
}
