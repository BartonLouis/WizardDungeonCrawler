using CharacterMechanics.Stats;
using Louis.Patterns.ServiceLocator;

namespace Interfaces {
    public interface IPlayerStatsService : IService {
        PrimaryStat this[PrimaryStatTag tag] { get; }
        SecondaryStat this[SecondaryStatTag tag] { get; }

        public void GenerateStats();
    }
}