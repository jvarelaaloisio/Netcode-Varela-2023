﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace Events.Runtime.Channels.Propagators
{
	public class VoidChannelPropagator : MonoBehaviour
	{
		[SerializeField, Tooltip("Not Null")]
		private VoidChannelSo channel;

		public UnityEvent onEvent;

		private void Awake()
		{
			try
			{
				channel.Subscribe(onEvent.Invoke);
			}
			catch (NullReferenceException)
			{
				throw new NullReferenceException($"No channel was provided for the propagation" +
												$" in the {gameObject.name} GameObject");
			}
		}
	}
}