using Louis.Patterns.ServiceLocator;
using Managers;
using UnityEngine;
using UnityEngine.Rendering;


namespace Weapons {
    public class WeaponMount : MonoBehaviour, IWeaponMountService {
        Weapon _mountedWeapon;
        Vector2 _mousePos = Vector2.zero;

        private void OnEnable() {
            InputManager.onMousePosition += OnMouseMove;
            ServiceLocator.Register<IWeaponMountService>(this);
        }

        private void OnDisable() {
            InputManager.onMousePosition -= OnMouseMove;
            ServiceLocator.Deregister<IWeaponMountService>(this);
        }

        public void MountWeapon(Weapon weapon) {
            if (_mountedWeapon != null) {
                Destroy(_mountedWeapon);
            }
            _mountedWeapon = weapon;
            _mountedWeapon.transform.parent = transform;
            _mountedWeapon.transform.localPosition = Vector2.zero;
            Logging.Log(this, $"Mounted weapon: {weapon.name}");
        }

        void OnMouseMove(Vector2 mousePos) {
            _mousePos = mousePos;
        }

        private void Update() {
            if (_mountedWeapon == null) return;
            var worldPos = Camera.main.ScreenToWorldPoint(_mousePos);
            _mountedWeapon.transform.right = new Vector2(worldPos.x - transform.position.x, worldPos.y - transform.position.y);
        }
    }
}