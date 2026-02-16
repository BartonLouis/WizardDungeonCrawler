using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stats {
    public interface IStatsContainer {
        PrimaryStat this[PrimaryStatType type] { get; }
        SecondaryStat this[SecondaryStatType type] { get; }

        IStatBlock BaseStats { get; set; }
        IStatBlock InvestedStats { get; set; }

        List<PrimaryStatModifier> PrimaryStatModifiers { get; }
        List<SecondaryStatModifier> SecondaryStatModifiers { get; }
    }

    public interface IStatBlock {
        PrimaryStatValue this[PrimaryStatType type] { get; set; }
    }


    public class StatsContainer : MonoBehaviour, IStatsContainer {
        Dictionary<PrimaryStatType, PrimaryStat> _primaryStats = new();
        Dictionary<SecondaryStatType, SecondaryStat> _secondaryStats = new();
        public PrimaryStat this[PrimaryStatType type] => _primaryStats[type];
        public SecondaryStat this[SecondaryStatType type] => _secondaryStats[type];

        // Base and Invested Stats
        [SerializeField] ScriptableStatBlock _baseStats;
        [SerializeField] SerializedStatBlock _investedStats;
        public IStatBlock BaseStats { get; set; }
        public IStatBlock InvestedStats { get; set; }

        // Modifiers
        [SerializeField] List<PrimaryStatModifier> _primaryStatModifiers;
        [SerializeField] List<SecondaryStatModifier> _secondaryStatModifiers;
        public List<PrimaryStatModifier> PrimaryStatModifiers => _primaryStatModifiers;
        public List<SecondaryStatModifier> SecondaryStatModifiers => _secondaryStatModifiers;

        private void Awake() {
            BaseStats = _baseStats;
            InvestedStats = _investedStats;
        }

        void CalculateStats() {

        }

        private void OnValidate() {
            _investedStats.Validate();
            foreach(PrimaryStatType type in Enum.GetValues(typeof(PrimaryStatType))) {
                if(!_primaryStats.ContainsKey(type)) {
                    _primaryStats[type] = new PrimaryStat();
                }
            }

            foreach(SecondaryStatType type in Enum.GetValues(typeof(SecondaryStatType))) {
                if(!_secondaryStats.ContainsKey(type)) {
                    _secondaryStats[type] = new SecondaryStat();
                }
            }
        }
    }
}