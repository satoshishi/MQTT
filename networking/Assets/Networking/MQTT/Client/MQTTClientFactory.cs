namespace Networking.MQTT.Client
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using MessagePipe;
    using MQTTnet.Client;

#if !LOCAL && !TEST
    using System;
    using System.Text;
    using MQTTnet;
    using MQTTnet.Client.Connecting;
    using MQTTnet.Client.Options;
    using MQTTnet.Client.Receiving;
    using UnityEngine;
#endif

    using VContainer;

    public class MQTTClientFactory
    {
        private IPublisher<MQTTReceivedMessage> publisher;

        private IEnumerable<IMQTTMessageListener> listeners;

#if FIXED_IP
        private MQTTClientParameter fixedParameter;
#endif
        [Inject]
        public MQTTClientFactory(IPublisher<MQTTReceivedMessage> publisher, IEnumerable<IMQTTMessageListener> listeners, MQTTClientParameter fixedParameter)
        {
            this.publisher = publisher;
            this.listeners = listeners;
#if FIXED_IP
            this.fixedParameter = fixedParameter;
#endif
        }

        public async UniTask<IMqttClient> CreateAsync()
        {
#if LOCAL || TEST
            await UniTask.Yield();
            MqttClient client = default;
#else
            IMqttClient client = new MqttFactory().CreateMqttClient();

            try
            {
#if FIXED_IP
                MQTTClientParameter parameter = this.fixedParameter;
#else
                MQTTClientParameter parameter = MQTTClientParameter.Load();
#endif
                IMqttClientOptions options = new MqttClientOptionsBuilder()
                    .WithTcpServer(parameter.Ip, parameter.Port)
                    .Build();

                client.ConnectedHandler = new MqttClientConnectedHandlerDelegate((e) => this.OnConnected(e, client));
                client.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate((e) => this.OnAppMessage(e).Forget());

                await client.ConnectAsync(options);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
#endif
            return client;
        }

#if !LOCAL && !TEST

        /// <summary>
        /// MQTTClientにIMQTTMessageListenerへメッセージを通知するための登録処理を実行する
        /// </summary>
        /// <param name="e">接続情報</param>
        /// <param name="client">client</param>
        private async void OnConnected(MqttClientConnectedEventArgs e, IMqttClient client)
        {
            foreach (IMQTTMessageListener listener in this.listeners)
            {
                await listener.SubscribeAsync(client);
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
#endif
    }
}
