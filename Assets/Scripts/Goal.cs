using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject resultCanvas;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject goalText;

    float time;
    [System.NonSerialized] public static bool resultFlag;

    //サウンド追加分
    [SerializeField] private CueManager cueManager;
    [SerializeField] private CuePlayer2D cuePlayer2D;

    private void Start()
    {
        resultCanvas.SetActive(false);
        goalText.SetActive(false);
        time = 0f;
        resultFlag = false;
    }

    private void Update()
    {
        if(GameManeger.goalFlag == true)
        {   
            time += Time.deltaTime;
            if (time >= 3)
            {
                goalText.SetActive(false);
                inGameUI.SetActive(false);
                resultCanvas.SetActive(true);
                resultFlag = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        CueManager.singleton.StopCueSheet("GameSE");
        cuePlayer2D.StopFadeout("GameBGMP", "GameBGMFade", 3f);
    }
}
