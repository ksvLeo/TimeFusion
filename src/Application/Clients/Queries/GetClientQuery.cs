﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Queries
{
    public class GetClientQuery : IRequest<ClientDto>
    {
        public int ClientId { get; set; }
    }

    public class GetClientQueryHandler : IRequestHandler<GetClientQuery, ClientDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetClientQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
                ClientDto client = await _context.Clients
                .AsNoTracking()
                .ProjectTo<ClientDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(c => c.Id == request.ClientId);

            if (client == null)
            {
                throw new ArgumentException($"Unable to find client with ID: #{request.ClientId}.");
            }

            return client;
        }
    }
}