using Louis.Patterns.ServiceLocator;
using Managers;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.Projectiles {
    public class ProjectilePool : MonoBehaviour, IProjectilePoolService {
        [Header("Settings")]
        [SerializeField] int defaultCapacity = 100;
        [SerializeField] int maxSize = 1000;
        [SerializeField] bool collectionCheck;

        [Header("Prefab")]
        [SerializeField] Projectile prefab;

        IObjectPool<Projectile> objectPool;

        private void Awake() {
            objectPool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);
        }

        private void OnEnable() {
            ServiceLocator.Register<IProjectilePoolService>(this);
        }

        private void OnDisable() {
            ServiceLocator.Deregister<IProjectilePoolService>(this);
        }

        public Projectile GetProjectile() {
            Projectile projectile = objectPool.Get();
            return projectile;
        }

        private Projectile CreateProjectile() {
            Projectile projectile = Instantiate(prefab, transform);
            projectile.Init(objectPool);
            return projectile;
        }

        private void OnGetFromPool(Projectile projectile) {
            projectile.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(Projectile projectile) {
            projectile.gameObject.SetActive(false);
            projectile.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }

        private void OnDestroyPooledObject(Projectile projectile) {
            Destroy(projectile.gameObject);
        }
    }

    public interface IProjectilePoolService : IService {
        public Projectile GetProjectile();
    }
}