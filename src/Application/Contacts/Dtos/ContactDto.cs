﻿using FusionIT.TimeFusion.Application.Common.Mappings;
using FusionIT.TimeFusion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Application.Contacts.Dtos
{
    public class ContactDto : IMapFrom<Contact>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

        public int ClientId { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool Active { get; set; }
    }
}