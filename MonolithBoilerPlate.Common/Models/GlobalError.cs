namespace MonolithBoilerPlate.Common.Models
{
    public class GlobalError
    {
        public string ErrorName { get; set; }
        public object Detail { get; set; }
        public object StackTrace { get; set; }
    }
}
