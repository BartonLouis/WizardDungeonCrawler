using System;

namespace Louis.Patterns.ServiceLocator {
    static class ServiceDefinition<T> where T : IService {
        public static T Service => _service;
        public static ServiceAvailabilityStatus Status => _status;
        public static event Action<Type, ServiceAvailabilityStatus, T> onStatusChanged = (a1, a2, a3) => { };


        static Type serviceType;
        static T _service = default;
        static ServiceAvailabilityStatus _status = ServiceAvailabilityStatus.Unavailable;


        public static void OverrideService(T service) {
            if (service != null) {
                serviceType = typeof(T);
            }
            _service = service;
            _status = _service == null ? ServiceAvailabilityStatus.Unavailable : ServiceAvailabilityStatus.Available;
            onStatusChanged.Invoke(serviceType, Status, Service);
        }

        private static void Clear() {
            onStatusChanged = null;
        }

    }


    public enum ServiceAvailabilityStatus {
        Available,
        Unavailable
    }
}