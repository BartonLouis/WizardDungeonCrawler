using DungeonGeneration;
using Events.Rooms;
using Louis.Patterns.EventSystem;
using Managers;
using UnityEngine;

namespace Gameplay.Rooms {
    [RequireComponent(typeof(BoxCollider2D))]
    public class RoomCollider : MonoBehaviour {
        BoxCollider2D _collider;
        RoomInfo _roomInfo;

        public void Init(Vector2 center, Vector2 size) {
            _collider = GetComponent<BoxCollider2D>();
            transform.position = center;
            _collider.size = size;
        }

        public void Init(RoomInfo roomInfo) {
            _collider = GetComponent<BoxCollider2D>();
            _roomInfo = roomInfo;
            transform.position = roomInfo.bounds.center;
            float width = roomInfo.bounds.size.x - 2 * roomInfo.border - 2 * roomInfo.margin;
            float height = roomInfo.bounds.size.y - 2 * roomInfo.border - 2 * roomInfo.margin;
            _collider.size = new Vector2(width, height);
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (_collider == null) _collider = GetComponent<BoxCollider2D>();
            if (collision.CompareTag("Player")) {
                EventBus.Raise(new RoomEnteredEvent(_roomInfo, _collider.bounds));
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (_collider == null) _collider = GetComponent<BoxCollider2D>();
            if (collision.CompareTag("Player")) {
                EventBus.Raise(new RoomExitedEvent(_roomInfo, _collider.bounds));
            }
        }
    }
}