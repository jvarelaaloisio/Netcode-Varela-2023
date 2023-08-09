using Events.Runtime.Channels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Events.Runtime.GenericListeners
{
	public class IdEventListener : MonoBehaviour
	{
		[SerializeField] private int id;
		[FormerlySerializedAs("dataChannel")] [SerializeField, Tooltip("Not Null")] private IntChannelSo channel;
		[SerializeField] private UnityEvent onEvent;

		private void Awake()
		{
			channel.Subscribe(listenedId =>
			{
				if (listenedId == id) onEvent.Invoke();
			});
		}

		public void Debug() => UnityEngine.Debug.Log("event raised with id: " + id);
	}
}