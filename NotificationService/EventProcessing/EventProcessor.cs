using MapsterMapper;
using NotificationService.DataLayer.DTOs;
using NotificationService.DataLayer.Models;
using NotificationService.DataLayer.Repository;
using NotificationService.SyncDataService;
using System.Text.Json;

namespace NotificationService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch (eventType)
            {
                case EventType.NewPost:
                    NewPostEvent(message);
                    break;
                case EventType.LikePost:
                    LikePostEvent(message);
                    break;
                case EventType.CommentPost:
                    CommentPostEvent(message);
                    break;
                case EventType.ReplyComment:
                    ReplyCommentEvent(message);
                    break;
                default:
                    break;
            }
        }

        private async void NewPostEvent(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                var _userDataClient = scope.ServiceProvider.GetRequiredService<IUserDataClient>();

                var notificationMessageDTO = JsonSerializer.Deserialize<NotificationReadDTO>(message);

                var listUserFollower = await _userDataClient.GetUserFollower(notificationMessageDTO.UserId);

                foreach ( var item in listUserFollower)
                {
                    try
                    {
                        var noti = _mapper.Map<Notification>(notificationMessageDTO);
                        noti.Id = Guid.NewGuid();
                        noti.UserId = item.UserId;
                        noti.CreatedDate = DateTime.Now;
                        noti.IsAlreadySeen = false;
                        await _repo.AddNew(noti);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                    }
                }
                
            }
        }
        private async void ReplyCommentEvent(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                var notificationMessageDTO = JsonSerializer.Deserialize<NotificationReadDTO>(message);

                try
                {
                    var noti = _mapper.Map<Notification>(notificationMessageDTO);
                    noti.Id = Guid.NewGuid();
                    noti.CreatedDate = DateTime.Now;
                    noti.IsAlreadySeen = false;
                    await _repo.AddNew(noti);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                }
            }
        }
        private async void CommentPostEvent(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                var notificationMessageDTO = JsonSerializer.Deserialize<NotificationReadDTO>(message);

                try
                {
                    var noti = _mapper.Map<Notification>(notificationMessageDTO);
                    noti.Id = Guid.NewGuid();
                    noti.CreatedDate = DateTime.Now;
                    noti.IsAlreadySeen = false;
                    await _repo.AddNew(noti);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                }
            }
        }
        private async void LikePostEvent(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();

                var notificationMessageDTO = JsonSerializer.Deserialize<NotificationReadDTO>(message);

                try
                {
                    var noti = _mapper.Map<Notification>(notificationMessageDTO);
                    noti.Id = Guid.NewGuid();
                    noti.CreatedDate = DateTime.Now;
                    noti.IsAlreadySeen = false;
                    await _repo.AddNew(noti);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                }
            }
        }


        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<NotificationReadDTO>(notificationMessage);
            switch (eventType.EventType)
            {
                case "NewPost":
                    Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.NewPost;
                default:
                    Console.WriteLine("--> Could not determine Event type");
                    return EventType.Undetermined;
            }
        }
    }
    enum EventType
    {
        NewPost,
        LikePost,
        CommentPost,
        ReplyComment,
        Undetermined,
    }
}
