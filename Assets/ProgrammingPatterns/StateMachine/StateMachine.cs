using Louis.Patterns.Blackboards;

namespace Louis.Patterns.StateMachine {
    public class StateMachine<T> where T : IState {
        public T CurrentState { get; private set; }
        protected Blackboard blackboard { get; }

        public StateMachine(Blackboard blackboard) {
            this.blackboard = blackboard;
        }

        public void SetState(T state) {
            CurrentState?.OnStateLeave();
            CurrentState = state;
            CurrentState.OnStateEnter();
        }
    }

    public interface IState {
        public void OnStateEnter();
        public void OnStateLeave();
        public void LogicUpdate();
        public void PhysicsUpdate();
    }
}