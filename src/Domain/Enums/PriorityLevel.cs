namespace FusionIT.TimeFusion.Domain.Enums
{

    public enum PriorityLevel
    {
        None = 0,
        Low = 1,
        Medium = 2,
        High = 3
    }

    public enum DeleteClientResult
    {
        Error = 0,
        Success = 1,
        Error_NotFound = 2,
        Error_ActiveProjects = 3
    }

}
