using MapsterMapper;
using NotificationService.Common;
using NotificationService.DataLayer.DTOs;
using NotificationService.DataLayer.Models;
using NotificationService.DataLayer.Repository;
using NotificationService.SyncDataService;

namespace NotificationService.Service
{
    public interface INotificationService
    {
        Task<ResponseData<IEnumerable<Notification>>> GetAll();
        Task<ResponseData<List<NotificationRead2DTO>>> GetByUserId(int userId);
    }

    public class S_Notification : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUserDataClient _userDataClient;

        public S_Notification(
            INotificationRepository repo,
            IMapper mapper,
            IUserDataClient userDataClient)
        {
            _repo = repo;
            _mapper = mapper;
            _userDataClient = userDataClient;
        }

        public async Task<ResponseData<IEnumerable<Notification>>> GetAll()
        {
            var res = new ResponseData<IEnumerable<Notification>>();
            try
            {
                res.data = await _repo.GetAll();
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

        public async Task<ResponseData<List<NotificationRead2DTO>>> GetByUserId(int userId)
        {
            var res = new ResponseData<List<NotificationRead2DTO>>();
            try
            {
                var listNoti = await _repo.GetByUserId(userId);
                var listNotiReadDTO = new List<NotificationRead2DTO>();

                foreach (var item in listNoti)
                {
                    var newNoti = _mapper.Map<NotificationRead2DTO>(item);
                    var user = await ResolveUserAsync(item.UserInvoke);
                    newNoti.UserId = item.UserInvoke;
                    newNoti.Name = user.NickName;
                    newNoti.Avatar = user.Avatar;
                    newNoti.CreatedDate = item.CreatedDate;
                    listNotiReadDTO.Add(newNoti);
                }

                res.data = listNotiReadDTO.OrderByDescending(x => x.CreatedDate).ToList();
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

        private async Task<UserReadDTO> ResolveUserAsync(int userId)
        {
            try
            {
                return await _userDataClient.GetUserById(userId);
            }
            catch
            {
                return new UserReadDTO
                {
                    UserId = userId,
                    NickName = string.Empty,
                    FullName = string.Empty,
                    Avatar = string.Empty
                };
            }
        }
    }
}
