using FluentValidation;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Contacts.Commands.UpdateContact
{
    public class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateContactCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(c => c.newContact.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MustAsync(BeUniqueName).WithMessage("The specified name already exists in other contact.");
        }

        public async Task<bool> BeUniqueName(UpdateContactCommand model, string name, CancellationToken cancellationToken)
        {
            return await _context.Contacts.Where(c => c.ClientId == model.ClientId && c.Id != model.newContact.Id)
                .AllAsync(c => c.Name != name);
        }
    }
}
