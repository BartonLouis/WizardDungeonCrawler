using Louis.Patterns.ServiceLocator;
using Gameplay.Weapons;
using Gameplay;


namespace Services {
    public interface IWeaponMountService : IService {
        IProjectileOwner MountWeapon(Weapon weapon);
    }
}