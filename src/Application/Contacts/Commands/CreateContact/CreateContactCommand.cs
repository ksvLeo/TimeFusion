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

namespace FusionIT.TimeFusion.Application.Contacts.Commands.CreateContact
{
    public class CreateContactCommand : IRequest<Dtos.CreateContactResultDto>
    {
        public int ClientId { get; set; }

        public ContactDto Contact { get; set; }
    }

    public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, Dtos.CreateContactResultDto>
    {
        private readonly IApplicationDbContext _context;

        public CreateContactCommandHandler (IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dtos.CreateContactResultDto> Handle(CreateContactCommand request, CancellationToken cancellationToken)
        {
            Dtos.CreateContactResultDto result = new Dtos.CreateContactResultDto();

            if (string.IsNullOrEmpty(request.Contact.Name))
            {
                result.Result = Domain.Enums.CreateContactResult.EmptyName;
                return result;
                //throw new ArgumentException("Contact name can't be null.");
            }

            bool nameExists = await _context.Contacts.AnyAsync(c => c.ClientId == request.ClientId &&
                                                                    c.Name == request.Contact.Name);

            if (nameExists)
            {
                result.Result = Domain.Enums.CreateContactResult.Error_NameExists;
                return result;
                //throw new ArgumentException("Contact attached to Client: " + request.ClientId + " already exists with that name.");
            }

            Contact contact = new Contact
            {
                Name = request.Contact.Name,
                Title = request.Contact.Title,
                ClientId = request.ClientId,
                Email = request.Contact.Email,
                PhoneNumber = request.Contact.PhoneNumber,
                Active = true
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync(cancellationToken);

            result.Id = contact.Id;
            result.Result = Domain.Enums.CreateContactResult.Success;

            return result;
        }
    }
}
