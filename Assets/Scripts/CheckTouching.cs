using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTouching : MonoBehaviour
{
    [SerializeField] private bool touchingFlag;

    // Update is called once per frame
    void Update()
    {
        touchingFlag = false;
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "player")
        {
            touchingFlag = true;
        }
    }

    public bool GetTouching()
    {
        return touchingFlag;
    }
}
