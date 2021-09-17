using FusionIT.TimeFusion.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Contacts.Queries.ValidateContactName
{
    public class ValidateContactNameQuery : IRequest<bool>
    {
        public int? ContactId { get; set; }

        public int ClientId { get; set; }
                                                                    
        public string ContactName { get; set; }
    }

    public class ValidateContactNameQueryHandler : IRequestHandler<ValidateContactNameQuery, bool>
    {
        private readonly IApplicationDbContext _context;

        public ValidateContactNameQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ValidateContactNameQuery request, CancellationToken cancellationToken)
        {
            bool nameExists;

            if (request.ContactId != null && request.ContactId > 0)
            {
            
                nameExists = await _context.Contacts.AnyAsync(c => c.ClientId == request.ClientId && c.Id != request.ContactId && c.Name == request.ContactName);
            }
            else
            {
                nameExists = await _context.Contacts.AnyAsync(c => c.ClientId == request.ClientId && c.Name == request.ContactName);
            }

            return nameExists;
        }
    }

}
