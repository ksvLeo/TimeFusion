using AutoMapper;
using AutoMapper.QueryableExtensions;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Contacts.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Contacts.Queries.GetClients
{
    public class GetContactListByClientQuery : IRequest<List<ContactDto>>
    {
        public int ClientId { get; set; }
    }

    public class GetContactListByClientQueryHandler : IRequestHandler<GetContactListByClientQuery, List<ContactDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContactListByClientQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ContactDto>> Handle(GetContactListByClientQuery request, CancellationToken cancellationToken)
        {
            List<ContactDto> contactList = await _context.Contacts
                .Where(c => c.ClientId == request.ClientId)
                .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return contactList;
        }

    }
}
