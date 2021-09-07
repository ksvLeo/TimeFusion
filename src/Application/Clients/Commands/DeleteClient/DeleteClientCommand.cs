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
        public int ClientId { get; set; }
    }

    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteClientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            Client client = _context.Clients.FirstOrDefault(c => c.Id == request.ClientId);

            if (client == null)
            {
                throw new ArgumentException($"Unable to find client with ID #{ request.ClientId }.");
            }

            var userProjects = await _context.Projects
                                .Where(p => p.ClientId == request.ClientId)
                                .SingleOrDefaultAsync(cancellationToken);

            if (userProjects != null)
            {
                throw new ArgumentException("User has active projects with to this client.");
            }

            _context.Clients.First(c => c.Id == request.ClientId).Active = false;

            await _context.SaveChangesAsync(cancellationToken);

            return request.ClientId;
        }
    }
}