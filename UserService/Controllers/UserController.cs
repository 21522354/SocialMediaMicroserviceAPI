﻿using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.DataLayer;
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
        private readonly UserDBContext _context;

        public UserController(IUserRepository repo, IUserFollowRepository userFollowRepo, IMapper mapper, UserDBContext context)
        {
            _userRepo = repo;
            _mapper = mapper;
            _userFollowRepo = userFollowRepo;
            _context = context;
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
                return NotFound("Can't find this user");
            }
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpGet("nickName/{nickName}")]
        public async Task<IActionResult> GetUserByNickName(string nickName)
        {
            var user = await _userRepo.GetByNickName(nickName);
            if (user == null)
            {
                return NotFound("Can't find this user");
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
        [HttpGet("recommend/{userId}")]
        public async Task<IActionResult> GetRecommendFriend(Guid userId)
        {
            var listUserFollowing = await _userRepo.GetListUserFollowing(userId);
            var listUserRecommend = new List<UserReadDto>();
            int count = 0;
            foreach (var user in listUserFollowing)
            {
                var friendFollowing = await _userRepo.GetListUserFollowing(user.UserId);
                foreach (var friend in friendFollowing)
                {
                    if (!listUserFollowing.Contains(friend) && friend.UserId != userId)
                    {
                        listUserRecommend.Add(_mapper.Map<UserReadDto>(friend));
                        count++;
                    }
                    if (count == 5) break;
                }
            }
            if (count < 5)
            {
                var users = await _userRepo.GetAllUsers();
                users = users.Where(p => p.UserId != userId && !listUserFollowing.Contains(p)
                && !listUserRecommend.Select(u => u.UserId).Contains(p.UserId));

                foreach (var user in users)
                {
                    listUserRecommend.Add(_mapper.Map<UserReadDto>(user));
                    if (count == 5) break;
                }
            }
            return Ok(listUserRecommend);
        }
        [HttpGet("email={email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var user = await _userRepo.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpGet("friends/{userId}/{searchName}")]
        public async Task<IActionResult> GetFriendsForTag(string searchName, Guid userId)
        {
            var user = _userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("Can't find this user");
            }
            var listFriends = await _userRepo.GetListUserFollowing(userId);
            if (!String.IsNullOrEmpty(searchName))
            {
                listFriends = listFriends.Where(p => p.FullName.ToLower().Contains(searchName.ToLower())).ToList();
            }
            return Ok(listFriends);
        }
        [HttpGet("search/{searchNickName}")]
        public async Task<IActionResult> SearchGlobalUser(string searchNickName)
        {
            if (string.IsNullOrEmpty(searchNickName))
                return BadRequest("Search nickname cannot be empty.");

            var listUser = await _userRepo.GetRelateNickNameUser(searchNickName);
            return Ok(listUser);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignUpAndSignInRequest request)
        {
            var user = await _userRepo.SignIn(request.Email, request.Password);
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpPost("signUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpAndSignInRequest request)
        {
            var user = await _userRepo.SignUp(request.Email, request.Password);
            return Ok(_mapper.Map<UserReadDto>(user));
        }
        [HttpPost("setNickName")]
        public async Task<IActionResult> SetNickName([FromBody] SetNickNameRequest request)
        {
            var user = await _userRepo.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return NotFound("Can't find this user");
            }
            var checkNickName = await _userRepo.GetByNickName(request.NickName);
            if (checkNickName != null)
            {
                return BadRequest("This nickname is already exist!! Please choose other nickname");
            }
            user.NickName = request.NickName;
            await _userRepo.UpdateAsync(user);
            return Ok("Set nickname successfully");
        }
        [HttpPost("setNameAndAvatar")]
        public async Task<IActionResult> SetNameAndAvatar([FromBody] SetNameAndAvatarRequest request)
        {
            await _userRepo.SetNameAndAvatar(request.UserId, request.Name, request.Avatar);
            return Ok("Set name and avatar successfully");
        }
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            await _userRepo.ChangePassword(request.UserId, request.OldPassword, request.NewPassword);
            return Ok("Change password successfully");
        }
        [HttpPost("follow")]
        public async Task<IActionResult> FollowUser([FromBody] FollowUserRequest request)
        {
            var userFollow = await _userFollowRepo.GetBySelfIdAndFollowId(request.SelfId, request.UserFollowId);
            if (userFollow != null)
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
            if (userFollow == null)
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

        [HttpPost("SignUp2")]
        public async Task<IActionResult> SignUp2(SignUp2Request request)
        {
            var checkEmail = await _context.Users.FirstOrDefaultAsync(p => p.Email == request.Email);
            if (checkEmail != null)
            {
                return BadRequest("This email is already exist");
            }
            var checkNickName = await _context.Users.FirstOrDefaultAsync(p => p.NickName == request.NickName);
            if (checkNickName != null)
            {
                return BadRequest("This nickname is already exist");
            }
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Email = request.Email,
                NickName = request.NickName,
                FullName = request.FullName,
                Password = request.Password,
                Avatar = "https://img.icons8.com/?size=100&id=kDoeg22e5jUY&format=png&color=000000"
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(new { UserId = user.UserId });
        }
    }
}
