using FluentValidation;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateClientCommandValidator(IApplicationDbContext context)
        {
            _context = context;

            RuleFor(c => c.Client.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
        }
        public async Task<bool> BeUniqueName(UpdateClientCommand model, string name, CancellationToken cancellationToken)
        {
            return await _context.Clients
                .Where(c => c.Id != model.Client.Id)
                .AllAsync(c => c.Name != name);
        }
    }
}