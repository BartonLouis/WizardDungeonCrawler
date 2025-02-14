using CustomAttributes;
using UnityEngine;

namespace DungeonGeneration {
    [CreateAssetMenu(menuName = "Dungeon Generation/Room")]
    public class RoomSO : ScriptableObject {
        [ScriptableObjectId]
        public string roomId;
        public RoomType roomType;
        public int maxOpenDoors;

        public string roomName;
        public int width;
        public int height;
    }


    public enum RoomType : byte{
        Normal = 0,
        Spawn = 1,
        Shop = 2,
        BossFight = 3,
        Treasure = 4
    }
}