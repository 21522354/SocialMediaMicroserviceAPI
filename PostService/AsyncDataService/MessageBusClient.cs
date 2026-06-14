using Newtonsoft.Json;
using PostService.Data_Layer.DTOs;
using RabbitMQ.Client;
using System.Text;

namespace PostService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQHost"],
                Port = int.Parse(configuration["RabbitMQPort"] ?? "5672")
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("posts", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public Task PublishNewNotification(NotificationMessageDTO notificationReadDTO)
        {
            var json = JsonConvert.SerializeObject(notificationReadDTO);
            var body = Encoding.UTF8.GetBytes(json);

            _channel.BasicPublish(
                exchange: string.Empty,
                routingKey: "posts",
                body: body);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }
}
