namespace EventBus
{
    using System;

    internal interface IBlockingQueue : IDisposable
    {
        void Enqueue(IEvent eventMessage);
    }
}