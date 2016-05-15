using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FireOnWheels.Notification.Service
{
    public class Program
    {
        static ManualResetEvent resetEvent = new ManualResetEvent(false);
        public static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, eArgs) => {
                resetEvent.Set();
                eArgs.Cancel = true;
            };

            using (var mgr = new NotificationRabbitMqManager()) {
                Console.WriteLine("Listening for order registered event...");
                mgr.ListenForOrderRegisteredEvent();
                resetEvent.WaitOne();
            }
        }
    }
}
