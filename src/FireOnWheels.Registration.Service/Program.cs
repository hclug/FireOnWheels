using System;
using System.Threading;

namespace FireOnWheels.Registration.Service
{
    public class Program
    {
        static ManualResetEvent resetEvent = new ManualResetEvent(false);
        public static void Main(string[] args) {
            Console.CancelKeyPress += (sender, eArgs) => {
                resetEvent.Set();
                eArgs.Cancel = true;
            };

            using (var mgr = new RegistrationRabbitMqManager()) {
                Console.WriteLine("Listening for register order command...");
                mgr.ListenForRegisterOrderCommand();
                resetEvent.WaitOne();
            }
        }
    }
}
