namespace Networking.MQTT.Streaming
{
    using System;
    using Networking.Core.Streaming;
    using VContainer;

    public class MQTTStreamingService : INetworkStreamingService
    {
        private INetworkStreamerFactory factory;

        [Inject]
        public MQTTStreamingService(INetworkStreamerFactory factory)
        {
            this.factory = factory;
        }

        IDisposable INetworkStreamingService.AddListener<T>(Action<T> listener)
        {
            IDisposable disposable = this.factory.Create<T>().AddListener(listener);
            return disposable;
        }
    }
}
