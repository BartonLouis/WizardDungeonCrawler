using Gameplay.Player.Movement;
using Gameplay.Player.Stats;
using Louis.Patterns.ServiceLocator;
using Louis.Patterns.StateMachine;
using Managers;
using Managers.CameraManagement;
using UnityEngine;


namespace CharacterMechanics {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerStatsManager))]
    public class PlayerMovement : MonoBehaviour, IPlayerMovementService {

        #region Storage
        [Header("Settings")]
        [Tooltip("Players acceleration is calculated using this coefficient in front of the max move speed. e.g.\n " +
            "Move speed = 10m/s\n Coefficient = 20\n Acceleration = 10 * 20 = 200m/s^2")]
        [SerializeField] float _accelerationCoefficient = 20f;

        Rigidbody2D _rigidbody;
        Vector2 _movement;
        float _moveSpeed;

        #region Public Properties
        public float AccelerationCoefficient => _accelerationCoefficient;
        public Vector2 Movement => _movement;
        public float MoveSpeed => _moveSpeed;

        public Rigidbody2D RB { get; private set; }
        public IPlayerStatsService Stats { get; private set; }

        #region State Machine Properties
        public StateMachine<PlayerMovementBaseState> StateMachine { get; private set; }
        public LockedState LockedState { get; private set; }
        public FreeMoveState FreeMoveState { get; private set; }
        #endregion
        #endregion
        #endregion

        void OnEnable() {
            ServiceLocator.Register<IPlayerMovementService>(this);
            InputManager.onMove += OnMove;
            Stats[SecondaryStatTag.MoveSpeed].onChanged += OnMoveSpeedChanged;
        }

        void OnDisable() {
            ServiceLocator.Deregister<IPlayerMovementService>(this);
            InputManager.onMove -= OnMove;
            Stats[SecondaryStatTag.MoveSpeed].onChanged -= OnMoveSpeedChanged;
        }

        private void Awake() {
            RB = GetComponent<Rigidbody2D>();
            Stats = GetComponent<IPlayerStatsService>();
        }

        void Start() {
            ServiceLocator.TryGetService<ICameraService>(out var cameraService);
            cameraService?.SetTarget(transform);
            _moveSpeed = Stats[SecondaryStatTag.MoveSpeed].Value;

            StateMachine = new StateMachine<PlayerMovementBaseState>(null);
            LockedState = new(this);
            FreeMoveState = new(this);
            StateMachine.SetState(LockedState);
        }

        private void OnMove(Vector2 movement) {
            _movement = movement;
        }

        void OnMoveSpeedChanged(float movespeed) {
            _moveSpeed = movespeed;
        }

        private void Update() {
            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate() {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        public void SetLock(bool locked) {
            if (locked) {
                StateMachine.SetState(LockedState);
            } else {
                StateMachine.SetState(FreeMoveState);
            }
        }

        public void SetPosition(Vector2 position) {
            RB.position = position;
        }
    }

    public interface IPlayerMovementService : IService {
        public void SetPosition(Vector2 position);
        public void SetLock(bool locked);
    }
}