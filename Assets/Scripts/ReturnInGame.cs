using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnInGame : MonoBehaviour
{
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] Handle hd;
    [SerializeField] private CuePlayer2D soundManager;

    private void Update()
    {
        if ((hd.GetRightBrakeDown() == true) ||
            Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            pauseCanvas.SetActive(false);
            Pause.pauseNow = false;
            soundManager.Play("MenuBack");
        }
    }
}
