using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace CustomTypes {
    [Serializable]
    public class EnumMatrix<T> : ISerializationCallbackReceiver, IEquatable<EnumMatrix<T>> where T : Enum{
        public bool[,] matrix;

        public EnumMatrix() {
            IEnumerable<T> values = EnumUtils.GetValues<T>();
            matrix = new bool[values.Count(), values.Count()];
        }

        public override bool Equals(object obj) {
            return obj is EnumMatrix<T> other && Equals(other);
        }

        public bool Equals(EnumMatrix<T> other) {
            return other.matrix == matrix;
        }

        public override int GetHashCode() {
            return HashCode.Combine(matrix);
        }

        public void OnAfterDeserialize() {
        }

        public void OnBeforeSerialize() {

        }

        public static bool operator ==(EnumMatrix<T> left, EnumMatrix<T> right) {
            return EqualityComparer<EnumMatrix<T>>.Default.Equals(left, right);
        }

        public static bool operator !=(EnumMatrix<T> left, EnumMatrix<T> right) {
            return !(left == right);
        }
    }
}