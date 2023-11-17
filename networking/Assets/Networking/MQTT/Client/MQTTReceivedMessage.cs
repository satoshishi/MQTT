namespace Networking.MQTT.Client
{
    using System;

    /// <summary>
    /// Brokerから受け取ったメッセージを表現する
    /// </summary>
    public readonly struct MQTTReceivedMessage : IEquatable<MQTTReceivedMessage>
    {
        public MQTTReceivedMessage(string topic, string payload)
        {
            this.Topic = topic;
            this.Payload = payload;
        }

        public string Topic { get; }

        public string Payload { get; }

        public bool Equals(MQTTReceivedMessage other)
        {
            return this.Topic == other.Topic && this.Payload == other.Payload;
        }

        public override bool Equals(object obj)
        {
            return obj is MQTTReceivedMessage other && this.Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Topic, this.Payload);
        }
    }
}
