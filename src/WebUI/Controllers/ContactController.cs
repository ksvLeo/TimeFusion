using FusionIT.TimeFusion.Application.Contacts.Commands.UpdateContact;
using FusionIT.TimeFusion.Application.Contacts.Dtos;
using FusionIT.TimeFusion.Application.Contacts.Queries.GetClients;
using FusionIT.TimeFusion.Application.Contacts.Queries.GetContacts;
using FusionIT.TimeFusion.Application.Contacts.Queries.ValidateContactName;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.WebUI.Controllers
{
    public class ContactController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ContactDto>> Get([FromQuery] GetContactByIdQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<ContactDto>>> GetContactsByClient(int id)
        {
            return await Mediator.Send(new GetContactListByClientQuery { ClientId = id });
        }

        [HttpGet]
        public async Task<ActionResult<bool>> Get([FromQuery] ValidateContactNameQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Update([FromBody] UpdateContactCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
