using AutoMapper;
using AutoMapper.QueryableExtensions;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Projects.Dtos;
using FusionIT.TimeFusion.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteCustomerCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            Customer customer = _context.Customers.FirstOrDefault(c => c.Id == request.CustomerId);

            if (customer == null)
            {
                throw new ArgumentException($"Unable to find customer with ID #{ request.CustomerId }");
            }

            //IList<Project> ClientProjects = _context.Projects.SelectMany(p => p.ClientId == request.CustomerId);
            var userProjects = await _context.Projects
                                .Where(p => p.ClientId == request.CustomerId)
                                .SingleOrDefaultAsync(cancellationToken);

            if (userProjects != null)
            {
                throw new ArgumentException("User has active projects with to this client");
            }

            _context.Customers.First(c => c.Id == request.CustomerId).Active = false;

            await _context.SaveChangesAsync(cancellationToken);

            return request.CustomerId;
        }
    }
}