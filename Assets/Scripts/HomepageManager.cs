//for Sword Search, copyright Fancy Bus Games 2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MyBox;
using DG.Tweening;
using Unity.VisualScripting;

public class HomepageManager : MonoBehaviour{

    [Header("Continue Adventure")]
    public Text continueAdventureText;
    public GameObject hometownBackground;
    public GameObject hometownSceneChangeBackground;
    public GameObject grasslandsBackground;
    public GameObject grasslandsSceneChangeBackground;
    public GameObject forestBackground;
    public GameObject forestSceneChangeBackground;
    public GameObject desertBackground;
    public GameObject desertSceneChangeBackground;
    public GameObject duskvaleBackground;
    public GameObject duskvaleSceneChangeBackground;
    public GameObject frostlandsBackground;
    public GameObject frostlandsSceneChangeBackground;
    public GameObject cavernsBackground;
    public GameObject cavernsSceneChangeBackground;
    public Transform enemySpace;
    [Header("Endless Mode")]
    public Transform endlessModeEnemiesParent;
    public Transform endlessModeEndPosition;
    public Transform endlessModePosition1;
    public Transform endlessModePosition2;
    public Transform endlessModePosition3;
    public Transform endlessModePosition4;
    public GameObject emptyGameObject;
    private List<GameObject> endlessModeEnemyPrefabs;
    private int endlessModeEnemyIndex = 0;
    private readonly float endlessModeMoveDuration = 9f;

    public void HitContinueAdventureButton(){
        //some way to designate which stage to go to when the level loads
        //todo figure out what happens after you beat the last stage in the game
        StaticVariables.lastVisitedStage = StaticVariables.highestBeatenStage.nextStage;
        SceneChanger.GoWorld(StaticVariables.lastVisitedStage.world);
    }

    public void HitEndlessModeButton(){
        print("there's no endless mode yet, sorry :(");
    }

    public void HitSettingsButton(){
        SceneChanger.GoSettings();
    }

    public void HitMapButton(){
        StaticVariables.lastVisitedStage = StaticVariables.highestBeatenStage.nextStage;
        SceneChanger.GoAtlas();
    }

    private List<GameObject> CreateEndlessModeEnemyList(){
        endlessModeEnemyPrefabs = new List<GameObject>();
        if (StaticVariables.highestBeatenStage == StaticVariables.allStages[0])
            return endlessModeEnemyPrefabs;
        bool addNextEnemy = true;
        StageData stage = StaticVariables.allStages[1]; //skip the "first stage", it represents 0 progress in the game
        while (addNextEnemy){
            if (stage.enemyPrefab.name != "Overworld Book")
                endlessModeEnemyPrefabs.Add(stage.enemyPrefab);
            if ((stage == StaticVariables.highestBeatenStage) || (stage.nextStage == null))
                addNextEnemy = false;
            else
                stage = stage.nextStage;
        }
        return endlessModeEnemyPrefabs;
    }

    public void ShowEndlessModeEnemies(){
        endlessModeEnemyPrefabs = CreateEndlessModeEnemyList();
        endlessModeEnemyPrefabs.Shuffle();
        if (endlessModeEnemyPrefabs.Count == 0)
            return;

        endlessModeEnemyIndex = 0;
        
        ShowNextEndlessModeEnemy(endlessModePosition1, endlessModeMoveDuration*0.25f);
        ShowNextEndlessModeEnemy(endlessModePosition2, endlessModeMoveDuration*0.5f);
        ShowNextEndlessModeEnemy(endlessModePosition3, endlessModeMoveDuration*0.75f);
        ShowNextEndlessModeEnemy();
    }

    private void ShowNextEndlessModeEnemy(Transform startingPosition, float moveDuration){
        GameObject enemyParent = GameObject.Instantiate(emptyGameObject, endlessModeEnemiesParent);
        enemyParent.transform.localPosition = startingPosition.localPosition;
        GameObject enemy = GameObject.Instantiate(endlessModeEnemyPrefabs[endlessModeEnemyIndex], enemyParent.transform);
        if (enemy.GetComponent<EnemyData>().isHorde) {
            foreach (Transform t in enemy.transform)
                t.GetChild(0).GetComponent<Animator>().Play("Walk");
        }
        else
            enemy.GetComponent<Animator>().Play("Walk");
        enemyParent.transform.DOLocalMoveX(endlessModeEndPosition.localPosition.x, moveDuration).SetEase(Ease.Linear).OnComplete(ShowNextEndlessModeEnemy);
        StaticVariables.WaitTimeThenCallFunction(moveDuration, GameObject.Destroy, enemyParent);
        endlessModeEnemyIndex++;
        if (endlessModeEnemyIndex >= endlessModeEnemyPrefabs.Count)
            endlessModeEnemyIndex = 0;
    }

    private void ShowNextEndlessModeEnemy(){
        //spawns an enemy at the default position, the rightmost spot outside of the render window
        ShowNextEndlessModeEnemy(endlessModePosition4, endlessModeMoveDuration);
    }

    public void DisplayProgress() {
        int nextEnemyWorldNum = StaticVariables.highestBeatenStage.nextStage.world;
        hometownBackground.SetActive(nextEnemyWorldNum == 1);
        grasslandsBackground.SetActive(nextEnemyWorldNum == 2);
        forestBackground.SetActive(nextEnemyWorldNum == 3);
        desertBackground.SetActive(nextEnemyWorldNum == 4);
        duskvaleBackground.SetActive(nextEnemyWorldNum == 5);
        frostlandsBackground.SetActive(nextEnemyWorldNum == 6);
        cavernsBackground.SetActive(nextEnemyWorldNum == 7);

        GameObject enemyPrefab = StaticVariables.highestBeatenStage.nextStage.enemyPrefab;

        GameObject enemyParent = GameObject.Instantiate(emptyGameObject, enemySpace.parent);
        enemyParent.transform.localPosition = enemySpace.localPosition;
        GameObject enemy = GameObject.Instantiate(enemyPrefab, enemyParent.transform);

        if ((nextEnemyWorldNum == 1) && (StaticVariables.highestBeatenStage.nextStage.stage == 1))
            continueAdventureText.text = "BEGIN\nADVENTURE";
    }

    public void ShowSceneChangerBackground(int worldNum){
        hometownSceneChangeBackground.SetActive(worldNum == 1);
        grasslandsSceneChangeBackground.SetActive(worldNum == 2);
        forestSceneChangeBackground.SetActive(worldNum == 3);
        desertSceneChangeBackground.SetActive(worldNum == 4);
        duskvaleSceneChangeBackground.SetActive(worldNum == 5);
        frostlandsSceneChangeBackground.SetActive(worldNum == 6);
        cavernsSceneChangeBackground.SetActive(worldNum == 7);
    }
}
