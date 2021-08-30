using UnityEngine;

namespace Helpers
{
    [RequireComponent(typeof(Camera))]
    public class CameraFit : MonoBehaviour
    {

        public SpriteRenderer spriteToFitTo;

        private void Start()
        {
            var bounds = spriteToFitTo.bounds.extents;
            var height = bounds.x / GetComponent<Camera>().aspect;
            if (height < bounds.y)
            {
                height = bounds.y;
            }
            GetComponent<Camera>().orthographicSize = height;
        }
    }
}
