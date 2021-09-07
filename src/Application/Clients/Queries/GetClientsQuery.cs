using AutoMapper;
using AutoMapper.QueryableExtensions;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Queries
{
    public class GetClientsQuery : IRequest<List<ClientDto>>
    {
        public class GetClientsQueryHandle : IRequestHandler<GetClientsQuery, List<ClientDto>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;

            public GetClientsQueryHandle(IApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<ClientDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
            {
                List<ClientDto> clients = await _context.Clients
                .ProjectTo<ClientDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

                return clients;
            }
        }
    }
}
