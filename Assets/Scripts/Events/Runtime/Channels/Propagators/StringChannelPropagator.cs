using Events.Runtime.Channels.Helpers;
using Events.Runtime.UnityEvents;
using UnityEngine;

namespace Events.Runtime.Channels.Propagators
{
    public class StringChannelPropagator : MonoBehaviour
    {
        [SerializeField] private StringChannelSo channel;

        public StringUnityEvent onEvent;

        private void Awake()
        {
            if (!channel.TrySubscribe(onEvent.Invoke))
            {
                Debug.LogError($"No channel was provided for the propagation" +
                               $" in the {gameObject.name} GameObject");
            }
        }
    }
}