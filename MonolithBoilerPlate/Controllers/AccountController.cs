using MonolithBoilerPlate.Entity.Dtos;
using MonolithBoilerPlate.Entity.ViewModels;
using MonolithBoilerPlate.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MonolithBoilerPlate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto param)
        {
            return Ok(await _userService.LoginAsync(param));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenVm param)
        {
            return Ok(await _userService.ValidateAndRenewTokens(param));
        }

        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> RegisterUser(UserRegisterDto param)
        //{
        //    return Ok(await _userService.RegisterUserAsync(param));
        //}

        //[HttpPost]
        //[Route("change")]
        //public async Task<IActionResult> ChangePassword(PasswordResetDto param)
        //{
        //    return Ok(await _userService.ChangePassword(param));
        //}


    }
}
