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

namespace FusionIT.TimeFusion.Application.Clients.Queries
{
    public class GetClientsByNameQuery : IRequest<List<ClientDto>>
    {
        public string Name { get; set; }
    }

    public class GetCustomersByNameQueryHandle : IRequestHandler<GetClientsByNameQuery, List<ClientDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomersByNameQueryHandle(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ClientDto>> Handle(GetClientsByNameQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentException("Name field cant be null.");
            }

            List<ClientDto> customers = await _context.Clients
                .Where(c => c.Name.Contains(request.Name))
                .ProjectTo<ClientDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return customers;
        }
    }
}
