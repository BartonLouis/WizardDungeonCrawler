using System;

namespace CharacterMechanics.Stats {
    public static class StatFactory {
        public static PrimaryStat CreatePrimaryStat(Player player, PrimaryStatTag tag) {
            return new PrimaryStat(player, tag);
        }

        public static SecondaryStat CreateSecondaryStat(Player player, SecondaryStatTag tag) {
            return tag switch { 
                SecondaryStatTag.None => new HPStat(player), 
                SecondaryStatTag.CarryCapacity => new CarryCapacityStat(player), 
                SecondaryStatTag.Range => new RangeStat(player), 
                SecondaryStatTag.HP => new HPStat(player), 
                SecondaryStatTag.Defence => new DefenceStat(player), 
                SecondaryStatTag.MoveSpeed => new MoveSpeedStat(player), 
                SecondaryStatTag.Dodge => new DodgeStat(player), 
                SecondaryStatTag.CritRate => new CritRateStat(player), 
                SecondaryStatTag.ModSlots => new ModSlotsStat(player), 
                SecondaryStatTag.Accuracy => new AccuracyStat(player), 
                SecondaryStatTag.BaseDamage => new BaseDamageStat(player), 
                SecondaryStatTag.Mana => new ManaStat(player), 
                SecondaryStatTag.FireRate => new FireRateStat(player), 
                SecondaryStatTag.ShopPrice => new ShopPriceStat(player),
                SecondaryStatTag.PassiveHPRegen => new PassiveHPRegenStat(player),
                SecondaryStatTag.LifeSteal => new LifeStealStat(player), 
                SecondaryStatTag.PassiveManaRegen => new PassiveManaRegenStat(player),
                SecondaryStatTag.ManaOnKill => new ManaOnKillStat(player),
                SecondaryStatTag.Luck => new LuckStat(player),
                _ => throw new ArgumentException("Cannot insantiate a stat for multiple stats at once"), };
        }
    }

}