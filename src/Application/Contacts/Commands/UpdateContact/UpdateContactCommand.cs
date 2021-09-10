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
                throw new ArgumentException("Contact name must not be null.");
            }

            bool nameExists = await _context.Contacts.AnyAsync(c => c.ClientId == request.ClientId &&
                                                                    c.Id != request.newContact.Id &&
                                                                    c.Name == request.newContact.Name);

            if (nameExists)
            {
                throw new ArgumentException($"Contact name : {request.newContact.Name} already exists in client.");
            }

            // Obtaining contact
            Contact contact = _context.Contacts.Where(c =>  c.Id == request.newContact.Id).FirstOrDefault();

            // Updating contact
            contact.Email = request.newContact.Email;
            contact.Name = request.newContact.Name;
            contact.PhoneNumber = request.newContact.PhoneNumber;
            contact.Title = request.newContact.Title;

            await _context.SaveChangesAsync(cancellationToken);

            return contact.Id;
    
        }
    }
}
