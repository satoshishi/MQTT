namespace Networking.PluginComponents.MQTT
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Cysharp.Threading.Tasks;
    using MessagePipe;
    using MQTTnet;
    using MQTTnet.Client;
    using MQTTnet.Client.Connecting;
    using MQTTnet.Client.Options;
    using MQTTnet.Client.Receiving;
    using UnityEngine;
    using VContainer;

    public class MQTTClientFactory
    {
        private IPublisher<MQTTReceivedMessage> publisher;

        private ISubscriber<MQTTReceivedMessage> subscriber;

        private IEnumerable<IMQTTMessageListener> listeners;

        [Inject]
        public MQTTClientFactory(IPublisher<MQTTReceivedMessage> publisher, ISubscriber<MQTTReceivedMessage> subscriber, IEnumerable<IMQTTMessageListener> listeners)
        {
            this.publisher = publisher;
            this.subscriber = subscriber;
            this.listeners = listeners;
        }

        public async UniTask<IMqttClient> CreateAsync()
        {
            IMqttClient client = new MqttFactory().CreateMqttClient();
            IMqttClientOptions options = new MqttClientOptionsBuilder()
                .WithTcpServer("ip address", -999)
                .Build();

            try
            {
                client.ConnectedHandler = new MqttClientConnectedHandlerDelegate((e) => this.OnConnected(e, client));
                client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate((e) => this.OnAppMessage(e).Forget());
                await client.ConnectAsync(options);
            }
            catch (Exception e)
            {
                throw e;
            }

            return client;
        }

        /// <summary>
        /// MQTTClientにIMQTTMessageListenerへメッセージを通知するための登録処理を実行する
        /// </summary>
        /// <param name="e">接続情報</param>
        /// <param name="client">client</param>
        private async void OnConnected(MqttClientConnectedEventArgs e, IMqttClient client)
        {
            foreach (IMQTTMessageListener listener in this.listeners)
            {
                await listener.SubscribeAsync(client, this.subscriber);
            }

            Debug.Log("Completed Clients Subscribe");
        }

        /// <summary>
        /// Brokerから受けとったメッセージをMessagePipeのPublisherに通知する
        /// メッセージは結果的にIMQTTMessageListener群に通知される
        /// </summary>
        /// <param name="e">message情報</param>
        /// <returns>async</returns>
        private async UniTaskVoid OnAppMessage(MqttApplicationMessageReceivedEventArgs e)
        {
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            string topic = e.ApplicationMessage.Topic;
            MQTTReceivedMessage message = new MQTTReceivedMessage(topic, payload);

            await UniTask.Yield();
            this.publisher.Publish(message);
        }
    }
}
