using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ServiceLocator
{
    public static class Bootstrapper
    {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initiailze()
        {
            // Initialize default service locator.
            Locator.Initialize();
            Application.targetFrameRate = 60;

            // Register all your services next.
            Locator.Current.Register(new GameService());

            // Application is ready to start, load your main scene.
            SceneManager.LoadSceneAsync(1);
        }
    }
}


