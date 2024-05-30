using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MyBox;
using DG.Tweening;
using Unity.VisualScripting;

public class HomepageManager : MonoBehaviour{

    //continue adventure display
    //public Text currentProgressText;
    public GameObject hometownBackground;
    public Transform hometownEnemySpace;
    public GameObject grasslandsBackground;
    public Transform grasslandsEnemySpace;

    //endless mode display
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

    public List<GameObject> hometownEnemies;
    public List<GameObject> grasslandsEnemies;

    void Start(){
        Setup();
    }

    public void Setup(){
        if (StaticVariables.currentBattleLevel < 0)
            StaticVariables.currentBattleLevel = 0;
        if (StaticVariables.currentBattleWorld < 0)
            StaticVariables.currentBattleWorld = 0;
        DisplayProgress();
        ShowEndlessModeEnemies();
        //set up endless mode display - cycle through all enemies currently defeated in order, starting at a random spot?
        //also show which powers have been unlocked so far, maybe a symbol for each power?
        //also put an info button in the corner of the endless mode pane
    }

    public void HitContinueAdventureButton(){
        //temp cause i dont like the current/highest system
        StaticVariables.currentBattleWorld = StaticVariables.highestUnlockedWorld;
        StaticVariables.currentBattleLevel = StaticVariables.highestUnlockedLevel;
        switch (StaticVariables.highestUnlockedWorld){
            case 0:
                StaticVariables.FadeOutThenLoadScene(StaticVariables.world0Name);
                break;
            case 1:
                StaticVariables.FadeOutThenLoadScene(StaticVariables.world1Name);
                break;
        }
    }

    public void HitEndlessModeButton(){
        print("there's no endless mode yet, sorry :(");
    }

    public void HitSettingsButton(){
        StaticVariables.FadeOutThenLoadScene("Settings");
    }

    public void HitMapButton(){
        StaticVariables.FadeOutThenLoadScene("Map Scene");
    }

    private void AddAllEnemiesToEndlessModeList(List<GameObject> enemies){
        foreach (GameObject go in enemies){
            if (go.name != "Overworld Book")
                endlessModeEnemyPrefabs.Add(go);
        }
    }

    private void AddFirstXEnemiesToEndlessModeList(List<GameObject> enemies, int count){
        print(count);
        for (int i = 0; i < count; i++){
            GameObject go = enemies[i];
            if (go.name != "Overworld Book")
                endlessModeEnemyPrefabs.Add(go);
        }
    }

    private List<GameObject> CreateEndlessModeEnemyList(){
        endlessModeEnemyPrefabs = new List<GameObject>();

        int maxWorldCount = 2;
        for (int i = 0; i < maxWorldCount; i++){
            List<GameObject> enemies = i switch {
                (0) => hometownEnemies,
                (1) => grasslandsEnemies,
                _ => hometownEnemies,
            };
            if (StaticVariables.highestUnlockedWorld == i)
                AddFirstXEnemiesToEndlessModeList(enemies, StaticVariables.highestUnlockedLevel);
            else if (StaticVariables.highestUnlockedWorld > i)
                AddAllEnemiesToEndlessModeList(enemies);
        }
        return endlessModeEnemyPrefabs;
    }

    private void ShowEndlessModeEnemies(){
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
        enemyParent.transform.DOLocalMoveX(endlessModeEndPosition.localPosition.x, moveDuration).SetEase(Ease.Linear).OnComplete(ShowNextEndlessModeEnemy);
        StaticVariables.WaitTimeThenCallFunction(moveDuration, GameObject.Destroy, enemyParent);
        endlessModeEnemyIndex++;
        if (endlessModeEnemyIndex >= endlessModeEnemyPrefabs.Count)
            endlessModeEnemyIndex = 0;
        //print(enemyParent.transform.localPosition + "    " + startingPosition.localPosition);
    }

    private void ShowNextEndlessModeEnemy(){
        //spawns an enemy at the default position, the rightmost spot outside of the render window
        ShowNextEndlessModeEnemy(endlessModePosition4, endlessModeMoveDuration);
    }

    private void DisplayProgress(){
        hometownBackground.SetActive(false);
        grasslandsBackground.SetActive(false);
        Transform enemySpace = hometownEnemySpace;
        GameObject enemyPrefab = hometownEnemies[1];
        switch (StaticVariables.highestUnlockedWorld){
            case 0:
                hometownBackground.SetActive(true);
                enemySpace = hometownEnemySpace;
                if (StaticVariables.highestUnlockedLevel <= hometownEnemies.Count)
                    enemyPrefab = hometownEnemies[StaticVariables.highestUnlockedLevel - 1];
                break;
            case 1:
                grasslandsBackground.SetActive(true);
                enemySpace = grasslandsEnemySpace;
                if (StaticVariables.highestUnlockedLevel <= grasslandsEnemies.Count)
                    enemyPrefab = grasslandsEnemies[StaticVariables.highestUnlockedLevel - 1];
                break;
            default:
                break;
        }

        GameObject enemyParent = GameObject.Instantiate(emptyGameObject, enemySpace.parent);
        enemyParent.transform.localPosition = enemySpace.localPosition;
        GameObject enemy = GameObject.Instantiate(enemyPrefab, enemyParent.transform);
        enemy.transform.localScale = enemySpace.localScale;
    }
}
