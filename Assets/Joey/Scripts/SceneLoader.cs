using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace jsch {

    public class SceneLoader : MonoBehaviour
    {
        [System.Serializable]
        public class SceneData
        {
            public string name;
            public bool loadOnStart = true;
        }

        public SceneData[] scenesToLoad;

        
        void Start()
        {
            foreach(var sceneData in scenesToLoad) {
                if(sceneData.loadOnStart) {
                    SceneManager.LoadScene(sceneData.name, LoadSceneMode.Additive);
                }
            }
        }
    }
}
