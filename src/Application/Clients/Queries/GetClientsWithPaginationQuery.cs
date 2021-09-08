using AutoMapper;
using AutoMapper.QueryableExtensions;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Common.Mappings;
using FusionIT.TimeFusion.Application.Common.Models;
using FusionIT.TimeFusion.Domain.Entities;
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
    public class GetClientsWithPaginationQuery : IRequest<PaginatedList<ClientDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public PaginationOrder Order { get; set; }
        public string OrderField { get; set; }
    }

    public class GetClientsWithPaginationQueryHandle : IRequestHandler<GetClientsWithPaginationQuery, PaginatedList<ClientDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetClientsWithPaginationQueryHandle(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ClientDto>> Handle(GetClientsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var propertyInfo = typeof(Client).GetProperty(request.OrderField);

            PaginatedList<ClientDto> clients = await _context.Clients
                .OrderBy(c => propertyInfo.GetValue(c, null))
                .ProjectTo<ClientDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return clients;
        }
    }

    public enum PaginationOrder
    {
        ASD = 1,
        DESC = 2
    }
}
