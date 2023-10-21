namespace Networking.CoreSystem.Streaming
{
    using System;

    public interface INetworkStreamingService
    {
        void AddListener<T>(Action<T> listener);

        void RemoveListener<T>(Action<T> listener);
    }
}
