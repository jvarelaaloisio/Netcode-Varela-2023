
using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Networking
{
    [DisallowMultipleComponent]
    public class NonAuthClientNetworkAnimator : NetworkAnimator
    {
        private void Reset()
        {
            Animator = GetComponent<Animator>();
        }

        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}