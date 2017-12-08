# EventBus
An internal pub/sub library for C#

## Usage

```C#
class Program
    {
        static void Main(string[] args)
        {
            IEventBus eventBus = new EventBus();

            eventBus.Subscribe<TestEvent>(EventReceived, 5);

            for (int i = 0; i < 5; i++)
            {
                eventBus.Publish(new TestEvent(i));
            }
            
            Console.WriteLine("Rest of program is carrying on while events get processed in parallel");
            Console.ReadLine();
        }

        private static void EventReceived(TestEvent evt)
        {
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Command {evt.Id} running {i}...");
                Thread.Sleep(100);
            }
        }
    }
```
Subscribe to a message type, passing the function that's going to process that message and also the number of threads that will be used to process incoming events.
