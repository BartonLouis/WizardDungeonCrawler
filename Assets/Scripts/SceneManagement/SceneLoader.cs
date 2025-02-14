using Louis.Patterns.ServiceLocator;
using Louis.Patterns.Singleton;
using Eflatun.SceneReference;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Levels;
using System.Collections;
using System.Linq;

namespace Managers {
    public class SceneLoader : PersistentSingleton<SceneLoader>, ISceneManagerService {

        [Header("References")]
        [SerializeField] GameObject loadingScreen;
        [SerializeField] Level loadOnStart;
        List<SceneReference> loadedScenes = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init() {
            SceneManager.LoadScene("Bootstrap");
        }

        private void Start() {
            loadingScreen.SetActive(false);
            StartCoroutine(LoadLevelAsync(loadOnStart));
        }

        public void LoadLevel(Level level) {
            StartCoroutine(LoadLevelAsync(level));
        }

        public IEnumerator LoadLevelAsync(Level level) {
            loadingScreen.SetActive(true);
            List<AsyncOperation> ops = new List<AsyncOperation>();
            foreach(var scene in level.scenes) {
                if (loadedScenes.Contains(scene.scene)) continue;
                ops.Add(SceneManager.LoadSceneAsync(scene.scene.Name, LoadSceneMode.Additive));
            }
            while (!ops.All(op => op.isDone)) {
                yield return null;
            }
            loadingScreen.SetActive(false);
        }

        private IEnumerator LoadScene(SceneReference scene) {
            if (loadedScenes.Contains(scene)) yield break;
            AsyncOperation op = SceneManager.LoadSceneAsync(scene.Name, LoadSceneMode.Additive);
            while (!op.isDone) yield return null;
        }
    }


    public interface ISceneManagerService : IService {
        public void LoadLevel(Level level);
    }
}