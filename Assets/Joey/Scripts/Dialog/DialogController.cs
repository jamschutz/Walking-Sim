using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace jsch
{
    [System.Serializable]
    public class Dialog 
    {
        public string text;
        public string speakerName;
        public bool showSpeakerName = false;
        public bool waitForClick = false;
        public float waitTime = 5.0f;
    }

    [RequireComponent(typeof(DialogAudio))]
    [RequireComponent(typeof(Collider))]
    public class DialogController : MonoBehaviour
    {
    // ------ PUBLIC VARS -------------------------------------- //
        public Dialog[] dialog;

        [Range(0.001f, 1)]
        public float typingSpeed;

        public bool onlyShowOnce = true;



    // ------ PRIVATE VARS -------------------------------------- //
        private DialogAudio dialogAudio;
        int currentDialogIndex;
        TMP_Text textMeshPro;
        
        const float maxTimeBetweenTypingChars = 1.001f;
        float timeBetweenTypingChars;



    // ------ LIFECYCLE FUNCTIONS -------------------------------------- //
        void Start()
        {
            dialogAudio = GetComponent<DialogAudio>();
            textMeshPro = GetComponent<TMP_Text>();
            currentDialogIndex = 0;
            timeBetweenTypingChars = maxTimeBetweenTypingChars - typingSpeed;
        }


        void OnTriggerEnter(Collider other)
        {
            // only show dialog if the player enters trigger box
            if(other.tag == "Player") {
                StartDialog();

                // if we only want to show once, destroy the collider
                // so that it doesn't trigger again
                if(onlyShowOnce) {
                    var collider = GetComponent<Collider>();
                    Destroy(collider);
                }
            }
        }



    
    // ------ HELPER FUNCTIONS -------------------------------------- //
        void StartDialog()
        {
            ShowNextDialog();
        }


        void ShowNextDialog()
        {
            textMeshPro.text = "";
        }


        IEnumerator TypeDialogText()
        {
            // get current text
            string text = dialog[currentDialogIndex].text;
            float waitTime = dialog[currentDialogIndex].waitTime;
            textMeshPro.text = "";

            // show character one by one, pausing between characters
            for(int i = 0; i < text.Length; i++) {
                textMeshPro.text += text[i];
                yield return new WaitForSeconds(timeBetweenTypingChars);
            }            
        } 

    }
}

