using FusionIT.TimeFusion.Application.Contacts.Commands.CreateContact;
using FusionIT.TimeFusion.Application.Contacts.Commands.DeleteContact;
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
        public async Task<ActionResult<ContactDto>> GetContact([FromQuery] GetContactByIdQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<List<ContactDto>>> GetContactsByClient([FromQuery] int id)
        {
            return await Mediator.Send(new GetContactListByClientQuery { ClientId = id });
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<bool>> ValidateName([FromQuery] ValidateContactNameQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<int>> UpdateContact(UpdateContactCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateContact(CreateContactCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete]
        public async Task<ActionResult<int>> DeleteContact ([FromQuery] DeleteContactCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut]
        public async Task<ActionResult<int>> ReactivateClient ([FromQuery] ReactivateContactCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
