namespace Networking.MQTT.Simulation
{
#if LOCAL
    using MessagePipe;
#else
    using Networking.MQTT.Publishing;
#endif

    using Cysharp.Threading.Tasks;
    using Networking.MQTT.Client;
    using UnityEngine;
    using VContainer;

    public class MQTTSimulationPublisher : MonoBehaviour
    {
#if LOCAL
        private IPublisher<MQTTReceivedMessage> publisher;

        public async UniTask Publish(MQTTReceivedMessage message)
        {
            await UniTask.Yield();
            this.publisher.Publish(message);
        }

        [Inject]
        internal void Inection(IPublisher<MQTTReceivedMessage> publisher)
        {
            this.publisher = publisher;
        }
#else
        private IMQTTCommunicator publisher;

        public async UniTask Publish(MQTTReceivedMessage message)
        {
            await this.publisher.Publish(message.Topic, message.Payload);
        }

        [Inject]
        internal void Inection(IMQTTCommunicator publisher)
        {
            this.publisher = publisher;
        }
#endif
    }
}
