using System;
using FireOnWheels.Messaging;
using FireOnWheels.Messaging.Commands;
using FireOnWheels.Messaging.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace FireOnWheels.Registration.Service
{
    internal class RegisteredOrderCommandConsumer : DefaultBasicConsumer
    {
        private RabbitMqManager rabbitMqManager;

        public RegisteredOrderCommandConsumer(RegistrationRabbitMqManager rabbitMqManager) {
            this.rabbitMqManager = rabbitMqManager;
        }

        public override void HandleBasicDeliver(string consumerTag
            , ulong deliveryTag, bool redelivered
            , string exchange, string routingKey
            , IBasicProperties properties, byte[] body) {

            if (properties.ContentType != RabbitMqConstants.JsonMimeType)
                throw new ArgumentException("The content type is unknown", "contentType");
            var message = System.Text.Encoding.UTF8.GetString(body);
            var commandObj =
                JsonConvert.DeserializeObject<RegisterOrderCommand>(
                    message);
            Consume(commandObj);
            rabbitMqManager.SendAck(deliveryTag);
        }

        static int orderId = 12;
        private void Consume(IRegisterOrderCommand command) {

            Console.WriteLine($"Successfully consumed order from {command.PickupName} to {command.DeliveryName}");

            var orderRegisteredEvent = new OrderRegisteredEvent(command, orderId++);
            rabbitMqManager.SendOrderRegisteredEvent(orderRegisteredEvent);
        }
    }
}