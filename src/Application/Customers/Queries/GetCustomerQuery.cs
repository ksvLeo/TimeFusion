using AutoMapper;
using AutoMapper.QueryableExtensions;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Customers.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Customers.Queries
{
    public class GetCustomerQuery : IRequest<CustomerDto>
    {
        public int CustomerId { get; set; }
    }

    public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomerQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            CustomerDto customer = await _context.Customers
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == request.CustomerId);

            if (customer == null)
            {
                throw new ArgumentException($"Unable to find customer with #ID {request.CustomerId}");
            }

            return customer;
        }
    }
}
