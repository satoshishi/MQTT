namespace Networking.MQTT.Client
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using MessagePipe;
    using VContainer;

    public class MQTTClientFactory
    {
        private IPublisher<MQTTReceivedMessage> publisher;

        private IEnumerable<IMQTTMessageListener> listeners;

        private IMQTTClient client;

        [Inject]
        public MQTTClientFactory(IPublisher<MQTTReceivedMessage> publisher, IEnumerable<IMQTTMessageListener> listeners, IMQTTClient client)
        {
            this.publisher = publisher;
            this.listeners = listeners;
            this.client = client;
        }

        public async UniTask<IMQTTClient> CreateAsync(string ip, int port)
        {
#if LOCAL || TEST
            return new LocalMQTTClient(this.publisher);
#else
            return await this.client.Connecting(ip, port, this.listeners, this.publisher);
#endif
        }
    }
}
