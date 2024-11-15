using NotificationService.EventProcessing;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace NotificationService.AsyncDataService
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IEventProcessor _eventProcessor;
        private IConnection _connection;
        private IModel _chanel;
        private string _queueName;

        public MessageBusSubscriber(IEventProcessor eventProcessor)
        {
            _eventProcessor = eventProcessor;

            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5673
            };
            _connection = factory.CreateConnection();
            _chanel = _connection.CreateModel();
            _chanel.ExchangeDeclare(exchange: "posts", type: ExchangeType.Fanout);
            _queueName = _chanel.QueueDeclare().QueueName;
            _chanel.QueueBind(queue: _queueName, exchange: "posts", routingKey: "");
            Console.WriteLine("--> Listening on the message bus");

            _connection.ConnectionShutdown += _connection_ConnectionShutdown;
        }
        private void _connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection Shutdown");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_chanel);

            consumer.Received += (ModuleHandle, ea) =>
            {
                Console.WriteLine("--> Event Received");
                var body = ea.Body;
                var notificationMessage = Encoding.UTF8.GetString(body.ToArray());

                _eventProcessor.ProcessEvent(notificationMessage);
            };
            _chanel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
