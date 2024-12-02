using Louis.Patterns.ServiceLocator;
using UnityEngine;


namespace Weapons {
    public class Weapon : MonoBehaviour {

        private void Start() {
            ServiceLocator.TryGetService<IWeaponMountService>(out var weaponMountService);
            weaponMountService?.MountWeapon(this);
        }

        private void OnEnable() {
            
        }

        private void OnDisable() {
            
        }
    }
}