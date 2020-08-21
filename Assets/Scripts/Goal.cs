using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject resultCanvas;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject goalText;
    public Animator PlayerAni;
    float time;
    [System.NonSerialized] public static bool resultFlag;

    //サウンド追加する
    [SerializeField] private CueManager cueManager;

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
            PlayerAni.SetBool("Goal", true);
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
        GameManeger.goalFlag = true;
        goalText.SetActive(true);

        cueManager.PauseCueSheet("GameSE");
    }
}
