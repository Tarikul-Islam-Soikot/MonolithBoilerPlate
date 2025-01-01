namespace MonolithBoilerPlate.Common
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(string.Format(ApplicationExceptionMessage.NotFound, message))
        {

        }
    }
}
