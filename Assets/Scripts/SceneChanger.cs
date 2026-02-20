//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneChanger{

    public enum Scene {None, Homepage, Settings}
    static public Scene goingFrom = Scene.None;
    static public Scene goingTo = Scene.None;
    static public SceneChangerVisuals visuals;
    static public string nextSceneName;
    static public float waitTime = 0.2f;
    static public float animationTime = 0.5f;


    static public void GoHome(){
        nextSceneName = "Homepage";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Homepage;
        visuals.AnimateLeavingScene();
        StaticVariables.WaitTimeThenCallFunction(1f, LoadScene);
    }

    static public void GoSettings(){
        nextSceneName = "Settings";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Settings;
        visuals.AnimateLeavingScene();
        StaticVariables.WaitTimeThenCallFunction(1f, LoadScene);
    }

    static private void LoadScene(){
        SceneManager.LoadScene(nextSceneName);
    }

}
