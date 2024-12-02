using Louis.Patterns.ServiceLocator;
using Louis.Patterns.Singleton;
using Eflatun.SceneReference;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Levels;

namespace Managers {
    public class SceneLoader : PersistentSingleton<SceneLoader>, ISceneManagerService {

        [Header("References")]
        [SerializeField] Level loadOnStart;
        List<SceneReference> loadedScenes = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init() {
            SceneManager.LoadScene("Bootstrap");
        }

        private void Start() {
            LoadLevel(loadOnStart);
        }

        public void LoadLevel(Level level) {
            foreach(var scene in level.scenes) {
                LoadScene(scene.scene);
            }
        }

        private void LoadScene(SceneReference scene) {
            if (loadedScenes.Contains(scene)) return;
            SceneManager.LoadScene(scene.Name, LoadSceneMode.Additive);
        }
    }


    public interface ISceneManagerService : IService {
        public void LoadLevel(Level level);
    }
}