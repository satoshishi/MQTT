namespace Sample
{
    using MessagePipe;
    using Networking.MQTT.Client;
    using Networking.MQTT.Streaming;
    using VContainer;

    public class PayloadSampleStramer : MQTTStreamer<PayloadSample>
    {
        [Inject]
        public PayloadSampleStramer(ISubscriber<PayloadSample> subscriber, IPublisher<PayloadSample> publisher, ISubscriber<MQTTReceivedMessage> mqttMessage)
            : base(PayloadSample.TOPIC, subscriber, publisher, mqttMessage)
        {
        }
    }
}
