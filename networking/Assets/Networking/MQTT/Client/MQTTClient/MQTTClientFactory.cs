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

        [Inject]
        public MQTTClientFactory(IPublisher<MQTTReceivedMessage> publisher, IEnumerable<IMQTTMessageListener> listeners)
        {
            this.publisher = publisher;
            this.listeners = listeners;
        }

        public async UniTask<IMQTTClient> CreateAsync(string ip, int port)
        {
#if LOCAL || TEST
            return new LocalMQTTClient(this.publisher);
#else
            return await MQTTClient.Connecting(ip, port, this.listeners, this.publisher);
#endif
        }
    }
}
