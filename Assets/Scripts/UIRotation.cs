using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRotation : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;
    private int nowCenterPanel;
    private bool isNowRotation;

    void Start()
    {
        nowCenterPanel = 1;
        isNowRotation = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && isNowRotation == false)
        {
            nowCenterPanel--;
            if (nowCenterPanel < 0)
            {
                nowCenterPanel = 2;
            }
            isNowRotation = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && isNowRotation == false)
        {
            nowCenterPanel++;
            if(nowCenterPanel > 2)
            {
                nowCenterPanel = 0;
            }
            isNowRotation = true;
        }

        if(isNowRotation == true)
        {
            //Panles[0].Rotation.y
        }

    }
}
