using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using UserService.Common;
using UserService.DataLayer;
using UserService.DataLayer.DTOs.Request;
using UserService.DataLayer.DTOs.Response;
using UserService.DataLayer.Models;

namespace UserService.Service
{
    public interface IUserService
    {
        Task<ResponseData<UserReadDto>> SignIn(string email, string password);
        Task<ResponseData<UserReadDto>> SignUp(string email, string password);
        Task<ResponseData<UserReadDto>> SignInWithFB(string fbId);
        Task<ResponseData<string>> SetNickName(SetNickNameRequest request);
        Task<ResponseData<string>> SetNameAndAvatar(SetNameAndAvatarRequest request);
        Task<ResponseData<string>> ChangePassword(ChangePasswordRequest request);
        Task<ResponseData<object>> SignUp2(SignUp2Request request);
        Task<ResponseData<IEnumerable<UserReadDto>>> GetAllUsers();
        Task<ResponseData<UserReadDto>> GetById(int userId);
        Task<ResponseData<UserReadDto>> GetByNickName(string nickName);
        Task<ResponseData<UserReadDto>> GetUserByEmail(string email);
        Task<ResponseData<IEnumerable<UserReadDto>>> SearchGlobalUser(string searchNickName);
    }

    public class S_User : IUserService
    {
        private readonly UserDBContext _context;
        private readonly IMapper _mapper;

        public S_User(UserDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseData<IEnumerable<UserReadDto>>> GetAllUsers()
        {
            var res = new ResponseData<IEnumerable<UserReadDto>>();
            try
            {
                var users = await _context.Users.ToListAsync();
                res.data = _mapper.Map<IEnumerable<UserReadDto>>(users);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<UserReadDto>> GetById(int userId)
        {
            var res = new ResponseData<UserReadDto>();
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this user";
                    return res;
                }

                res.data = _mapper.Map<UserReadDto>(user);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<UserReadDto>> GetByNickName(string nickName)
        {
            var res = new ResponseData<UserReadDto>();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.NickName == nickName);
                if (user == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this user";
                    return res;
                }

                res.data = _mapper.Map<UserReadDto>(user);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<UserReadDto>> GetUserByEmail(string email)
        {
            var res = new ResponseData<UserReadDto>();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Email.Trim() == email.Trim());
                if (user == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this user";
                    return res;
                }

                res.data = _mapper.Map<UserReadDto>(user);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<IEnumerable<UserReadDto>>> SearchGlobalUser(string searchNickName)
        {
            var res = new ResponseData<IEnumerable<UserReadDto>>();
            try
            {
                if (string.IsNullOrEmpty(searchNickName))
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "Search nickname cannot be empty.";
                    return res;
                }

                var users = await _context.Users
                    .Where(u => EF.Functions.Like(u.NickName, $"%{searchNickName}%"))
                    .ToListAsync();

                res.data = _mapper.Map<IEnumerable<UserReadDto>>(users);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<UserReadDto>> SignIn(string email, string password)
        {
            var res = new ResponseData<UserReadDto>();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);
                if (user == null)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "Wrong username or password";
                    return res;
                }

                res.data = _mapper.Map<UserReadDto>(user);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<UserReadDto>> SignUp(string email, string password)
        {
            var res = new ResponseData<UserReadDto>();
            try
            {
                var checkEmail = await _context.Users.AnyAsync(p => p.Email == email);
                if (checkEmail)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "This email is already exist";
                    return res;
                }

                var user = new User
                {
                    Email = email,
                    Password = password,
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                res.data = _mapper.Map<UserReadDto>(user);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<UserReadDto>> SignInWithFB(string fbId)
        {
            var res = new ResponseData<UserReadDto>();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.FbId == fbId);
                if (user == null)
                {
                    user = new User { FbId = fbId, Email = string.Empty, Password = string.Empty };
                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }

                res.data = _mapper.Map<UserReadDto>(user);
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<string>> SetNickName(SetNickNameRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var user = await _context.Users.FindAsync(request.UserId);
                if (user == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this user";
                    return res;
                }

                var checkNickName = await _context.Users.AnyAsync(p => p.UserId != request.UserId && p.NickName == request.NickName);
                if (checkNickName)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "This nickname is already exist!! Please choose other nickname";
                    return res;
                }

                user.NickName = request.NickName;
                await _context.SaveChangesAsync();
                res.data = "Set nickname successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<string>> SetNameAndAvatar(SetNameAndAvatarRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var user = await _context.Users.FindAsync(request.UserId);
                if (user == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "User not found";
                    return res;
                }

                user.FullName = request.Name;
                user.Avatar = request.Avatar;
                await _context.SaveChangesAsync();
                res.data = "Set name and avatar successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<string>> ChangePassword(ChangePasswordRequest request)
        {
            var res = new ResponseData<string>();
            try
            {
                var user = await _context.Users.FindAsync(request.UserId);
                if (user == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "User not found";
                    return res;
                }

                if (user.Password != request.OldPassword)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "Old password does not correct";
                    return res;
                }

                user.Password = request.NewPassword;
                await _context.SaveChangesAsync();
                res.data = "Change password successfully";
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

        public async Task<ResponseData<object>> SignUp2(SignUp2Request request)
        {
            var res = new ResponseData<object>();
            try
            {
                var checkEmail = await _context.Users.AnyAsync(p => p.Email == request.Email);
                if (checkEmail)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "This email is already exist";
                    return res;
                }

                var checkNickName = await _context.Users.AnyAsync(p => p.NickName == request.NickName);
                if (checkNickName)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "This nickname is already exist";
                    return res;
                }

                var user = new User
                {
                    Email = request.Email,
                    NickName = request.NickName,
                    FullName = request.FullName,
                    Password = request.Password,
                    Avatar = "https://img.icons8.com/?size=100&id=kDoeg22e5jUY&format=png&color=000000"
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                res.data = new { user.UserId };
                res.result = 1;
            }
            catch (Exception ex)
            {
                res.result = -1;
                res.error.code = 500;
                res.error.message = $"Exception: {ex.Message}\r\n{ex.InnerException?.Message}";
            }
            return res;
        }

    }
}
