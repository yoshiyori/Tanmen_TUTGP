using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameST : MonoBehaviour
{
    public bool goalFlag;
    private bool isTransition;

    [SerializeField] Handle hd;
    [SerializeField] FadeController fc;

    void Start()
    {
        goalFlag = false;
        isTransition = false;
    }

    void Update()
    {
        if (goalFlag == true)
        {
            isTransition = true;
            fc.isFadeOut = true;
        }
        if (isTransition == true && fc.isFadeOut == true)
        {
            SceneManager.LoadScene("Ranking");
        }
    }
}
