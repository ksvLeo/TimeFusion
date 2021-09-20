using FluentValidation;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Commands.ReactivateClient
{
    public class ReactivateClientCommandValidator : AbstractValidator<ReactivateClientCommand>
    {
        private readonly IApplicationDbContext _context;
        public ReactivateClientCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            RuleFor(v => v.ClientId)
                .NotEmpty()
                .MustAsync(ClientIdShouldHaveMatch);
        }

        public async Task<bool> ClientIdShouldHaveMatch(int clientId, CancellationToken cancellationToken)
        {
            return await _context.Clients
                .AnyAsync(c => c.Id == clientId);
        }
    }
}
