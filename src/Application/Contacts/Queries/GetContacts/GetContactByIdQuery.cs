using AutoMapper;
using AutoMapper.QueryableExtensions;
using FusionIT.TimeFusion.Application.Contacts.Dtos;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Contacts.Queries.GetContacts
{
    public class GetContactByIdQuery : IRequest<ContactDto>
    {
        public int ContactId { get; set; }
    }

    public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto>
    { 
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetContactByIdQueryHandler(IApplicationDbContext context, IMapper mapper) 
        {   
            _context = context;
            _mapper = mapper;
        }

        public async Task<ContactDto> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
        {
            ContactDto contact = await _context.Contacts
                .AsNoTracking()
                .ProjectTo<ContactDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == request.ContactId);

            if (contact == null)
            {
                throw new ArgumentException($"Unable to find contact with ID: #{ request.ContactId }.");
            }

            return contact;
        }
    }
}
