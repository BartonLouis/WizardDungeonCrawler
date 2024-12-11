using Louis.Patterns.ServiceLocator;
using Louis.Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.Projectiles {
    [RequireComponent(typeof(IProjectilePool))]
    public class ProjectileManager : RegulatorSingleton<ProjectileManager>, IProjectileManager {

        HashSet<Projectile> activeProjectiles = new();
        HashSet<Projectile> toRemove = new();

        IProjectilePool _projectilePool;

        private void OnEnable() {
            _projectilePool = GetComponent<IProjectilePool>();
            ServiceLocator.Register<IProjectileManager>(this);
        }

        private void OnDisable() {
            ServiceLocator.Deregister<IProjectileManager>(this);
        }

        public void Fire(IProjectileOwner owner, Vector2 start, Quaternion direction, ProjectileConfig config) {
            Projectile p = _projectilePool.GetProjectile();
            p.Fire(owner, start, direction, config);
            activeProjectiles.Add(p);
        }

        private void FixedUpdate() {
            foreach (var projectile in activeProjectiles) {
                UpdateProjectile(projectile);
            }
        }

        void LateUpdate() {
            foreach (var projectile in toRemove) {
                activeProjectiles.Remove(projectile);
                _projectilePool.Release(projectile);
            }
            toRemove.Clear();
        }

        void MarkProjectileForDestruction(Projectile p, HitInfo info) {
            p.Owner?.ReportProjectileHitInfo(info);
            toRemove.Add(p);
        }


        void UpdateProjectile(Projectile p) {
            p.Time += Time.fixedDeltaTime;
            if (p.Time > p.Config.lifetime) {
                MarkProjectileForDestruction(p, new HitInfo());
                return;
            }

            float normTime = p.Time / p.Config.lifetime;
            float verticalOffset = p.Config.maxVerticalOffset * p.Config.verticalOffsetCurve.Evaluate(normTime);
            float velocity = p.Config.baseSpeed * p.Config.velocityCurve.Evaluate(normTime);
            float verticalDelta = verticalOffset - p.CurrentVerticalOffset;

            p.transform.position = p.transform.position + verticalDelta * p.transform.up + velocity * Time.fixedDeltaTime * p.transform.right;
            p.CurrentVerticalOffset = verticalOffset;

            Collider2D[] collisions = Physics2D.OverlapCircleAll(p.transform.position, p.Config.colliderRadius, p.Config.collidesWith);
            if (collisions.Length > 0) {
                MarkProjectileForDestruction(p, new HitInfo());
            }
        }
    }



    public interface IProjectileManager : IService {
        public void Fire(IProjectileOwner owner, Vector2 start, Quaternion direction, ProjectileConfig config);
    }

    public struct HitInfo {
    }

}