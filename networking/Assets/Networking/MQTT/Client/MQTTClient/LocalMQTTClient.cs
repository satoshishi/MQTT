namespace Networking.MQTT.Client
{
    using Cysharp.Threading.Tasks;
    using MessagePipe;

    public class LocalMQTTClient : IMQTTClient
    {
        private IPublisher<MQTTReceivedMessage> publisher;

        internal LocalMQTTClient(IPublisher<MQTTReceivedMessage> publisher)
        {
            this.publisher = publisher;
            this.Valid = true;
        }

        public bool Valid { get; }

        public async UniTask PublishMessage(string topic, string payload)
        {
            await UniTask.Yield();

            MQTTReceivedMessage message = new MQTTReceivedMessage(topic, payload);
            this.publisher.Publish(message);
        }

        public void Dispose()
        {
        }
    }
}
