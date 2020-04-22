using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleST : MonoBehaviour
{
    [SerializeField] private GameObject flashObject;
    [SerializeField] private float flashInterval;
    [SerializeField] Handle hd;
    [SerializeField] FadeController fc;

    private float time;
    private bool isTransition;
    [SerializeField] private bool isGoRanking;

    [SerializeField] private GameObject titleCanvas;
    [SerializeField] private GameObject modeSelectCanvas;
    //[SerializeField] private GameObject rankingCanvas;

    private void Start()
    {
        time = 0.0f;
        isTransition = false;
        //isRanking = false;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > flashInterval)
        {
            time = 0.0f;
            flashObject.SetActive(!flashObject.activeInHierarchy);
        }

        if ((hd.GetRightBrake() == true || hd.GetLeftBrake() == true) ||
            Input.anyKeyDown)
        {
            //fc.isFadeOut = true;
            isTransition = true;
        }

        if (isTransition == true && fc.isFadeOut == false)
        {
            isTransition = false;
            modeSelectCanvas.SetActive(!modeSelectCanvas.activeInHierarchy);
            titleCanvas.SetActive(!titleCanvas.activeInHierarchy);
        }
        if (isTransition == true && fc.isFadeOut == false && isGoRanking == true)
        {
            isTransition = false;
            //rankingCanvas.SetActive(!rankingCanvas.activeInHierarchy);
            titleCanvas.SetActive(!titleCanvas.activeInHierarchy);
            Debug.Log("まだできてない");
        }

    }
}
