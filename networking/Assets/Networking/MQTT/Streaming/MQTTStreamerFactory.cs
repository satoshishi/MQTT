namespace Networking.MQTT.Streaming
{
    using Networking.Core.Streaming;
    using VContainer;

    public class MQTTStreamerFactory : INetworkStreamerFactory
    {
        private IObjectResolver resolver;

        [Inject]
        public MQTTStreamerFactory(IObjectResolver resolver)
        {
            this.resolver = resolver;
        }

        public INetworkStreamer<T> Create<T>()
        {
            return this.resolver.Resolve<INetworkStreamer<T>>();
        }
    }
}
