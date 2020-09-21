using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTitle : MonoBehaviour
{
    [SerializeField] Handle hd;
    [SerializeField] private CuePlayer2D soundManager;
    [SerializeField] bool inPause;

    private void Update()
    {
        if (hd.GetRightBrakeDown() == true 
            || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if(inPause == true)
            {
                GameManeger.pauseMove = true;
            }
            GameManeger.moveTitle = true;
            SceneManager.LoadSceneAsync("CourceSelect");
            soundManager.Play("Decision", 1);
        }
        else if ((hd.GetLeftBrakeDown() == true || Input.GetKeyUp(KeyCode.Backspace)) 
            && inPause == true)
        {
            this.gameObject.SetActive(false);
            AlertSet.alertFlag = false;
            soundManager.Play("MenuBack", 1);
        }
    }
}
