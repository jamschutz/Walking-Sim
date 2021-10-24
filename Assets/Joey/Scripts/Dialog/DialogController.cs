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
        public bool clearAfterWait = false;
        public TMP_Text tmp;
        public bool waitForClick = false;
        public float waitTime = 5.0f;
    }



    [RequireComponent(typeof(Collider))]
    public class DialogController : MonoBehaviour
    {
    // ------ PUBLIC VARS -------------------------------------- //
        public Dialog[] dialog;

        [Range(0.9f, 1)]
        public float typingSpeed;

        public bool onlyShowOnce = true;
        public bool playSound = true;



    // ------ PRIVATE VARS -------------------------------------- //
        private DialogAudio dialogAudio;
        int currentDialogIndex;
        
        const float maxTimeBetweenTypingChars = 1.001f;
        float timeBetweenTypingChars;
        bool isWaitingForClick;



    // ------ LIFECYCLE FUNCTIONS -------------------------------------- //
        void Start()
        {
            if(playSound) {
                dialogAudio = GetComponent<DialogAudio>();

                // show error if no DialogAudio component
                if(dialogAudio == null)
                    Debug.LogError($"You set 'Play Sound' to true for Dialog Controller {gameObject.name} but there's no Dialog Audio component. Did you forget to add the component?");
            }
            currentDialogIndex = 0;
            timeBetweenTypingChars = maxTimeBetweenTypingChars - typingSpeed;
            isWaitingForClick = false;

            // wipe text at the start
            WipeAllDialog();
        }


        // checking left click seems to have problems in coroutines, so doing it in here
        void Update()
        {
            if(isWaitingForClick) {
                // check for left mouse click
                if(Input.GetMouseButtonDown(0)) {
                    isWaitingForClick = false;
                }
            }
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
                
                // play audio
                if(playSound)
                    dialogAudio.Play();

                yield return new WaitForSeconds(timeBetweenTypingChars);
            }          
            

            // if we're waiting for player to click...
            if(dialog[currentDialogIndex].waitForClick) {
                // wait for click...
                isWaitingForClick = true;

                while(isWaitingForClick) {
                    // we do the actual check in Update, eems like 
                    // Coroutines are a little spotty catching left click events
                    yield return null;
                }
            }
            // otherwise, wait "waitTime" and auto queue the next dialog text
            else {
                yield return new WaitForSeconds(dialog[currentDialogIndex].waitTime);
            }
            
            // clear text if desired
            if(dialog[currentDialogIndex].clearAfterWait) {
                tmp.text = "";
            }

            currentDialogIndex++;
            ShowNextDialog();
        } 


        void WipeAllDialog()
        {
            foreach(var d in dialog) {
                d.tmp.text = "";
            }
        }

    }
}

