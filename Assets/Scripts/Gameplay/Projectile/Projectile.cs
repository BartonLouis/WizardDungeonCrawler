using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Gameplay.Projectiles {
    [RequireComponent(typeof(Light2D))]
    public class Projectile : MonoBehaviour {
        Light2D _light;

        public IProjectileOwner Owner { get; private set; }
        public ProjectileConfig Config { get; private set; }
        public float CurrentVerticalOffset { get; set; }
        public float Time { get; set; }


        private void Awake() {
            _light = GetComponent<Light2D>();
        }

        public void Fire(IProjectileOwner owner, Vector2 start, Quaternion direction, ProjectileConfig config) {
            Owner = owner;
            Config = config;
            transform.SetPositionAndRotation(start, direction);
            transform.localScale = Config.projectileRadius * Vector2.one;

            _light.color = config.lightColor;
            _light.pointLightInnerRadius = config.lightInnerRadius;
            _light.pointLightOuterRadius = config.lightOuterRadius;
            _light.intensity = config.lightIntensity;
            Time = 0;
            CurrentVerticalOffset = 0;
        }
    }
}