namespace EventBus
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;

    internal class BlockingQueue : IBlockingQueue
    {
        private readonly MessageAction _messageAction;
        private readonly BlockingCollection<Action> _actionQueue;
        private bool _isDisposing;

        public BlockingQueue(int workerCount, MessageAction messageAction)
        {
            _messageAction = messageAction;
            _actionQueue = new BlockingCollection<Action>();
            for (int i = 0; i < workerCount; i++)
            {
                Task.Factory.StartNew(StartConsuming, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            }
        }

        private void StartConsuming()
        {
            foreach (var action in _actionQueue.GetConsumingEnumerable())
            {
                if (!_isDisposing)
                {
                    action.Invoke();
                }
            }
        }

        public void Enqueue(IEvent message)
        {
            if (!_isDisposing)
            {
                _actionQueue.Add(() => _messageAction.Execute(message));
            }
        }

        public void Dispose()
        {
            if (!_isDisposing)
            {
                _isDisposing = true;
                _actionQueue.CompleteAdding();
            }
        }
    }
}