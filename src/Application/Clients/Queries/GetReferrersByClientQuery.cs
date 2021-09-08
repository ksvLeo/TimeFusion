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
    public class GetReferrersByClientQuery : IRequest<List<ReferrerDto>>
    {
        public int ClientId { get; set; }
    }

    public class GetReferrersByClientQueryHandler : IRequestHandler<GetReferrersByClientQuery, List<ReferrerDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetReferrersByClientQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ReferrerDto>> Handle(GetReferrersByClientQuery request, CancellationToken cancellationToken)
        {
            List<ReferrerDto> referrers = await _context.Referrers
                .Where(r => r.ClientId == request.ClientId)
                .ProjectTo<ReferrerDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return referrers;
        }
    }
}
