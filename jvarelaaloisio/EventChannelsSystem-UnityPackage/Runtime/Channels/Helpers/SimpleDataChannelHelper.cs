using System;

namespace Events.Runtime.Channels.Helpers
{
	public static class SimpleDataChannelHelper
	{
		public static bool TrySubscribe<T>(
			this ChannelSo<T> channel,
			in Action<T> handler)
		{
			if (channel) channel.Subscribe(handler);
			return channel;
		}

		public static bool TryRaiseEvent<T>(
			this ChannelSo<T> channel,
			in T data)
		{
			if (channel) channel.RaiseEvent(data);
			return channel;
		}
	}
}