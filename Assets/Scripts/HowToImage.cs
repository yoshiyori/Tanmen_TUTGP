using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToImage : MonoBehaviour
{
    [SerializeField] private GameObject[] howToImages;
    [SerializeField] private float imageWaitTime;       //画像一枚でとどまる時間(秒)

    private float time;
    private int imageNum;
    private int currentImage;

    void Start()
    {
        if (imageWaitTime == 0)
        {
            imageWaitTime = 1.5f;
        }
        imageNum = howToImages.Length;
        currentImage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > imageWaitTime)
        {
            time = 0.0f;
            howToImages[currentImage].SetActive(false);
            currentImage++;
            if (currentImage >= imageNum) currentImage = 0;
            howToImages[currentImage].SetActive(true);
        }
    }
}
