using DungeonGeneration;
using Events.Rooms;
using Louis.Patterns.EventSystem;
using Louis.Patterns.ServiceLocator;
using Louis.Patterns.Singleton;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Managers.CameraManagement {
    public class CameraManager : PersistentSingleton<CameraManager>, ICameraService {
        enum AxisLock {
            X,
            Y
        }

        [Header("Settings")]
        [SerializeField] CameraSettings _settings;
        Camera _camera;


        EventBinding<RoomEnteredEvent> roomEnteredBinding;
        EventBinding<RoomExitedEvent> roomExitedBinding;

        Transform _followTarget;
        Vector2 _mousePos;
        Vector3 _velocity;
        float _targetSize;

        RoomInfo _roomTarget;
        AxisLock _lock;


        private void OnEnable() {
            _camera = Camera.main;
            roomEnteredBinding = new EventBinding<RoomEnteredEvent>(OnRoomEntered);
            roomExitedBinding = new EventBinding<RoomExitedEvent>(OnRoomExited);
            InputManager.onMousePosition += OnMouseMove;
            _camera.orthographicSize = _targetSize = _settings.freeCameraSize;

            ServiceLocator.Register<ICameraService>(this);
        }

        private void OnDisable() {
            ServiceLocator.Deregister<ICameraService>(this);
            InputManager.onMousePosition -= OnMouseMove;

            roomEnteredBinding?.Dispose();
            roomExitedBinding?.Dispose();
        }

        void OnMouseMove(Vector2 mousePos) {
            _mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        }

        void OnRoomEntered(RoomEnteredEvent args) {
            _roomTarget = args.roomInfo;
            Bounds bounds = args.colliderBounds;
            float cameraRatio = _camera.aspect;
            if (bounds.size.y <= bounds.size.x) {
                _targetSize = Mathf.Max(_settings.borderWhenInRoom + 0.5f * bounds.size.y, _settings.minCameraSize);
                _lock = AxisLock.X;
            } else {
                _targetSize = Mathf.Max(_settings.borderWhenInRoom + 0.5f * bounds.size.x / _camera.aspect, _settings.minCameraSize);
                _lock = AxisLock.Y;
            }
        }

        void OnRoomExited(RoomExitedEvent args) {
            _roomTarget = default;
            _targetSize = _settings.freeCameraSize;
        }

        public void SetTarget(Transform target) {
            _followTarget = target;
            Logging.Log(this, $"Camera follow target set to {target.name}");
        }

        private void LateUpdate() {
            if (_followTarget == null) return;
            Vector3 lookDir = new Vector3(_mousePos.x - _followTarget.position.x, _mousePos.y - _followTarget.position.y) * _settings.mouseLookWeight;

            Vector3 targetPos;
            if (_roomTarget == default) {
                targetPos = _followTarget.position + lookDir + _settings.offset;
            } else {
                if (_lock == AxisLock.X) {
                    float x = (_followTarget.position.x + lookDir.x);
                    targetPos = new Vector3(x, _roomTarget.bounds.center.y) + _settings.offset;
                } else {
                    float y = (_followTarget.position.y + lookDir.y);
                    targetPos = new Vector3(_roomTarget.bounds.center.x, y) + _settings.offset;
                }
            }

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, _settings.smoothTime);
            _camera.orthographicSize = Mathf.MoveTowards(_camera.orthographicSize, _targetSize, _settings.zoomSpeed * Time.deltaTime);
        }
    }
}