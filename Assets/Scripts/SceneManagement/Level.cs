using Eflatun.SceneReference;
using System;
using UnityEngine;

namespace Levels {
    [CreateAssetMenu(menuName = "Data/Level")]
    public class Level : ScriptableObject {
        public SceneAndSceneType[] scenes;
    }

    [Serializable]
    public class SceneAndSceneType {
        public string name;
        public SceneReference scene;
        public SceneType sceneType;
    }

    public enum SceneType {
        Gameplay,
        Environment,
        UI
    }
}