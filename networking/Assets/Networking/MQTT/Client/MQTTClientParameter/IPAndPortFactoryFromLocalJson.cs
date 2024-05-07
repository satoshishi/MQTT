namespace Networking.MQTT.Client
{
    using System;
    using System.IO;
    using UnityEngine;

    public class IPAndPortFactoryFromLocalJson
    {
        public static readonly string FILENAME = "mqtt.json";

        public static MQTTBrokerIPAndPort Create()
        {
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/{FILENAME}";

            if (File.Exists(path))
            {
                MQTTBrokerIPAndPort exitsParameter = JsonUtility.FromJson<MQTTBrokerIPAndPort>(File.ReadAllText(path));
                return exitsParameter;
            }
            else
            {
                MQTTBrokerIPAndPort newParameter = new MQTTBrokerIPAndPort("localhost", 1883);
                string json = JsonUtility.ToJson(newParameter);
                File.WriteAllText(path, json);

                return newParameter;
            }
        }
    }
}
