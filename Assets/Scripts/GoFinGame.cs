using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoFinGame : MonoBehaviour
{
    [SerializeField] private GameObject sTObject;
    [SerializeField] private GameObject finGameCanvas;
    [SerializeField] private CuePlayer2D soundManager;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            finGameCanvas.SetActive(true);
            sTObject.SetActive(false);
            soundManager.Play("Decision");
        }
    }
}
