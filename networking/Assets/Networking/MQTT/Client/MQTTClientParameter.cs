namespace Networking.MQTT.Client
{
    using System;
    using UnityEngine;

    [Serializable]
    public class MQTTClientParameter
    {
        [SerializeField]
        private string ip;

        [SerializeField]
        private int port;

        public MQTTClientParameter(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public string Ip => this.ip;

        public int Port => this.port;

        public static MQTTClientParameter Load()
        {
            return MQTTParameterFactory.Create();
        }
    }
}
