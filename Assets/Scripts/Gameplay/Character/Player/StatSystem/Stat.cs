using System;

namespace CharacterMechanics.Stats {
    public interface IStat<T> : Louis.Patterns.Observable.IObservable<T> {
        public void CalculateValue();
    }

    [Flags]
    public enum PrimaryStatTag {
        None = 0,
        Strength = 1,
        Vitality = 2,
        Dexterity = 4,
        Intelligence = 8,
        Arcane = 16,
        Eloquence = 32
    }

    [Flags]
    public enum SecondaryStatTag {
        None            = 0,
        CarryCapacity   = 1,
        Range           = 2,
        HP              = 4,
        Defence         = 8,
        MoveSpeed       = 16,
        Dodge           = 32,
        CritRate        = 64,
        ModSlots        = 128,
        Accuracy        = 256,
        BaseDamage      = 512,
        Mana            = 1024,
        FireRate        = 2048,
        ShopPrice       = 4096,
        PassiveHPRegen  = 8192,
        LifeSteal       = 32768,
        PassiveManaRegen= 65536,
        ManaOnKill      = 131072,
        Luck            = 262144
    }

}