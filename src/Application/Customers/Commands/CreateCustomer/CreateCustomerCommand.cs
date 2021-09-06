using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Customers.Dtos;
using FusionIT.TimeFusion.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<int>
    {
        public CustomerDto Customer { get; set; }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Customer.Name))
            {
                throw new ArgumentException("Name field can't be null.");
            }

            Customer customer = _context.Customers.FirstOrDefault(c => c.Name == request.Customer.Name);

            if (customer != null)
            {
                throw new ArgumentException("Customer Name already exist.");
            }

            Currency currency = _context.Currencies.FirstOrDefault(c => c.Id == request.Customer.Currency.Id);

            if (currency == null)
            {
                throw new ArgumentException("Currency not exist.");
            }

            Referrer referrer;
            referrer = _context.Referrers.FirstOrDefault(r => r.Email == request.Customer.Referrer.Email);

            if (referrer == null)
            {
                referrer = new Referrer
                {
                    Name = request.Customer.Referrer.Name,
                    Email = request.Customer.Referrer.Email,
                    PhoneNumber = request.Customer.Referrer.PhoneNumber
                };

                _context.Referrers.Add(referrer);
                await _context.SaveChangesAsync(cancellationToken);
            }

            Customer entity = new Customer
            {
                Name = request.Customer.Name,
                Address = request.Customer.Address,
                Currency = currency,
                Referrer = referrer,
                Active = true
            };

            _context.Customers.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
