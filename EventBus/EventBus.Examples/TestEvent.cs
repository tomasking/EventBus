namespace EventBus.Examples
{
    public class TestEvent : IEvent
    {
        public int Id { get; }

        public TestEvent(int id)
        {
            Id = id;
        }
    }
}