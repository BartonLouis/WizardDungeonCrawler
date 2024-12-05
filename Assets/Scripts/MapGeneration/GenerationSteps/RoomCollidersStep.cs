using Gameplay.Rooms;
using Louis.Patterns.ServiceLocator;
using Managers;
using UnityEngine;

namespace DungeonGeneration {

    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Room Collider Placement Step")]
    public class RoomColliderStep : AbstractGenerationStep {

        [Header("Settings")]
        [SerializeField] RoomCollider _prefab;

        public override void Generate(DungeonInfo dungeon) {
            ServiceLocator.TryGetService<IDungeonGeneratorService>(out var generatorService);
            Transform parent = generatorService != null ? generatorService.Transform : null;
            foreach (var room in dungeon.Rooms) {
                RoomCollider collider = Instantiate(_prefab);
                collider.transform.parent = parent;
                collider.Init(room);
            }
        }
    }
}