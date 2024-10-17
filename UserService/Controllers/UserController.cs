using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.DataLayer.DTOs;
using UserService.DataLayer.Models;
using UserService.DataLayer.Repository;

namespace UserService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(await _repo.GetAllUsers()));   
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();      
            }
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpGet("/followers/{id}")]
        public async Task<IActionResult> GetListUserFollower(Guid id)
        {
            var listUser = await _repo.GetListUserFollower(id);
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(listUser));
        }
        [HttpGet("/following/{id}")]
        public async Task<IActionResult> GetListUserFollowing(Guid id)
        {
            var listUser = await _repo.GetListUserFollowing(id);
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(listUser));
        }
        [HttpGet("email={email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _repo.GetUserByEmail(email);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] SignUpAndSignInRequest request)
        {
            var user = await _repo.SignIn(request.Email, request.Password);
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody]SignUpAndSignInRequest request)
        {
            var user = await _repo.SignUp(request.Email, request.Password);
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpPost("SetNameAndAvatar")]
        public async Task<IActionResult> SetNameAndAvatar([FromBody]SetNameAndAvatarRequest request)
        {
            await _repo.SetNameAndAvatar(request.UserId, request.Name, request.Avatar);
            return Ok("Set name and avatar successfully");
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequest request)
        {
            await _repo.ChangePassword(request.UserId, request.OldPassword, request.NewPassword);
            return Ok("Change password successfully");
        }
    }
}
