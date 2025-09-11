using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation.GenerationSteps {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Remove Filler Rooms Step", order = 3)]
    internal class RemoveFillerRoomsGenerationStep : MapGenerationStep {
        public override void ApplyStep(Map map, Random random) {
            map.rooms = map.rooms.Where(room => room.tag != RoomType.Filler).ToArray();
        }
    }
}