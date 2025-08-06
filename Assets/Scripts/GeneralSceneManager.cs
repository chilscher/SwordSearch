using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GeneralSceneManager : MonoBehaviour{

    public Image fadeImage;
    private bool hasStarted = false;

    void Start(){
        Setup();
    }

    //private void MakeYellowIfBlack() {
    //    //for testing why the heck the black screen effect is happening
    //    if (fadeImage.color.a == 0)
    //        fadeImage.color = Color.yellow;
    //}

    public void Setup(){
        if (!hasStarted){
            StaticVariables.tweenDummy = transform;
            StaticVariables.fadeImage = fadeImage;
            StaticVariables.FadeIntoScene();
            //StaticVariables.WaitTimeThenCallFunction(3f, MakeYellowIfBlack);
            hasStarted = true;
        }
    }
}
