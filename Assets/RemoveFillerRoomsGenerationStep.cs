using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace Map.Generation {
    [CreateAssetMenu(menuName = "Data/Generation Steps/Remove Filler Rooms Step", order = 3)]
    public class RemoveFillerRoomsGenerationStep : MapGenerationStep {

        public override bool ApplyStep(Map map, Random random) {
            map.rooms = map.rooms.Where(room => room.roomId >= 0).ToArray();
            return true;
        }
    }
}