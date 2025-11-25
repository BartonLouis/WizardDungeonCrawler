using UnityEngine;

namespace Stats {
    public class StatsContainer : MonoBehaviour {
        [SerializeField] Stats _baseStats;

        private void OnValidate() {
            _baseStats.Validate();
        }
    }
}