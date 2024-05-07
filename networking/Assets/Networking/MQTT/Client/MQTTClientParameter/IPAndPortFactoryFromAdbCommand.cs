namespace Networking.MQTT.Client
{
    using System;
    using System.IO;
    using UnityEngine;

    public class IPAndPortFactoryFromAdbCommand
    {
        public static readonly string FILENAME = "mqtt.json";

        public static MQTTBrokerIPAndPort Create()
        {
            string path = $"{Application.persistentDataPath}/{FILENAME}";

            if (File.Exists(path))
            {
                MQTTBrokerIPAndPort exitsParameter = JsonUtility.FromJson<MQTTBrokerIPAndPort>(File.ReadAllText(path));
                return exitsParameter;
            }
            else
            {
                try
                {
                    AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject intent = currentActivity.Call<AndroidJavaObject>("getIntent");

                    int[] ips = intent.Call<int[]>("getIntArrayExtra", "ip") ?? new int[0];
                    string ip = string.Join('.', ips);

                    int port = intent.Call<int>("getIntExtra", "port", 9999);

                    if (string.IsNullOrEmpty(ip) || port == 9999)
                    {
                        return new MQTTBrokerIPAndPort("localhost", 1883);
                    }

                    MQTTBrokerIPAndPort parameter = new MQTTBrokerIPAndPort(ip, port);
                    return parameter;
                }
                catch (Exception)
                {
                    return new MQTTBrokerIPAndPort("localhost", 1883);
                }
            }
        }
    }
}
