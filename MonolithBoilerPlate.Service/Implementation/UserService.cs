using AutoMapper;
using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Repository.Interface;
using MonolithBoilerPlate.Repository.UnitOfWork;
using MonolithBoilerPlate.Service.Base;
using MonolithBoilerPlate.Service.Interface;
using MonolithBoilerPlate.Entity.Dtos;
using MonolithBoilerPlate.Common;
using MonolithBoilerPlate.Service.Helper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using MonolithBoilerPlate.Entity.ViewModels;

namespace MonolithBoilerPlate.Service.Implementation
{
    public class UserService : BaseService<User>, IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtConfig _jwtConfig;
        private readonly EncryptionConfig _encryptionConfig;

        public UserService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<AppSettings> appSettings
            ) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtConfig = appSettings.Value.JwtConfig;
            _encryptionConfig = appSettings.Value.EncryptionConfig;
        }

        public async Task<bool> RegisterUserAsync(UserRegisterDto request)
        {
            request.UserName = request?.UserName?.ToUpper()?.Trim();

            if (await _unitOfWork.GetRepository<IUserRepository>().AnyAsync(u => u.UserName.ToUpper().Trim() == request.UserName))
            {
                throw new BadRequestException("Username has already been taken");
            }

            var user = new User
            {
                UserName = request.UserName,
                CompanyId = request.CompanyId,
                PasswordHash = PasswordHelper.HashPassword(_encryptionConfig.SecretKey, request.Password),
                IsActive = true
            };

            await _unitOfWork.GetRepository<IUserRepository>().AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<TokenVm> LoginAsync(LoginDto param)
        {
            if (param is null)
                throw new ArgumentNullException("Credential not provided!");

            param.UserName = param?.UserName?.ToUpper()?.Trim();
            var user = await _unitOfWork.GetRepository<IUserRepository>().FirstOrDefaultAsync(u => u.UserName == param.UserName);
            if (user == null || !PasswordHelper.VerifyPassword(_encryptionConfig.SecretKey, param.Password, user.PasswordHash))
            {
                throw new InternalServerException("Invalid username or password!");
            }

            TokenVm token = new TokenVm();
            token.AccessToken = CreateAccessToken(param);
            token.RefreshToken = CreateRefreshToken();
            await UpdateRefreshTokenInfoAsync(user.Id, token.RefreshToken);

            return token;
        }

        public async Task<string> ChangePassword(PasswordResetDto param)
        {
            var user = await _unitOfWork.GetRepository<IUserRepository>().FirstOrDefaultAsync(u => u.UserName == param.UserName);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.PasswordHash = PasswordHelper.HashPassword(_encryptionConfig.SecretKey, param.NewPassword);
            await _unitOfWork.GetRepository<IUserRepository>().UpdateAsync(user);
            await _unitOfWork.CompleteAsync();

            return "Password reset successfully!";
        }

        public string CreateAccessToken(LoginDto param)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, param.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var accessToken = new JwtSecurityToken(
                    issuer: _jwtConfig.Issuer,
                    audience: _jwtConfig.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(_jwtConfig.ExpirationInMinutes),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var token = new JwtSecurityTokenHandler().WriteToken(accessToken);
            return token;
        }

        public string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        public async Task<TokenVm> ValidateAndRenewTokens(TokenVm param)
        {
            if (param is null)
                throw new ArgumentNullException("Invalid client request!");

            var principal = ExtractPrincipalFromExpiredToken(param?.AccessToken);
            var userName = principal?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value?.ToUpper();
            var user = await _unitOfWork.GetRepository<IUserRepository>().FirstOrDefaultAsync(u => u.UserName == userName);

            if (user is null || user.RefreshToken != param.RefreshToken || user.RefreshTokenExpiryDate < DateTime.Now)
            {
                throw new InternalServerException("Cannot issue token!");
            }

            var token = await GenerateTokenAsync(user);

            return token;
        }

        public async Task<TokenVm> GenerateTokenAsync(User user)
        {
            TokenVm token = new TokenVm();

            if (string.IsNullOrEmpty(user.UserName))
                throw new BadRequestException("No Username specified!");

            var dto = new LoginDto() { UserName = user.UserName };
            token.AccessToken = CreateAccessToken(dto);
            token.RefreshToken = CreateRefreshToken();
            await UpdateRefreshTokenInfoAsync(user.Id, token.RefreshToken);

            return token;
        }

        private ClaimsPrincipal ExtractPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token!");

            return principal;
        }

        public async Task UpdateRefreshTokenInfoAsync(long userId, string refreshToken)
        {
            var user = await _unitOfWork.GetRepository<IUserRepository>().FirstOrDefaultAsync(u => u.Id == userId);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryDate = DateTime.Now.AddDays(_jwtConfig.RefreshTokenExpireInDay);

            await _unitOfWork.GetRepository<IUserRepository>().UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
        }

    }
}
