namespace Networking.MQTT.Client
{
    using System.Collections.Generic;
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

        public async UniTask<IMQTTClient> Connecting(string ip, int port, IEnumerable<IMQTTMessageListener> listeners, IPublisher<MQTTReceivedMessage> publisher)
        {
            await UniTask.Yield();
            return this;
        }

        public async UniTask PublishMessage(string topic, string payload, int qos = 0)
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
