using Louis.CustomPackages.Logging;
using System.Diagnostics;
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
            ResetGeneration();
        }

        private void Update() {
            if(Keyboard.current.spaceKey.wasPressedThisFrame) {
                RunStep();
            }
            if(Keyboard.current.backspaceKey.wasPressedThisFrame) {
                ResetGeneration();
            }
            if(Keyboard.current.escapeKey.wasPressedThisFrame) {
                Random rnd = new();
                _seed = rnd.Next();
                ResetGeneration();
            }
            _map.Draw();
        }

        void ResetGeneration() {
            currentIndex = 0;
            _random = new Random(_seed);
            _map = new();
        }

        public void RunStep() {
            if(_map == null) return;
            Stopwatch sw = new();
            sw.Start();
            IMapGenerationStep step = _settings[currentIndex];
            step.ApplyStep(_map, _random);
            sw.Stop();
            Logging.Log(this, $"Generating Step {_settings[currentIndex].GetType().Name} in {sw.Elapsed.TotalMilliseconds}ms");
            currentIndex++;
        }
    }
}