using System.Collections.Generic;
using System.Linq;


namespace Louis.Patterns.EventSystem {
    public static class EventBus<T> where T : IEvent {
        static readonly HashSet<IEventBinding<T>> bindings = new HashSet<IEventBinding<T>>();

        public static void Register(IEventBinding<T> binding) => bindings.Add(binding);
        public static void Deregister(IEventBinding<T> binding) => bindings.Remove(binding);

        public static void Raise(T @event) {
            List<IEventBinding<T>> copy = bindings.ToList();
            foreach (var binding in copy) {
                binding.OnEvent.Invoke(@event);
                binding.OnEventNoArgs.Invoke();
            }
        }

        static void Clear() {
            bindings.Clear();
        }
    }

    public static class EventBus {
        public static void Raise<T>(T @event) where T : IEvent {
            EventBus<T>.Raise(@event);
        }

        public static void Register<T>(IEventBinding<T> binding) where T : IEvent {
            EventBus<T>.Register(binding);
        }
        public static void Deregister<T>(IEventBinding<T> binding) where T : IEvent {
            EventBus<T>.Deregister(binding);
        }
    }

    public interface IEvent { }
}