namespace Networking.MQTT.Client
{
    using Cysharp.Threading.Tasks;
    using MQTTnet.Client;

    /// <summary>
    /// MQTTからのメッセージを購読するリスナー
    /// </summary>
    public interface IMQTTMessageListener
    {
        UniTask SubscribeAsync(IMqttClient client);
    }
}
