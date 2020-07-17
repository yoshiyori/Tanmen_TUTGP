using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasActive : MonoBehaviour
{
    [SerializeField] GameObject newCanvas;
    [SerializeField] GameObject oldCanvas;
    [SerializeField] Handle hd;
    [SerializeField] private CuePlayer2D soundManager;
    public bool configFlag; //コンフィグを呼び出すときに使う

    private void Update()
    {
        if ((hd.GetRightBrakeDown() == true) ||
            Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            newCanvas.SetActive(true);
            oldCanvas.SetActive(false);
            soundManager.Play("Decision");
        }
    }
}
