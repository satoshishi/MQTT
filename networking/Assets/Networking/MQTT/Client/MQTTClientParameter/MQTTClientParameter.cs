namespace Networking.MQTT.Client
{
    using System;
    using UnityEngine;

    [Serializable]
    public class MQTTClientParameter
    {
        [SerializeField]
        private MQTTBrokerIPAndPort iPAndPort;

        [SerializeField]
        private bool connectAuto;

        public MQTTClientParameter(string ip, int port)
        {
            this.iPAndPort = new MQTTBrokerIPAndPort(ip, port);
            this.connectAuto = true;
        }

        public MQTTClientParameter(GetIPAndPortFrom getIPAndPortFrom)
        {
            this.connectAuto = true;

            switch (getIPAndPortFrom)
            {
                case GetIPAndPortFrom.DesktopJson:
                    this.iPAndPort = IPAndPortFactoryFromLocalJson.Create();
                    break;

                case GetIPAndPortFrom.AdbCommand:
                    this.iPAndPort = IPAndPortFactoryFromAdbCommand.Create();
                    break;
                default:
                    break;
            }
        }

        public MQTTClientParameter()
        {
            this.connectAuto = false;
        }

        public enum GetIPAndPortFrom
        {
            DesktopJson, // デスクトップ上のJsonファイルから指定されたipを使用する.
            AdbCommand, // android adbコマンドから指定されたipを使用する.
        }

        public string Ip => this.iPAndPort.Ip;

        public int Port => this.iPAndPort.Port;

        public bool ConnectAuto => this.connectAuto;
    }
}
