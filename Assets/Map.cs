using UnityEngine;

namespace MapGeneration {
    public class Map {
        public Vector2Int size;
        public Room[] rooms;
        public Corridor[] corridors;

        public Map() { 
            size = new Vector2Int();
            rooms = new Room[0];
            corridors = new Corridor[0];
        }
    }

    public struct Room {
        public Vector2Int position;
        public Vector2Int size;
        public int border;
        public int margin;

        public RectInt GetBounds() {
            int totalWidth = size.x + border * 2;
            int totalHeight = size.y + border * 2;
            return new RectInt(
                position.x - totalWidth / 2,
                position.y - totalHeight / 2,
                totalWidth, totalHeight
                );
        }

        public void Draw() {
            Vector2 topLeft = new Vector2(position.x - size.x / 2, position.y + size.y / 2);
            Vector2 topRight = new Vector2(position.x + size.x / 2, position.y + size.y / 2);
            Vector2 bottomLeft = new Vector2(position.x - size.x / 2, position.y - size.y / 2);
            Vector2 bottomRight = new Vector2(position.x + size.x / 2, position.y - size.y / 2);

            Debug.DrawLine(topLeft, topRight, Color.red, 5);
            Debug.DrawLine(topRight, bottomRight, Color.red, 5);
            Debug.DrawLine(bottomRight, bottomLeft, Color.red, 5);
            Debug.DrawLine(bottomLeft, topLeft, Color.red, 5);

            topLeft = topLeft + border * (Vector2.up + Vector2.left);
            topRight = topRight + border * (Vector2.up + Vector2.right);
            bottomLeft = bottomLeft + border * (Vector2.down + Vector2.left);
            bottomRight = bottomRight + border * (Vector2.down + Vector2.right);

            Debug.DrawLine(topLeft, topRight, Color.green, 5);
            Debug.DrawLine(topRight, bottomRight, Color.green, 5);
            Debug.DrawLine(bottomRight, bottomLeft, Color.green, 5);
            Debug.DrawLine(bottomLeft, topLeft, Color.green, 5);

            topLeft = topLeft + margin * (Vector2.up + Vector2.left);
            topRight = topRight + margin * (Vector2.up + Vector2.right);
            bottomLeft = bottomLeft + margin * (Vector2.down + Vector2.left);
            bottomRight = bottomRight + margin * (Vector2.down + Vector2.right);

            Debug.DrawLine(topLeft, topRight, Color.blue, 5);
            Debug.DrawLine(topRight, bottomRight, Color.blue, 5);
            Debug.DrawLine(bottomRight, bottomLeft, Color.blue, 5);
            Debug.DrawLine(bottomLeft, topLeft, Color.blue, 5);
        }
    }

    public struct Corridor {
        public Vector2Int[] positions;
        public int width;
    }
}