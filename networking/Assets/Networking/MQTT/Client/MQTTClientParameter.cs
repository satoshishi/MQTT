namespace Networking.MQTT.Client
{
    using System;
    using System.IO;
    using UnityEngine;

    [Serializable]
    public class MQTTClientParameter
    {
        private static readonly string FILENAME = "mqtt.json";

        [SerializeField]
        private string ip;

        [SerializeField]
        private int port;

        public string Ip => this.ip;

        public int Port => this.port;

        public static MQTTClientParameter Load()
        {
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/{FILENAME}";
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<MQTTClientParameter>(json);
        }
    }
}
