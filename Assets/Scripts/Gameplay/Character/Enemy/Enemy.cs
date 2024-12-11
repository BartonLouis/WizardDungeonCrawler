using Gameplay;
using UnityEngine;
using UnityEngine.AI;


namespace Enemy {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour, IDamageable {

        [SerializeField] Transform target;

        NavMeshAgent _agent;

        private void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateUpAxis = false;
            _agent.updateRotation = false;
        }

        private void Update() {
            _agent.SetDestination(target.position);
        }

        public float Damage(IProjectileOwner owner, float damage) {
            return 0f;
        }
    }
}