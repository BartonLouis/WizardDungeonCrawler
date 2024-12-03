using Gameplay.Projectiles;
using UnityEngine;


namespace Gameplay.Weapons {
    [CreateAssetMenu(menuName = "Data/Weapon")]
    public class WeaponConfig : ScriptableObject {

        public Vector3 firePointOffset;
        public ProjectileConfig projectile;
        public float fireRate;

        private void OnValidate() {
            if (fireRate == 0) fireRate = 1;
        }
    }
}