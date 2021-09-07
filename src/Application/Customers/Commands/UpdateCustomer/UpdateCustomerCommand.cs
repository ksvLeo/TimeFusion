using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Customers.Dtos;
using FusionIT.TimeFusion.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public CustomerDto Customer { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            Customer customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Id == request.Customer.Id);

            if (customer == null)
            {
                /// Task to Esteban, send throw new Argument Exception or NotFound()?
                throw new ArgumentException($"Unable to find customer with ID #{request.Customer.Id}");
            }

            if (string.IsNullOrEmpty(request.Customer.Name))
            {
                throw new ArgumentException("Name field can't be null.");
            }

            Currency currency = await _context.Currencies
                .FirstOrDefaultAsync(c => c.Id == request.Customer.Currency.Id);

            if (currency == null)
            {
                throw new ArgumentException($"Unable to find currency with ID #{request.Customer.Currency.Id}");
            }


            Referrer referrer = await _context.Referrers
                .FirstOrDefaultAsync(c => c.Id == request.Customer.Referrer.Id);

            if (referrer == null)
            {
                throw new ArgumentException($"Unable to find referrer with ID #{request.Customer.Referrer.Id}");
            }

            // Map referrer update
            referrer.Name = request.Customer.Referrer.Name;
            referrer.PhoneNumber = request.Customer.Referrer.PhoneNumber;
            referrer.Email = request.Customer.Referrer.Email;

            // Map customer update
            customer.Name = request.Customer.Name;
            customer.Address = request.Customer.Address;
            customer.Currency = currency;
            customer.Referrer = referrer;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
