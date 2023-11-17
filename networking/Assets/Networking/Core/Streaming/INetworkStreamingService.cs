namespace Networking.Core.Streaming
{
    using System;

    public interface INetworkStreamingService
    {
        IDisposable AddListener<T>(Action<T> listener);
    }
}
