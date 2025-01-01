namespace MonolithBoilerPlate.Common
{
    public class UnAuthorizedException : Exception
    {
        public UnAuthorizedException(string message) : base(string.Format(ApplicationExceptionMessage.UnAuthorized, message))
        {

        }
    }
}
