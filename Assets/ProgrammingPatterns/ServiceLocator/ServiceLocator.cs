using System;

namespace Louis.Patterns.ServiceLocator {

    public static class ServiceLocator {
        public static T GetService<T>() where T : IService {
            return ServiceDefinition<T>.Service;
        }

        public static bool TryGetService<T>(out T service) where T : IService {
            if (ServiceDefinition<T>.Service == null) {
                service = default;
                return false;
            } else {
                service = ServiceDefinition<T>.Service;
                return true;
            }

        }

        public static void Register<T>(T service) where T : IService {
            ServiceDefinition<T>.OverrideService(service);
        }

        public static void Deregister<T>(T service) where T : IService {
            if ((IService)service == (IService)ServiceDefinition<T>.Service) {
                ServiceDefinition<T>.OverrideService(default);
            }
        }

        public static void AddServiceStatusChangeListener<T>(Action<Type, ServiceAvailabilityStatus, T> callback) where T : IService {
            ServiceDefinition<T>.onStatusChanged += callback;
            callback?.Invoke(typeof(T), ServiceDefinition<T>.Status, ServiceDefinition<T>.Service);
        }

        public static void RemoveServiceStatusChangeListener<T>(Action<Type, ServiceAvailabilityStatus, T> callback) where T : IService {
            ServiceDefinition<T>.onStatusChanged -= callback;
        }
    }

    public interface IService { }
}