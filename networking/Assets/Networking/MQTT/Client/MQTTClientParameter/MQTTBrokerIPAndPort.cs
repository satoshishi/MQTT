namespace Networking.MQTT.Client
{
    using UnityEngine;
    using System;

    [Serializable]
    public class MQTTBrokerIPAndPort
    {
        [SerializeField]
        private string ip;

        [SerializeField]
        private int port;

        public string Ip => this.ip;

        public int Port => this.port;

        internal MQTTBrokerIPAndPort(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
    }
}
