using System;
using System.Collections.Generic;
using UnityEngine;

namespace Stats {
    [Serializable]
    public class Stats {
        Dictionary<StatType, Stat> _stats;
        [SerializeField] List<StatValue> _baseStats;
        [SerializeField] List<StatValue> _investedPoints;
        [SerializeField] List<StatValue> _modifiers;

        public void Validate() {// Convert enum to array once
            var allTypes = (StatType[])Enum.GetValues(typeof(StatType));

            // Remove duplicates / unknown entries
            // (HashSet keeps track of what we've seen)
            var baseStatsSeen = new HashSet<StatType>();
            for(int i = _baseStats.Count - 1; i >= 0; i--) {
                var t = _baseStats[i].type;
                if(!Enum.IsDefined(typeof(StatType), t) || !baseStatsSeen.Add(t)) {
                    _baseStats.RemoveAt(i);
                }
            }

            var investedPointsSeen = new HashSet<StatType>();
            for(int i = _investedPoints.Count - 1; i >= 0; i--) {
                var t = _investedPoints[i].type;
                if(!Enum.IsDefined(typeof(StatType), t) || !investedPointsSeen.Add(t)) {
                    _investedPoints.RemoveAt(i);
                }
            }

            // Add any missing types
            foreach(var t in allTypes) {
                if(!baseStatsSeen.Contains(t)) {
                    _baseStats.Add(new StatValue(t, 0));
                }
                if (!investedPointsSeen.Contains(t)) {
                    _investedPoints.Add(new StatValue(t, 0));
                }
            }

            // Finally: reorder list to match enum order
            _baseStats.Sort((a, b) => a.type.CompareTo(b.type));
            _investedPoints.Sort((a, b) => a.type.CompareTo(b.type));
        }
    }



    public class Stat : IObservable<int> {
        public event Action<int> onValueChanged = delegate { };
        public StatType Type { get; private set; }

        int _value;
        public int Value { 
            get => _value;
            set {
                if (value == _value) return;
                _value = value;
                onValueChanged.Invoke(_value);
            }
        }
    }

    [Serializable]
    public struct StatValue {
        public StatType type;
        public int value;

        public StatValue(StatType type, int value) {
            this.type = type;
            this.value = value;
        }
    }

    public enum StatType {
        Vitality,
        Strength,
        Agility,
        Arcana,
        Intelligence,
        Elloquence
    }




    public interface IObservable<T> {
        public T Value { get; }
        public event Action<T> onValueChanged;
    }
}
