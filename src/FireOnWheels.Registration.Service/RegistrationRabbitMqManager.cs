using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireOnWheels.Messaging;

namespace FireOnWheels.Registration.Service
{
    public class RegistrationRabbitMqManager : FireOnWheels.Messaging.RabbitMqManager
    {
        public void ListenForRegisterOrderCommand() {
            channel.QueueDeclare(
                    queue: RabbitMqConstants.RegisterOrderQueue
                    , durable: true
                    , autoDelete: false
                    , arguments: null
                    , exclusive: false
                );

            channel.BasicQos(prefetchCount: 1, prefetchSize: 0, global: false);

            var consumer = new RegisteredOrderCommandConsumer(this);

            channel.BasicConsume(
                    queue: RabbitMqConstants.RegisterOrderQueue
                    , noAck: false
                    , consumer: consumer
                );
        }
    }
}
