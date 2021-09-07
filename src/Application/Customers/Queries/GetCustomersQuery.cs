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
    public class GetCustomersQuery : IRequest<List<CustomerDto>>
    {
        public class GetCustomersQueryHandle : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetCustomersQueryHandle(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
            {
                List<CustomerDto> customers = await _context.Customers
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

                return customers;
            }
        }
    }
}
