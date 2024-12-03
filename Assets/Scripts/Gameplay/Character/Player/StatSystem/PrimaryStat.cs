using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CharacterMechanics.Stats {
    public class PrimaryStat : IStat<int> {
        public PrimaryStatTag Tag { get; private set; }
        private int _value;
        public int Value {
            get => _value;
            protected set {
                _value = value;
                onChanged?.Invoke(_value);
            }
        }
        public event Action<int> onChanged;
        protected PlayerStatsManager _player;

        public PrimaryStat(PlayerStatsManager player, PrimaryStatTag tag) {
            _player = player;
            Tag = tag;
        }

        public void CalculateValue() {
            Value = _player.GetBaseStat(Tag) + _player.GetInvestedPoints(Tag) + Enumerable.Sum(_player.GetPrimaryModifiers(Tag), m => m.value);
            Value = Mathf.Clamp(Value, PlayerStatsManager.STAT_MIN_VALUE, PlayerStatsManager.STAT_MAX_VALUE);
        }

        public class PrimaryStatComparer : IEqualityComparer<PrimaryStat> {
            public bool Equals(PrimaryStat x, PrimaryStat y) {
                return x.Tag == y.Tag;
            }

            public int GetHashCode(PrimaryStat obj) {
                return obj.GetHashCode();
            }
        }
    }

}