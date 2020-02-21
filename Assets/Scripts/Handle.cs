using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    private Joycon m_joyconR;
    private Joycon m_joyconL;
    private Vector3 accel;
    //[SerializeField] bool rlCheck;//checkの場合right

    private void Start()
    {
        var joycons = JoyconManager.Instance.j;
        //m_joyconR = joycons.Find(c => !c.isLeft);
        m_joyconL = joycons.Find(c => c.isLeft);
    }

    private void Update()
    {
        accel = m_joyconL.GetAccel();
    }


    public float GetControlllerAccel()
    {
        return Mathf.Round(accel.x * -150);
    }

}
