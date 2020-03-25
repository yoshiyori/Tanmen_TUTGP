using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{

    [SerializeField] private GameObject pauseUI;

    void Update()
    {
        if(Input.GetKeyDown("q"))
        {
            pauseUI.SetActive(!pauseUI.activeSelf);

            if(pauseUI.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }
}
