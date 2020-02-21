using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSticks : MonoBehaviour{
    [SerializeField] GameObject Stick;
    private Vector3 stickPos;
    //private GameObject[] Sticks;

    void Awake(){
        stickPos.z = 15f;
        stickPos.y = 2.5f;

        for(int i = 0; i < 200; i++){
            stickPos.x = i * -5f;
            Instantiate(Stick, stickPos, Quaternion.identity);
        }
    }
}
