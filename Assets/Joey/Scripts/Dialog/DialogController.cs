using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public class DialogController : MonoBehaviour
    {
        public Dialog[] dialog;
        [Range(0.01f, 1)]
        public float typingSpeed;

    }
}

