namespace Networking.MQTT.Client
{
    using System;
    using Cysharp.Threading.Tasks;

    public interface IMQTTClient : IDisposable
    {
        public bool Valid { get; }

        public UniTask PublishMessage(string topic, string payload);
    }
}
