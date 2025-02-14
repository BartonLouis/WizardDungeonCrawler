using UnityEngine;

namespace Gameplay.Player.Stats {
    [CreateAssetMenu(menuName = "Data/Character/Scaling Settings")]
    public class PlayerSecondaryStatScalingSettings : ScriptableObject {
        [Header("Carry Capacity Scaling")]
        public float maxStrength = 500;
        public float strengthScaling = 7;

        [Header("Range Scaling")]
        public float accuracyGainPerLevel = 10;

        [Header("HP Scaling")]
        public float baseHealth = 100;
        public float hpPerLevel = 45;

        [Header("Defence Scaling")]
        public float defencePerLevel = 2.5f;

        [Header("Move Speed Scaling")]
        public float baseMoveSpeed = 7.5f;
        public float moveSpeedPerLevel = .4f;

        [Header("Dodge Scaling")]
        public float dodgeChancePerLevel = 4f;

        [Header("Crit Rate Scaling")]
        public float baseCritRate = 5;
        public float critRatePerLevel = 3.5f;

        [Header("Mod Slot Scaling")]
        public float baseModSlots = 1;
        public float levelsPerModSlot = 2;

        [Header("Accuracy Scaling")]
        [Header("Base Damage Scaling")]


        [Header("Mana Scaling")]
        [Header("Fire Rate Scaling")]
        [Header("Shop Price Scaling")]
        [Header("Passive HP Regen Scaling")]
        [Header("Life Steal Scaling")]
        [Header("Thorns Scaling")]
        [Header("Passive Mana Scaling")]
        [Header("Mana On Kill Scaling")]
        [Header("Luck Scaling")]
        public float luckPerLevel;




        private void OnValidate() {
            if (strengthScaling == 0) strengthScaling = 1;
        }
    }
}