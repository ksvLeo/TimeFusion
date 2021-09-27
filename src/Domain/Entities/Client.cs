﻿using FusionIT.TimeFusion.Domain.Common;
using FusionIT.TimeFusion.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Domain.Entities
{
    public class Client : AuditableEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public Currency Currency { get; set; }

        public List<Contact> ContactList { get; set; }

        public ClientStatus Status { get; set; }

        public IList<Project> Projects { get; set; }
    }
}