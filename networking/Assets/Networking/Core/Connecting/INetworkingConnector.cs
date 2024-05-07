namespace Networking.Core.Connecting
{
    using Cysharp.Threading.Tasks;

    public interface INetworkingConnector
    {
        UniTask<bool> Connecting(string ip, int port = 1883);
    }
}
