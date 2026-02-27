//for Sword Search, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneChanger{

    public enum Scene {None, Homepage, Settings, Credits, Atlas, Hometown}
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
    }

    static public void GoSettings(){
        nextSceneName = "Settings";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Settings;
        visuals.AnimateLeavingScene();
    }

    static public void GoAtlas(){
        nextSceneName = "Atlas";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Atlas;
        visuals.AnimateLeavingScene();
    }

    static public void GoCredits(){
        nextSceneName = "Credits";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Credits;
        visuals.AnimateLeavingScene();
    }

    static public void GoHometown(){
        nextSceneName = "World 1 - Hometown";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Hometown;
        visuals.AnimateLeavingScene();
    }
    /*
    static public void GoGrasslands(){
        nextSceneName = "World 2 - Grasslands";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Grasslands;
        visuals.AnimateLeavingScene();
    }

    static public void GoForest(){
        nextSceneName = "World 3 - Enchanted Forest";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Forest;
        visuals.AnimateLeavingScene();
    }

    static public void GoDesert(){
        nextSceneName = "World 4 - Sunscorched Desert";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Desert;
        visuals.AnimateLeavingScene();
    }

    static public void GoDuskvale(){
        nextSceneName = "World 5 - Duskvale";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Duskvale;
        visuals.AnimateLeavingScene();
    }

    static public void GoFrostlands(){
        nextSceneName = "World 6 - Frostlands";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Frostlands;
        visuals.AnimateLeavingScene();
    }

    static public void GoCaverns(){
        nextSceneName = "World 7 - Caverns";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Caverns;
        visuals.AnimateLeavingScene();
    }
    */
    static public void LoadScene(){
        SceneManager.LoadScene(nextSceneName);
    }

}
