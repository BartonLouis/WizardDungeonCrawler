using Louis.Patterns.ServiceLocator;

namespace Gameplay.Player.Stats {
    public interface IPlayerStatsService : IService {
        PrimaryStat this[PrimaryStatTag tag] { get; }
        SecondaryStat this[SecondaryStatTag tag] { get; }

        public void GenerateStats();
    }
}