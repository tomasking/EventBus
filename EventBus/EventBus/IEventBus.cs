namespace EventBus
{
    using System;

    public interface IEventBus : IDisposable
    {
        void Subscribe<T>(Action<T> action, int numberOfThreads = 1) where T : IEvent;

        void Publish<T>(T message) where T : IEvent;
    }
}
