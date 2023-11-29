namespace Networking.MQTT.Client
{
    using System;
    using System.IO;
    using UnityEngine;

    [Serializable]
    public class MQTTClientParameter
    {
        public static readonly string FILENAME = "mqtt.json";

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
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            string path = $"{Application.persistentDataPath}/{FILENAME}";
#else
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/{FILENAME}";
#endif

            if (!File.Exists(path))
            {
                MQTTClientParameter newParameter = new MQTTClientParameter("localhost", 1883);
                string newParameterJson = JsonUtility.ToJson(newParameter);
                File.WriteAllText(path, newParameterJson);
            }

            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<MQTTClientParameter>(json);
        }
    }
}
