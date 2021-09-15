using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Domain.Entities;
using FusionIT.TimeFusion.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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

            bool activeProject = await _context.Projects.AnyAsync(c => c.ClientId == request.ClientId && c.ProjectStatus.Id.Equals(1));

            if (activeProject)
            {
                throw new ArgumentException("User has active projects with to this client.");
            }

            client.Status = ClientStatus.Inactive;

            await _context.SaveChangesAsync(cancellationToken);

            return request.ClientId;
        }
    }
}