using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingConfirm : MonoBehaviour
{
    [SerializeField] private SwingJumpJudge[] sjjs;
    private int judgesNum;
    public bool allSwingJumpFlag;
    private int judgeCount;

    // Start is called before the first frame update
    void Start()
    {
        allSwingJumpFlag = false;
        judgeCount = 0;
        judgesNum = sjjs.Length;

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < judgesNum; i++)
        {
            if (sjjs[i].nowJunpFlag == true)
            {
                judgeCount++;
            }
        }
        if (judgeCount == 0)
        {
            allSwingJumpFlag = false;
        }
        else
        {
            allSwingJumpFlag = true;
            judgeCount = 0;
        }
    }
}
