using UnityEngine;

namespace Events.Runtime.Channels
{
	[CreateAssetMenu(menuName = "Event Channels/Data Channels/Transform[]", fileName = "TransformArrayChannel")]
	public class TransformArrayChannelSo : ChannelSo<Transform[]> { }
}