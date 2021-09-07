using AutoMapper;
using AutoMapper.QueryableExtensions;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Projects.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Projects.Queries
{
    public class GetUserActiveProjectsQuery : IRequest<List<ProjectDto>>
    {
        public string Name { get; set; }
    }

    public class GetCustomersByNameQueryHandle : IRequestHandler<GetUserActiveProjectsQuery, List<ProjectDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomersByNameQueryHandle(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProjectDto>> Handle(GetUserActiveProjectsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ArgumentException("Name field cant be null.");
            }

            List<ProjectDto> customers = await _context.Customers
                .Where(c => c.Name.Contains(request.Name))
                .ProjectTo<ProjectDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return customers;
        }
    }
}
