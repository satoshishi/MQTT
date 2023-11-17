namespace Networking.Core.Streaming
{
    /// <summary>
    /// データをStreamingするためのインスタンスを提供するFactory
    /// </summary>
    public interface INetworkStreamerFactory
    {
        INetworkStreamer<T> Create<T>();
    }
}
