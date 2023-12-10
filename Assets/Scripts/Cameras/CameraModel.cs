using System.Collections;
using System.Linq;
using Core.Models;
using UnityEngine;

namespace Cameras
{
    [CreateAssetMenu(menuName = "Models/Camera", fileName = "_CameraModel")]
    public class CameraModelImpl : CameraModel
    {
        [SerializeField] private AnimationCurve duration = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private float size = 1;
        [SerializeField] private Vector2 position;
        
        public override IEnumerator Apply(Camera camera)
        {
            if (camera == null)
            {
                Debug.LogError($"{name}: {nameof(camera)} is null!");
                yield break;
            }
            
            var start = Time.time;
            var applyDuration = duration.keys.Last().time - duration.keys.First().time;
            var originalSize = camera.orthographicSize;
            var cameraTransform = camera.transform;
            var originalPosition = cameraTransform.position;
            var destination = new Vector3(position.x, position.y, originalPosition.z);
            float now = 0;
            do
            {
                now = Time.time;
                var lerp = (now - start) / applyDuration;
                camera.orthographicSize = Mathf.Lerp(originalSize, size, lerp);
                cameraTransform.position = Vector3.Lerp(originalPosition, destination, duration.Evaluate(lerp));
                yield return null;
            } while (now < start + applyDuration);

            camera.orthographicSize = size;
            cameraTransform.position = destination;
        }
    }
}
