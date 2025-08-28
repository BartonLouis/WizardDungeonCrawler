using Louis.CustomPackages.Logging;
using UnityEngine;

namespace MapGeneration {
    public class MapGenerator : MonoBehaviour {

        [Header("Settings")]
        [SerializeField] MapGenerationSettings _settings;

        public void Test() {
            Map map = Generate();
            Logging.Log(this, $"Generated a map with size: {map.size}, {map.rooms.Length} rooms, and {map.corridors.Length} corridors.");

            foreach(var room in map.rooms) {
                room.Draw();
            }
        }

        Map Generate() {
            Map map = new();
            foreach(var step in _settings.Steps) {
                map = step.ApplyStep(map);
            }
            return map;
        }
    }
}