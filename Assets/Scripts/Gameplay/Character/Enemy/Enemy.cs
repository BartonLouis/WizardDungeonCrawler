using Gameplay;
using UnityEngine;
using UnityEngine.AI;


namespace Enemy {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour, IDamageable {

        [SerializeField] Transform target;

        NavMeshAgent _agent;
        public float health = 100;


        private void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateUpAxis = false;
            _agent.updateRotation = false;
        }

        private void Update() {
            _agent.SetDestination(target.position);
        }

        public float Damage(float damage) {
            float result = Mathf.Min(damage, health);
            health -= damage;
            if (health <= 0) {
                Destroy(gameObject);
            }
            return result;
        }
    }
}