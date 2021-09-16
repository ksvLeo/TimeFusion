﻿using FusionIT.TimeFusion.Application.Clients.Commands.CreateClient;
using FusionIT.TimeFusion.Application.Clients.Commands.DeleteCustomer;
using FusionIT.TimeFusion.Application.Clients.Commands.UpdateClient;
using FusionIT.TimeFusion.Application.Clients.Commands.UpdateCustomer;
using FusionIT.TimeFusion.Application.Clients.Dtos;
using FusionIT.TimeFusion.Application.Clients.Queries;
using FusionIT.TimeFusion.Application.Common.Models;
using FusionIT.TimeFusion.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.WebUI.Controllers
{
    public class ClientController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PaginatedList<ClientDto>>> Get([FromQuery] GetClientsWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<ClientDto>>> GetClientsByName([FromQuery] GetClientsByNameQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<bool>> ValidateClientNameExistQuery([FromQuery] ValidateClientNameExistQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("client")]
        public async Task<ActionResult<ClientDto>> GetClient([FromQuery] GetClientQuery query)
            {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<CreateClientResultDto>> CreateClient(CreateClientCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<ReactivateClientResult>> ReactivateClient([FromQuery]ReactivateClientCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<UpdateClientResult>> UpdateClient(UpdateClientCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete]
        public async Task<ActionResult<DeleteClientResult>> DeleteClient([FromQuery]DeleteClientCommand command)
        {
            return await Mediator.Send(command);
        }

    }
}