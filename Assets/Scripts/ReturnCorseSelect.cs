using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnCorseSelect : MonoBehaviour
{
    [SerializeField] Handle hd;
    [SerializeField] private CuePlayer2D soundManager;

    private void Update()
    {
        if (hd.GetRightBrakeDown() == true
            || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameManeger.pauseMove = true;
            GameManeger.moveCorceSelect = true;
            SceneManager.LoadSceneAsync("CourceSelect");
            soundManager.Play("Decision");
        }
        else if (hd.GetLeftBrakeDown() == true || Input.GetKeyUp(KeyCode.Backspace))
        {
            this.gameObject.SetActive(false);
            AlertSet.alertFlag = false;
            soundManager.Play("MenuBack");
        }
    }
}
