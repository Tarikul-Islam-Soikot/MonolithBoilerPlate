namespace MonolithBoilerPlate.Common
{
    public class DuplicateException : Exception
    {
        public DuplicateException(string message) : base(string.Format(ApplicationExceptionMessage.AlreadyExists, message))
        {

        }
    }
}
