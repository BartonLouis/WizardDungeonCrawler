using Louis.Patterns.ServiceLocator;
using Managers;
using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Gameplay.Projectiles {
    public class ProjectilePool : MonoBehaviour, IProjectilePool {
        [Header("Settings")]
        [SerializeField] int defaultCapacity = 100;
        [SerializeField] int maxSize = 1000;
        [SerializeField] bool collectionCheck;

        [Header("Prefab")]
        [SerializeField] Projectile prefab;

        IObjectPool<Projectile> _objectPool;


        private void Awake() {
            _objectPool = new ObjectPool<Projectile>(CreateProjectile, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, collectionCheck, defaultCapacity, maxSize);
        }

        #region Public Functions
        public void Release(Projectile p) {
            _objectPool.Release(p);
        }

        public Projectile GetProjectile() {
            Projectile projectile = _objectPool.Get();
            return projectile;
        }
        #endregion

        #region Object Pool Functions
        Projectile CreateProjectile() {
            Projectile projectile = Instantiate(prefab, transform);
            return projectile;
        }

        void OnGetFromPool(Projectile projectile) {
            projectile.gameObject.SetActive(true);
        }

        void OnReleaseToPool(Projectile projectile) {
            projectile.gameObject.SetActive(false);
        }

        void OnDestroyPooledObject(Projectile projectile) {
            Destroy(projectile.gameObject);
        }
        #endregion
    }

    public interface IProjectilePool {
        public Projectile GetProjectile();
        public void Release(Projectile p);
    }
}