using AutoMapper;
using AutoMapper.QueryableExtensions;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Common.Mappings;
using FusionIT.TimeFusion.Application.Common.Models;
using MediatR;
using System;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Queries
{
    public class GetClientsWithPaginationQuery : IRequest<PaginatedList<ClientDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public PaginationOrder Order { get; set; } = PaginationOrder.ASC;
        public string OrderField { get; set; } = "Name";
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
            var property = typeof(ClientDto).GetProperty(request.OrderField, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                throw new ArgumentException("Order field not found");

            PaginatedList<ClientDto> clients = await _context.Clients
                .OrderBy(request.OrderField + " " + request.Order)
                .ProjectTo<ClientDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);

            return clients;
        }
    }

    public enum PaginationOrder
    {
        ASC = 1,
        DESC = 2
    }
}
