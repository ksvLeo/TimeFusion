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

    public enum ReactivateClientResult
    {
        Error = 0,
        Success = 1,
        Error_NotFound = 2,
        Error_AlreadyActive = 3
    }

    public enum UpdateClientResult
    {
        Error = 0,
        Success = 1,
        Error_NameExists = 2,
        EmptyName = 3
    }
    
    public enum CreateContactResult
    {
        Error = 0,
        Success = 1,
        Error_NameExists = 2,
        EmptyName = 3
    }

    public enum DeleteContactResult
    {
        Error = 0,
        Success = 1,
        Error_NotFound = 2
    }
    public enum ReactivateContactResult
    {
        Error = 0,
        Success = 1,
        Error_NotFound = 2
    }

    public enum UpdateContactResult
    {
        Error = 0,
        Success = 1,
        Error_NameExists = 2,
        EmptyName = 3
    }
    
}
