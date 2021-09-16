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
    public class DeleteClientCommand : IRequest<DeleteClientResult>
    {
        public int ClientId { get; set; }
    }

    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, DeleteClientResult>
    {
        private readonly IApplicationDbContext _context;

        public DeleteClientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteClientResult> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {

            Client client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == request.ClientId, cancellationToken);

            if (client == null)
            {
                return DeleteClientResult.Error_NotFound; 
            }

            bool activeProject = await _context.Projects.AnyAsync(c => c.ClientId == request.ClientId && c.ProjectStatus.Id.Equals(1), cancellationToken);

            if (activeProject)
            {
                return DeleteClientResult.Error_ActiveProjects;
            }

            client.Status = ClientStatus.Inactive;

            await _context.SaveChangesAsync(cancellationToken);

            client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == client.Id, cancellationToken);

            if(client.Status.Equals(ClientStatus.Active))
            {
                return DeleteClientResult.Error;
            }

            return DeleteClientResult.Success;
        }
    }
}