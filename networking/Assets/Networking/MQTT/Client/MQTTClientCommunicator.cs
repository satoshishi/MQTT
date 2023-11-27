namespace Networking.MQTT.Client
{
    using System;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using MQTTnet;
    using MQTTnet.Client;
    using Networking.MQTT.Publishing;
    using VContainer;
    using VContainer.Unity;

    public class MQTTClientCommunicator : IDisposable, IInitializable, IMQTTCommunicator
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

            if(this.client != null && this.client.IsConnected)
            {
                MqttApplicationMessage message = this.BuildMessage(topic, payload);
                await this.client.PublishAsync(message, CancellationToken.None);
            }
        }

        public void Dispose()
        {
            this.client?.Dispose();
            this.client = null;
        }

        /// <summary>
        /// VContainerのEntryPointとしてコールされる
        /// </summary>
        public void Initialize()
        {
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
