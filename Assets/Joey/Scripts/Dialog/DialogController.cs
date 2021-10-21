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
        public Color textColor;
        public string speakerName;
        public bool waitForClick = false;
        public float waitTime = 5.0f;
    }

    [RequireComponent(typeof(DialogAudio))]
    [RequireComponent(typeof(Collider))]
    public class DialogController : MonoBehaviour
    {
    // ------ PUBLIC VARS -------------------------------------- //
        public Dialog[] dialog;

        [Range(0.9f, 1)]
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

            // wipe text at the start
            textMeshPro.text = "";
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
                    collider.enabled = false;
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
            // if we're out of bounds, clear text and return
            if(currentDialogIndex >= dialog.Length) {
                textMeshPro.text = "";
                return;
            }

            textMeshPro.text = "";
            Color textColor = dialog[currentDialogIndex].textColor;
            textColor.a = 1.0f;
            textMeshPro.color = textColor;
            StartCoroutine("TypeDialogText");
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
            
            

            // if we're waiting for player to click...
            if(dialog[currentDialogIndex].waitForClick) {
                // wait for click...
                currentDialogIndex++;
            }
            // otherwise, wait "waitTime" and auto queue the next dialog text
            else {
                yield return new WaitForSeconds(dialog[currentDialogIndex].waitTime);
                currentDialogIndex++;
                ShowNextDialog();
            }
        } 

    }
}

