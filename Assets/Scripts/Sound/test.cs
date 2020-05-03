using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour{
    [SerializeField] private CuePlayer2D cuePlayer2D;
    //[SerializeField] private CueManager cueManager;
    [SerializeField] private CriAtom atom;
    [SerializeField] private GameObject a;

    private void Reset(){
        //soundManager = FindObjectOfType<CuePlayer2D>();
        //cueManager = FindObjectOfType<CueManager>();
        //atom = FindObjectOfType<CriAtom>();
    }

    private void Start(){
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.A)){
            SceneManager.LoadScene("10yen");
        }
        if(Input.GetKeyDown(KeyCode.S)){
        cuePlayer2D = FindObjectOfType<CuePlayer2D>();
        a = FindObjectOfType<Transform>().gameObject;
        }
    }

    private void SceneLoaded(){
        atom = FindObjectOfType<CriAtom>();
        cuePlayer2D = FindObjectOfType<CuePlayer2D>();
        a = FindObjectOfType<Transform>().gameObject;
        atom.AddCueSheetInternal("MenuSE", "MenuSE.acb", "", null);
        cuePlayer2D.Play("Decision");
    }
}
