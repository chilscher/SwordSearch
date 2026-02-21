using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsSceneManager : MonoBehaviour{

    public RectTransform canvas;
    public RectTransform backgroundPage;
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

    void Start(){
        SetBackgroundSize();
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

    public void PushedCreditsButton(){
        SceneChanger.GoCredits();
    }
}
