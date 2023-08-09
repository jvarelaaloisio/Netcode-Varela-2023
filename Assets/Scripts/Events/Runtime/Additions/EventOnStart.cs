using UnityEngine;
using UnityEngine.Events;

namespace Events.Runtime.Additions
{
	public class EventOnStart : MonoBehaviour
	{
		public UnityEvent onStart;
		
		private void Start()
			=> onStart.Invoke();
	}
}
