using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    bool isListeningForQuit = false;

    void Start()
    {
        isListeningForQuit = false;
    }

    void Update()
    {
        if(isListeningForQuit && Input.GetKeyDown(KeyCode.Escape)) {
            Quit();
        }
    }
    
    public void Quit()
    {
        Application.Quit();
    }


    public void ListenForEscape()
    {
        isListeningForQuit = true;
    }
}
