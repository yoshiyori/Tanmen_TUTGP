using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecEnd : MonoBehaviour
{

    //サウンド追加分
    [SerializeField] private CuePlayer2D soundManager;
    [SerializeField] private bool isGoal;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            TimeManager.secFlag = true;

            //サウンド追加分
            if(isGoal){}
            else{
                soundManager.Play("SectionPass");
            }
        }
    }
}
