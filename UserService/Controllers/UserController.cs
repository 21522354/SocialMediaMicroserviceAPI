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
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly IUserFollowRepository _userFollowRepo;

        public UserController(IUserRepository repo, IUserFollowRepository userFollowRepo, IMapper mapper)
        {
            _userRepo = repo;
            _mapper = mapper;
            _userFollowRepo = userFollowRepo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(await _userRepo.GetAllUsers()));   
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();      
            }
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpGet("followers/{id}")]
        public async Task<IActionResult> GetListUserFollower(Guid id)
        {
            var listUser = await _userRepo.GetListUserFollower(id);
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(listUser));
        }
        [HttpGet("following/{id}")]
        public async Task<IActionResult> GetListUserFollowing(Guid id)
        {
            var listUser = await _userRepo.GetListUserFollowing(id);
            return Ok(_mapper.Map<IEnumerable<UserReadDto>>(listUser));
        }
        [HttpGet("email={email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userRepo.GetUserByEmail(email);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignUpAndSignInRequest request)
        {
            var user = await _userRepo.SignIn(request.Email, request.Password);
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody]SignUpAndSignInRequest request)
        {
            var user = await _userRepo.SignUp(request.Email, request.Password);
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpPost("setNameAndAvatar")]
        public async Task<IActionResult> SetNameAndAvatar([FromBody]SetNameAndAvatarRequest request)
        {
            await _userRepo.SetNameAndAvatar(request.UserId, request.Name, request.Avatar);
            return Ok("Set name and avatar successfully");
        }
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequest request)
        {
            await _userRepo.ChangePassword(request.UserId, request.OldPassword, request.NewPassword);
            return Ok("Change password successfully");
        }
        [HttpPost("follow")]
        public async Task<IActionResult> FollowUser([FromBody] FollowUserRequest request)
        {
            var userFollow = await _userFollowRepo.GetBySelfIdAndFollowId(request.SelfId, request.UserFollowId);
            if(userFollow != null)
            {
                return Ok("You are already follow this user");
            }
            var newUserFollow = new UserFollow() { UserFromId = request.SelfId, UserToId = request.UserFollowId };
            await _userFollowRepo.AddAsync(newUserFollow);
            return Ok("Follow user successfully");
        }
        [HttpPost("unfollow")]
        public async Task<IActionResult> UnfollowUser([FromBody] FollowUserRequest request)
        {
            var userFollow = await _userFollowRepo.GetBySelfIdAndFollowId(request.SelfId, request.UserFollowId);
            if(userFollow == null)
            {
                return NotFound("You are not following this user");
            }
            await _userFollowRepo.DeleteAsync(userFollow.Id);
            return Ok("UnFollow user successfully");

        }
        [HttpGet("GetAllFollow")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userFollowRepo.GetAllAsync());
        }
        
    }
}
