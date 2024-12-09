using Gameplay.Rooms;
using Louis.Patterns.ServiceLocator;
using UnityEngine;
using Utils;

namespace DungeonGeneration {
    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Draw Rooms Step")]
    public class DrawRoomBordersStep : AbstractGenerationStep {
        [Header("Settings")]
        [SerializeField] RoomCollider _prefab;

        public override void Generate(Dungeon dungeon) {
            ServiceLocator.TryGetService<IDungeonCreationService>(out var generatorService);
            Transform parent = generatorService != null ? generatorService.Transform : null;
            foreach (var room in dungeon.Rooms) {
                RoomCollider collider = Instantiate(_prefab);
                collider.transform.parent = parent;
                collider.Init(room);
            }

            foreach(RoomInfo room in dungeon.Rooms) {
                dungeon.DrawRoom(room);
            }
        }
    }
}