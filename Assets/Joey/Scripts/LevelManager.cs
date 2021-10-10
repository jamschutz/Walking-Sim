using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace jsch 
{
    public class LevelManager : MonoBehaviour
    {

    // ------------------------------------------------------- //
    // ----------- Variables --------------------------------- //
    // ------------------------------------------------------- //
        [System.Serializable]
        public class SceneData
        {
            public string name;
            public bool loadOnStart = true;
        }

        public SceneData[] scenesToLoad;

        // we'll delete the character controller if we aren't the singleton instance
        public GameObject characterController; 
        public GameObject[] stuffToDeleteNoMatterWhat;

        // vars for making this a singleton
        private static LevelManager instance;
        public static LevelManager Instance { get { return instance; } }

        
        
    // ------------------------------------------------------- //
    // ----------- Main Methods ------------------------------ //
    // ------------------------------------------------------- //

        // ensure there are no other LevelManagers in the scene
        private void Awake()
        {
            // always delete "stuff to delete no matter what"
            foreach(var obj in stuffToDeleteNoMatterWhat) {
                Destroy(obj);
            }

            // if already a singleton instance in the world,
            // delete ourselves and our character controller
            if (instance != null && instance != this) {
                Destroy(characterController);
                Destroy(this.gameObject);
            } 
            // otherwise, mark ourselves as the singleton
            else {
                instance = this;
            }
        }

        void Start()
        {
            // load scenes
            foreach(var sceneData in scenesToLoad) {
                if(sceneData.loadOnStart) {
                    SceneManager.LoadScene(sceneData.name, LoadSceneMode.Additive);
                }
            }
        }
    }
}

