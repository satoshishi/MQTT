namespace Networking.MQTT.Simulation
{
    using Cysharp.Threading.Tasks;
    using Networking.MQTT.Client;
    using Networking.Simulation;
    using UnityEngine;

    [RequireComponent(typeof(MQTTSimulationPublisher))]
    public abstract class MQTTSimulator : NetworkingSimulator
    {
        public MQTTSimulationPublisher publisher { get; private set; }

        protected override void Publish()
        {
            MQTTReceivedMessage message = this.ToMessage();

            this.publisher.Publish(message).Forget();
        }

        protected abstract MQTTReceivedMessage ToMessage();

        private void Awake()
        {
            this.publisher = this.GetComponent<MQTTSimulationPublisher>();
        }

        private void Update()
        {
            if (this.EveryFrame)
            {
                this.Publish();
            }
        }
    }
}
