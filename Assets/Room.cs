using System;
using System.Collections.Generic;
using UnityEngine;

namespace Map {
    [Serializable]
    public struct Room {
        /// <summary>
        /// The position of the bottom left corner of the inside of the room
        /// </summary>
        public Vector2Int position;
        public Vector2Int size;
        public int border;
        public int margin;
        public int roomId;

        public BoundsInt2D SafeBounds => new BoundsInt2D {
            minX = position.x - border - margin,
            minY = position.y - margin - margin,
            maxX = position.x + size.x + border + margin,
            maxY = position.y + size.y + border + margin
        };

        public BoundsInt2D BorderBounds => new BoundsInt2D {
            minX = position.x - border,
            minY = position.y - border,
            maxX = position.x + size.x + border,
            maxY = position.y + size.y + border
        };


        public readonly float HalfWidth => size.x / 2f + border + margin;
        public readonly float HalfHeight => size.y / 2f + border + margin;
        public readonly Vector2 Center => new Vector2(position.x + size.x / 2, position.y + size.y / 2);

        public static float RequiredGapX(Room a, Room b) => a.HalfWidth + b.HalfWidth /*+ Mathf.Max(a.margin, b.margin)*/;
        public static float RequiredGapY(Room a, Room b) => a.HalfHeight + b.HalfHeight /*+ Mathf.Max(a.margin, b.margin)*/;

        public IEnumerable<Vector2Int> Doors {
            get {
                yield return position + (size.x / 2) * Vector2Int.right;   // Bottom
                yield return position + (size.y / 2) * Vector2Int.up;     // Left
                yield return position + (size.x / 2) * Vector2Int.right + size.y * Vector2Int.up;  // Top
                yield return position + (size.y / 2) * Vector2Int.up + size.x * Vector2Int.right; // Right
            }
        }

        public void Draw() {
            Vector2 bottomLeft = new(position.x, position.y);
            Vector2 topLeft = new(position.x, position.y + size.y);
            Vector2 bottomRight = new(position.x + size.x, position.y);
            Vector2 topRight = new(position.x + size.x, position.y + size.y);

            Debug.DrawLine(topLeft, topRight, Color.red);
            Debug.DrawLine(topRight, bottomRight, Color.red);
            Debug.DrawLine(bottomRight, bottomLeft, Color.red);
            Debug.DrawLine(bottomLeft, topLeft, Color.red);

            topLeft = topLeft + border * (Vector2.up + Vector2.left);
            topRight = topRight + border * (Vector2.up + Vector2.right);
            bottomLeft = bottomLeft + border * (Vector2.down + Vector2.left);
            bottomRight = bottomRight + border * (Vector2.down + Vector2.right);

            Debug.DrawLine(topLeft, topRight, Color.green);
            Debug.DrawLine(topRight, bottomRight, Color.green);
            Debug.DrawLine(bottomRight, bottomLeft, Color.green);
            Debug.DrawLine(bottomLeft, topLeft, Color.green);

            topLeft = topLeft + margin * (Vector2.up + Vector2.left);
            topRight = topRight + margin * (Vector2.up + Vector2.right);
            bottomLeft = bottomLeft + margin * (Vector2.down + Vector2.left);
            bottomRight = bottomRight + margin * (Vector2.down + Vector2.right);

            Debug.DrawLine(topLeft, topRight, Color.blue);
            Debug.DrawLine(topRight, bottomRight, Color.blue);
            Debug.DrawLine(bottomRight, bottomLeft, Color.blue);
            Debug.DrawLine(bottomLeft, topLeft, Color.blue);
        }
    }

    public enum Direction {
        North,
        South,
        East,
        West
    }

    public struct BoundsInt2D {
        public int minX, maxX, minY, maxY;

        public readonly int Width => maxX - minX;
        public readonly int Height => maxY - minY;
    }
}