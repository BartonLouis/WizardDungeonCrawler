using System;
using UnityEngine;

namespace CharacterMechanics.Stats {
    public abstract class SecondaryStat : IStat<float> {
        public abstract SecondaryStatTag Tag { get; }
        public abstract PrimaryStatTag ScalesOnTag { get; }
        public abstract string Unit { get; }
        private float _value;
        public float Value {
            get => _value;
            protected set {
                _value = value;
                onChanged?.Invoke(value);
            }
        }
        public event Action<float> onChanged;
        protected PlayerStatsManager _player;

        public SecondaryStat(PlayerStatsManager player) {
            _player = player;
        }

        public abstract void CalculateValue();
    }


    public class CarryCapacityStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.CarryCapacity;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Strength;
        public override string Unit => "Kg";
        public CarryCapacityStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            float val = _player.ScalingSettings.maxStrength *
                MathF.Pow(MathF.E,
                ((float)_player[PrimaryStatTag.Strength].Value - PlayerStatsManager.STAT_MAX_VALUE)
                / _player.ScalingSettings.strengthScaling);
            Value = Mathf.CeilToInt(val);
        }
    }

    public class RangeStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.Range;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Strength;
        public override string Unit => "%";
        public RangeStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = Mathf.Clamp(100 + _player.ScalingSettings.accuracyGainPerLevel * ((float)_player[PrimaryStatTag.Strength].Value - 10), 1, float.MaxValue);
        }
    }

    public class HPStat : SecondaryStat {
        static readonly float BASE_HEALTH = 100f;
        static readonly float HP_PER_LEVEL = 45f;

        public override SecondaryStatTag Tag => SecondaryStatTag.HP;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Vitality;
        public override string Unit => "HP";
        public HPStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            int vitality = _player[PrimaryStatTag.Vitality].Value;
            float health;
            if (vitality <= 10) {
                health = BASE_HEALTH + BASE_HEALTH * (vitality - 10) / 10;
            } else {
                health = BASE_HEALTH + HP_PER_LEVEL * (vitality - 10);
            }
            Value = Mathf.Clamp(health, 1, float.MaxValue);
        }
    }

    public class DefenceStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.Defence;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Vitality;
        public override string Unit => "%";
        public DefenceStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = _player.ScalingSettings.defencePerLevel * (_player[PrimaryStatTag.Vitality].Value - 10);
        }
    }

    public class MoveSpeedStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.MoveSpeed;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Dexterity;
        public override string Unit => "m/s";
        public MoveSpeedStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            int dex = _player[PrimaryStatTag.Dexterity].Value;
            Value = _player.ScalingSettings.baseMoveSpeed + (dex - 10) * _player.ScalingSettings.moveSpeedPerLevel;
        }
    }

    public class DodgeStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.Dodge;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Dexterity;
        public override string Unit => "%";
        public DodgeStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            int dex = _player[PrimaryStatTag.Dexterity].Value;
            if (dex <= 10) {
                Value = 0;
            } else {
                Value = _player.ScalingSettings.dodgeChancePerLevel * (dex - 10);
            }
        }
    }

    public class CritRateStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.CritRate;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Dexterity;
        public override string Unit => "%";
        public CritRateStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            int dex = _player[PrimaryStatTag.Dexterity].Value;
            float critRate = _player.ScalingSettings.baseCritRate + (dex - 10) * _player.ScalingSettings.critRatePerLevel;
            Value = Mathf.Clamp(critRate, 0f, 100f);
        }
    }

    public class ModSlotsStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.ModSlots;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Intelligence;
        public override string Unit => "Slots";
        public ModSlotsStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            float intelligence = _player[PrimaryStatTag.Intelligence].Value;
            float slots;
            if (intelligence <= 10) {
                slots = _player.ScalingSettings.baseModSlots;
            } else {
                slots = _player.ScalingSettings.baseModSlots + (intelligence - 10) / _player.ScalingSettings.levelsPerModSlot; ;
            }
            Value = Mathf.FloorToInt(slots);
        }
    }

    public class AccuracyStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.Accuracy;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Intelligence;
        public override string Unit => "Deg";
        public AccuracyStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = _player[PrimaryStatTag.Intelligence].Value;
        }
    }

    public class BaseDamageStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.BaseDamage;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Arcane;
        public override string Unit => "%";

        public BaseDamageStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = _player[PrimaryStatTag.Arcane].Value;
        }
    }

    public class ManaStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.Mana;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Arcane;
        public override string Unit => "FP";

        public ManaStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = _player[PrimaryStatTag.Arcane].Value;
        }
    }

    public class FireRateStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.FireRate;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Eloquence;
        public override string Unit => "/s";

        public FireRateStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = _player[PrimaryStatTag.Eloquence].Value;
        }
    }

    public class ShopPriceStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.ShopPrice;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Eloquence;
        public override string Unit => "%";

        public ShopPriceStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = _player[PrimaryStatTag.Eloquence].Value;
        }
    }

    public class PassiveHPRegenStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.PassiveHPRegen;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Vitality | PrimaryStatTag.Arcane;
        public override string Unit => "HP/s";

        public PassiveHPRegenStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = 0;
        }
    }

    public class LifeStealStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.LifeSteal;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Vitality | PrimaryStatTag.Intelligence;
        public override string Unit => "HP/k";

        public LifeStealStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = 0;
        }
    }

    public class ThornsStat : SecondaryStat {
        public override SecondaryStatTag Tag => throw new NotImplementedException();
        public override PrimaryStatTag ScalesOnTag => throw new NotImplementedException();
        public override string Unit => "%";

        public ThornsStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = 0;
        }
    }

    public class PassiveManaRegenStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.PassiveManaRegen;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Arcane | PrimaryStatTag.Eloquence;
        public override string Unit => "FP/s";

        public PassiveManaRegenStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = 0;
        }
    }

    public class ManaOnKillStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.ManaOnKill;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Strength | PrimaryStatTag.Dexterity;
        public override string Unit => "FP/k";

        public ManaOnKillStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = 0;
        }
    }

    public class LuckStat : SecondaryStat {
        public override SecondaryStatTag Tag => SecondaryStatTag.Luck;
        public override PrimaryStatTag ScalesOnTag => PrimaryStatTag.Eloquence | PrimaryStatTag.Arcane;
        public override string Unit => "%";

        public LuckStat(PlayerStatsManager player) : base(player) { CalculateValue(); }

        public override void CalculateValue() {
            Value = 0;
        }
    }
}