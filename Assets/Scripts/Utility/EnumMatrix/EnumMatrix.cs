using DungeonGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace CustomTypes {
    [Serializable]
    public class EnumMatrix {
        [Serializable]
        public class RowData {
            public List<bool> row = new();
            public bool this[RoomType i] => row[row.Count - 1 - (int)i];
        }

        private RowData this[RoomType x] => matrix[(int)x];
        public bool this[RoomType x, RoomType y] {
            get {
                
                if (x < y) return this[x][y];
                return this[y][x];
            }
        }


        public List<string> valueNames = new();
        public List<RowData> matrix = new();



        public EnumMatrix() {
            ResetGrid();
        }

        public void ResetGrid() {
            valueNames?.Clear();
            matrix?.Clear();
            var enumValues = EnumUtils.GetValues<RoomType>();
            foreach (var item in enumValues) {
                matrix.Add(new RowData());
                valueNames.Add(item.ToString());
            }
            foreach (var item in enumValues) {
                foreach (var row in matrix) {
                    row.row.Add(true);
                }
            }
        }
    }
}