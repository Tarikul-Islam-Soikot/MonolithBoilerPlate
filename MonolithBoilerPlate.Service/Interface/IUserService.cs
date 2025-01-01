using MonolithBoilerPlate.Entity.Dtos;
using MonolithBoilerPlate.Entity.Entities;
using MonolithBoilerPlate.Entity.ViewModels;
using MonolithBoilerPlate.Service.Base;

namespace MonolithBoilerPlate.Service.Interface
{
    public interface IUserService : IBaseService<User>
    {
        Task<bool> RegisterUserAsync(UserRegisterDto request);
        Task<TokenVm> LoginAsync(LoginDto param);
        Task<string> ChangePassword(PasswordResetDto param);
        Task<TokenVm> ValidateAndRenewTokens(TokenVm param);
    }
}
