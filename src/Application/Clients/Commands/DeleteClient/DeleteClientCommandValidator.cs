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

namespace FusionIT.TimeFusion.Application.Clients.Commands.DeleteClient
{
    public class DeleteClientCommandValidator : AbstractValidator<DeleteClientCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteClientCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(v => v.ClientId)
                .NotNull()
                .MustAsync(ClientIdShouldHaveMatch);
        }
        public async Task<bool> ClientIdShouldHaveMatch(int clientId, CancellationToken cancellationToken)
        {
            return await _context.Clients
                .AnyAsync(c => c.Id == clientId);
        }
    }
}
