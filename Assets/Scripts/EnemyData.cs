using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class EnemyData : MonoBehaviour{

    public bool isBattleable = true;
    public string nameOverride = "";

    
    [Header("Battle Stats")]
    [ConditionalField(nameof(isBattleable))]    public int startingHealth = 10;
    //[ConditionalField(nameof(isBattleable))]    public float attackSpeed = 6f;
    //[ConditionalField(nameof(isBattleable))]    public int attackDamage = 2;
    [ConditionalField(nameof(isBattleable))]    public CollectionWrapper<EnemyAttack> attackOrder;
    [ConditionalField(nameof(isBattleable))]    public bool isDraconic = false;
    [ConditionalField(nameof(isBattleable))]    public bool isHorde = false;
    [ConditionalField(nameof(isBattleable))]    public bool isHoly = false;
    [ConditionalField(nameof(isBattleable))]    public bool isDark = false;
    [ConditionalField(nameof(isBattleable))]    public bool isNearWater = false;

    [Header("Chatheads")]
    public Sprite normal;
    public Sprite angry;
    public Sprite defeated;
    public Sprite excited;
    public Sprite questioning;

    [Header("Dialogue Steps")]
    public DialogueStep[] overworldDialogueSteps;
    public DialogueStep[] victoryDialogueSteps;

    public string GetDisplayName(){
        string n = name;
        if (nameOverride != "")
            n = nameOverride;
        return n;
    }
}


[System.Serializable]
public class DialogueStep{
    public enum DialogueType{PlayerTalking, EnemyTalking, OtherTalking, Event};
    
    public enum Emotion{Normal, Angry, Defeated, Excited, Happy, Questioning, Worried};
    public DialogueType type;    
    [ConditionalField(nameof(type), false, DialogueType.OtherTalking)] public EnemyData talker;
    public Emotion emotion;
    [TextArea(2,5)]
    public string description;
}

[System.Serializable]
public class EnemyAttack{
    public enum EnemyAttackTypes{ThrowRocks}

    public float attackSpeed = 2f;
    public int attackDamage = 6;
    public bool isSpecial = false;
    [ConditionalField(nameof(isSpecial))] public EnemyAttackTypes specialType;
    [ConditionalField(nameof(isSpecial))] public Color specialColor;
}
