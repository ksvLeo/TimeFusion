using AutoMapper;
using AutoMapper.QueryableExtensions;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Clients.Dtos;
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
    public class GetCustomersQuery : IRequest<List<ClientDto>>
    {
        public class GetCustomersQueryHandle : IRequestHandler<GetCustomersQuery, List<ClientDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetCustomersQueryHandle(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<ClientDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
            {
                List<ClientDto> customers = await _context.Clients
                .ProjectTo<ClientDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

                return customers;
            }
        }
    }
}
