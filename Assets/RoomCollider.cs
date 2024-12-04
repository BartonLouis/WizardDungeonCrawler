using DungeonGeneration;
using Managers;
using UnityEngine;

namespace Gameplay.Rooms {
    [RequireComponent(typeof(BoxCollider2D))]
    public class RoomCollider : MonoBehaviour {
        BoxCollider2D _collider;

        public void Init(Vector2 center, Vector2 size) {
            _collider = GetComponent<BoxCollider2D>();
            transform.position = center;
            _collider.size = size;
        }

        public void Init(RoomInfo roomInfo) {
            _collider = GetComponent<BoxCollider2D>();
            transform.position = roomInfo.bounds.center;
            float width = roomInfo.bounds.size.x - 2 * roomInfo.border - 2 * roomInfo.margin;
            float height = roomInfo.bounds.size.y - 2 * roomInfo.border - 2 * roomInfo.margin;
            _collider.size = new Vector2(width, height);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (_collider == null) _collider = GetComponent<BoxCollider2D>();
            if (collision.CompareTag("Player")) {
                Logging.Log(this, $"Player entered room: {_collider.bounds.center}");
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (_collider == null) _collider = GetComponent<BoxCollider2D>();
            if (collision.CompareTag("Player")) {
                Logging.Log(this, $"Player exited room: {_collider.bounds.center}");
            }
        }
    }
}