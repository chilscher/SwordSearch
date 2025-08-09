//for SwordSearch, copyright Cole Hilscher 2025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SaveData{
    //handles the player's save data

    public int worldProgress;
    public int stageProgress;
    //public int lastVisitedWorld;
    //public int lastVisitedStage;
    public string playerName;
    public string difficultyMode;
    public bool hasTalkedToNewestEnemy;
    public float gameVersionNumber;
    public string buffedType;
    public bool allowProfanities;

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO SAVE PLAYER DATA
    // ---------------------------------------------------

    public SaveData() {
        worldProgress = StaticVariables.highestBeatenStage.world;
        stageProgress = StaticVariables.highestBeatenStage.stage;
        //lastVisitedWorld = StaticVariables.lastVisitedStage.world;
        //lastVisitedStage = StaticVariables.lastVisitedStage.stage;
        hasTalkedToNewestEnemy = StaticVariables.hasTalkedToNewestEnemy;
        playerName = StaticVariables.playerName;
        allowProfanities = StaticVariables.allowProfanities;
        switch (StaticVariables.difficultyMode) {
            case (StaticVariables.DifficultyMode.Normal):
                difficultyMode = "normal";
                break;
            case (StaticVariables.DifficultyMode.Story):
                difficultyMode = "story";
                break;
            case (StaticVariables.DifficultyMode.Puzzle):
                difficultyMode = "puzzle";
                break;
            case (StaticVariables.DifficultyMode.Easy):
                difficultyMode = "easy";
                break;
            case (StaticVariables.DifficultyMode.Hard):
                difficultyMode = "hard";
                break;
        }
        buffedType = StaticVariables.buffedType switch {
            (BattleManager.PowerupTypes.Water) => "water",
            (BattleManager.PowerupTypes.Heal) => "heal",
            (BattleManager.PowerupTypes.Earth) => "earth",
            (BattleManager.PowerupTypes.Fire) => "fire",
            (BattleManager.PowerupTypes.Lightning) => "lightning",
            (BattleManager.PowerupTypes.Dark) => "dark",
            (BattleManager.PowerupTypes.Sword) => "sword",
            _ => "none",
        };
        gameVersionNumber = StaticVariables.gameVersionNumber;
    }

    // ---------------------------------------------------
    //ALL OF THE FUNCTIONS THAT ARE USED TO LOAD PLAYER DATA
    // ---------------------------------------------------

    public void LoadData() {
        StaticVariables.highestBeatenStage = StaticVariables.GetStage(worldProgress, stageProgress);
        //StaticVariables.lastVisitedStage = StaticVariables.GetStage(lastVisitedWorld, lastVisitedStage);
        StaticVariables.playerName = playerName;
        StaticVariables.allowProfanities = allowProfanities;
        switch (difficultyMode) {
            case ("normal"):
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Normal;
                break;
            case ("story"):
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Story;
                break;
            case ("puzzle"):
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Puzzle;
                break;
            case ("easy"):
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Easy;
                break;
            case ("hard"):
                StaticVariables.difficultyMode = StaticVariables.DifficultyMode.Hard;
                break;
        }
        StaticVariables.buffedType = buffedType switch {
            "water" => (BattleManager.PowerupTypes.Water),
            "heal" => (BattleManager.PowerupTypes.Heal),
            "earth" => (BattleManager.PowerupTypes.Earth),
            "fire" => (BattleManager.PowerupTypes.Fire),
            "lightning" => (BattleManager.PowerupTypes.Lightning),
            "dark" => (BattleManager.PowerupTypes.Dark),
            "sword" => (BattleManager.PowerupTypes.Sword),
            _ => BattleManager.PowerupTypes.None,
        };
        StaticVariables.hasTalkedToNewestEnemy = hasTalkedToNewestEnemy;
        StaticVariables.gameVersionNumber = gameVersionNumber; //if there is no saved version number, it defaults to 0
    }
}
