using UnityEngine;

namespace Map {
    [CreateAssetMenu(menuName = "Data/Room Config")]
    public class RoomConfig : ScriptableObject {
        public Vector2Int size;
        public int border;
        public int margin;
        public RoomType tag;
    }
}
