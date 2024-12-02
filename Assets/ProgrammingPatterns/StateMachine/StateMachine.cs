using Louis.Patterns.Blackboards;

namespace Louis.Patterns.StateMachine {
    public class StateMachine {
        public IState CurrentState { get; private set; }
        protected Blackboard blackboard { get; }

        public StateMachine(Blackboard blackboard) {
            this.blackboard = blackboard;
        }

        public void SetState(IState state) {
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