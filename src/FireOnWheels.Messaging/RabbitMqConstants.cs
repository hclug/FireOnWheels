using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireOnWheels.Messaging
{
    public static class RabbitMqConstants
    {
        public const string RabbitMqUri =
            "amqp://fire:fire@localhost:5672/";
        public const string JsonMimeType = "application/json";

        public const string RegisterOrderExchange =
            "fireonwheels.registerorder.exchange";
        public const string RegisterOrderQueue =
            "fireonwheels.registerorder.queue";

        public const string OrderRegisteredExchange =
            "fireonwheels.orderegistered.exchange";
        public const string OrderRegisteredNotificationQueue =
            "fireonwheels.orderregistered.notification.queue";
    }
}
