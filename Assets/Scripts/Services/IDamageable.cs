using UnityEngine;

namespace Gameplay {
    public interface IProjectileOwner { }


    public interface IDamageable {
        public float Damage(IProjectileOwner owner, float damage);
    }
}