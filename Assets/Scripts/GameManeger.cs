using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour
{
    [System.NonSerialized]public static GameManeger instance; //シングルトンにするために使うもの

    //ゲームスタートのためのフラグ
    [System.NonSerialized] public static bool gameStartFlag;

    //ゴールした時のためのフラグ
    [System.NonSerialized] public static bool goalFlag;

    //ポーズから移動したときに使う
    [System.NonSerialized] public static bool pauseMove;

    //他のシーンからそれぞれの画面に戻るときに使う
    [System.NonSerialized] public static bool moveTitle;
    [System.NonSerialized] public static bool moveModeSelect;
    [System.NonSerialized] public static bool moveCorceSelect;

    [SerializeField] bool inGameTestFlag; //制作中、タイトル画面を介さずいきなりゲーム本編をテストするときに使う
    private void Start()
    {
        goalFlag = false;
    }

    void Awake()
    {
        //シングルトン処理
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        if(inGameTestFlag == true)
        {
            gameStartFlag = true;
        }
    }

}
