using FusionIT.TimeFusion.Application.Common.Interfaces;
using System;

namespace FusionIT.TimeFusion.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
