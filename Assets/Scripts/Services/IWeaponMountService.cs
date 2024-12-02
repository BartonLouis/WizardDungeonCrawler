using Louis.Patterns.ServiceLocator;


namespace Weapons {
    public interface IWeaponMountService : IService {
        void MountWeapon(Weapon weapon);
    }
}