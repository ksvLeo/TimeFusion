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
    public class GetCustomersByNameQuery : IRequest<List<CustomerDto>>
    {
        public string Name { get; set; }
    }

    public class GetCustomersByNameQueryHandle : IRequestHandler<GetCustomersByNameQuery, List<CustomerDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomersByNameQueryHandle(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CustomerDto>> Handle(GetCustomersByNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentException("Name field cant be null.");
            }

            List<CustomerDto> customers = await _context.Customers.Where(c => c.Name.Contains(request.Name)).ProjectTo<CustomerDto>(_mapper.ConfigurationProvider).ToListAsync();

            return customers;
        }
    }
}
