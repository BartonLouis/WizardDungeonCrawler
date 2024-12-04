using Louis.Patterns.ServiceLocator;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGeneration {
    public interface IDungeonGeneratorService : IService {
        public Transform Transform { get; }
        public void Generate(int seed);
        public void Generate();
    }

    [RequireComponent(typeof(TilemapVisualiser))]
    public class DungeonGenerator : MonoBehaviour, IDungeonGeneratorService {

        public Transform Transform => transform;
        [Header("Settings")]
        [SerializeField] int _seed;

        [Header("Generation Steps")]
        [SerializeField] List<AbstractGenerationStep> _generationSteps;

        TilemapVisualiser _visualiser;

        private void OnEnable() {
            ServiceLocator.Register<IDungeonGeneratorService>(this);
        }

        private void OnDisable() {
            ServiceLocator.Deregister<IDungeonGeneratorService>(this);
        }

        private void Awake() {
            Generate();
        }

        public void Generate(int seed) {
            _seed = seed;
            Generate();
        }

        public void Generate() {
            ServiceLocator.Register<IDungeonGeneratorService>(this);
            if (Application.isPlaying) {
                foreach (Transform child in transform) {
                    Destroy(child.gameObject);
                }
            } else {
                for (int i = transform.childCount; i > 0; --i) {
                    DestroyImmediate(transform.GetChild(0).gameObject);
                }
            }


            DungeonInfo dungeon = new DungeonInfo(_seed, Vector2Int.zero);
            foreach (var step in _generationSteps) {
                step.Generate(dungeon);
            }

            _visualiser = GetComponent<TilemapVisualiser>();
            _visualiser.Clear();
            foreach (var tile in dungeon) {
                _visualiser.PaintSingleTile(new Vector2Int(tile.x, tile.y), tile.layer);
            }
        }

    }

    public interface IGenerationStep {
        public void Generate(DungeonInfo dungeon);
    }
}