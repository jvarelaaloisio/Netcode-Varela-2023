using System;
using UnityEngine;

namespace Events.Runtime.Channels
{
	[CreateAssetMenu(menuName = "Event Channels/Void Channel", fileName = "VoidChannel")]
	public class VoidChannelSo : ScriptableObject
	{
		private Action voidEvent;

		public void Subscribe(in Action handler)
		{
			voidEvent += handler;
		}

		public void UnSubscribe(in Action handler)
		{
			voidEvent -= handler;
		}

		public void RaiseEvent()
		{
			voidEvent?.Invoke();
		}
	}
}