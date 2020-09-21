using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SoundSystem{
    public class VolManager : MonoBehaviour{
        //Slider
        public Slider seSlider;
        public Slider bgmSlider;
        private List<Slider> Sliders;

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
                SetVolume();
                /*foreach(Object slider in FindObjectsOfType(typeof(Slider))){
                    Debug.Log(slider.name);
                    if(slider.name.Equals("BGMSlider")){
                        bgmSlider = ((GameObject)slider).GetComponent<Slider>();
                    }
                    if(slider.name.Equals("SESlider")){
                        seSlider = ((GameObject)slider).GetComponent<Slider>();
                    }
                }*/
            }
        }

        public void SetVolume(){
            Debug.Log(se.ToString() + " ," + bgm.ToString());
            CriAtom.SetCategoryVolume("SE", se);
            CriAtom.SetCategoryVolume("BGM", bgm);
        }
    }
}
