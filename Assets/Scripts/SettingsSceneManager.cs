using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsSceneManager : MonoBehaviour{

    public RectTransform canvas;
    public RectTransform backgroundPage;
    public Text worldNameDisplay;
    public Text stageNumberDisplay;
    public InputField playerNameInput;
    //public Text storyModeDisplay;
    [Header("Difficulty Modes")]
    public Text difficultyModeDisplay;
    public Text difficultyModeDescription;
    [TextArea(2,5)]
    public string normalModeDescription;
    public Color difficultyTextColorNormal;
    [TextArea(2,5)]
    public string storyModeDescription;
    [TextArea(2,5)]
    public string puzzleModeDescription;
    public Color difficultyTextColorPuzzle;
    [TextArea(2,5)]
    public string easyModeDescription;
    public Color difficultyTextColorEasy;
    [TextArea(2,5)]
    public string hardModeDescription;
    public Color difficultyTextColorHard;
    [Header("Misc")]
    public GameObject profanityFilterIcon;
    public GameObject difficultyBackIcon;
    public GameObject difficultyForwardIcon;
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
        DisplayPlayerName();
        DisplayDifficultyMode();
        DisplayProfanitySelection();
    }

    private void SetBackgroundSize(){
        float fullHeight = canvas.rect.height;
        float fullWidth = canvas.rect.width;
        backgroundPage.sizeDelta = new Vector2(fullWidth + 500, fullHeight);
        
    }

    public void HitBackButton(){
        StaticVariables.FadeOutThenLoadScene("Homepage");
    }

    private void DisplayProgress(){
        SetIsOnFinalWorldAndStage();
        bool isAtBeginning = ((StaticVariables.highestBeatenStage.nextStage.world == 1) && (StaticVariables.highestBeatenStage.nextStage.stage == 1));
        worldNextIcon.SetActive(!isOnFinalStage);
        stageNextIcon.SetActive(!isOnFinalStage);
        worldPreviousIcon.SetActive(!isAtBeginning);
        stagePreviousIcon.SetActive(!isAtBeginning);
        worldNameDisplay.text = StaticVariables.highestBeatenStage.nextStage.worldName.ToUpper();
        stageNumberDisplay.text = "STAGE  " + StaticVariables.highestBeatenStage.nextStage.stage;
        //if ((StaticVariables.highestBeatenStage.nextStage.world == 1) && (StaticVariables.highestBeatenStage.nextStage.stage == 1)){
        //    worldPreviousIcon.SetActive(false);
        //    stagePreviousIcon.SetActive(false);
        //}
        //else if (StaticVariables.highestBeatenStage.enemyPrefab.name == finalPlaytestStageString){
        //    worldNextIcon.SetActive(false);
        //    stageNextIcon.SetActive(false);
        //}
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
        //if (StaticVariables.highestBeatenStage.nextStage.worldName.ToUpper().Contains(finalPlaytestWorldString)) //for alpha test
        //    while (StaticVariables.highestBeatenStage.enemyPrefab.name != finalPlaytestStageString)
        //        StaticVariables.highestBeatenStage = StaticVariables.highestBeatenStage.nextStage;
        //return;
        //else
        //{
        //}
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
        if (isOnFinalStage) //for alpha test
            return;
        else if ((StaticVariables.highestBeatenStage.nextStage != null) && (StaticVariables.highestBeatenStage.nextStage.nextStage != null))
            StaticVariables.highestBeatenStage = StaticVariables.highestBeatenStage.nextStage;
        DisplayProgress();
        SaveSystem.SaveGame();
    }

    private void DisplayPlayerName(){
        playerNameInput.text = StaticVariables.playerName;
    }

    public void UpdatePlayerName(){
        if (playerNameInput.text == "")
            DisplayPlayerName();
        else{
        StaticVariables.playerName = playerNameInput.text;
            DisplayPlayerName();
        }
        SaveSystem.SaveGame();
    }
    
    public void NextDifficultyMode(){
        //puzzle > easy > normal > hard
        switch (StaticVariables.difficultyMode){
            case StaticVariables.DifficultyMode.Puzzle:
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Easy;
                break;
            case StaticVariables.DifficultyMode.Easy:
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Normal;
                break;
            case StaticVariables.DifficultyMode.Normal:
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Hard;
                break;
            case StaticVariables.DifficultyMode.Hard:
                //don't advance;
                break;
            default:
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Normal;
                break;
            //case StaticVariables.DifficultyMode.Story:
            //    StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Normal;
            //    break;
        }
        DisplayDifficultyMode();
        SaveSystem.SaveGame();
    }
    
    public void PreviousDifficultyMode(){ 
        //hard > normal > easy > puzzle     
        switch (StaticVariables.difficultyMode){
            case StaticVariables.DifficultyMode.Hard:
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Normal;
                break;
            case StaticVariables.DifficultyMode.Normal:
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Easy;
                break;
            case StaticVariables.DifficultyMode.Easy:
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Puzzle;
                break;
            case StaticVariables.DifficultyMode.Puzzle:
                //don't advance;
                break;
            default:
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Normal;
                break;
        }
        DisplayDifficultyMode();
        SaveSystem.SaveGame();
    }
    
    public void DisplayDifficultyMode(){
        difficultyBackIcon.SetActive(true);
        difficultyForwardIcon.SetActive(true);
        switch (StaticVariables.difficultyMode) {
            case StaticVariables.DifficultyMode.Normal:
                difficultyModeDisplay.text = "NORMAL";
                difficultyModeDescription.text = normalModeDescription;
                difficultyModeDisplay.color = difficultyTextColorNormal;
                break;
            case StaticVariables.DifficultyMode.Story:
                difficultyModeDisplay.text = "STORY";
                difficultyModeDescription.text = storyModeDescription;
                break;
            case StaticVariables.DifficultyMode.Puzzle:
                difficultyModeDisplay.text = "PUZZLE";
                difficultyModeDescription.text = puzzleModeDescription;
                difficultyModeDisplay.color = difficultyTextColorPuzzle;
                difficultyBackIcon.SetActive(false);
                break;
            case StaticVariables.DifficultyMode.Easy:
                difficultyModeDisplay.text = "EASY";
                difficultyModeDescription.text = easyModeDescription;
                difficultyModeDisplay.color = difficultyTextColorEasy;
                break;
            case StaticVariables.DifficultyMode.Hard:
                difficultyModeDisplay.text = "HARD";
                difficultyModeDescription.text = hardModeDescription;
                difficultyModeDisplay.color = difficultyTextColorHard;
                difficultyForwardIcon.SetActive(false);
                break;
            default:
                difficultyModeDisplay.text = "ERROR";
                difficultyModeDescription.text = "Attempting to use an invalid difficulty mode.";
                break;
        }
    }
    
    public void ToggleProfanityFilter(){
        StaticVariables.allowProfanities = !StaticVariables.allowProfanities;
        DisplayProfanitySelection();
        SaveSystem.SaveGame();
    }

    private void DisplayProfanitySelection() {
        profanityFilterIcon.SetActive(!StaticVariables.allowProfanities);
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

    public void PushedCreditsButton(){
        print("go to credits");
    }
}
