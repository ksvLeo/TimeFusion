using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Contacts.Commands.CreateContact
{
    public class CreateContactCommandValidator : AbstractValidator<CreateContactCommand>
    {
        public CreateContactCommandValidator()
        {
            RuleFor(v => v.ClientId)
                .NotEmpty();

            RuleFor(c => c.Contact.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}
