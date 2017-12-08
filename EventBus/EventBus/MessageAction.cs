namespace EventBus
{
    using System;

    internal class MessageAction<T> : MessageAction
    {
        private readonly Action<T> _action;
        public MessageAction(Action<T> action)
        {
            _action = action;
        }

        public Action<T> Action => _action;

        public override void Execute(object obj)
        {
            var message = (T)obj;
            _action?.Invoke(message);
        }
    }

    internal abstract class MessageAction : IExecute
    {
        public abstract void Execute(object parameter);
    }
}