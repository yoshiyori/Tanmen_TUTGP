using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SoundSystem{
    public class VolManager : MonoBehaviour{
        //Slider
        [SerializeField] private Slider seSlider;
        [SerializeField] private Slider bgmSlider;

        //flag
        [HideInInspector] public bool changed = false;

        [SerializeField] private float se = 1f;
        [SerializeField] private float bgm = 1f;

        private void Awake(){
            DontDestroyOnLoad(this.gameObject);

            SceneManager.activeSceneChanged += ActiveSceneChanged;
        }

        private void Update(){
            if(changed){
                CriAtom.SetCategoryVolume("SE", seSlider.value);
                CriAtom.SetCategoryVolume("BGM", bgmSlider.value);

                se = seSlider.value;
                bgm = bgmSlider.value;

                changed = false;
            }

            if(Input.GetKeyDown(KeyCode.G)){
                Debug.Log(CriAtom.GetCategoryVolume("BGM").ToString() + ", " + CriAtom.GetCategoryVolume("SE"));
            }
        }

        private void ActiveSceneChanged(Scene thisScene, Scene nextScene){
            if(nextScene.name.Equals("main")){
                Debug.Log(se.ToString() + " ," + bgm.ToString());
                CriAtom.SetCategoryVolume("SE", se);
                CriAtom.SetCategoryVolume("BGM", bgm);
            }
        }
    }
}
