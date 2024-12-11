using Gameplay.Projectiles;

namespace Gameplay {
    public interface IProjectileOwner {
        public void ReportProjectileHitInfo(HitInfo hitInfo);
    }


    public interface IDamageable {
        public float Damage(IProjectileOwner owner, float damage);
    }
}