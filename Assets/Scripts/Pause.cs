using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private CueManager cueManager;

    private bool pauseNow = false; //ポーズ判断フラグ

    void Reset()
    {
        cueManager = (CueManager)FindObjectOfType(typeof(CueManager));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !pauseNow)
        {
            pauseUI.SetActive(!pauseUI.activeSelf);
            Time.timeScale = 0f; //timeScaleを0にして各種動作を止める

            //キューシート「GameSE」に属するキューの再生を全て停止させる
            cueManager.PauseCueSheet("GameSE");
            pauseNow = true;
        }
        else if(Input.GetKeyDown(KeyCode.Q) && pauseNow)
        {
            pauseUI.SetActive(!pauseUI.activeSelf);
            Time.timeScale = 1f; //timeScaleを1にして各種動作を再開する

            //キューシート「GameSE」に属するキューの再生を全て再開する
            cueManager.RestartCueSheet("GameSE");
            pauseNow = false;
        }
    }
}
