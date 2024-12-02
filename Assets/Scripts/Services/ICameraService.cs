using Louis.Patterns.ServiceLocator;
using UnityEngine;


namespace Managers.CameraManagement {
    public interface ICameraService : IService {
        public void SetTarget(Transform target);
    }
}