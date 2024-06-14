namespace Networking.MQTT.Publishing
{
    using Cysharp.Threading.Tasks;

    public interface IMQTTCommunicator
    {
        UniTask Publish(string topic, string message, int qos = 0);
    }
}
