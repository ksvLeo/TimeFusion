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
    public class ValidateClientNameExistQuery : IRequest<bool>
    {
        public string Name { get; set; }
    }

    public class ValidateClientNameExistQueryHandler : IRequestHandler<ValidateClientNameExistQuery, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ValidateClientNameExistQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(ValidateClientNameExistQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentException("Name field can't be null.");
            }

            bool clientByNameExist = await _context.Clients.AnyAsync(c => c.Name == request.Name);

            return clientByNameExist;
        }
    }

    
}
