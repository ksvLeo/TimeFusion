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
    public class GetClientByNameQuery : IRequest<ClientDto>
    {
        public string Name { get; set; }
    }

    public class GetClientByNameQueryHandler : IRequestHandler<GetClientByNameQuery, ClientDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetClientByNameQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(GetClientByNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentException("Name field can't be null.");
            }

            ClientDto client = await _context.Clients
                .ProjectTo<ClientDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Name == request.Name);

            return client;
        }
    }

    
}
