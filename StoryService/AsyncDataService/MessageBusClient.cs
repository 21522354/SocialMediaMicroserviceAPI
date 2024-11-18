﻿using Newtonsoft.Json;
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

        // Constructor: Thiết lập kết nối và channel khi khởi tạo
        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;

            // Khởi tạo kết nối và channel trong hàm khởi tạo
            var factory = new ConnectionFactory { HostName = "localhost", Port = 5673 };
            _connection = factory.CreateConnection();  // Tạo kết nối
            _channel = _connection.CreateModel();      // Tạo channel

            // Đảm bảo rằng queue đã được tạo sẵn
            _channel.QueueDeclare("posts", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }


        // Phương thức hủy kết nối (nên được gọi khi không còn sử dụng MessageBusClient)
        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }

        public async Task PublishNewNotification(NotificationMessageDTO notificationReadDTO)
        {
            var json = JsonConvert.SerializeObject(notificationReadDTO);
            var body = Encoding.UTF8.GetBytes(json);

            // Tái sử dụng channel đã tạo
            _channel.BasicPublish(
                exchange: "posts",
                routingKey: "",
                body: body);
            await Task.CompletedTask;
        }
    }
}
