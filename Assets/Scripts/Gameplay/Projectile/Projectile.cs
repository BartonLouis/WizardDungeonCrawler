using Managers;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;
using Utils;

namespace Gameplay.Projectiles {
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Light2D))]
    public class Projectile : MonoBehaviour {
        IObjectPool<Projectile> _pool;

        ProjectileConfig _config;
        Rigidbody2D _rigidbody;
        CircleCollider2D _collider;
        Light2D _light;


        float _currentVerticalOffset;
        float _time;

        public void Init(IObjectPool<Projectile> pool) {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CircleCollider2D>();
            _light = GetComponent<Light2D>();
            _pool = pool;
        }

        public void Fire(IProjectileOwner owner, Vector2 start, Quaternion direction, ProjectileConfig config) {
            transform.SetPositionAndRotation(start, direction);
            _config = config;
            transform.localScale = _config.projectileRadius * Vector2.one;
            _collider.radius = _config.colliderRadius * 0.5f * 1 / config.projectileRadius;

            _light.color = config.lightColor;
            _light.pointLightInnerRadius = config.lightInnerRadius;
            _light.pointLightOuterRadius = config.lightOuterRadius;
            _light.intensity = config.lightIntensity;
            _time = 0;
            _currentVerticalOffset = 0;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (_config.collidesWith.Contains(collision.gameObject.layer)) {
                _pool.Release(this);
            }
        }

        private void FixedUpdate() {
            _time += Time.fixedDeltaTime;
            if (_time > _config.lifetime) {
                _pool.Release(this);
                return;
            }

            float normTime = _time / _config.lifetime;
            float verticalOffset = _config.maxVerticalOffset * _config.verticalOffsetCurve.Evaluate(normTime);
            float velocity = _config.baseSpeed * _config.velocityCurve.Evaluate(normTime);
            float verticalDelta = verticalOffset - _currentVerticalOffset;
            _rigidbody.MovePosition(transform.position + verticalDelta * transform.up + velocity * Time.fixedDeltaTime * transform.right);
            _currentVerticalOffset = verticalOffset;
        }

    }
}