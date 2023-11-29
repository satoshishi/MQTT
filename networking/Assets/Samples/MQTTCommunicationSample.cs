namespace Sample
{
    using Networking.Core.Publishing;
    using Networking.Core.Streaming;
    using UnityEngine;
    using VContainer;

    public class MQTTCommunicationSample : MonoBehaviour
    {
        [SerializeField]
        private TextMesh log;

        private INetworkStreamer<PayloadSample> streamer;

        private INetworkPublisher publisher;

        [Inject]
        internal void Injection(INetworkStreamer<PayloadSample> streamer, INetworkPublisher publisher)
        {
            this.streamer = streamer;
            this.publisher = publisher;

            this.streamer.AddListener((payload) => this.log.text = payload.Message);
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                this.publisher.PublishAsync(new PayloadSample("hello world"));
            }
        }
    }
}
