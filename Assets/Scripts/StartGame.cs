using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] Handle hd;
    [SerializeField] private CuePlayer2D soundManager;
    [SerializeField] string sceanName;

    private void Update()
    {
        if(GameManeger.gameStartFlag == false)
        {
            if (hd.GetRightBrakeDown() == true
            || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                GameManeger.gameStartFlag = true;
                GameManeger.goalFlag = false;
                SceneManager.LoadSceneAsync(sceanName);
                soundManager.Play("Decision");
            }
        }
    }
}
