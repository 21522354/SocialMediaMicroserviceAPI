using Microsoft.AspNetCore.Mvc;
using UserService.Common;
using UserService.DataLayer.DTOs;
using UserService.DataLayer.DTOs.Request;
using UserService.Service;

namespace UserService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] string? nickname,
            [FromQuery] string? email,
            [FromQuery] string? search)
        {
            if (!string.IsNullOrEmpty(nickname))
            {
                var userByNickname = await _userService.GetByNickName(nickname);
                return Ok(userByNickname);
            }

            if (!string.IsNullOrEmpty(email))
            {
                var userByEmail = await _userService.GetUserByEmail(email);
                return Ok(userByEmail);
            }

            if (!string.IsNullOrEmpty(search))
            {
                var searchedUsers = await _userService.SearchGlobalUser(search);
                return Ok(searchedUsers);
            }

            var res = await _userService.GetAllUsers();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var res = await _userService.GetById(id);
            return Ok(res);
        }

        [HttpPost("~/api/auth/sessions")]
        public async Task<IActionResult> Login([FromBody] SignUpAndSignInRequest request)
        {
            var res = await _userService.SignIn(request.Email, request.Password);
            return Ok(res);
        }


        [HttpPatch("{id}/nickname")]
        public async Task<IActionResult> SetNickName(int id, [FromBody] SetNickNameRequest request)
        {
            request.UserId = id;
            var res = await _userService.SetNickName(request);
            return Ok(res);
        }

        [HttpPatch("{id}/profile")]
        public async Task<IActionResult> SetNameAndAvatar(int id, [FromBody] SetNameAndAvatarRequest request)
        {
            request.UserId = id;
            var res = await _userService.SetNameAndAvatar(request);
            return Ok(res);
        }

        [HttpPatch("{id}/password")]
        public async Task<IActionResult> ChangePassword(int id, [FromBody] ChangePasswordRequest request)
        {
            request.UserId = id;
            var res = await _userService.ChangePassword(request);
            return Ok(res);
        }

        [HttpPost("registrations/email")]
        public async Task<IActionResult> SignUp([FromBody] SignUpAndSignInRequest request)
        {
            var res = await _userService.SignUp(request.Email, request.Password);
            return Ok(res);
        }

        [HttpPost("registrations/normal")]
        public async Task<IActionResult> SignUp2(SignUp2Request request)
        {
            var res = await _userService.SignUp2(request);
            return Ok(res);
        }

        [HttpPost("signIn/fb")]
        public async Task<IActionResult> SignInWithFb(string fbId)
        {
            var res = await _userService.SignInWithFB(fbId);
            return Ok(res);
        }
    }
}
