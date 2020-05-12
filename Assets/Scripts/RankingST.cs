using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingST : MonoBehaviour
{
    [SerializeField] Handle hd;
    [SerializeField] FadeController fc;

    private bool isTransition;

    [SerializeField] private CuePlayer2D soundManager;

    void Start()
    {
        isTransition = false;
    }

    void Update()
    {
        if ((hd.GetRightBrakeDown() == true || hd.GetLeftBrakeDown() == true) ||
            Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //isTransition = true;
            //fc.isFadeOut = true;
        }

        if (isTransition == true && fc.isFadeOut == false)
        {
            soundManager.Play("BackMenu");
            SceneManager.LoadScene("Title");
        }

    }
}
