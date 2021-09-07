using AutoMapper;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Commands.DeleteCustomer
{
    public class DeleteClientCommand : IRequest<int>
    {
        public int CustomerId { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteClientCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteCustomerCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            Client customer = _context.Clients.FirstOrDefault(c => c.Id == request.CustomerId);

            if (customer == null)
            {
                throw new ArgumentException($"Unable to find customer with ID #{ request.CustomerId }");
            }

            var userProjects = await _context.Projects
                                .Where(p => p.ClientId == request.CustomerId)
                                .SingleOrDefaultAsync(cancellationToken);

            if (userProjects != null)
            {
                throw new ArgumentException("User has active projects with to this client");
            }

            _context.Clients.First(c => c.Id == request.CustomerId).Active = false;

            await _context.SaveChangesAsync(cancellationToken);

            return request.CustomerId;
        }
    }
}