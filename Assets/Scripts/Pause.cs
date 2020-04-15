using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private CueManager cueManager;

    void Reset()
    {
        cueManager = (CueManager)FindObjectOfType(typeof(CueManager));
    }

    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            pauseUI.SetActive(!pauseUI.activeSelf);

            if (pauseUI.activeSelf)
            {
                Time.timeScale = 0f;
                //キューシート「Game_SE」に属するキューの再生を全て一時停止する
                //一時停止中に再度、再生要求があると再生を再開するため注意 (後々修正するかも)
                cueManager.PauseCueSheet("Game_SE");
            }
            else
            {
                Time.timeScale = 1f;
                //キューシート「Game_SE」に属するキューの再生を全て再開するする
                cueManager.RestartCueSheet("Game_SE");
            }
        }
    }
}
