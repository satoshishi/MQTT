namespace Sample
{
    using System;
    using Networking.MQTT.Payload;
    using UnityEngine;

    [Serializable]
    public class PayloadSample : MQTTPayload
    {
        public static readonly string TOPIC = "Sample";

        [SerializeField]
        private string message;

        public PayloadSample(string message)
        {
            this.message = message;
        }

        public string Message => this.message;

        public override string Topic => TOPIC;
    }
}
