namespace Networking.MQTT.Publishing
{
    using Cysharp.Threading.Tasks;
    using Networking.Core.Publishing;
    using Networking.MQTT.Payload;
    using UnityEngine;
    using VContainer;

    public class MQTTPublisher : INetworkPublisher
    {
        private IMQTTCommunicator communicator;

        [Inject]
        public MQTTPublisher(IMQTTCommunicator communicator)
        {
            this.communicator = communicator;
        }

        public async UniTask PublishAsync<T>(T message)
        {
            Debug.Assert(message is MQTTPayload);

            string topic = (message as MQTTPayload).Topic;
            string payload = JsonUtility.ToJson(message);

            await this.communicator.Publish(topic, payload);
        }
    }
}
