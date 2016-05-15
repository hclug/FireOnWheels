using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireOnWheels.Messaging;
using FireOnWheels.Messaging.Events;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace FireOnWheels.Notification.Service
{
    public class NotificationRabbitMqManager : FireOnWheels.Messaging.RabbitMqManager
    {
        public void ListenForOrderRegisteredEvent() {
            channel.QueueDeclare(
                    queue: RabbitMqConstants.OrderRegisteredNotificationQueue
                    , durable: true
                    , autoDelete: false
                    , arguments: null
                    , exclusive: false
                );

            channel.BasicQos(prefetchCount: 1, prefetchSize: 0, global: false);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (chan,eventArgs) => {
                var contentType = eventArgs.BasicProperties.ContentType;
                if (contentType != RabbitMqConstants.JsonMimeType)
                    throw new ArgumentException("Cannot handle content type", "contentType");

                var message = System.Text.Encoding.UTF8.GetString(eventArgs.Body);
                var orderConsumer = new OrderRegisteredConsumer();
                var commandObj =
                JsonConvert.DeserializeObject<OrderRegisteredEvent>(message);
                orderConsumer.Consume(commandObj);
                SendAck(eventArgs.DeliveryTag);
            };

            channel.BasicConsume(
                    queue: RabbitMqConstants.OrderRegisteredNotificationQueue
                    ,noAck: false
                    ,consumer: consumer
                );
        }
    }
}
