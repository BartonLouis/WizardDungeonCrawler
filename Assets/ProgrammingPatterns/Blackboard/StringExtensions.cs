﻿namespace Louis.Patterns.Blackboards {
    public static class StringExtensions {
        public static int ComputeHash(this string str) {
            uint hash = 2166136261;
            foreach (char c in str) {
                hash = (hash ^ c) * 16777619;
            }
            return unchecked((int)hash);
        }
    }
}