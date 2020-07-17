using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertSet : MonoBehaviour
{
    [SerializeField] GameObject alertPanel;
    [SerializeField] Handle hd;
    [SerializeField] private CuePlayer2D soundManager;
    [System.NonSerialized]public static bool alertFlag;

    private void Start()
    {
        alertFlag = false;
    }

    void Update()
    {
        if ((hd.GetRightBrakeDown() == true) ||
            Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            alertPanel.SetActive(true);
            soundManager.Play("Alart");
            alertFlag = true;
        }
    }
}
