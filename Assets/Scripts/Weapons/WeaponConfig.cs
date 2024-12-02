using UnityEngine;


namespace Weapons {
    [CreateAssetMenu(menuName = "Data/Weapon")]
    public class WeaponConfig : ScriptableObject {
        public float fireRate;

        private void OnValidate() {
            if (fireRate == 0) fireRate = 1;
        }
    }
}