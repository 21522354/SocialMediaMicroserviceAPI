using Newtonsoft.Json;
using RabbitMQ.Client;
using StoryService.Data_Layer.DTOs;
using System.Text;

namespace StoryService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            // Khởi tạo kết nối và channel
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Khai báo Exchange với kiểu Fanout
            _channel.ExchangeDeclare(exchange: "posts", type: ExchangeType.Fanout);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }

        public async Task PublishNewNotification(NotificationMessageDTO notificationReadDTO)
        {
            var json = JsonConvert.SerializeObject(notificationReadDTO);
            var body = Encoding.UTF8.GetBytes(json);

            // Publish message đến Fanout Exchange
            _channel.BasicPublish(
                exchange: "posts",
                routingKey: "", // Fanout không cần routingKey
                basicProperties: null,
                body: body);

            Console.WriteLine("--> Published new notification to RabbitMQ");
            await Task.CompletedTask;
        }
    }
}
