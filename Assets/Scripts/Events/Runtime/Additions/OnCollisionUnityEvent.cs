using UnityEngine;
using UnityEngine.Events;

namespace Events.Runtime.Additions
{
	public class OnCollisionUnityEvent : MonoBehaviour
	{
		[SerializeField] private UnityEvent<Collision> onCollisionEnterEvent;
		[SerializeField] private UnityEvent<Collision> onCollisionExitEvent;

		private void OnCollisionEnter(Collision collision)
		{
			onCollisionEnterEvent?.Invoke(collision);
		}

		private void OnCollisionExit(Collision collision)
		{
			onCollisionExitEvent?.Invoke(collision);
		}
	}
	public class OnTriggerUnityEvent : MonoBehaviour
	{
		[SerializeField] private UnityEvent<Collider> onTriggerEnterEvent;
		[SerializeField] private UnityEvent<Collider> onTriggerExitEvent;

		private void OnTriggerEnter(Collider other)
		{
			onTriggerEnterEvent?.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			onTriggerExitEvent?.Invoke(other);
		}
	}
}