using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Clients.Commands.CreateClient
{
    public class CreateClientCommand : IRequest<int>
    {
        public ClientDto Client { get; set; }
    }

    public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateClientCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
             if (string.IsNullOrEmpty(request.Client.Name))
             {
                throw new ArgumentException("Name field can't be null.");
             }

            Client client = _context.Clients.FirstOrDefault(c => c.Name == request.Client.Name);

            if (client != null)
            {
                throw new ArgumentException("Client Name already exist.");
            }

            Currency currency = _context.Currencies.FirstOrDefault(c => c.Id == request.Client.Currency.Id);

            if (currency == null)
            {
                throw new ArgumentException("Currency not exist.");
            }

            List<Referrer> referrers = new List<Referrer>();

            Referrer referrer = new Referrer
            {
                Title = request.Client.Referrer[0].Email,
                Name = request.Client.Referrer[0].Name,
                Email = request.Client.Referrer[0].Email,
                PhoneNumber = request.Client.Referrer[0].PhoneNumber,
                Active = true
            };

            referrers.Add(referrer);
            
            Client entity = new Client
            {
                Name = request.Client.Name,
                Address = request.Client.Address,
                Currency = currency,
                Referrer = referrers,
                Active = true
            };

            _context.Clients.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
