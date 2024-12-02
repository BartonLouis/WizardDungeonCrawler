using System;

namespace Louis.Patterns.Observable {
    public interface IObservable<T> {
        event Action<T> onChanged;

        public T Value { get; }
    }
}