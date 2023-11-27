namespace Networking.Simulation
{
    using UnityEngine;

    public abstract class NetworkingSimulator : MonoBehaviour
    {
        [SerializeField]
        private bool everyFrame;

        protected bool EveryFrame => this.everyFrame;

        [ContextMenu("Publish")]
        protected void PublishFromContextMenu()
        {
            this.Publish();
        }

        protected abstract void Publish();
    }
}
