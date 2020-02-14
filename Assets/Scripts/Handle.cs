using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    private Joycon m_joyconR;
    [SerializeField] Quaternion angles;
    [SerializeField] Vector3 euAngles;
    [SerializeField] float rlCheck;

    private void Start()
    {
        var joycons = JoyconManager.Instance.j;
        m_joyconR = joycons.Find(c => !c.isLeft);
    }

    private void Update()
    {
        angles = m_joyconR.GetVector();
        euAngles = m_joyconR.GetVector().eulerAngles;
        rlCheck = angles.x + angles.w;//-:左、+:右
    }
}
