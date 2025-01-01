using MonolithBoilerPlate.Entity.Enums;

namespace MonolithBoilerPlate.Infrastructure.Utility
{
    public interface ICurrentUserInfo
    {
        public long UserId { get; }
        public string UserName { get; }
        public bool IsAuthenticated { get; }
        public long CompanyId { get; }
        public string AccessToken { get; }
    }
}
