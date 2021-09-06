using FusionIT.TimeFusion.Application.Currencies.Dtos;
using FusionIT.TimeFusion.Application.CurrencyReferences.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.WebUI.Controllers
{
    public class CurrencyReferenceController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CurrencyDto>>> GetCurrencyReferences()
        {
            return await Mediator.Send(new GetCurrencyReferencesQuery());
        }
    }
}
