namespace Networking.MQTT.Client
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using MessagePipe;
    using MQTTnet;
    using MQTTnet.Client;
    using MQTTnet.Client.Connecting;
    using MQTTnet.Client.Options;
    using MQTTnet.Client.Receiving;
    using UnityEngine;

    public class MQTTClient : IMQTTClient
    {
        private IMqttClient client;

        private MQTTClient(IMqttClient client, bool valid)
        {
            this.client = client;
            this.Valid = valid;
        }

        public bool Valid { get; }

        public async UniTask<IMQTTClient> Connecting(string ip, int port, IEnumerable<IMQTTMessageListener> listeners, IPublisher<MQTTReceivedMessage> publisher)
        {
            IMqttClient client = await CreateMQTTnetClient(ip, port, listeners, publisher);

            return new MQTTClient(client, client != null);
        }

        public async UniTask PublishMessage(string topic, string payload, int qos = 0)
        {
            MqttApplicationMessage message = this.CreateMQTTMessage(topic, payload);
            await this.client.PublishAsync(message, CancellationToken.None);
        }

        public void Dispose()
        {
            this.client?.Dispose();
        }

        /// <summary>
        /// MQTTnetのclientインスタンスを生成する
        /// </summary>
        /// <param name="ip">ip</param>
        /// <param name="port">port</param>
        /// <param name="listeners">mqttからのメッセージを購読するリスナー</param>
        /// <param name="publisher">messagepipe publisher</param>
        /// <returns>client</returns>
        private static async UniTask<IMqttClient> CreateMQTTnetClient(string ip, int port, IEnumerable<IMQTTMessageListener> listeners, IPublisher<MQTTReceivedMessage> publisher)
        {
            IMqttClient client = new MqttFactory().CreateMqttClient();

            try
            {
                IMqttClientOptions options = new MqttClientOptionsBuilder()
                    .WithTcpServer(ip, port)
                    .Build();

                client.ConnectedHandler = new MqttClientConnectedHandlerDelegate((e) => OnConnected(e, client, listeners));
                client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate((e) => OnReceivedMessage(e, publisher).Forget());

                await client.ConnectAsync(options);
                return client;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }

        /// <summary>
        /// 各リスナーが購読したいtopicをClientに登録する
        /// </summary>
        /// <param name="e">args</param>
        /// <param name="client">client</param>
        /// <param name="listeners">リスナー</param>
        private static async void OnConnected(MqttClientConnectedEventArgs e, IMqttClient client, IEnumerable<IMQTTMessageListener> listeners)
        {
            foreach (IMQTTMessageListener listener in listeners)
            {
                await listener.SubscribeAsync(client);
            }

            Debug.Log("Completed Clients Subscribe");
        }

        private static async UniTaskVoid OnReceivedMessage(MqttApplicationMessageReceivedEventArgs e, IPublisher<MQTTReceivedMessage> publisher)
        {
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            string topic = e.ApplicationMessage.Topic;
            MQTTReceivedMessage message = new MQTTReceivedMessage(topic, payload);

            await UniTask.Yield();
            publisher.Publish(message);
        }

        private MqttApplicationMessage CreateMQTTMessage(string topic, string payload, int qos = 0)
        {
            return new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
                .WithRetainFlag()
                .Build();
        }
    }
}
