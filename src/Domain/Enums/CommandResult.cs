using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FusionIT.TimeFusion.Domain.Enums
{

    public enum CreateClientResult
    {
        Error = 0,
        Success = 1,
        Error_NameExists = 2
    }

    public enum DeleteClientResult
    {
        Error = 0,
        Success = 1,
        Error_NotFound = 2,
        Error_ActiveProjects = 3
    }
    
    public enum CreateContactResult
    {
        Error = 0,
        Success = 1,
        Error_NameExists = 3
    }
    
}
