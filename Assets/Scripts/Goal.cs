using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //これはOC用のスクリプトです。本当のゴールはSecTime.csのほうに処理が書いてある

    [SerializeField] GameObject player;
    [SerializeField] GameObject returnModeSelect;
    [SerializeField] GameObject goalText;
    float time;

    //サウンド追加する
    [SerializeField] private CueManager cueManager;

    private void Start()
    {
        returnModeSelect.SetActive(false);
        goalText.SetActive(false);
        time = 0f;
    }

    private void Update()
    {
        if(GameManeger.goalFlag == true)
        {
            time += Time.deltaTime;
            if (time >= 3)
            {
                returnModeSelect.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManeger.goalFlag = true;
        goalText.SetActive(true);

        cueManager.PauseCueSheet("GameSE");
    }
}
