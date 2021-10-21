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
        public bool isPlayerSpeaking;
        public TMP_Text tmp;
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
        // TMP_Text textMeshPro;
        
        const float maxTimeBetweenTypingChars = 1.001f;
        float timeBetweenTypingChars;

        // TMP_Text globalPlayerTMP;



    // ------ LIFECYCLE FUNCTIONS -------------------------------------- //
        void Start()
        {
            dialogAudio = GetComponent<DialogAudio>();
            // textMeshPro = GetComponent<TMP_Text>();
            // globalPlayerTMP = GameObject.Find("Player TMP").GetComponent<TMP_Text>();
            currentDialogIndex = 0;
            timeBetweenTypingChars = maxTimeBetweenTypingChars - typingSpeed;

            // wipe text at the start
            WipeAllDialog();
            // textMeshPro.text = "";
            // globalPlayerTMP.text = "";
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
                WipeAllDialog();
                return;
            }

            // textMeshPro.text = "";
            // globalPlayerTMP.text = "";
            Color textColor = dialog[currentDialogIndex].textColor;
            textColor.a = 1.0f;
            dialog[currentDialogIndex].tmp.color = textColor;
            StartCoroutine("TypeDialogText");
        }


        IEnumerator TypeDialogText()
        {
            // get current text
            string text = dialog[currentDialogIndex].text;
            float waitTime = dialog[currentDialogIndex].waitTime;
            var tmp = dialog[currentDialogIndex].tmp;
            tmp.text = "";
            // globalPlayerTMP.text = "";

            string textOutput = "";

            // show character one by one, pausing between characters
            for(int i = 0; i < text.Length; i++) {
                if(dialog[currentDialogIndex].isPlayerSpeaking) {
                    textOutput += text[i];
                    tmp.text = $"[{textOutput}]";
                }
                else {
                    tmp.text += text[i];
                }
                
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


        void WipeAllDialog()
        {
            foreach(var d in dialog) {
                d.tmp.text = "";
            }
        }

    }
}

