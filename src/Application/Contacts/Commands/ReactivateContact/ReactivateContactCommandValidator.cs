using FluentValidation;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Contacts.Commands.ReactivateContact;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Commands.ReactivateClient
{
    public class ReactivateContactCommandValidator : AbstractValidator<ReactivateContactCommand>
    {
        private readonly IApplicationDbContext _context;

        public ReactivateContactCommandValidator(IApplicationDbContext context)
        {
            _context = context;
            RuleFor(v => v.ContactId)
                .NotEmpty()
                .MustAsync(ContactIdShouldHaveMatch);

            RuleFor(v => v.ClientId)
                .NotNull();
        }

        public async Task<bool> ContactIdShouldHaveMatch(int contactId, CancellationToken cancellationToken)
        {
            return await _context.Contacts
                .AnyAsync(c => c.Id == contactId, cancellationToken);
        }
    }
}
