using FusionIT.TimeFusion.Application.Customers.Commands.CreateCustomer;
using FusionIT.TimeFusion.Application.Customers.Commands.DeleteCustomer;
using FusionIT.TimeFusion.Application.Customers.Commands.UpdateCustomer;
using FusionIT.TimeFusion.Application.Customers.Dtos;
using FusionIT.TimeFusion.Application.Customers.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.WebUI.Controllers
{
    public class CustomerController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<CustomerDto>>> GetCustomersByName([FromQuery] GetCustomersByNameQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("customer")]
        public async Task<ActionResult<CustomerDto>> GetCustomer([FromQuery] GetCustomerQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateCustomer(CreateCustomerCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete]
        public async Task<ActionResult<int>> DeleteCustomer(DeleteCustomerCommand command)
        {
            return await Mediator.Send(command);
        }


        [HttpPut]
        public async Task<ActionResult<bool>> UpdateCustomer(UpdateCustomerCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
