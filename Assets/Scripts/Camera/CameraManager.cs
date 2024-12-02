using Louis.Patterns.ServiceLocator;
using Louis.Patterns.Singleton;
using UnityEngine;

namespace Managers.CameraManagement {
    public class CameraManager : PersistentSingleton<CameraManager>, ICameraService {
        [Header("Settings")]
        [SerializeField] CameraSettings _settings;
        Camera _camera;

        Transform _target;
        Vector2 _mousePos;
        Vector3 _velocity;


        private void OnEnable() {
            ServiceLocator.Register<ICameraService>(this);
            _camera = Camera.main;
            _camera.orthographicSize = _settings.cameraSize;
            InputManager.onMousePosition += OnMouseMove;
        }

        private void OnDisable() {
            ServiceLocator.Deregister<ICameraService>(this);
            InputManager.onMousePosition -= OnMouseMove;
        }

        void OnMouseMove(Vector2 mousePos) {
            _mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        }

        public void SetTarget(Transform target) {
            _target = target;
            Logging.Log(this, $"Camera follow target set to {target.name}");
        }

        private void LateUpdate() {
            if (_target == null) return;
            Vector3 lookDir = new Vector3(_mousePos.x - _target.position.x, _mousePos.y - _target.position.y) * _settings.mouseLookWeight;


            Vector3 targetPos = (_target.position + lookDir) + _settings.offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, _settings.smoothTime);
        }
    }
}