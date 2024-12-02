using Louis.Patterns.ServiceLocator;
using Managers;
using Managers.CameraManagement;
using UnityEngine;


namespace CharacterMechanics {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Player))]
    public class PlayerMovement : MonoBehaviour {

        [Header("Settings")]
        [Tooltip("Players acceleration is calculated using this coefficient in front of the max move speed. e.g.\n " +
            "Move speed = 10m/s\n Coefficient = 20\n Acceleration = 10 * 20 = 200m/s^2")]
        [SerializeField] float accelerationCoefficient = 20f;

        Player _player;
        Rigidbody2D _rigidbody;
        Vector2 _movement;
        float _moveSpeed;

        void OnEnable() {
            _player = GetComponent<Player>();
            _rigidbody = GetComponent<Rigidbody2D>();
            InputManager.onMove += OnMove;
            _player[Stats.SecondaryStatTag.MoveSpeed].onChanged += OnMoveSpeedChanged;
        }

        void OnDisable() {
            InputManager.onMove -= OnMove;
            _player[Stats.SecondaryStatTag.MoveSpeed].onChanged -= OnMoveSpeedChanged;
        }

        void Start() {
            ServiceLocator.TryGetService<ICameraService>(out var cameraService);
            cameraService?.SetTarget(transform);
        }

        private void OnMove(Vector2 movement) {
            _movement = movement;
        }

        void OnMoveSpeedChanged(float movespeed) {
            _moveSpeed = movespeed;
        }

        private void FixedUpdate() {
            _rigidbody.linearVelocity = Vector2.MoveTowards(
                _rigidbody.linearVelocity, 
                _moveSpeed * _movement, 
                Time.fixedDeltaTime * _moveSpeed * 20);
        }
    }
}