using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnModeSelect : MonoBehaviour
{
    [SerializeField] Handle hd;
    [SerializeField] private CuePlayer2D soundManager;

    private void Update()
    {
        if (hd.GetRightBrakeDown() == true
            || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameManeger.moveModeSelect = true;
            SceneManager.LoadSceneAsync("CourceSelect");
            soundManager.Play("Decision");
        }
    }
}
