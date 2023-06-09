using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StaticVariables
{
    static public Transform tweenDummy;
    static public System.Random rand = new System.Random();
    static public BattleData battleData = null;
    static private string sceneName = "";
    static public Image fadeImage;
    static public float sceneFadeDuration = 0.5f;
    static public bool healActive = true;
    static public bool waterActive = true;
    static public bool fireActive = false;
    static public bool earthActive = false;
    static public bool lightningActive = false;
    static public bool darkActive = false;
    static public bool swordActive = false;
    static public BattleManager.PowerupTypes buffedType = BattleManager.PowerupTypes.None;
    static public int highestUnlockedWorld = 0; //0 for start of game
    static public int highestUnlockedLevel = 5;
    static public int currentBattleWorld = 0;
    static public int currentBattleLevel = 0;
    static public string battleSceneName = "Battle Scene";
    static public string world0Name = "World 0 - Intro";
    static public string world1Name = "World 1 - Grasslands";
    static public string world2Name = "World 2 - Magical Forest";
    static public string world3Name = "World 3 - Desert";
    static public string world4Name = "World 4 - Darklands";
    static public string world5Name = "World 5 - Frostlands";
    static public string world6Name = "World 6 - Dragonlands";
    static public bool beatCurrentBattle = false;
    static public bool hasTalkedToNewestEnemy = false;
    static public CutsceneManager.Cutscene cutsceneID;


    static public void WaitTimeThenCallFunction(float delay, TweenCallback function) {
        tweenDummy.DOLocalMove(tweenDummy.transform.localPosition, delay, false).OnComplete(function);
    }    
    static public void WaitTimeThenCallFunction(float delay, TweenCallback<string> function, string param) {
        tweenDummy.DOLocalMove(tweenDummy.transform.localPosition, delay, false).OnComplete(()=>function(param));
    }


    static public void FadeOutThenLoadScene(string name){
        sceneName = name;
        StartFadeDarken(sceneFadeDuration);
        WaitTimeThenCallFunction(sceneFadeDuration, LoadScene);
    }

    static public void FadeIntoScene(){
        StartFadeLighten(sceneFadeDuration);
    }

    static public void StartFadeDarken(float duration){
        Color currentColor = Color.black;
        currentColor.a = 0;
        fadeImage.color = currentColor;
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(Color.black, duration);
    }

    static public void StartFadeLighten(float duration){
        Color nextColor = Color.black;
        nextColor.a = 0;
        fadeImage.color = Color.black;
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOColor(nextColor, duration).OnComplete(HideFadeObject);
    }

    static private void HideFadeObject(){
        fadeImage.gameObject.SetActive(false);
    }

    static private void LoadScene(){
        SceneManager.LoadScene(sceneName);
    }

    static public string GetCurrentWorldName(){
        switch (currentBattleWorld){
            //temp
            case -2:
                return "Map Scene";
            case 0:
                return world0Name;
            case 1:
                return world1Name;
            case 2:
                return world2Name;
            case 3:
                return world3Name;
            case 4:
                return world4Name;
            case 5:
                return world5Name;
            default:
                return world6Name;
        }
    }

}