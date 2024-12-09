using UnityEngine;

namespace Utils {

    public static class GameObjectExtensions {
        public static void SmartDestroy(this GameObject go) {
            if (go == null) { return; }

#if UNITY_EDITOR
            if (!Application.isPlaying) {
                GameObject.DestroyImmediate(go);
            } else 
#endif           
            {
                GameObject.Destroy(go);
            }
        }
    }

}