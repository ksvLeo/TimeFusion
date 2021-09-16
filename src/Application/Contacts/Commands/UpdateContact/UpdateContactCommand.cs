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

            if (string.IsNullOrEmpty(request.newContact.Name))
            {
                return UpdateContactResult.EmptyName;
                //throw new ArgumentException("Contact name must not be null.");
            }

            bool nameExists = await _context.Contacts.AnyAsync(c => c.ClientId == request.ClientId &&
                                                                    c.Id != request.newContact.Id &&
                                                                    c.Name == request.newContact.Name);

            if (nameExists)
            {
                return UpdateContactResult.Error_NameExists;
                //throw new ArgumentException($"Contact name : {request.newContact.Name} already exists in client.");
            }

            Contact contact = new Contact
            {
                Id = request.newContact.Id,
                Name = request.newContact.Name,
                Title = request.newContact.Title,
                ClientId = request.ClientId,
                Email = request.newContact.Email,
                PhoneNumber = request.newContact.PhoneNumber,
                Active = true
            };

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync(cancellationToken);

            return UpdateContactResult.Success;
        }
    }
}
