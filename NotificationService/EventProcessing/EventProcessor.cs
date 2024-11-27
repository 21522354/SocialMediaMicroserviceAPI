using MapsterMapper;
using Microsoft.AspNetCore.SignalR;
using NotificationService.DataLayer.DTOs;
using NotificationService.DataLayer.Models;
using NotificationService.DataLayer.Repository;
using NotificationService.Hubs;
using NotificationService.SyncDataService;
using System.Text.Json;

namespace NotificationService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper, IHubContext<NotificationHub> hubContext)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
            _hubContext = hubContext;
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
                case EventType.NewStory:
                    NewStoryEvent(message);
                    break;
                default:
                    break;
            }
        }

        private async void NewStoryEvent(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                var _userDataClient = scope.ServiceProvider.GetRequiredService<IUserDataClient>();

                var notificationMessageDTO = JsonSerializer.Deserialize<NotificationReadDTO>(message);

                var listUserFollower = await _userDataClient.GetUserFollower(notificationMessageDTO.UserInvoke);

                var listUserReceive = new List<Guid>();

                foreach (var item in listUserFollower)
                {
                    try
                    {
                        var noti = _mapper.Map<Notification>(notificationMessageDTO);
                        noti.Id = Guid.NewGuid();
                        noti.UserId = item.UserId;
                        noti.CreatedDate = DateTime.Now;
                        noti.IsAlreadySeen = false;
                        await _repo.AddNew(noti);

                        listUserReceive.Add(noti.UserId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                    }
                }

                var notificationMessage = _mapper.Map<NotificationMessageDTO>(notificationMessageDTO);
                notificationMessage.ListUserReceiveMessage = listUserReceive;
                notificationMessage.EventType = "NewStory";

                var notificationMessageJson = JsonSerializer.Serialize(notificationMessage);

                await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationMessageJson);

            }
        }

        private async void NewPostEvent(string message)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _repo = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                var _userDataClient = scope.ServiceProvider.GetRequiredService<IUserDataClient>();

                var notificationMessageDTO = JsonSerializer.Deserialize<NotificationReadDTO>(message);

                var listUserFollower = await _userDataClient.GetUserFollower(notificationMessageDTO.UserInvoke);

                var listUserReceive = new List<Guid>();

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

                        listUserReceive.Add(noti.UserId);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                    }
                }

                var notificationMessage = _mapper.Map<NotificationMessageDTO>(notificationMessageDTO);
                notificationMessage.ListUserReceiveMessage = listUserReceive;
                notificationMessage.EventType = "NewPost";

                var notificationMessageJson = JsonSerializer.Serialize(notificationMessage);

                await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationMessageJson);

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

                    var notificationMessage = _mapper.Map<NotificationMessageDTO>(noti);
                    List<Guid> listUserReceive = new List<Guid>() { noti.UserId };
                    notificationMessage.ListUserReceiveMessage = listUserReceive;
                    notificationMessage.EventType = "LikePost";
                    var notificationMessageJson = JsonSerializer.Serialize(notificationMessage);

                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationMessageJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                }
                Console.WriteLine(message);
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

                    var notificationMessage = _mapper.Map<NotificationMessageDTO>(noti);
                    List<Guid> listUserReceive = new List<Guid>() { noti.UserId };
                    notificationMessage.ListUserReceiveMessage = listUserReceive;
                    notificationMessage.EventType = "LikePost";
                    var notificationMessageJson = JsonSerializer.Serialize(notificationMessage);

                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationMessageJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                }
                Console.WriteLine(message);
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

                    var notificationMessage = _mapper.Map<NotificationMessageDTO>(noti);
                    List<Guid> listUserReceive = new List<Guid>() { noti.UserId };
                    notificationMessage.ListUserReceiveMessage = listUserReceive;
                    notificationMessage.EventType = "LikePost";
                    var notificationMessageJson = JsonSerializer.Serialize(notificationMessage);

                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", notificationMessageJson);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Notification to DB {ex.Message}");
                }
                Console.WriteLine(message);
            }
        }


        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<NotificationReadDTO>(notificationMessage);
            switch (eventType.EventType)
            {
                case "NewPost":
                    return EventType.NewPost;
                case "LikePost":
                    return EventType.LikePost;
                case "CommentPost":
                    return EventType.CommentPost;
                case "ReplyComment":
                    return EventType.ReplyComment;
                case "NewStory":
                    return EventType.NewStory;
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
        NewStory,
        Undetermined,
    }
}
