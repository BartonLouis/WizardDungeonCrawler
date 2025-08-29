using Louis.CustomPackages.Logging;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

namespace Map.Generation {
    public class MapGenerator : MonoBehaviour {

        [Header("Settings")]
        [SerializeField] int _seed;
        [SerializeField] MapGenerationSettings _settings;
        [SerializeReference] Map _map;
        Random _random;

        int currentIndex;

        private void Start() {
            currentIndex = 0;
            _random = new Random(_seed);
            _map = new();
        }

        private void Update() {
            if(Keyboard.current.spaceKey.wasPressedThisFrame) {
                RunStep();
            }
            _map.Draw();
        }

        public void RunStep() {
            if(_map == null) return;
            IMapGenerationStep step = _settings[currentIndex];
            Logging.Log(this, $"Generating Step {_settings[currentIndex]}");
            step.ApplyStep(_map, _random);
            currentIndex++;
        }
    }
}