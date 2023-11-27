namespace Sample
{
    using Networking.MQTT.Client;
    using Networking.MQTT.Simulation;
    using UnityEngine;

    public class PayloadSampleSimulator : MQTTSimulator
    {
        [SerializeField]
        private PayloadSample payload;

        protected override MQTTReceivedMessage ToMessage()
        {
            return new MQTTReceivedMessage(
                this.payload.Topic,
                JsonUtility.ToJson(this.payload));
        }
    }
}
