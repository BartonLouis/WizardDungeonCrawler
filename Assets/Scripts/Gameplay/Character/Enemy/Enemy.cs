using Gameplay;
using UnityEngine;


namespace Enemy {
    public class Enemy : MonoBehaviour, IDamageable {
        public float Damage(IProjectileOwner owner, float damage) {
            return 0f;
        }
    }
}