using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace MonolithBoilerPlate.Infrastructure.Utility
{
    public class CurrentUserInfo : ICurrentUserInfo
    {
        #region Properties
        public long UserId { get; private set; }
        public string UserName { get; private set; } = string.Empty;
        public bool IsAuthenticated { get; private set; }
        public long CompanyId { get; private set; }
        public string AccessToken { get; private set; } = string.Empty;
        public string RequestOrigin { get; private set; } = string.Empty;
        #endregion

        private readonly IHttpContextAccessor _httpContextAccessor;
        public readonly IApplicationDbContext _dbContext;

        public CurrentUserInfo(IHttpContextAccessor httpContextAccessor,
            IApplicationDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;

            Task.Run(() => InitializeAsync()).ConfigureAwait(false);
        }

        private async Task InitializeAsync()
        {
            if (_httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated ?? false)
            {
                var user = _httpContextAccessor.HttpContext.User;
                UserName = user?.FindFirstValue(ClaimTypes.NameIdentifier)?.Trim()?.ToUpper() ?? string.Empty;
                IsAuthenticated = user?.Identity?.IsAuthenticated ?? false;
                var dbUser = await _dbContext.Set<User>().AsNoTracking().FirstOrDefaultAsync(x => x.UserName == UserName);
                UserName = user?.FindFirstValue(ClaimTypes.NameIdentifier)?.Trim()?.ToUpper() ?? string.Empty;
                UserId = dbUser?.Id?? 0;
                CompanyId = dbUser?.CompanyId ?? 0;
            }

            var request = _httpContextAccessor?.HttpContext?.Request;
            AccessToken = request?.Headers?.Authorization.ToString() ?? string.Empty;
            RequestOrigin = request?.Headers?.Origin.ToString()?.Trim() ?? string.Empty;
            AccessToken = AccessToken?.Split(" ").ElementAtOrDefault(1) ?? string.Empty;
        }
    }
}
