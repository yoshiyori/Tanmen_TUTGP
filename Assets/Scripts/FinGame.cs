using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;

public class FinGame : MonoBehaviour
{
    [SerializeField] private GameObject nowCanvas;
    [SerializeField] Handle hd;
    [SerializeField] private CuePlayer2D soundManager;
    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private GameObject modeSelectCanvas;
    [SerializeField] private GameObject TitleSTObject;
    [SerializeField] private GameObject ModeSelectSTObject;

    public bool configFlag; //コンフィグを呼び出すときに使う
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((hd.GetLeftBrakeDown() == true) || Input.GetKeyDown(KeyCode.Backspace))
        {
            if (titleCanvas.activeInHierarchy == true)
            {
                TitleSTObject.SetActive(true);
            }
            else if (modeSelectCanvas.activeInHierarchy == true)
            {
                ModeSelectSTObject.SetActive(true);
            }
            nowCanvas.SetActive(false);
            soundManager.Play("MenuBack");
        }

        if (hd.GetRightBrakeDown() == true || Input.GetKeyDown(KeyCode.Space))
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }

    }
}
