namespace Sample
{
    using MessagePipe;
    using Networking.Core.Connecting;
    using Networking.Core.Publishing;
    using Networking.Core.Streaming;
    using Networking.MQTT.Client;
    using Networking.MQTT.Publishing;
    using VContainer;
    using VContainer.Unity;

    public class MQTTNetworkingLifeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            MessagePipeOptions options = builder.RegisterMessagePipe();

            this.RegistrationClients(builder);

            this.RegistrationStreamer(builder, options);

            this.RegistrationPublisher(builder);
        }

        private void RegistrationClients(IContainerBuilder builder)
        {
            builder.Register<MQTTClientFactory>(Lifetime.Singleton);
            builder.Register<MQTTClientCommunicator>(Lifetime.Singleton).As<IMQTTCommunicator>().As<INetworkingConnector>();

            MQTTClientParameter parameter = new MQTTClientParameter(MQTTClientParameter.GetIPAndPortFrom.DesktopJson);
            builder.RegisterInstance(parameter);
        }

        private void RegistrationStreamer(IContainerBuilder builder, MessagePipeOptions options)
        {
            builder.RegisterMessageBroker<MQTTReceivedMessage>(options);

            builder.Register<PayloadSampleStramer>(Lifetime.Singleton).As<INetworkStreamer<PayloadSample>, IMQTTMessageListener>();
            builder.RegisterMessageBroker<PayloadSample>(options);
        }

        private void RegistrationPublisher(IContainerBuilder builder)
        {
            builder.Register<MQTTPublisher>(Lifetime.Singleton).As<INetworkPublisher>();
        }
    }
}
