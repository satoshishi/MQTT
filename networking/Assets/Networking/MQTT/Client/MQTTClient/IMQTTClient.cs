namespace Networking.MQTT.Client
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using MessagePipe;

    public interface IMQTTClient : IDisposable
    {
        public bool Valid { get; }

        public UniTask PublishMessage(string topic, string payload);

        public UniTask<IMQTTClient> Connecting(string ip, int port, IEnumerable<IMQTTMessageListener> listeners, IPublisher<MQTTReceivedMessage> publisher);
    }
}
