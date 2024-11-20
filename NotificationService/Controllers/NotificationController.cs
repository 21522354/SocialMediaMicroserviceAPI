using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.DataLayer.DTOs;
using NotificationService.DataLayer.Repository;
using NotificationService.SyncDataService;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private INotificationRepository _repo;
        private readonly IMapper _mapper;
        private readonly IUserDataClient _userDataClient;

        public NotificationController(INotificationRepository repo, IMapper mapper, IUserDataClient userDataClient)
        {
            _repo = repo;
            _mapper = mapper;
            _userDataClient = userDataClient;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _repo.GetAll());
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(Guid userId)
        {
            try
            {
                var listNoti = await _repo.GetByUserId(userId);
                var listNotiReadDTO = new List<NotificationRead2DTO>();
                foreach (var item in listNoti)
                {
                    var newNoti = _mapper.Map<NotificationRead2DTO>(item);
                    var user = await _userDataClient.GetUserById(item.UserInvoke);
                    newNoti.UserId = item.UserInvoke;
                    newNoti.Name = user.Name;
                    newNoti.Avatar = user.Image;
                    listNotiReadDTO.Add(newNoti);
                }

                return Ok(listNotiReadDTO);
            }
            catch
            {
                return BadRequest("Can't find this user");
            }
        }
    }
}
