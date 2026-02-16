using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stats {


    [CreateAssetMenu(menuName = "Data/Character/StatBlock")]
    public class ScriptableStatBlock : ScriptableObject, IStatBlock {
        [SerializeField] List<PrimaryStatValue> _stats;

        public PrimaryStatValue this[PrimaryStatType type] {
            get => _stats.Find(s => s.type == type);
            set {
                int index = _stats.FindIndex(s => s.type == type);
                if(index >= 0) {
                    _stats[index] = value;
                } else {
                    _stats.Add(value);
                }
            }
        }

        public void SetValues(IStatBlock other) {
            foreach(var type in Enum.GetValues(typeof(PrimaryStatType)).Cast<PrimaryStatType>()) {
                this[type] = other[type];
            }
        }

        private void OnValidate() {
            _stats ??= new List<PrimaryStatValue>();

            // Get all enum values
            var enumValues = Enum.GetValues(typeof(PrimaryStatType))
                             .Cast<PrimaryStatType>()
                             .ToList();

            // Remove any entries whose type is no longer in the enum (handles deleted/changed enum entries)
            _stats.RemoveAll(x => !enumValues.Contains(x.type));

            // Ensure each enum value exists exactly once
            foreach(var type in enumValues) {
                if(!_stats.Any(x => x.type == type)) {
                    _stats.Add(new PrimaryStatValue(type, 10));
                }
            }

            // If duplicates exist, keep the first and remove the rest
            foreach(var type in enumValues) {
                var duplicates = _stats.Where(x => x.type == type).Skip(1).ToList();
                foreach(var d in duplicates)
                    _stats.Remove(d);
            }

            // Clamp values between 0 and 20
            for(int i = 0; i < _stats.Count; i++) {
                var item = _stats[i];
                item.value = Mathf.Clamp(item.value, 0, 20);
                _stats[i] = item; // Required because it's a struct
            }

            // Sort by enum order
            _stats = _stats
                .OrderBy(x => x.type)
                .ToList();
        }
    }

    [Serializable]
    public class SerializedStatBlock : IStatBlock {
        [SerializeField] List < PrimaryStatValue > _stats;

        public PrimaryStatValue this[PrimaryStatType type] {
            get => _stats.Find(s => s.type == type);
            set {
                int index = _stats.FindIndex(s => s.type == type);
                if(index >= 0) {
                    _stats[index] = value;
                } else {
                    _stats.Add(value);
                }
            }
        }

        public SerializedStatBlock(IStatBlock other) {
            _stats = new List<PrimaryStatValue>();
            foreach(var type in Enum.GetValues(typeof(PrimaryStatType)).Cast<PrimaryStatType>()) {
                _stats.Add(other[type]);
            }
            Validate();
        }

        public SerializedStatBlock() {
            Validate();
        }

        public void Validate() {
            _stats ??= new List<PrimaryStatValue>();

            // Get all enum values
            var enumValues = Enum.GetValues(typeof(PrimaryStatType))
                             .Cast<PrimaryStatType>()
                             .ToList();

            // Remove any entries whose type is no longer in the enum (handles deleted/changed enum entries)
            _stats.RemoveAll(x => !enumValues.Contains(x.type));

            // Ensure each enum value exists exactly once
            foreach(var type in enumValues) {
                if(!_stats.Any(x => x.type == type)) {
                    _stats.Add(new PrimaryStatValue(type, 10));
                }
            }

            // If duplicates exist, keep the first and remove the rest
            foreach(var type in enumValues) {
                var duplicates = _stats.Where(x => x.type == type).Skip(1).ToList();
                foreach(var d in duplicates)
                    _stats.Remove(d);
            }

            // Clamp values between 0 and 20
            for(int i = 0; i < _stats.Count; i++) {
                var item = _stats[i];
                item.value = Mathf.Clamp(item.value, 0, 20);
                _stats[i] = item; // Required because it's a struct
            }

            // Sort by enum order
            _stats = _stats
                .OrderBy(x => x.type)
                .ToList();
        }
    }
}