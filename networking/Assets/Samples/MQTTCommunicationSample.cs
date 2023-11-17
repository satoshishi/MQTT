namespace Sample
{
    using Networking.Core.Publishing;
    using Networking.Core.Streaming;
    using UnityEngine;
    using VContainer;

    public class MQTTCommunicationSample : MonoBehaviour
    {
        private INetworkStreamingService streamingService;

        private INetworkPublishingService publishingService;

        [Inject]
        internal void Injection(INetworkStreamingService streamingService, INetworkPublishingService publishingService)
        {
            this.streamingService = streamingService;
            this.publishingService = publishingService;

            this.streamingService.AddListener<PayloadSample>((payload) => Debug.Log(payload.Message));
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                this.publishingService.Publish(new PayloadSample("hello world"));
            }
        }
    }
}
