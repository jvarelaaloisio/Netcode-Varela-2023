using System;

namespace Events.Runtime.Channels.Helpers
{
	public static class VoidChannelHelper
	{
		public static bool TrySubscribe(
			this VoidChannelSo channel,
			in Action handler)
		{
			if (channel) channel.Subscribe(handler);
			return channel;
		}

		public static bool TryRaiseEvent(this VoidChannelSo channel)
		{
			if (channel) channel.RaiseEvent();
			return channel;
		}
	}
}