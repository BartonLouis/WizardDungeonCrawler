using Louis.Patterns.ServiceLocator;
using Managers;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DungeonGeneration {

    [RequireComponent(typeof(TilemapVisualiser))]
    public class DungeonGenerator : MonoBehaviour, IDungeonGeneratorService {

        public Transform Transform => transform;
        [Header("References")]
        [SerializeField] Transform _labelCanvas;
        [SerializeField] TextMeshProUGUI _labelPrefab;

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
                try {
                    step.Generate(dungeon);
                } catch (Exception ex) {
                    Logging.Log(this, $"Step {step.name} Failed generation Step: {ex.StackTrace}", LogLevel.Error);
                }
            }

            _visualiser = GetComponent<TilemapVisualiser>();
            _visualiser.Clear();
            _visualiser.Fill(TileLayer.Floor, new Vector3Int(-dungeon.Width / 2, -dungeon.Height / 2), new Vector3Int(dungeon.Width / 2, dungeon.Height / 2));
            foreach (var tile in dungeon) {
                _visualiser.PaintSingleTile(new Vector2Int(tile.x, tile.y), tile.layer);
            }
            DateTime end = DateTime.Now;

            dungeon.ShowRoomLabels(_labelCanvas, _labelPrefab);
            Logging.Log(this, $"Generated a dungeon with Size: {dungeon.Width}, {dungeon.Height} in {(end - start).Seconds}.{(end - start).Milliseconds} seconds");
        }
    }

    public interface IGenerationStep {
        public void Generate(Dungeon dungeon);
    }
}