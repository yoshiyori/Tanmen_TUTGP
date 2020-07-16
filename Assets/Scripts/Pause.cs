﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private CueManager cueManager;
    [SerializeField] Handle hd;

    [System.NonSerialized]public static bool pauseNow; //ポーズ判断フラグ

    private void Start()
    {
        pauseNow = false;
    }

    void Reset()
    {
        cueManager = (CueManager)FindObjectOfType(typeof(CueManager));
    }

    void Update()
    {
        //Debug.Log(pauseNow);
        if ((Input.GetKeyDown(KeyCode.Escape) /*ここにZLZRを押したかどうかの判定を書く*/)
            && pauseNow == false)
        {
            pauseUI.SetActive(!pauseUI.activeSelf);
            Time.timeScale = 0f; //timeScaleを0にして各種動作を止める

            //キューシート「GameSE」に属するキューの再生を全て停止させる
            cueManager.PauseCueSheet("GameSE");
            pauseNow = true;
        }
        else if (pauseNow == false)
        {
            Time.timeScale = 1f; //timeScaleを1にして各種動作を再開する

            //キューシート「GameSE」に属するキューの再生を全て再開する
            cueManager.RestartCueSheet("GameSE");
        }
    }
}
