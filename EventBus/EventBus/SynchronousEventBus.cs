namespace EventBus
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class SynchronousEventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, ISet<MessageAction>> _actions = new ConcurrentDictionary<Type, ISet<MessageAction>>();
        
        public void Subscribe<T>(Action<T> action, int numberOfThreads = 1) where T : IEvent
        {
            MessageAction messageAction = new MessageAction<T>(action);
            _actions.AddOrUpdate(typeof(T), new HashSet<MessageAction> { messageAction },
                (key, existingVal) =>
                {
                    existingVal.Add(messageAction);
                    return existingVal;
                });
        }

        public void Publish<T>(T message) where T : IEvent
        {
            var type = message.GetType();
            if (_actions.TryGetValue(type, out var messageActionsForType))
            {
                foreach (var messageActionForType in messageActionsForType)
                {
                    messageActionForType.Execute(message);
                }
            }
        }

        public void Dispose()
        {
        }
    }
}