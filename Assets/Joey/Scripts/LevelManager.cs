using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace jsch 
{
    public class LevelManager : MonoBehaviour
    {
    
    // =========================================================================== //
    // ==============       Variables                   ========================== //
    // =========================================================================== //
        [System.Serializable]
        public class SceneData
        {
            public string name;
            public bool loadOnStart = true;
        }

        public SceneData[] scenesToLoad;

        // this is used so we make sure to delete it if we aren't the singleton instance
        public GameObject characterController; 
        public GameObject[] stuffToDeleteNoMatterWhat;

        // vars for making this a singleton
        private static LevelManager instance;
        public static LevelManager Instance { get { return instance; } }

        
    // =========================================================================== //
    // ==============       Main methods                ========================== //
    // =========================================================================== //

        // ensure there are no other LevelManagers in the scene
        private void Awake()
        {
            foreach(var obj in stuffToDeleteNoMatterWhat) {
                Destroy(obj);
            }

            if (instance != null && instance != this) {
                Destroy(characterController);
                Destroy(this.gameObject);
            } 
            else {
                instance = this;
            }
        }

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

