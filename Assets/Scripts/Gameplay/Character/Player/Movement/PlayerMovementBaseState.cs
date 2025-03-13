using CharacterMechanics;
using Louis.Patterns.StateMachine;
using UnityEngine;

namespace Gameplay.Player.Movement {
    public abstract class PlayerMovementBaseState : IState {
        protected readonly PlayerMovement player;
        public PlayerMovementBaseState(PlayerMovement movement) {
            this.player = movement;
        }

        public virtual void LogicUpdate() { }
        public virtual void OnStateEnter() { }
        public virtual void OnStateLeave() { }
        public virtual void PhysicsUpdate() { }
    }

    public class LockedState : PlayerMovementBaseState {
        public LockedState(PlayerMovement movement) : base(movement) { }
        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            player.RB.linearVelocity = Vector2.MoveTowards(
                player.RB.linearVelocity,
                Vector2.zero,
                Time.fixedDeltaTime * player.AccelerationCoefficient);
        }
    }

    public class FreeMoveState : PlayerMovementBaseState {
        public FreeMoveState(PlayerMovement movement) : base(movement) { }

        public override void PhysicsUpdate() {
            base.PhysicsUpdate();
            player.RB.linearVelocity = Vector2.MoveTowards(
                player.RB.linearVelocity,
                player.MoveSpeed * player.Movement,
                Time.fixedDeltaTime * player.MoveSpeed * player.AccelerationCoefficient);
        }
    }
}