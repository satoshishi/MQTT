namespace Networking.MQTT.Client
{
    using System;
    using Cysharp.Threading.Tasks;
    using Networking.Core.Connecting;
    using Networking.MQTT.Publishing;
    using VContainer;
    using VContainer.Unity;

    public class MQTTClientCommunicator : IDisposable, IInitializable, IMQTTCommunicator, INetworkingConnector
    {
        private MQTTClientFactory clientFactory;

        private IMQTTClient clinet = null;

        [Inject]
        public MQTTClientCommunicator(MQTTClientFactory factory, MQTTClientParameter parameter)
        {
            this.clientFactory = factory;

            if (parameter.ConnectAuto)
            {
                this.Connecting(parameter.Ip, parameter.Port).Forget();
            }
        }

        public async UniTask<bool> Connecting(string ip, int port = 1883)
        {
            this.clinet = await this.clientFactory.CreateAsync(ip, port);

            return this.clinet.Valid;
        }

        /// <summary>
        /// BrokerにメッセージをPublishする
        /// </summary>
        /// <param name="topic">トピック</param>
        /// <param name="payload">ペイロードメッセージ</param>
        /// <param name="qos">qos</param>
        /// <returns>unitask</returns>
        public async UniTask Publish(string topic, string payload, int qos = 0)
        {
            if (this.clinet != null && this.clinet.Valid)
            {
                await this.clinet.PublishMessage(topic, payload, qos);
            }
        }

        public void Dispose()
        {
            this.clinet?.Dispose();
        }

        /// <summary>
        /// VContainerのEntryPointとしてコールされる
        /// </summary>
        public void Initialize()
        {
        }
    }
}
