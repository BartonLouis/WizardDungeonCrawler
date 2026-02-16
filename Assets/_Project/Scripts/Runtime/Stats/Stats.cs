using System;

namespace Stats {
    public interface IStatModifier { }

    [Serializable]
    public struct PrimaryStatModifier : IStatModifier {
        public PrimaryStatType statType;
        public ModifierType modifierType;
        public float value;
    }

    [Serializable]
    public struct SecondaryStatModifier : IStatModifier {
        public SecondaryStatType statType;
        public ModifierType modifierType;
        public float value;
    }

    public enum ModifierType {
        Additive,
        Multiplicative
    }


    public class PrimaryStat : IObservable<int> {
        public event Action<int> onValueChanged = delegate { };
        public PrimaryStatType Type { get; private set; }

        int _value;
        public int Value {
            get => _value;
            set {
                if(value == _value) return;
                _value = value;
                onValueChanged.Invoke(_value);
            }
        }
    }

    public class SecondaryStat : IObservable<float> {
        public event Action<float> onValueChanged = delegate { };
        public PrimaryStatType Type { get; private set; }

        float _value;
        public float Value {
            get => _value;
            set {
                if(value == _value) return;
                _value = value;
                onValueChanged.Invoke(_value);
            }
        }
    }

    [Serializable]
    public struct PrimaryStatValue {
        public PrimaryStatType type;
        public int value;

        public PrimaryStatValue(PrimaryStatType type, int value) {
            this.type = type;
            this.value = value;
        }
    }

    [Serializable]
    public struct SecondaryStatValue {
        public SecondaryStatType type;
        public int value;

        public SecondaryStatValue(SecondaryStatType type, int value) {
            this.type = type;
            this.value = value;
        }
    }

    public enum PrimaryStatType {
        Vitality,
        Strength,
        Agility,
        Arcana,
        Intelligence,
        Elloquence
    }

    public enum SecondaryStatType {
        MaxHealth,
        MoveSpeed,
        CarryCapacity
    }

    public interface IObservable<T> {
        public T Value { get; }
        public event Action<T> onValueChanged;
    }
}
