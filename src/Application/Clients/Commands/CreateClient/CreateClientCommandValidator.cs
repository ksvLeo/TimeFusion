﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Commands.CreateClient
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientCommandValidator()
        {
            RuleFor(v => v.NewClient.Name)
                .NotEmpty().WithMessage("Name input cannot be empty or null.");

            RuleFor(v => v.NewClient)
                .NotNull();
        }
    }
}
