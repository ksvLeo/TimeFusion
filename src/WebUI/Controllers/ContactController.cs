using FusionIT.TimeFusion.Application.Contacts.Commands.CreateContact;
using FusionIT.TimeFusion.Application.Contacts.Commands.UpdateContact;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.WebUI.Controllers
{
    public class ContactController : ApiControllerBase
    { 

        [HttpPost]
        public async Task<ActionResult<int>> CreateContact(CreateContactCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
