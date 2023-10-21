namespace Networking.PluginComponents.MQTT
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using MQTTnet;
    using MQTTnet.Client;
    using UnityEngine;
    using VContainer;

    public class MQTTClientCommunicator : IDisposable
    {
        private MQTTClientFactory clientFactory;
        private IMqttClient client;

        [Inject]
        public MQTTClientCommunicator(MQTTClientFactory factory)
        {
            this.clientFactory = factory;
            this.Init().Forget();
        }

        /// <summary>
        /// BrokerにメッセージをPublishする
        /// </summary>
        /// <param name="topic">トピック</param>
        /// <param name="payload">ペイロードメッセージ</param>
        /// <returns>unitask</returns>
        public async UniTask Publish(string topic, string payload)
        {
            Debug.Assert(this.client.IsConnected);

            MqttApplicationMessage message = this.BuildMessage(topic, payload);
            await this.client.PublishAsync(message, CancellationToken.None);
        }

        public void Dispose()
        {
            this.client.Dispose();
            this.client = null;
        }

        private async UniTask Init()
        {
            this.client = await this.clientFactory.CreateAsync();
        }

        private MqttApplicationMessage BuildMessage(string topic, string payload)
        {
            return new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce)
                .WithRetainFlag()
                .Build();
        }
    }
}
