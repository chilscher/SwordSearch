using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditsSceneManager : MonoBehaviour{
    public RectTransform canvas;
    public RectTransform backgroundPage;
    public Text worldNameDisplay;
    public Text stageNumberDisplay;
    public GameObject worldPreviousIcon;
    public GameObject worldNextIcon;
    public GameObject stagePreviousIcon;
    public GameObject stageNextIcon;
    private string finalPlaytestWorldString = "DUSKVALE";
    private string finalPlaytestStageString = "Bat";
    private bool isOnFinalWorld = false;
    private bool isOnFinalStage = false;

    void Start(){
        SetBackgroundSize();
        DisplayProgress();
    }

    private void SetBackgroundSize(){
        float fullHeight = canvas.rect.height;
        float fullWidth = canvas.rect.width;
        backgroundPage.sizeDelta = new Vector2(fullWidth + 500, fullHeight);  
    }

    private void DisplayProgress(){
        SetIsOnFinalWorldAndStage();
        bool isAtBeginning = ((StaticVariables.highestBeatenStage.nextStage.world == 1) && (StaticVariables.highestBeatenStage.nextStage.stage == 1));
        worldNextIcon.SetActive(!isOnFinalStage);
        stageNextIcon.SetActive(!isOnFinalStage);
        worldPreviousIcon.SetActive(!isAtBeginning);
        stagePreviousIcon.SetActive(!isAtBeginning);
        worldNameDisplay.text = "WORLD  " + StaticVariables.highestBeatenStage.nextStage.world;
        stageNumberDisplay.text = "STAGE  " + StaticVariables.highestBeatenStage.nextStage.stage;
    }

    public void WorldDown(){
        int newWorld = StaticVariables.highestBeatenStage.nextStage.world - 1;
        if (newWorld < 1)
            newWorld = 1;
        int newStage = 1;
        StageData stage = StaticVariables.GetStage(newWorld, newStage);
        StaticVariables.highestBeatenStage = stage.previousStage;
        DisplayProgress();
        SaveSystem.SaveGame();
    }

    public void WorldUp(){
        if (isOnFinalStage)
            return;
        else if (isOnFinalWorld) {
            while (StaticVariables.highestBeatenStage.nextStage.enemyPrefab.name != finalPlaytestStageString)
                StaticVariables.highestBeatenStage = StaticVariables.highestBeatenStage.nextStage; 
        }
        else{
            int newWorld = StaticVariables.highestBeatenStage.nextStage.world + 1;
            if (newWorld > 8)
                newWorld = 8;
            int newStage = 1;
            StageData stage = StaticVariables.GetStage(newWorld, newStage);
            StaticVariables.highestBeatenStage = stage.previousStage;
        }
        DisplayProgress(); 
        SaveSystem.SaveGame();
    }
    public void StageDown(){
        if (StaticVariables.highestBeatenStage.previousStage != null)
            StaticVariables.highestBeatenStage = StaticVariables.highestBeatenStage.previousStage;
        DisplayProgress();
        SaveSystem.SaveGame();
    }

    public void StageUp(){
        if (isOnFinalStage)
            return;
        else if ((StaticVariables.highestBeatenStage.nextStage != null) && (StaticVariables.highestBeatenStage.nextStage.nextStage != null))
            StaticVariables.highestBeatenStage = StaticVariables.highestBeatenStage.nextStage;
        DisplayProgress();
        SaveSystem.SaveGame();
    }

    private void SetIsOnFinalWorldAndStage(){
        bool w = false;
        bool s = false;
        if (StaticVariables.highestBeatenStage.nextStage == null) {
            w = true;
            s = true;
        }
        else if (StaticVariables.highestBeatenStage.nextStage.worldName.ToUpper().Contains(finalPlaytestWorldString)) {
            w = true;
            if (StaticVariables.highestBeatenStage.nextStage.enemyPrefab.name == finalPlaytestStageString)
                s = true;
        }
        isOnFinalWorld = w;
        isOnFinalStage = s;
    }
}
