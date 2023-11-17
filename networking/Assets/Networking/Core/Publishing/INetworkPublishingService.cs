namespace Networking.Core.Publishing
{
    using Cysharp.Threading.Tasks;

    public interface INetworkPublishingService
    {
        UniTask Publish<T>(T message);
    }
}
