using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //これはOC用のスクリプトです。本当のゴールはSecTime.csのほうに処理が書いてある

    [SerializeField] GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        GameManeger.goalFlag = true;
    }
}
