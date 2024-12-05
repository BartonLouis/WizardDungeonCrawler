using Louis.Patterns.ServiceLocator;
using Managers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace DungeonGeneration {

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

            // Find any existing dungeons and destroy them to make sure there is only one
            ServiceLocator.TryGetService<IDungeonCreationService>(out var oldDungeon);
            if (oldDungeon == null) {
                GameObject old = GameObject.Find("Dungeon");
                if (old != null) {
                    old.TryGetComponent(out oldDungeon);
                }
            }
            if (oldDungeon != null && oldDungeon.Transform != null) {
                if (Application.isPlaying) {
                    Destroy(oldDungeon.Transform.gameObject);
                } else {
                    DestroyImmediate(oldDungeon.Transform.gameObject);
                }
            }

            DateTime start = DateTime.Now;
            GameObject go = new GameObject("Dungeon");
            Dungeon dungeon = go.AddComponent<Dungeon>();
            dungeon.Init(_seed, Vector2Int.zero);
            foreach (var step in _generationSteps) {
                step.Generate(dungeon);
            }

            _visualiser = GetComponent<TilemapVisualiser>();
            _visualiser.Clear();
            foreach (var tile in dungeon) {
                _visualiser.PaintSingleTile(new Vector2Int(tile.x, tile.y), tile.layer);
            }
            DateTime end = DateTime.Now;
            Logging.Log(this, $"Generated a dungeon with Size: {dungeon.Width}, {dungeon.Height} in {(end - start).Seconds}.{(end - start).Milliseconds} seconds");
        }
    }

    public interface IGenerationStep {
        public void Generate(Dungeon dungeon);
    }
}