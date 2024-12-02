using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Managers {
    [CreateAssetMenu(menuName = "InputManager")]
    public class InputManager : ScriptableObject {
        [SerializeField] InputActionAsset _asset;

        public static event UnityAction<Vector2> onMove = delegate { };
        public static event UnityAction<Vector2> onMousePosition = delegate { };
        public static event UnityAction<bool> onFire = delegate { };
        public static event UnityAction onInput = delegate { };

        InputAction moveAction;
        InputAction mousePositionAction;
        InputAction clickAction;

        private void OnEnable() {
            if (_asset == null) return;
            moveAction = _asset.FindAction("Movement");
            mousePositionAction = _asset.FindAction("MousePosition");
            clickAction = _asset.FindAction("Fire");

            moveAction.started += OnMove;
            moveAction.performed += OnMove;
            moveAction.canceled += OnMove;

            mousePositionAction.started += OnMouseMove;
            mousePositionAction.performed += OnMouseMove;
            mousePositionAction.canceled += OnMouseMove;

            clickAction.started += OnFire;
            clickAction.performed += OnFire;
            clickAction.canceled += OnFire;

            moveAction.Enable();
            mousePositionAction.Enable();
            clickAction.Enable();
        }

        private void OnDisable() {
            if (_asset == null) return;
            moveAction.started -= OnMove;
            moveAction.performed -= OnMove;
            moveAction.canceled -= OnMove;

            mousePositionAction.started -= OnMouseMove;
            mousePositionAction.performed -= OnMouseMove;
            mousePositionAction.canceled -= OnMouseMove;

            clickAction.started -= OnFire;
            clickAction.performed -= OnFire;
            clickAction.canceled -= OnFire;

            moveAction.Disable();
            mousePositionAction.Disable();
            clickAction.Disable();
        }

        void OnMove(InputAction.CallbackContext context) {
            onMove.Invoke(context.ReadValue<Vector2>());
            onInput.Invoke();
        }

        void OnMouseMove(InputAction.CallbackContext context) {
            onMousePosition.Invoke(context.ReadValue<Vector2>());
        }

        void OnFire(InputAction.CallbackContext context) {
            onFire.Invoke(context.performed);
            if (context.performed) {
                onInput.Invoke();
            }
        }

        public void Reconnect() {
            try {
                OnDisable();
            } catch { }
            finally {
                OnEnable();
                Logging.Log(this, "Reconnected Successfully", LogLevel.Debug);
            }
        }
    }
}
