namespace Sample
{
    using Networking.Core.Publishing;
    using Networking.Core.Streaming;
    using UnityEngine;
    using VContainer;

    public class MQTTCommunicationSample : MonoBehaviour
    {
        private INetworkStreamer<PayloadSample> streamer;

        private INetworkPublisher publisher;

        [Inject]
        internal void Injection(INetworkStreamer<PayloadSample> streamer, INetworkPublisher publisher)
        {
            this.streamer = streamer;
            this.publisher = publisher;

            this.streamer.AddListener((payload) => Debug.Log(payload.Message));
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
