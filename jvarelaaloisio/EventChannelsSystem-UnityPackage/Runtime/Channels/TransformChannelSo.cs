using UnityEngine;

namespace Events.Runtime.Channels
{
	[CreateAssetMenu(menuName = "Event Channels/Data Channels/Transform", fileName = "TransformChannel")]
	public class TransformChannelSo : ChannelSo<Transform> { }
}