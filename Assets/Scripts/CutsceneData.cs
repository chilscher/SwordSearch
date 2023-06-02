using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CutsceneData{

    public Sprite startingImage;
    public CutsceneStep[] cutsceneSteps;
}


[System.Serializable]
public class CutsceneStep{

    public enum CutsceneType{PlayerTalking, OtherTalking, ChangeImage, PlayAnimation, GoToBattle, GoToOverworld, GoToTutorial};
    public CutsceneType type;
    public EnemyData characterTalking;
    public DialogueStep.Emotion emotion;
    public Sprite newImage;
    
    [TextArea(2,5)]
    public string description;
}