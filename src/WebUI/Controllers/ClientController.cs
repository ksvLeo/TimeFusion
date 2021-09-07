using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
using FusionIT.TimeFusion.Application.Clients.Commands.UpdateClient;
using FusionIT.TimeFusion.Application.Clients.Queries;
using FusionIT.TimeFusion.Clients.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.WebUI.Controllers
{
    public class ClientController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ClientDto>>> GetClientsByName([FromQuery] GetClientsByNameQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("client")]
        public async Task<ActionResult<ClientDto>> GetClient([FromQuery] GetClientQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateCustomer(CreateClientCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> UpdateCustomer(UpdateClientCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
