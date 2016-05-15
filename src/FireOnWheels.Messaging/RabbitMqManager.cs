using System;
using FireOnWheels.Messaging.Commands;
using FireOnWheels.Messaging.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace FireOnWheels.Messaging
{
    public class RabbitMqManager : IDisposable
    {
        protected readonly RabbitMQ.Client.IModel channel;

        public RabbitMqManager() {
            var connectionFactory =
                new ConnectionFactory { Uri = RabbitMqConstants.RabbitMqUri };
            var connection = connectionFactory.CreateConnection();
            channel = connection.CreateModel();
            channel.ConfirmSelect();
            channel.BasicAcks += HandleAcks;
            channel.BasicNacks += HandleNacks;
            connection.AutoClose = true;
        }

        public void SendRegisterOrderCommand(IRegisterOrderCommand command) {
            channel.ExchangeDeclare(
                exchange: RabbitMqConstants.RegisterOrderExchange,
                type: ExchangeType.Direct
                , durable: true);
            channel.QueueDeclare(
                queue: RabbitMqConstants.RegisterOrderQueue, durable: true,
                exclusive: false, autoDelete: false, arguments: null
                );
            channel.QueueBind(
                queue: RabbitMqConstants.RegisterOrderQueue,
                exchange: RabbitMqConstants.RegisterOrderExchange,
                routingKey: ""
                );
            var serializedCommand = JsonConvert.SerializeObject(command);
            var messageProperties = channel.CreateBasicProperties();
            messageProperties.ContentType = RabbitMqConstants.JsonMimeType;
            messageProperties.CorrelationId = command.CorrelationId.ToString("N");
            channel.BasicPublish(
                 exchange: RabbitMqConstants.RegisterOrderExchange,
                 routingKey: "",
                 basicProperties: messageProperties,
                 body: System.Text.Encoding.UTF8.GetBytes(serializedCommand)
                );
        }

        public void SendOrderRegisteredEvent(IOrderRegisteredEvent command) {
            channel.ExchangeDeclare(
            exchange: RabbitMqConstants.OrderRegisteredExchange,
            type: ExchangeType.Fanout
            , durable: true
        );

            channel.QueueDeclare(
                queue: RabbitMqConstants.OrderRegisteredNotificationQueue
                , durable: true,
                exclusive: false, autoDelete: false, arguments: null
                );
            channel.QueueBind(
                queue: RabbitMqConstants.OrderRegisteredNotificationQueue,
                exchange: RabbitMqConstants.OrderRegisteredExchange,
                routingKey: ""
                );
            var serializedCommand = JsonConvert.SerializeObject(command);
            var messageProperties = channel.CreateBasicProperties();
            messageProperties.ContentType = RabbitMqConstants.JsonMimeType;

            channel.BasicPublish(
                 exchange: RabbitMqConstants.OrderRegisteredExchange,
                 routingKey: "",
                 basicProperties: messageProperties,
                 body: System.Text.Encoding.UTF8.GetBytes(serializedCommand)
                );
        }

        public void SendAck(ulong deliveryTag) {
            channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
        }

        private void HandleNacks(object sender, RabbitMQ.Client.Events.BasicNackEventArgs e) {
           
        }

        private void HandleAcks(object sender, RabbitMQ.Client.Events.BasicAckEventArgs e) {
            
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    if (channel.IsOpen)
                        channel.Close();
                }
                disposedValue = true;
            }
        }

        void IDisposable.Dispose() {
            Dispose(true);
        }
        #endregion
    }
}
