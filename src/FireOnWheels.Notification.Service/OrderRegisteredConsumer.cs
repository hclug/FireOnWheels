using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireOnWheels.Messaging.Events;

namespace FireOnWheels.Notification.Service
{
    public class OrderRegisteredConsumer
    {
        public void Consume(IOrderRegisteredEvent registeredEvent) {
            Console.WriteLine($"Customer notification sent: Order Id {registeredEvent.OrderId} registered.");
        }
    }
}
