using System.ComponentModel;

namespace MonolithBoilerPlate.Entity.Enums
{
    public enum ResourceUriType: short
    {
        PreviousPage = 1,
        NextPage = 2
    }

    public enum StatusType: short
    {
        Inactive = 0,
        Active = 1
    }

    public enum RateSelectionType : short
    {
        None = 0,
        [Description("Only Minimum Rate")]
        OnlyMinRate = 1,
        [Description("Both Min & Max Rate")]
        BothRate = 2,
    }

    public enum ProcessingStatus : short
    {
        [Description("Initial")]
        Initial = 0,
        [Description("Synchorizing with IRBM is in Progress")]
        InProgress = 1,
        [Description("Synchorization with AGBS completed")]
        Completed = 2,
        [Description("Synchorization failed")]
        Failed = 3
    }

    public enum DocumentFormatType : short
    {
        Normal= 0,
        LeftAlignment = 1,
        RightAlignement = 2,
    }

    public enum Company: short
    {
        None = 0,
    }

}
