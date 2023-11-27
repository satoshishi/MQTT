namespace Networking.Core.Publishing
{
    using Cysharp.Threading.Tasks;

    public interface INetworkPublisher
    {
        UniTask PublishAsync<T>(T message);
    }
}
