using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceanManeger : MonoBehaviour
{
    [SerializeField] GameObject titleCanvas;
    [SerializeField] GameObject modeSelectCanvas;
    [SerializeField] GameObject corseSelectCanvas;

    private void Start()
    {
        if (GameManeger.pauseMove == true)
        {
            Time.timeScale = 1f;
            GameManeger.pauseMove = false;
        }
        if (GameManeger.moveTitle == true || GameManeger.moveModeSelect == true 
            || GameManeger.moveCorceSelect == true)
        {

            if (GameManeger.moveTitle == true)
            {
                titleCanvas.SetActive(true);
                CanvasFalse(modeSelectCanvas);
                CanvasFalse(corseSelectCanvas);
                GameManeger.moveTitle = false;
            }
            else if (GameManeger.moveModeSelect == true)
            {
                CanvasFalse(titleCanvas);
                modeSelectCanvas.SetActive(true);
                CanvasFalse(corseSelectCanvas);
                GameManeger.moveModeSelect = false;
            }
            else if (GameManeger.moveCorceSelect == true)
            {
                CanvasFalse(titleCanvas);
                CanvasFalse(modeSelectCanvas);
                corseSelectCanvas.SetActive(true);
                GameManeger.moveCorceSelect = false;
            }
        }
    }

    private void CanvasFalse(GameObject canvas)
    {
        if (canvas.activeSelf == true)
        {
            canvas.SetActive(false);
        }
    }
}
