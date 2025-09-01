using Louis.CustomPackages.Logging;
using Map.Generation.GenerationSteps;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

namespace Map.Generation {
    public class MapGenerator : MonoBehaviour {

        [Header("Settings")]
        [SerializeField] int _seed;
        [SerializeField] bool _regenerateTilemapAfterEachStep = true;
        [SerializeField] bool _generateInstant = true;
        [SerializeField] MapGenerationSettings _settings;
        [SerializeField] TilemapGenerator _tilemap;
        [SerializeReference] Map _map;
        Random _random;

        int currentIndex;

        private void Start() {
            ResetGeneration();
        }

        private void Update() {
            if(Keyboard.current.spaceKey.wasPressedThisFrame) {
                if(_generateInstant) GenerateInstant();
                else RunStep();
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
            _tilemap = GetComponent<TilemapGenerator>();
            _tilemap.Clear();
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
            if(_regenerateTilemapAfterEachStep)
                _tilemap.GenerateTilemap(_map);
            sw.Stop();
            Logging.Log(this, $"Generating Step {_settings[currentIndex].GetType().Name} in {sw.Elapsed.TotalMilliseconds}ms");
            currentIndex++;
            if(step.GetType() == typeof(NoopGenerationStep) && !_regenerateTilemapAfterEachStep)
                _tilemap.GenerateTilemap(_map);
        }

        public void GenerateInstant() {
            ResetGeneration();
            Stopwatch sw = new();
            sw.Start();
            foreach(var step in _settings.Steps) {
                step.ApplyStep(_map, _random);
            }
            _tilemap.GenerateTilemap(_map);
            sw.Stop();
            Logging.Log($"Generated a map of size {_map.Size} in {sw.Elapsed.TotalMilliseconds}ms");
        }
    }
}