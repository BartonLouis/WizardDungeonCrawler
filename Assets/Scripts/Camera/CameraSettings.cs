using UnityEngine;


namespace Managers.CameraManagement {
    [CreateAssetMenu(menuName = "Data/Camera/CameraSettings")]
    public class CameraSettings : ScriptableObject {
        [Header("Movement Settings")]
        public Vector3 offset = new Vector3(0, 0, -10);
        public float mouseLookWeight = .5f;
        public float smoothTime = 0.25f;

        [Header("Display Settings")]
        public float cameraSize = 10;
    }
}