using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //これはOC用のスクリプトです。本当のゴールはSecTime.csのほうに処理が書いてある

    [SerializeField] GameObject player;
    [SerializeField] GameObject returnModeSelect;
    float time;

    private void Start()
    {
        returnModeSelect.SetActive(false);
        time = 0f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            GameManeger.goalFlag = true;
        }
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
    }
}
