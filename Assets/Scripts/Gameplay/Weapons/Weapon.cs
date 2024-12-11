using Gameplay.Projectiles;
using Louis.Patterns.ServiceLocator;
using Managers;
using Services;
using UnityEngine;


namespace Gameplay.Weapons {
    public class Weapon : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] Transform _firePoint;
        [SerializeField] WeaponConfig _config;
        IProjectileManager _projectileManager;
        IProjectileOwner _owner;

        float _cooldown;
        bool _firing;

        private void Start() {
            ServiceLocator.TryGetService<IWeaponMountService>(out var weaponMountService);
            ServiceLocator.TryGetService<IProjectileManager>(out _projectileManager);
            _owner = weaponMountService?.MountWeapon(this);
            _firePoint.localPosition = _config.firePointOffset;
        }

        private void OnEnable() {
            InputManager.onFire += FireButtonStateChanged;
        }

        private void OnDisable() {
            InputManager.onFire -= FireButtonStateChanged;
        }

        void FireButtonStateChanged(bool performed) {
            _firing = performed;
            if (_cooldown <= 0) {
                Fire();
            }
        }

        private void Update() {
            if (_firing && _cooldown <= 0) {
                Fire();
            }
            _cooldown = Mathf.MoveTowards(_cooldown, 0, Time.deltaTime);
        }

        void Fire() {
            _cooldown = 1f / _config.fireRate;
            _projectileManager.Fire(_owner, _firePoint.position, transform.rotation, _config.projectile);
        }
    }
}