using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using IdentityService.Common;
using IdentityService.DataLayer;
using IdentityService.DataLayer.DTOs.Request;
using IdentityService.DataLayer.DTOs.Response;
using IdentityService.DataLayer.Models;

namespace IdentityService.Service
{
    public interface IIdentityService
    {
        Task<ResponseData<IdentityReadDto>> SignIn(string email, string password);
        Task<ResponseData<IdentityReadDto>> SignUp(string email, string password);
        Task<ResponseData<IdentityReadDto>> SignInWithFB(string fbId);
        Task<ResponseData<string>> SetNickName(SetNickNameRequest request);
        Task<ResponseData<string>> SetNameAndAvatar(SetNameAndAvatarRequest request);
        Task<ResponseData<string>> ChangePassword(ChangePasswordRequest request);
        Task<ResponseData<object>> SignUp2(SignUp2Request request);
        Task<ResponseData<IEnumerable<IdentityReadDto>>> GetAllIdentitys();
        Task<ResponseData<IdentityReadDto>> GetById(int IdentityId);
        Task<ResponseData<IdentityReadDto>> GetByNickName(string nickName);
        Task<ResponseData<IdentityReadDto>> GetIdentityByEmail(string email);
        Task<ResponseData<IEnumerable<IdentityReadDto>>> SearchGlobalIdentity(string searchNickName);
    }

    public class S_Identity : IIdentityService
    {
        private readonly IdentityDBContext _context;
        private readonly IMapper _mapper;

        public S_Identity(IdentityDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResponseData<IEnumerable<IdentityReadDto>>> GetAllIdentitys()
        {
            var res = new ResponseData<IEnumerable<IdentityReadDto>>();
            try
            {
                var Identitys = await _context.Identitys.ToListAsync();
                res.data = _mapper.Map<IEnumerable<IdentityReadDto>>(Identitys);
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

        public async Task<ResponseData<IdentityReadDto>> GetById(int IdentityId)
        {
            var res = new ResponseData<IdentityReadDto>();
            try
            {
                var Identity = await _context.Identitys.FindAsync(IdentityId);
                if (Identity == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this Identity";
                    return res;
                }

                res.data = _mapper.Map<IdentityReadDto>(Identity);
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

        public async Task<ResponseData<IdentityReadDto>> GetByNickName(string nickName)
        {
            var res = new ResponseData<IdentityReadDto>();
            try
            {
                var Identity = await _context.Identitys.FirstOrDefaultAsync(p => p.NickName == nickName);
                if (Identity == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this Identity";
                    return res;
                }

                res.data = _mapper.Map<IdentityReadDto>(Identity);
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

        public async Task<ResponseData<IdentityReadDto>> GetIdentityByEmail(string email)
        {
            var res = new ResponseData<IdentityReadDto>();
            try
            {
                var Identity = await _context.Identitys.FirstOrDefaultAsync(p => p.Email.Trim() == email.Trim());
                if (Identity == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this Identity";
                    return res;
                }

                res.data = _mapper.Map<IdentityReadDto>(Identity);
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

        public async Task<ResponseData<IEnumerable<IdentityReadDto>>> SearchGlobalIdentity(string searchNickName)
        {
            var res = new ResponseData<IEnumerable<IdentityReadDto>>();
            try
            {
                if (string.IsNullOrEmpty(searchNickName))
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "Search nickname cannot be empty.";
                    return res;
                }

                var Identitys = await _context.Identitys
                    .Where(u => EF.Functions.Like(u.NickName, $"%{searchNickName}%"))
                    .ToListAsync();

                res.data = _mapper.Map<IEnumerable<IdentityReadDto>>(Identitys);
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

        public async Task<ResponseData<IdentityReadDto>> SignIn(string email, string password)
        {
            var res = new ResponseData<IdentityReadDto>();
            try
            {
                var Identity = await _context.Identitys.FirstOrDefaultAsync(p => p.Email == email && p.Password == password);
                if (Identity == null)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "Wrong Identityname or password";
                    return res;
                }

                res.data = _mapper.Map<IdentityReadDto>(Identity);
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

        public async Task<ResponseData<IdentityReadDto>> SignUp(string email, string password)
        {
            var res = new ResponseData<IdentityReadDto>();
            try
            {
                var checkEmail = await _context.Identitys.AnyAsync(p => p.Email == email);
                if (checkEmail)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "This email is already exist";
                    return res;
                }

                var Identity = new Identity
                {
                    Email = email,
                    Password = password,
                };

                _context.Identitys.Add(Identity);
                await _context.SaveChangesAsync();
                res.data = _mapper.Map<IdentityReadDto>(Identity);
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

        public async Task<ResponseData<IdentityReadDto>> SignInWithFB(string fbId)
        {
            var res = new ResponseData<IdentityReadDto>();
            try
            {
                var Identity = await _context.Identitys.FirstOrDefaultAsync(p => p.FbId == fbId);
                if (Identity == null)
                {
                    Identity = new Identity { FbId = fbId, Email = string.Empty, Password = string.Empty };
                    _context.Identitys.Add(Identity);
                    await _context.SaveChangesAsync();
                }

                res.data = _mapper.Map<IdentityReadDto>(Identity);
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
                var Identity = await _context.Identitys.FindAsync(request.IdentityId);
                if (Identity == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Can't find this Identity";
                    return res;
                }

                var checkNickName = await _context.Identitys.AnyAsync(p => p.IdentityId != request.IdentityId && p.NickName == request.NickName);
                if (checkNickName)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "This nickname is already exist!! Please choose other nickname";
                    return res;
                }

                Identity.NickName = request.NickName;
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
                var Identity = await _context.Identitys.FindAsync(request.IdentityId);
                if (Identity == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Identity not found";
                    return res;
                }

                Identity.FullName = request.Name;
                Identity.Avatar = request.Avatar;
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
                var Identity = await _context.Identitys.FindAsync(request.IdentityId);
                if (Identity == null)
                {
                    res.result = 0;
                    res.error.code = 404;
                    res.error.message = "Identity not found";
                    return res;
                }

                if (Identity.Password != request.OldPassword)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "Old password does not correct";
                    return res;
                }

                Identity.Password = request.NewPassword;
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
                var checkEmail = await _context.Identitys.AnyAsync(p => p.Email == request.Email);
                if (checkEmail)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "This email is already exist";
                    return res;
                }

                var checkNickName = await _context.Identitys.AnyAsync(p => p.NickName == request.NickName);
                if (checkNickName)
                {
                    res.result = 0;
                    res.error.code = 400;
                    res.error.message = "This nickname is already exist";
                    return res;
                }

                var Identity = new Identity
                {
                    Email = request.Email,
                    NickName = request.NickName,
                    FullName = request.FullName,
                    Password = request.Password,
                    Avatar = "https://img.icons8.com/?size=100&id=kDoeg22e5jUY&format=png&color=000000"
                };

                _context.Identitys.Add(Identity);
                await _context.SaveChangesAsync();
                res.data = new { Identity.IdentityId };
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
