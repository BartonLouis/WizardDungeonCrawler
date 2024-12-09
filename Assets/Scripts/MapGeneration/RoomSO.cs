using CustomAttributes;
using NUnit.Framework;
using UnityEngine;

namespace DungeonGeneration {
    [CreateAssetMenu(menuName = "Dungeon Generation/Room")]
    public class RoomSO : ScriptableObject {
        [ScriptableObjectId]
        public string roomId;
        public RoomType roomType;

        public string roomName;
        public int width;
        public int height;
    }


    public enum RoomType {
        Normal      = 0,
        Spawn       = 1,
        Shop        = 2,
        BossFight   = 4,
        Treasure    = 8
    }
}