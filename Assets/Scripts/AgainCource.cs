using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AgainCource : MonoBehaviour
{
    [SerializeField] Handle hd;
    [SerializeField] private CuePlayer2D soundManager;

    private void Update()
    {
        if (hd.GetRightBrakeDown() == true
            || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameManeger.gameStartFlag = true;
            GameManeger.goalFlag = false;
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
            soundManager.Play("Decision");
        }
    }
}
