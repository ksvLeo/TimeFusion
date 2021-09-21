using FluentValidation;
using FusionIT.TimeFusion.Application.Clients.Commands.DeleteCustomer;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Contacts.Commands.DeleteContact
{
    class DeleteContactCommandValidator : AbstractValidator<DeleteClientCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteContactCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.ClientId)
                .NotNull()
                .MustAsync(ContactIdShouldHaveMatch);
        }

        public async Task<bool> ContactIdShouldHaveMatch(int clientId, CancellationToken cancellationToken)
        {
            return await _context.Clients
                .AnyAsync(c => c.Id == clientId);
        }
    }
}
