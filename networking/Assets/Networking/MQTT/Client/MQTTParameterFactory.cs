namespace Networking.MQTT.Client
{
    using System;
    using System.IO;
    using UnityEngine;

    /// <summary>
    /// MQTTと接続するためのパラメータを外部ファイルから取得する
    /// androidの場合: adbからの引数からMQTTParameterをpersistentdatapathに生成するスクリプト
    /// https://qiita.com/clvth14/items/75ae45575439690a6f88
    /// </summary>
    public class MQTTParameterFactory
    {
        public static readonly string FILENAME = "mqtt.json";

        private static MQTTClientParameter defalutParameter = new MQTTClientParameter("localhost", 1883);

        internal static MQTTClientParameter Create()
        {
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
            string path = $"{Application.persistentDataPath}/{FILENAME}";
#else
            string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)}/{FILENAME}";
#endif

            if (File.Exists(path))
            {
                return JsonUtility.FromJson<MQTTClientParameter>(File.ReadAllText(path));
            }

#if !UNITY_EDITOR && UNITY_ANDROID

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
                    return defalutParameter;
                }

                MQTTClientParameter parameter = new MQTTClientParameter(ip, port);
                string json = JsonUtility.ToJson(parameter);

                File.WriteAllText(path, json);

                return parameter;
            }
            catch (Exception)
            {
            }
#else
            string json = JsonUtility.ToJson(defalutParameter);
            File.WriteAllText(path, json);

            return defalutParameter;
#endif
        }
    }
}
