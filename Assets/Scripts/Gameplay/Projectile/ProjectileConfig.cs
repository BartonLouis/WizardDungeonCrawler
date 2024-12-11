using CustomTypes;
using UnityEngine;

namespace Gameplay.Projectiles {
    [CreateAssetMenu(menuName = "Data/Projectiles/Projectile Config")]
    public class ProjectileConfig : ScriptableObject {
        [Header("Damage Settings")]
        public float baseDamage;
        public float baseSpeed;
        public float lifetime = 1;
        public int samplesPerSecond;
        public LayerMask collidesWith;


        [Header("Size")]
        public float projectileRadius = 0.25f;
        public float colliderRadius = 0.25f;

        [Header("Movement Settings")]
        public float maxVerticalOffset;
        public AnimationCurve verticalOffsetCurve = AnimationCurve.Constant(0, 1, 0);
        public AnimationCurve velocityCurve = AnimationCurve.Constant(0, 1, 1);

        [Header("Appearance")]
        public Color lightColor;
        public float lightIntensity;
        public float lightInnerRadius;
        public float lightOuterRadius;

        private void OnValidate() {
            if (lifetime <= 0) lifetime = 1;
            projectileRadius = Mathf.Clamp(projectileRadius, 0.01f, float.MaxValue);
            colliderRadius = Mathf.Clamp(colliderRadius, 0.01f, float.MaxValue);
            lightOuterRadius = Mathf.Clamp(lightOuterRadius, lightInnerRadius, float.MaxValue);
            lightInnerRadius = Mathf.Clamp(lightInnerRadius, 0, lightOuterRadius);
        }
    }
}