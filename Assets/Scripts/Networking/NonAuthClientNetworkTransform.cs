using Unity.Netcode.Components;
using UnityEngine;

namespace Networking
{
    [DisallowMultipleComponent]
    public class NonAuthClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }
}
