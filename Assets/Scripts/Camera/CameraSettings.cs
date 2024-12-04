using UnityEngine;


namespace Managers.CameraManagement {
    [CreateAssetMenu(menuName = "Data/Camera/CameraSettings")]
    public class CameraSettings : ScriptableObject {
        [Header("Movement Settings")]
        public Vector3 offset = new Vector3(0, 0, -10);
        public float mouseLookWeight = .5f;
        public float smoothTime = 0.25f;
        public float zoomSpeed = 1;

        [Header("Display Settings")]
        public float borderWhenInRoom = 2;
        public float freeCameraSize = 10;
        public float minCameraSize = 8;
    }
}