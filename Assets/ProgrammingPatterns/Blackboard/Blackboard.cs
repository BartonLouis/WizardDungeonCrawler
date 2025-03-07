using System;
using System.Collections.Generic;

namespace Louis.Patterns.Blackboards {
    [Serializable]
    public class Blackboard {
        Dictionary<string, BlackboardKey> keyRegistry = new();
        Dictionary<BlackboardKey, object> entries = new();


        public BlackboardKey GetOrRegisterKey(string keyName) {
            Preconditions.CheckNotNull(keyName);

            if (!keyRegistry.TryGetValue(keyName, out BlackboardKey key)) {
                key = new BlackboardKey(keyName);
                keyRegistry[keyName] = key;
            }

            return key;
        }

        public bool ContainsKey(BlackboardKey key) => entries.ContainsKey(key);
        public void Remove(BlackboardKey key) => entries.Remove(key);

        public bool TryGetValue<T>(BlackboardKey key, out T value) {
            if (entries.TryGetValue(key, out var entry) && entry is BlackboardEntry<T> castedEntry) {
                value = castedEntry.Value;
                return true;
            }

            value = default;
            return false;
        }

        public void SetValue<T>(BlackboardKey key, T value) {
            entries[key] = new BlackboardEntry<T>(key, value);
        }

        public void Debug() {
            foreach (var entry in entries) {
                var entryType = entry.Value.GetType();
                if (entryType.IsGenericType && entryType.GetGenericTypeDefinition() == typeof(BlackboardEntry<>)) {
                    var valueProperty = entryType.GetProperty("Value");
                    if (valueProperty == null) continue;
                    var value = valueProperty.GetValue(entry.Value);
                    UnityEngine.Debug.Log($"Key {entry.Key}, Value: {value}");
                }
            }
        }
    }

    [Serializable]
    public struct BlackboardKey : IEquatable<BlackboardKey> {
        readonly string name;
        readonly int hashedKey;

        public BlackboardKey(string name) {
            this.name = name;
            hashedKey = name.ComputeHash();
        }

        public bool Equals(BlackboardKey other) => hashedKey == other.hashedKey;

        public override bool Equals(object obj) => obj is BlackboardKey other && Equals(other);
        public override int GetHashCode() => hashedKey;
        public override string ToString() => name;

        public static bool operator ==(BlackboardKey lhs, BlackboardKey rhs) => lhs.hashedKey == rhs.hashedKey;
        public static bool operator !=(BlackboardKey lhs, BlackboardKey rhs) => lhs.hashedKey != rhs.hashedKey;


    }

    [Serializable]
    public struct BlackboardEntry<T> {
        public BlackboardKey Key { get; }
        public T Value { get; }
        public Type ValueType { get; }

        public BlackboardEntry(BlackboardKey key, T value) {
            Key = key;
            Value = value;
            ValueType = typeof(T);
        }

        public override bool Equals(object obj) => obj is BlackboardEntry<T> other && other.Key == Key;
        public override int GetHashCode() => Key.GetHashCode();
    }
}