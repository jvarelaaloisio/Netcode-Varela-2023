using System.Collections;
using UnityEngine;

namespace Core.Models
{
    public abstract class CameraModel : ScriptableObject
    {
        public abstract IEnumerator Apply(Camera camera);
    }
}
