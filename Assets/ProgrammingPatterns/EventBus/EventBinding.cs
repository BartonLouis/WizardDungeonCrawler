using System;

namespace Louis.Patterns.EventSystem {
    public interface IEventBinding<T> : IDisposable {
        public Action<T> OnEvent { get; set; }
        public Action OnEventNoArgs { get; set; }
    }

    public class EventBinding<T> : IEventBinding<T> where T : IEvent {
        #region Properties
        Action<T> onEvent = _ => { };
        Action onEventNoArgs = () => { };

        public Action<T> OnEvent {
            get => onEvent;
            set => onEvent = value;
        }
        public Action OnEventNoArgs {
            get => onEventNoArgs;
            set => onEventNoArgs = value;
        }
        #endregion

        #region Constructors
        public EventBinding(bool autoRegister = true) {
            if (autoRegister) {
                EventBus.Register(this);
            }
        }
        public EventBinding(Action<T> onEvent, bool autoRegister = true) {
            this.onEvent = onEvent;
            if (autoRegister) {
                EventBus.Register(this);
            }
        }
        public EventBinding(Action onEvent, bool autoRegister = true) {
            this.onEventNoArgs = onEvent;
            if (autoRegister) {
                EventBus.Register(this);
            }
        }

        public void Dispose() {
            EventBus.Deregister(this);
        }
        #endregion

        #region Public Methods for adding and removing events
        public EventBinding<T> Add(Action onEvent) {
            onEventNoArgs += onEvent;
            return this;
        }
        public EventBinding<T> Remove(Action onEvent) {
            onEventNoArgs -= onEvent;
            return this;
        }

        public EventBinding<T> Add(Action<T> onEvent) {
            this.onEvent += onEvent;
            return this;
        }
        public EventBinding<T> Remove(Action<T> onEvent) {
            this.onEvent -= onEvent;
            return this;
        }
        #endregion
    }
}