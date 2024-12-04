using UnityEngine;
using Utils;

namespace DungeonGeneration {

    [CreateAssetMenu(menuName = "Dungeon Generation/Generation Step/Draw Rooms Step")]
    public class DrawRoomBordersStep : AbstractGenerationStep {
        public override void Generate(DungeonInfo dungeon) {
            foreach(RoomInfo room in dungeon.Rooms) {
                dungeon.DrawRoom(room);
            }
        }
    }
}