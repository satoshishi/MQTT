namespace Networking.PluginComponents.MQTT
{
    using Cysharp.Threading.Tasks;
    using MessagePipe;
    using MQTTnet.Client;

    /// <summary>
    /// MQTTからのメッセージを購読するリスナー
    /// </summary>
    public interface IMQTTMessageListener
    {
        UniTask SubscribeAsync(IMqttClient client, ISubscriber<MQTTReceivedMessage> subscriber);
    }
}
