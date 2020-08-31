using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] Handle hd;
    [SerializeField] string sceanName;

    //サウンド追加分
    [SerializeField] private CuePlayer2D soundManager;
    [SerializeField] private CueManager cueManager;

    private void Update()
    {
        if(GameManeger.gameStartFlag == false)
        {
            if (hd.GetRightBrakeDown() == true
            || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                //サウンド追加分
                soundManager.Stop(1);
                cueManager.AddTimeStrechVoicePool();
                
                GameManeger.gameStartFlag = true;
                GameManeger.goalFlag = false;
                SceneManager.LoadSceneAsync(sceanName);
                //soundManager.Play("Descision");
            }
        }
    }
}
