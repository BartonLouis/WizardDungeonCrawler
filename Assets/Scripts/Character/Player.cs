using CharacterMechanics.Stats;
using Louis.Patterns.ServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace CharacterMechanics {
    public class Player : MonoBehaviour, IService {
        public static readonly int STAT_MIN_VALUE = 1;
        public static readonly int STAT_MAX_VALUE = 30;

        [Header("Stats")]
        [field: SerializeField] public PlayerSecondaryStatScalingSettings ScalingSettings { get; private set; }
        [SerializeField] PlayerBaseStats _baseStats;
        [SerializeField] List<PrimaryStat> primaryStats = new();
        [SerializeField] List<SecondaryStat> secondaryStats = new();
        [Header("Modifiers")]
        [SerializeField] List<PrimaryStatValue> investedPoints = new();
        [SerializeField] List<PrimaryStatValue> primaryModifiers = new();
        [SerializeField] List<SecondaryStatValue> secondaryModifiers = new();


        Dictionary<PrimaryStatTag, PrimaryStat> primaryStatsDict = new();
        Dictionary<SecondaryStatTag, SecondaryStat> secondaryStatsDict = new();
        public PrimaryStat this[PrimaryStatTag tag] => primaryStatsDict[tag];
        public SecondaryStat this[SecondaryStatTag tag] => secondaryStatsDict[tag];

        private void Awake() {
            GenerateStats();
        }

        private void OnEnable() {
            ServiceLocator.Register(this);
        }

        private void OnDisable() {
            ServiceLocator.Register(this);
        }

        void ValidateStats() {
            // Make sure every stat appears at least once in primary stats and invested points lists
            foreach (var stat in EnumUtils.GetValues<PrimaryStatTag>()) {
                if (stat == PrimaryStatTag.None) continue;
                if (!primaryStats.Exists(s => s.Tag == stat)) {
                    PrimaryStat s = StatFactory.CreatePrimaryStat(this, stat);
                    primaryStats.Add(s);
                    primaryStatsDict[stat] = s;
                }
                if (!investedPoints.Exists(s => s.stat == stat)) {
                    investedPoints.Add(new PrimaryStatValue(stat));
                }
            }

            // Make sure there are no duplicates
            for (int i = primaryStats.Count() - 1; i >= 0; i--) {
                if (EnumUtils.MoreThanOneFlag(primaryStats[i].Tag)) {
                    primaryStats.RemoveAt(i);
                }
            }

            for (int i = investedPoints.Count() - 1; i >= 0; i--) {
                if (EnumUtils.MoreThanOneFlag(investedPoints[i].stat)) {
                    investedPoints.RemoveAt(i);
                }
            }
            primaryStats = primaryStats.Distinct(new PrimaryStat.PrimaryStatComparer()).ToList();
            primaryStats = primaryStats.OrderBy(s => (int)s.Tag).ToList();
            investedPoints = investedPoints.Distinct(new PrimaryStatValue.PrimaryStatValueComparer()).ToList();
            investedPoints = investedPoints.OrderBy(s => (int)s.stat).ToList();



            // Make sure every secondary stat appears in secondary stats list
            foreach (var stat in EnumUtils.GetValues<SecondaryStatTag>()) {
                if (stat == SecondaryStatTag.None) continue;
                if (!secondaryStats.Exists(s => s.Tag == stat)) {
                    SecondaryStat s = StatFactory.CreateSecondaryStat(this, stat);
                    secondaryStatsDict[stat] = s;
                    secondaryStats.Add(s);
                }
            }
        }

        public int GetBaseStat(PrimaryStatTag tag) {
            return _baseStats[tag];
        }

        public int GetInvestedPoints(PrimaryStatTag tag) {
            return investedPoints.Sum(v => v.stat == tag ? v.value : 0);
        }

        public IEnumerable<PrimaryStatValue> GetPrimaryModifiers(PrimaryStatTag tag) {
            return primaryModifiers.Where(m => (m.stat & tag) != 0);
        }

        public IEnumerable<SecondaryStatValue> GetSecondaryModifiers(SecondaryStatTag tag) {
            return secondaryModifiers.Where(m => (m.stat & tag) != 0);
        }

        [ContextMenu("Generate Stats")]
        public void GenerateStats() {
            ValidateStats();
            foreach (var stat in primaryStats) {
                stat.CalculateValue();
                primaryStatsDict[stat.Tag] = stat;
            }
            foreach (var stat in secondaryStats) {
                stat.CalculateValue();
                secondaryStatsDict[stat.Tag] = stat;
            }
        }

        private void OnValidate() {
            GenerateStats();
            foreach (var stat in investedPoints) {
                stat.Validate();
            }
            foreach (var stat in primaryModifiers) {
                stat.Validate();
            }
            foreach (var stat in secondaryModifiers) {
                stat.Validate();
            }
        }
    }

    [Serializable]
    public class PrimaryStatValue {
        [HideInInspector] public string name;
        public PrimaryStatTag stat;
        public int value;

        public PrimaryStatValue(PrimaryStatTag stat) {
            this.stat = stat;
            value = 0;
            name = stat + ": " + value;
        }

        public void Validate() {
            name = stat + ": " + value;
        }

        public class PrimaryStatValueComparer : IEqualityComparer<PrimaryStatValue> {
            public bool Equals(PrimaryStatValue x, PrimaryStatValue y) {
                return x.stat == y.stat;
            }

            public int GetHashCode(PrimaryStatValue obj) {
                return (int)obj.stat;
            }
        }
    }

    [Serializable]
    public class SecondaryStatValue {
        [HideInInspector] public string name;
        public SecondaryStatTag stat;
        public int value;

        public SecondaryStatValue(SecondaryStatTag stat) {
            this.stat = stat;
            value = 0;
            name = stat + ": " + value;
        }

        public void Validate() {
            name = stat + ": " + value;
        }
    }
}