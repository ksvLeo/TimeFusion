using AutoMapper;
using AutoMapper.QueryableExtensions;
using FusionIT.TimeFusion.Application.Common.Interfaces;
using FusionIT.TimeFusion.Application.Currencies.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.CurrencyReferences.Queries
{
    public class GetCurrencyReferencesQuery : IRequest<List<CurrencyDto>>
    {
    }

    public class GetCurrencyReferencesQueryHandler : IRequestHandler<GetCurrencyReferencesQuery, List<CurrencyDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCurrencyReferencesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CurrencyDto>> Handle(GetCurrencyReferencesQuery request, CancellationToken cancellationToken)
        {

            List<CurrencyDto> currencyReferences = await _context.Currencies
                    .ProjectTo<CurrencyDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

            return currencyReferences;
        }
    }
}
