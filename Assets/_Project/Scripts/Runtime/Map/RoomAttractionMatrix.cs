using UnityEngine;

namespace Map.Generation {
    [System.Serializable]
    public class RoomAttractionMatrix {
        [SerializeField]
        private int size = System.Enum.GetValues(typeof(RoomType)).Length;

        [SerializeField]
        private float[] values; // only upper triangle

        public RoomAttractionMatrix() {
            int count = size * (size + 1) / 2;
            values = new float[count];
        }

        private int Index(int i, int j) {
            if(i > j) (i, j) = (j, i); // enforce i <= j
                                       // triangular index mapping
            return i * size - (i * (i - 1)) / 2 + (j - i);
        }

        public float Get(RoomType a, RoomType b) {
            return values[Index((int)a, (int)b)];
        }

        public void Set(RoomType a, RoomType b, float val) {
            values[Index((int)a, (int)b)] = val;
        }

        public int Size => size;
        public float[] RawValues => values;
    }
}