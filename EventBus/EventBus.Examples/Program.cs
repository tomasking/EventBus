using System;

namespace EventBus.Examples
{
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            IEventBus eventBus = new EventBus();

            eventBus.Subscribe<TestEvent>(EventRecieved, 5);

            for (int i = 0; i < 5; i++)
            {
                eventBus.Publish(new TestEvent(i));
            }
            
            Console.WriteLine("Rest of program is carrying on while events get processed in parallel");
            Console.ReadLine();
        }

        private static void EventRecieved(TestEvent evt)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Command {evt.Id} running {i}...");
                Thread.Sleep(100);
            }
        }
    }
}
