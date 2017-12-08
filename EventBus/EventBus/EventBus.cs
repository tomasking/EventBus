namespace EventBus
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class EventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, ISet<BlockingQueue>> _queues = new ConcurrentDictionary<Type, ISet<BlockingQueue>>();

        public void Subscribe<T>(Action<T> action, int numberOfThreads = 1) where T : IEvent
        {
            MessageAction messageAction = new MessageAction<T>(action);
            BlockingQueue blockingQueue = new BlockingQueue(numberOfThreads, messageAction);

            _queues.AddOrUpdate(typeof(T), new HashSet<BlockingQueue> {blockingQueue},
                (key, existingVal) =>
                {
                    existingVal.Add(blockingQueue);

                    return existingVal;
                });
        }

        public void Publish<T>(T message) where T : IEvent
        {
            var type = message.GetType();
            if (_queues.TryGetValue(type, out var blockingQueuesForType))
            {
                foreach (var blockingQueue in blockingQueuesForType)
                {
                    blockingQueue.Enqueue(message);
                }
            }
        }

        public void Dispose()
        {
            foreach (var queueSet in _queues)
            {
                foreach (var queue in queueSet.Value)
                {
                    queue.Dispose();
                }
            }
        }
    }
}