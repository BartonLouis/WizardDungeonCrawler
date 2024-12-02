using Louis.Patterns.Singleton;
using UnityEngine;

namespace Managers {
    public class InputManagerMount : PersistentSingleton<InputManagerMount> {
        [SerializeField] InputManager _inputManager;
    }
}