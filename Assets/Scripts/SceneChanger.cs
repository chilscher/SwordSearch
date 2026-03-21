//for Sword Search, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SceneChanger{

    public enum Scene {None, Homepage, Settings, Credits, Atlas, World, Cutscene, Battle, Tutorial}
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

    static public void GoWorld(int num){
        nextSceneName = num switch {
            1 => "World 1 - Hometown",
            2 => "World 2 - Grasslands",
            3 => "World 3 - Enchanted Forest",
            4 => "World 4 - Sunscorched Desert",
            5 => "World 5 - Duskvale",
            6 => "World 6 - Frostlands",
            7 => "World 7 - Caverns",
            _ => "World 1 - Hometown",
        };
        goingFrom = visuals.thisScene;
        goingTo = Scene.World;
        visuals.AnimateLeavingScene();
    }

    static public void GoNextWorld(int num){
        //only used when you complete the last level in a world, and the next world is immediately loaded
        //the fade-in stuff should operate the same as if you came directly from the last scene, with no changes
        nextSceneName = num switch {
            1 => "World 1 - Hometown",
            2 => "World 2 - Grasslands",
            3 => "World 3 - Enchanted Forest",
            4 => "World 4 - Sunscorched Desert",
            5 => "World 5 - Duskvale",
            6 => "World 6 - Frostlands",
            7 => "World 7 - Caverns",
            _ => "World 1 - Hometown",
        };
        LoadScene();
    }
    
    static public void GoCutscene(){
        nextSceneName = "Cutscene";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Cutscene;
        visuals.AnimateLeavingScene();
    }
    
    static public void GoBattle(){
        nextSceneName = "Battle Scene";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Battle;
        visuals.AnimateLeavingScene();
    }
    
    static public void GoTutorial(){
        nextSceneName = "Tutorial";
        goingFrom = visuals.thisScene;
        goingTo = Scene.Tutorial;
        visuals.AnimateLeavingScene();
    }

    static public void LoadScene(){
        SceneManager.LoadScene(nextSceneName);
    }

}
