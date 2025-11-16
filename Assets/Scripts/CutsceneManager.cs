using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Unity.VisualScripting;

public class CutsceneManager : MonoBehaviour{

    private int cutsceneStep = 0;
    private enum Cond{Click, Wait, BackgroundChange, externalTrigger};
    private Cond advanceCondition;
    public enum Cutscene{Hometown1, Hometown2, Grasslands1, Grasslands2, Forest1, Forest2, Forest3, Desert1, Desert2, Desert3, City1};
    private Cutscene cutsceneID;
    private List<Animator> animatedObjectsInCutscene = new List<Animator>();
    private List<GameObject> searchableObjectsInCutscene = new List<GameObject>();
    private float shakeTimer = 0f;
    private Animator playerAnimator;
    private GameObject nextBackground;

    //public GeneralSceneManager generalSceneManager;
    public DialogueManager dialogueManager;
    public RectTransform backgroundParent;
    public GameObject emptyGameObject;
    public RectTransform screenshakeTransform;

    [Header("Cutscene Backgrounds")]
    public GameObject hometown1;
    public GameObject hometown2;
    public GameObject grasslands1;
    public GameObject grasslands2_pt1;
    public GameObject grasslands2_pt2;
    public GameObject forest1;
    public GameObject forest2_pt1;
    public GameObject forest2_pt2;
    public GameObject forest3;
    public GameObject desert1;
    public GameObject desert2;
    public GameObject desert3_pt1;
    public GameObject desert3_pt2;
    public GameObject desert3_pt3;
    public GameObject desert3_pt4;
    public GameObject desert3_pt5;
    public GameObject city1;

    private float externalTriggerParameter = 0f;

    

    public void Start() {
        GetComponent<GeneralSceneManager>().Setup();
        //generalSceneManager.Setup();
        SetCutsceneID();
        switch (cutsceneID){
            case (Cutscene.Hometown1):
                SetupHometown1();
                break;
            case (Cutscene.Hometown2):
                SetupHometown2();
                break;
            case (Cutscene.Grasslands1):
                SetupGrasslands1();
                break;
            case (Cutscene.Grasslands2):
                SetupGrasslands2();
                break;
            case (Cutscene.Forest1):
                SetupForest1();
                break;
            case (Cutscene.Forest2):
                SetupForest2();
                break;
            case (Cutscene.Forest3):
                SetupForest3();
                break;
            case (Cutscene.Desert1):
                SetupDesert1();
                break;
            case (Cutscene.Desert2):
                SetupDesert2();
                break;
            case (Cutscene.Desert3):
                SetupDesert3();
                break;
            case (Cutscene.City1):
                SetupCity1();
                break;
        }
        
        dialogueManager.SetButtonText("CONTINUE");

        SetupDialogueManager();
        StaticVariables.WaitTimeThenCallFunction(StaticVariables.sceneFadeDuration, AdvanceCutsceneStep);
        //generalSceneManager.FadeIn();
    }

    private void SetCutsceneID(){
        cutsceneID = StaticVariables.cutsceneID;
    }

    private void SetupHometown1(){
        SetCutsceneBackground(hometown1);
        ToggleObject("Player", false);
        //cutsceneStep = 50; //for testing goblin walking
        //cutsceneStep = 70; //for testing player throwing books
        //cutsceneStep = 79; //for testing the water spray
    }

    private void SetupHometown2(){
        SetCutsceneBackground(hometown2);
        PlayAnimation("Redhead Lady", "Idle Back");
        PlayAnimation("Bartender", "Idle Front");
        PlayAnimation("Child 1", "Idle Front");
        PlayAnimation("Child 2", "Idle Front");
        //cutsceneStep = 40; //for testing townspeople walking into a cirlce
    }

    private void SetupGrasslands1(){
        SetCutsceneBackground(grasslands1);
    }

    private void SetupGrasslands2(){
        SetCutsceneBackground(grasslands2_pt1);
        PlayAnimation("Player", "Idle Holding Book");
    }

    private void SetupForest1(){
        SetCutsceneBackground(forest1);
    }

    private void SetupForest2(){
        SetCutsceneBackground(forest2_pt1);
        PlayAnimation("Player", "Walk");
        Transform rabbitArea = GetObjectFromName("Starting area").transform;
        rabbitArea.DOLocalMoveX(rabbitArea.localPosition.x -3000, 2.5f).SetEase(Ease.Linear);     
        //cutsceneStep = 28;  
    }

    private void SetupForest3(){
        SetCutsceneBackground(forest3);
    }

    private void SetupDesert1(){
        SetCutsceneBackground(desert1);
        PlayAnimation("Player", "Walk");
        MoveEverythingExceptPlayer(-1050, 0, 3f);
    }

    private void SetupDesert2(){
        SetCutsceneBackground(desert2);
        //cutsceneStep = 25; //for testing horde loop
        //cutsceneStep = 80; //for testing walk off screen
    }

    private void SetupDesert3(){
        SetCutsceneBackground(desert3_pt1);
        PlayAnimation("Player", "Idle Holding Book");
        //cutsceneStep = 33; //for testing oasis section
    }

    private void SetupCity1(){
        SetCutsceneBackground(city1);
        //ToggleObject("Player", false);
        //cutsceneStep = 50; //for testing goblin walking
        //cutsceneStep = 79; //for testing the water spray
    }

    private void AdvanceCutsceneStep(){
        cutsceneStep ++;
        switch (cutsceneID){
            case (Cutscene.Hometown1):
                DoHometown1Step();
                break;
            case (Cutscene.Hometown2):
                DoHometown2Step();
                break;
            case (Cutscene.Grasslands1):
                DoGrasslands1Step();
                break;
            case (Cutscene.Grasslands2):
                DoGrasslands2Step();
                break;
            case (Cutscene.Forest1):
                DoForest1Step();
                break;
            case (Cutscene.Forest2):
                DoForest2Step();
                break;
            case (Cutscene.Forest3):
                DoForest3Step();
                break;
            case (Cutscene.Desert1):
                DoDesert1Step();
                break;
            case (Cutscene.Desert2):
                DoDesert2Step();
                break;
            case (Cutscene.Desert3):
                DoDesert3Step();
                break;
            case (Cutscene.City1):
                DoCity1Step();
                break;
        }
        CheckButtonAvailability();
    }

    private void CheckButtonAvailability(){
        switch (advanceCondition){
            case (Cond.Click):
                ToggleButton(true);
                break;
            default:
                ToggleButton(false);
                break;
        }
    }

    private void DoHometown1Step(){   
        int i = 0;
        if (++i == cutsceneStep){
            ToggleObject("Player", true);
            PlayAnimationAndMoveThenIdle("Player", "Walk", -201, 2025, 1.5f);
            AdvanceConditionWait(1.5f);
        }
        else if (++i == cutsceneStep){
            FlipDirection("Player");
            AdvanceConditionDialogue_PlayerTalking("Ah, spring! The warm sun always puts me in a reading mood!", DialogueStep.Emotion.Happy);
        }    
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Take Out Book Random Brown");
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh, who am I kidding... All weather puts me in a reading mood.", DialogueStep.Emotion.Normal);
        }    
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("Miss " + StaticVariables.playerName + "! Miss " + StaticVariables.playerName + "!", "Child 2", DialogueStep.Emotion.Happy);
            PlayAnimationAndMoveThenIdle("Child 1", "Walk", 26, 605, 2f);
            PlayAnimationAndMoveThenIdle("Child 2", "Walk Holding Newspaper", 85, 564, 2.3f);
            AdvanceConditionWaitThenClick(2.3f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Put that dumb book down! We need you to read this newspaper!!", "Child 1", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Mikey stop! You know how Miss " + StaticVariables.playerName + " feels when you insult her books!", "Child 2", DialogueStep.Emotion.Angry);
            PlayAnimation("Player", "Put Away Book Random Brown");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("She'll never stop talking about \"the prince's manticore wears shorts\"!", "Child 2", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("That doesn't sound right, I think it's \"a pig is manlier when it snores\".", "Child 1", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I've never said anything that sounds even remotely like that nonsense before!", DialogueStep.Emotion.Angry);
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Maybe if you had paid attention in class you'd know it's \"the pen is mightier than the sword\"!", DialogueStep.Emotion.Angry);
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("And you'd be able to read that newspaper for yourselves!", DialogueStep.Emotion.Angry);
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Oh yeah! Read it to us! Please???", "Child 2", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Okay, okay...\nLet's see here...", DialogueStep.Emotion.Normal);
        }     
        else if (++i == cutsceneStep){
            PlayAnimation("Child 2", "Idle Not Holding Newspaper");
            PlayAnimation("Player", "Idle Holding Newspaper");
            AdvanceConditionDialogue_PlayerTalking("Ahem!\nThe headline reads,\n\"Lich King Defeated! Duskvale in Ruins!\"", DialogueStep.Emotion.Normal);
        }       
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("What did you say?", "Blacksmith", DialogueStep.Emotion.Normal);
            AdvanceConditionWait(0.2f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Blacksmith", "Walk", -110, 11, 1.4f);
            AdvanceConditionWaitThenClick(1.6f);
        }   
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("My daughter lives in Duskvale!", "Brown Hair Lady No Hat", DialogueStep.Emotion.Angry);
            AdvanceConditionWait(0.2f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Brown Hair Lady No Hat", "Walk", -436, 368, 1.4f);
            AdvanceConditionWaitThenClick(1.6f);
        } 
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("I hate the Lich King!", "Redhead Lady", DialogueStep.Emotion.Angry);
            AdvanceConditionWait(0.2f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Redhead Lady", "Walk", 177, 334, 2f);
            AdvanceConditionWaitThenClick(2.2f);
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Keep Reading!!!", "Everyone - Hometown Intro", DialogueStep.Emotion.Custom1); //trio of adults, angry
        }
        else if (++i == cutsceneStep){
            HideEnemyChathead();
            AdvanceConditionDialogue_PlayerTalking("\"Late last night, a great hole appeared in the center of Duskvale.\"", DialogueStep.Emotion.Normal);
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("\"Out of the hole came a burst of flame, and the city was quickly beseiged by --\"", DialogueStep.Emotion.Normal);
        }     
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("ROOOOOOOAR!!!!", "Red Dragon", DialogueStep.Emotion.Mystery, "???");
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Red Dragon", "Fly");
            MoveObject("Red Dragon", 1200, 1377, 2f);
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            FlipDirection("Child 1");
            FlipDirection("Child 2");
            FlipDirection("Redhead Lady");
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            MoveObject("Red Dragon", 521, 1050, 2f);
            FlipDirection("Red Dragon");
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Red Dragon", "Land");
            MoveObject("Red Dragon", 521, 1000, 1.1f);
            AdvanceConditionWait(0.6f);
        }
        else if (++i == cutsceneStep){
            StartScreenShake(1f);
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("\"-- a group of dragons!\"", DialogueStep.Emotion.Worried);
            PlayAnimation("Player", "Put Away Newspaper");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Ha ha ha...\nPuny humans in your simple town...", "Red Dragon", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I could eat all of you on a whim!", "Red Dragon", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Instead I come with good news! Your hated Lich King has been driven from his throne!", "Red Dragon", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("This town now belongs to the mighty Queen of Ash! Bow down to her, me, and all of dragonkind!", "Red Dragon", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("We won't be bossed around by some overgrown lizard!", "Blacksmith", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("We're not afraid of you!", "Redhead Lady", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Don't antagonize the dragon! It could kill us all!", DialogueStep.Emotion.Worried);
        }        
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Yes, the young lady makes quite the compelling argument.", "Red Dragon", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Allow me to give you a little taste of fear!", "Red Dragon", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Red Dragon", "Prolonged Attack - Start");
            AdvanceConditionWait(0.7f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Red Dragon", "Prolonged Attack - End");

            ToggleObject("Fires", true);
            ToggleObject("Fire 1", true);
            ToggleObject("Fire 2", true);
            ToggleObject("Fire 3", true);
            ToggleObject("Fire 4", true);

            Image sky1 = GetObjectFromName("Sky 1").GetComponent<Image>();
            Image sky2 = GetObjectFromName("Sky 2").GetComponent<Image>();
            Image ground1 = GetObjectFromName("Ground 1").GetComponent<Image>();
            Image ground2 = GetObjectFromName("Ground 2").GetComponent<Image>();
            sky1.DOColor(sky2.color, 2f);
            sky2.color = sky1.color;
            ground1.DOColor(ground2.color, 2f);
            ground2.color = ground1.color;

            AdvanceConditionWait(3f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Ha ha ha... You humans are powerless!", "Red Dragon", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("What do you want from us?", "Brown Hair Lady No Hat", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Well of course you'll be paying taxes to your new draconic rulers!", "Red Dragon", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("But it appears this commotion drew the attention of some local goblins.", "Red Dragon", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Goblin 1", "Walk", 70, -96, 2.2f);
            PlayAnimationAndMoveThenIdle("Goblin 2", "Walk", 514, 310, 2f);
            PlayAnimationAndMoveThenIdle("Goblin 3", "Walk", 446, 489, 2f);
            PlayAnimationAndMoveThenIdle("Goblin General", "Walk", 240, 183, 1.5f);
            AdvanceConditionWait(2.5f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Humans! We come for your gold!", "Goblin General", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I'd love to stay and help you peasants, but I have other towns to visit.", "Red Dragon", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Do your best not to die. The Queen of Ash has no use for weaklings!", "Red Dragon", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Red Dragon", "Fly", -1500, 1400, 2f);
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Attack the invaders! Defend our homes!", "Everyone - Hometown Intro", DialogueStep.Emotion.Custom1);
        }
        else if (++i == cutsceneStep){
            DisplayNobodyTalking();
            HideEnemyChathead();
            AdvanceConditionWait(0.2f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Redhead Lady", "Walk");
            MoveObject("Redhead Lady", 274, 535, 1f);
            AdvanceConditionWait(0.1f);
        }       
        else if (++i == cutsceneStep){
            PlayAnimation("Blacksmith", "Walk");
            MoveObject("Blacksmith", -110, -48, 0.2f);
            AdvanceConditionWait(0.1f);
        }     
        else if (++i == cutsceneStep){
            PlayAnimation("Brown Hair Lady No Hat", "Walk");
            MoveObject("Brown Hair Lady No Hat", 29, 130, 0.8f);
            AdvanceConditionWait(0.1f);
        }     
        else if (++i == cutsceneStep){
            PlayAnimation("Goblin 1", "Walk");
            PlayAnimationAndMoveThenIdle("Goblin 2", "Walk", 575, 310, 0.5f);
            FlipDirection("Goblin 2");
            AdvanceConditionWait(0.4f);
        }     
        else if (++i == cutsceneStep){
            PlayAnimation("Goblin General", "Walk");
            AdvanceConditionWait(0.4f);
        }     
        else if (++i == cutsceneStep){
            PlayAnimation("Goblin 3", "Walk");
            AdvanceConditionWait(0.4f);
        }    
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I don't know anything about fighting...", DialogueStep.Emotion.Worried);
        }        
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Us neither!", "Child 1", DialogueStep.Emotion.Worried);
            FlipDirection("Child 1");
            FlipDirection("Child 2");
        }        
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("We should leave the town defense to the professionals...", DialogueStep.Emotion.Normal);
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh no! The library is on fire!", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("All of my precious books are burning! I have to save them!", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            DisplayNobodyTalking();
            HideChatheads();
            PlayAnimation("Player", "Walk");
            MoveObject("Player", -75, 2137, 1.5f);
            AdvanceConditionWait(1.5f);
        }
        else if (++i == cutsceneStep){
            ToggleObject("Player", false);
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Tossing Books").GetComponent<CutsceneBookThrow>().StartThrow();
            AdvanceConditionWait(3f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Her best idea was chucking books into a big pile?", "Child 1", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Couldn't we chuck 'em in a well or something?", "Child 1", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("No, you dummy! Water is really damaging for books!", "Child 2", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Oh, yeah! Maybe we should go in there and help her?", "Child 1", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Uh, no thanks. I don't want to get smacked by a flaming book.", "Child 2", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Besides, how many books can she even have in there? Do you think she's almost done?", "Child 2", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Oh, I know! Let's go find a goblin to fight!", "Child 1", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Wait, do you hear that?", "Child 2", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            DisplayNobodyTalking();
            HideEnemyChathead();
            StartScreenShake();
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Tossing Books").GetComponent<CutsceneBookThrow>().StopThrow();
            AdvanceConditionWait(0.66f);
        }
        else if (++i == cutsceneStep){              
            AdvanceConditionDialogue_PlayerTalking("What's happening now??", DialogueStep.Emotion.Questioning);
            ToggleObject("Player", true);
            FlipDirection("Player");
        }
        else if (++i == cutsceneStep){
            DisplayNobodyTalking();
            HideChatheads();
            PlayAnimationAndMoveThenIdle("Player", "Walk", -140, 1860, 1.5f);
            Transform t = GetObjectFromName("Tossing Books").transform;
            int index = t.parent.GetSiblingIndex();
            GetAnimatorFromName("Child 1").transform.parent.SetSiblingIndex(index - 3);
            GetAnimatorFromName("Child 2").transform.parent.SetSiblingIndex(index - 2);
            t.Find("Library door edge").gameObject.SetActive(false);
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep){  
            GetAnimatorFromName("Tossing Books").transform.Find("Water Spray").gameObject.SetActive(true);
            GetAnimatorFromName("Tossing Books").transform.Find("Water Spray 2").gameObject.SetActive(true);
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){  
            StopShakeScreen();   
            AdvanceConditionWait(0.1f);     
        }
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Tossing Books").transform.Find("Water Spray").Find("Book").gameObject.SetActive(false);
            GetAnimatorFromName("Tossing Books").transform.Find("Water Spray 2").Find("Book").gameObject.SetActive(true);
            MagicFlash flash = GetAnimatorFromName("Tossing Books").transform.Find("Water Spray 2").Find("Magic Flash").GetComponent<MagicFlash>();
            flash.gameObject.SetActive(true);
            flash.StartProcess(StaticVariables.waterPowerupColor);
            AdvanceConditionWait(flash.GetTotalTime() - flash.fadeTime);
        }
        else if (++i == cutsceneStep){
            ToggleObject("Rain", true);
            MoveObject("Rain", -33, -1700, 7f);
            ToggleObject("Fires", false);
            ToggleObject("Fire 1", false);
            ToggleObject("Fire 2", false);
            ToggleObject("Fire 3", false);
            ToggleObject("Fire 4", false);
            ToggleObject("Puddles", true);
            ToggleObject("Sky 1", false);
            ToggleObject("Sky 2", true);
            ToggleObject("Ground 1", false);
            ToggleObject("Ground 2", true);
            StopShakeScreen();
            AdvanceConditionWait(4f);
        }
        else if (++i == cutsceneStep){
            Transform player = GetObjectFromName("Player").transform.parent;
            player.SetSiblingIndex(player.parent.childCount -4);
            AdvanceConditionDialogue_PlayerTalking("There's a book coming out of the water!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            HideChatheads();
            GetAnimatorFromName("Tossing Books").transform.Find("Water Spray").GetComponent<Animator>().Play("End Spray");
            GetAnimatorFromName("Tossing Books").transform.Find("Water Spray 2").GetComponent<Animator>().Play("End Spray");
            AdvanceConditionWait(0.7f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Walk");
            MoveObject("Player", 300, 1860, 1.1f);
            AdvanceConditionWait(0.6f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Book Catch");
            AdvanceConditionWait(0.367f);
        }
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Tossing Books").transform.Find("Water Spray").gameObject.SetActive(false);
            GetAnimatorFromName("Tossing Books").transform.Find("Water Spray 2").gameObject.SetActive(false);
            PlayAnimation("Player", "Idle Holding Book Flipped");
            AdvanceConditionDialogue_PlayerTalking("Got it!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("This book is completely dry!", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It's called \n\"The Spell-Book\".", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Isn't this just an introduction to spelling and the alphabet?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But... now the pages are filled with random letters!", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Could it be an actual spellbook?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("And the letters react to my touch! It's definitely probably magical!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            EndNormalStage();
        }
    }
    
    private void DoHometown2Step(){
        int i = 0;
        if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I saw you shooting some <water>magic water<> at those goblins!", "Blacksmith", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I didn't know you had that fighting spirit in you!", "Blacksmith", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("I wouldn't call myself much of a warrior...", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Nonsense! You were incredible!", "Blacksmith", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("And the magic too! How did you do that??", "Brown Hair Lady No Hat", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("To be honest, I'm not sure.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("One of the old library books has these weird glowing letters, and...", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("It might make more sense if you give it a try yourself!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            PlayAnimation("Player", "Take Out Book");
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("The paper has a bunch of strange symbols on it...", "Brown Hair Lady No Hat", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Those are letters. You know, the kind people use for reading and writing?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Wow! Magic is crazy!", "Brown Hair Lady No Hat", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Um, no, that's what normal books are like...", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("You know, maybe it'll make more sense if you just touch some of the letters!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Brown Hair Lady No Hat", "Walk", -200, 585, 0.4f);
            AdvanceConditionWait(0.4f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Brown Hair Lady No Hat", "Walk", -117, 550, 0.4f);
            AdvanceConditionWait(0.4f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Nothing interesting happens...", "Brown Hair Lady No Hat", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("That's weird, it works for me. Watch!", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            PlayAnimation("Player", "Cast Spell");
            AdvanceConditionWait(0.33f);
        }
        else if (++i == cutsceneStep) {
            GetAnimatorFromName("Player").transform.parent.GetChild(2).gameObject.SetActive(true);
            AdvanceConditionWait(1.5f);
        }
        else if (++i == cutsceneStep) {
            GetAnimatorFromName("Player").transform.parent.GetChild(2).gameObject.SetActive(false);
            AdvanceConditionDialogue_EnemyTalking("Hey! Don't start throwing magic at people!", "Redhead Guy", DialogueStep.Emotion.Angry);
            FlipDirection("Redhead Guy");
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Yeah, if you want to sling spells, go to the Academy!", "Blond Lady", DialogueStep.Emotion.Angry);
            FlipDirection("Blond Lady");
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("It's been a long time since anyone used magic around here.", "Redhead Lady", DialogueStep.Emotion.Normal);
            PlayAnimation("Redhead Lady", "Idle");
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Yeah, it's been decades since Eldric was stomping around here, slinging his spells...", "Redhead Guy", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Hey, I'm not dead, I'm just old!", "Elder", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Elder", "Walk", 427, 471, 2f);
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("You whippersnappers are flappin' yer yappers about my magic?", "Elder", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Well, you're right. Back in the old days, when I could still touch my toes, I had control over the wind!", "Elder", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Like you could make tornadoes and stuff?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Yes! I could make tornadoes! And stuff!", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I was an adventurer! Slaying monsters, saving princesses...", "Elder", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I even fought an owlbear once!", "Elder", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("But one day I woke up with a shake in my hands, and that was it. I couldn't use magic after that.", "Elder", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("I've read some of the library's medical textbooks. Maybe there's something in one of them that could heal your hand tremors?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Oh! My book can do some <healing>healing magic<>! I bet I could fix your hands with that!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Young lady, that is very kind of you. But I'm afraid we have more important matters to discuss today.", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Gather 'round, everyone!", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            PlayAnimation("Child 2", "Walk");
            MoveObject("Child 2", 268, 172, 1.4f);
            AdvanceConditionWait(0.1f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Child 1", "Walk", 361, 370, 1.6f);
            AdvanceConditionWait(0.4f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Blond Lady", "Walk", 73, 803, 2.3f);
            AdvanceConditionWait(0.2f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Brown Hair Lady No Hat", "Walk", -384, 413, 1.8f);
            AdvanceConditionWait(0.2f);
        }
        else if (++i == cutsceneStep) {
            FlipDirection("Shopkeeper");
            PlayAnimationAndMoveThenIdle("Shopkeeper", "Walk", -156, 129, 1.6f);
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Chef", "Walk", -284, 254, 1.6f);
            AdvanceConditionWait(0.2f);
        }    
        else if (++i == cutsceneStep) {
            PlayAnimation("Child 2", "Idle Not Holding Newspaper");
            PlayAnimationAndMoveThenIdle("Redhead Lady", "Walk", 348, 573, 1.4f);
            AdvanceConditionWait(0.3f);
        }    
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Redhead Guy", "Walk", 219, 724, 0.8f);
            FlipDirection("Blue-haired Lady");
            PlayAnimationAndMoveThenIdle("Blue-haired Lady", "Walk", 106, 114, 0.8f);
            AdvanceConditionWait(0.3f);
        }    
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Blacksmith", "Walk", -205, 763, 0.5f);
            FlipDirection("Brown Hair Lady No Hat");
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            FlipDirection("Shopkeeper");
            FlipDirection("Blacksmith");
            AdvanceConditionWait(0.5f);
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionWait(0.5f);
       // }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionWait(0.5f);
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Allow me to be blunt here... If that dragon comes by again, we're toast!", "Elder", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("We need to do something!", "Blond Lady", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            HideEnemyChathead();
            AdvanceConditionDialogue_PlayerTalking("I have some information that may be helpful.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I was reading this newspaper before the dragon showed up!", DialogueStep.Emotion.Happy);
            PlayAnimation("Player", "Take Out Newspaper");
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_NobodyTalking();
        //}        
        //else if (++i == cutsceneStep){
        //    PlayAnimation("Player", "Take Out Newspaper");
        //    AdvanceConditionWait(1f);
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Let me read you all the full article...\n\"Last night a great hole appeared in the center of Duskvale.\"", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("\"Out of the hole came a burst of flame, and the city was quickly beseiged by a group of dragons!\"", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("\"The Lich King and his advisors promptly engaged the dragons in combat. Vibrant blasts of magic and jets of fire shook the city!\"", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("\"After a few minutes, the dragons claimed victory, and razed the city to the ground! His Lichness has not been seen since the attack.\"", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("\"Local experts believe the King has been killed, or is currently searching for the famed Sword of Dragonslaying.\"", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("\"Other local experts believe the Sword of Dragonslaying is purely a myth, and it would be dangerous to go searching for it with dragons about.\"", DialogueStep.Emotion.Worried);
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_PlayerTalking("\"A third group of local experts believe that despite the Lich King's low approval ratings, the dragon attack was not politically motivated.\"", DialogueStep.Emotion.Questioning);
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("\"A third group of local experts believe that, due to the Lich King's record low approval ratings, the attack may have been politically motivated.\"", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Well I'm not one for politics, but that bit about a dragon-killing sword sounds pretty useful right about now!", "Redhead Guy", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I had a book about that sword in the library, but I think it got ruined in the dragon attack.", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Oh don't worry about that, Miss " + StaticVariables.playerName + "!", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I knew a swordswoman who wielded the power of that very sword!", "Elder", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("There weren't many dragons around back then, so we sealed the sword away in a temple in the desert.", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("For all I know, it might just still be there!", "Elder", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("We have to go and get it!", "Brown Hair Lady No Hat", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Let's go now!", "Blacksmith", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Which way to the desert??", "Redhead Lady", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Now don't be too hasty! It's quite a journey to get there, and they call it the \"Sunscorched Desert\" for a reason!", "Elder", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I can go.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("With my <water>water magic<> I should be able to survive there.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I'll return with the sword, and then we can fight back against these dragon tyrants!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("If you're sure...", "Elder", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I am.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("To get to the desert, you'll have to get to the far side of the enchanted forest, beyond the grasslands to the south.", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("And don't worry about us, we will make sure the town stays safe!", "Blacksmith", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            EndLastStageOfWorld();
        }
    }

    private void DoGrasslands1Step(){   
        int i = 0;
        if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("What a great day to begin an adventure!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("The sun is shining, the river is flowing, and the grass is dancing in the wind!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Going out into the grasslands, just me and my... weird magical book...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }  
        else if (++i == cutsceneStep){       
            AdvanceConditionWait(2f);
            PlayAnimation("Player", "Take Out Book");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Or should I call you \"The Spell-Book\"?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("How about just \"Spellbook\", for short?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Can you even understand me?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The magic book says nothing.", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Okay, well your pages fill with random letters when it's magic time. Can you say something to me with them?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The book's pages are empty.", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It'd be nice to have a picnic in the warm grass, reading a book full of magical secrets. Doesn't that sound fun?", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The magic book continues to be silent.", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Fine, be that way!" , DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Miss "+ StaticVariables.playerName + "! Wait a moment!", "Elder", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }      
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Elder", "Walk", -38, 676, 2f);
            AdvanceConditionWait(2f);
        }      
        else if (++i == cutsceneStep){
            FlipDirection("Elder");
            AdvanceConditionWait(0.5f);
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Are you... arguing with your book?", "Elder", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Put Away Book");
            AdvanceConditionDialogue_PlayerTalking("Does this qualify as an argument?? I don't know if this thing can even hear me!" , DialogueStep.Emotion.Angry);   
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("What are you doing out here anyway?" , DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I wanted to thank you for offering to heal my hands and bring my magic back!", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Actually, about that..." , DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I think I should get some more practice with magic before I try it on people!" , DialogueStep.Emotion.Surprised);   
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("That's alright, I'm an old man now. I don't even know if I want to wield that power again...", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I do miss cooking though! Before I started adventuring, I was the head chef for the tavern.", "Elder", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("When you come back from your quest, I'll take you up on your offer. You can fix up my hands and I'll cook you a mean nine-course meal!", "Elder", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("That sounds lovely!" , DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Oh! But that's not why I came out here! You showed me kindness, and I wanted to give you something in return.", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep){ 
            AdvanceConditionWait(2f);
            PlayAnimation("Elder", "Take Out Bag");
        } 
        else if (++i == cutsceneStep){
            PlayAnimation("Elder", "Idle");
            PlayAnimation("Player", "Idle Holding Bag");
            AdvanceConditionWait(1.6f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("This is an enchanted pouch that can carry anything and everything you put inside it!", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("It's very convenient to have for grocery shopping, but I think you might get a bit more use out of it.", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I had the children fill it with every single one of your books that survived the flames.", "Elder", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("This is incredible! Thank you!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("You're welcome, Miss " + StaticVariables.playerName + ".", "Elder", DialogueStep.Emotion.Normal);
        }        
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("It'd be best not to get bored on your journey.", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("And it will be quite the long journey indeed! I shouldn't keep you any longer.", "Elder", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Make haste! The future of the whole continent may depend on it!", "Elder", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("You're right! I'd better get going!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }    
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Put Away Bag");
            AdvanceConditionWait(1.6f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionWait(2f);
            PlayAnimationAndMoveThenIdle("Player", "Walk", 500, 2012, 5f);
        }
        else if (++i == cutsceneStep){
            EndNormalStage();
        }
    }
    
    private void DoGrasslands2Step(){
        int i = 0;
        if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Alright, Spellbook.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("After that crazy fight, there's probably a lot of vague magical whatever energy in the air.", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Maybe you're feeling a little inspired to talk to me?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("For the first time, the book's pages have some clear writing on them.", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("It reads,\n\"ENTER THE CAVE\".", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("No way!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I knew it! You're a talking book! I have so many questions!!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("How old are you? Who made you? Can you see me?", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The book's text remains unchanged.", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Great. You're an actual real-life talking book! You want me to just ignore that and go into some dark cave?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Plus, we need to get through the forest, and there really isn't any time to waste!", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The ink shifts around, forming new words.", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("\"QUICKLY, BEFORE THE CYCLOPS AWAKENS.\"", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("You know what, you're a magic book. You can <water>summon water<>. You can <healing>heal the injured<>.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("And now you can talk!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Maybe I should just take your advice. Let's go!", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }  
        else if (++i == cutsceneStep){ 
            AdvanceConditionWait(1.5f);
            PlayAnimation("Player", "Put Away Book");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionWait(2f);
            PlayAnimation("Player", "Walk");
            MoveObject("Player", 480, 2280, 5f);
        }
        else if (++i == cutsceneStep){
            StartCutsceneImageTransition(grasslands2_pt2);
            advanceCondition = Cond.BackgroundChange;
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I can't see a thing!", DialogueStep.Emotion.Surprised);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        } 
        else if (++i == cutsceneStep){ 
            AdvanceConditionWait(3f);
            PlayAnimation("Player", "Walk");
            MoveObject("Entrance", -2066, 1892, 3f);
        } 
        else if (++i == cutsceneStep){
            StartScreenShake(.2f);
            AdvanceConditionDialogue_EnemyTalking("Bump!", "Dark Object", DialogueStep.Emotion.Normal);
            PlayAnimation("Player", "Idle");
        }
        else if (++i == cutsceneStep){
            HideEnemyChathead();
            AdvanceConditionDialogue_PlayerTalking("Ouch, it looks like I ran into something.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I should take a moment and let my eyes adjust...", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            DisplayNobodyTalking();
            AdvanceConditionWait(3f);
            Color c = Color.black;
            c.a = 0;
            GetObjectFromName("Darkness").GetComponent<Image>().DOColor(c, 10f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Ahh!!! A skeleton!!", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh, it's dead.", DialogueStep.Emotion.Normal); //relief?
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Well of course it's dead, it's a skeleton!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Come to think of it, I have to be careful. It might come back to life!", DialogueStep.Emotion.Normal);
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_PlayerTalking("In every fantasy adventure novel, there's always at least one reanimated skeleton!", DialogueStep.Emotion.Normal);
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Every magical adventure story needs at least one reanimated skeleton!", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("And it would be so cool to meet one in real life!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("What is a skeleton doing here, anyway? The cyclops talked about ecological conservation, not dead bodies.", DialogueStep.Emotion.Questioning);
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_PlayerTalking("You don't accidentally leave a skeleton in your house.", DialogueStep.Emotion.Questioning);
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Normal people don't have a literal skeleton in their metaphorical closet.", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Something isn't adding up. I should take a look around.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        } 
        else if (++i == cutsceneStep){
            FlipDirection("Player");
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            FlipDirection("Player");
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            FlipDirection("Player");
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Player", "Walk", -441, 1900, 1.5f);
            AdvanceConditionWait(1.5f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Let's see what you've been reading lately...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("\"From Here to Eternity\", \"Stiff\"...\nThese are books about burial practices!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Has he been doing something to these skeletons?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It's entirely possible that he could be a necromancer...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("They say the Lich King used magic to make himself undead. Maybe the cyclops is trying to do the same?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        } 
        else if (++i == cutsceneStep){
            FlipDirection("Player");
            AdvanceConditionWait(0.5f);
        } 
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Player", "Walk", 16, 1675, 2.5f);
            MoveEverythingExceptPlayer(-300, 0, 2.5f);
            AdvanceConditionWait(2.5f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("There's something in the dirt here...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Hang on a second, these are Lydian Lion coins! Some of the earliest currency in human history!", DialogueStep.Emotion.Surprised);
        }        
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("This is an incredibly precious archeological digsite!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Is this what you wanted me to see?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionWait(1.5f);
            PlayAnimation("Player", "Take Out Book");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The book reads,\n\"KEEP GOING, AND HURRY UP\".", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Are you serious? Do you know how cool this stuff is??", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Herodotus, the father of history, thought Lydia was the first civilization to ever use metal coins!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("This is the craziest thing that I've ever seen!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh actually, maybe it's the second craziest, after a talking magical spellbook...", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Fine, fine! I'll keep moving. But I'm not happy about it!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }  
        else if (++i == cutsceneStep){ 
            AdvanceConditionWait(1.5f);
            PlayAnimation("Player", "Put Away Book");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionWait(3f);
            PlayAnimation("Player", "Walk");
            MoveEverythingExceptPlayer(-591, 0, 3f);
            Color c = Color.black;
            c.a = 0.45f;
            GetObjectFromName("Rock Glow Darkness").GetComponent<Image>().DOColor(c, 6f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionWait(0.5f);
            PlayAnimation("Player", "Idle");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Okay, this glowing thing must be what we're here for!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It looks like some kind of tool... Maybe a trowel?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("And there's a book on the table, too...", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("The...\nNecronomicon???", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I don't understand, this shouldn't be possible. The Necronomicon isn't real.", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("H. P. Lovecraft, the famous horror novelist, referenced The Necronomicon in his stories.", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But it was ultimately just a narrative device that he made up.", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("No copies ever actually existed, and certainly not with any forbidden eldritch secrets.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But here one is, right in front of me. As real as I am.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("That settles it! The cyclops is definitely a necromancer!", DialogueStep.Emotion.Worried);
        }        
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionWait(1.5f);
            PlayAnimation("Player", "Take Out Book");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Alright, Spellbook! Tell me what you want me to do, and then let's get out of here!", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Ink forms into new words,\n\"TOUCH THE TROWEL\".", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Touch it, are you crazy?? It'd turn me into a skeleton or something!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Words reshape on the book's pages, \n\"YOUR PHYSICAL BODY WILL LIKELY BE UNHARMED\".", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("That doesn't make me feel any better!", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Okay, okay, fine, whatever! Let's just get this over with!", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionWait(1f);
            PlayAnimation("Player", "Book Catch");
        }
        else if (++i == cutsceneStep){
            StartScreenShake();
            AdvanceConditionWait(0.8f);
        }
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Rock rising 1").transform.parent.gameObject.SetActive(true);
            AdvanceConditionWait(0.4f);
        }
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Rock rising 2").transform.parent.gameObject.SetActive(true);
            AdvanceConditionWait(0.6f);
        }
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Rock rising 3").transform.parent.gameObject.SetActive(true);
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Rock rising 4").transform.parent.gameObject.SetActive(true);
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Rock falling 1").SetActive(true);
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Rock rising 5").transform.parent.gameObject.SetActive(true);
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Rock falling 2").SetActive(true);
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Rock falling 3").SetActive(true);
            AdvanceConditionWait(0.2f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Rock falling 7").SetActive(true);
            MagicFlash flash = GetObjectFromName("Magic Flash").GetComponent<MagicFlash>();
            flash.gameObject.SetActive(true);
            flash.StartProcess(StaticVariables.earthPowerupColor);
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Rock falling 4").SetActive(true);
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Rock falling 5").SetActive(true);
            AdvanceConditionWait(0.2f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Rock falling 6").SetActive(true);
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Rock falling 8").SetActive(true);
            MagicFlash flash = GetObjectFromName("Magic Flash").GetComponent<MagicFlash>();
            AdvanceConditionWait(flash.GetTotalTime() - flash.fadeTime - 1.3f);
        }
        else if (++i == cutsceneStep){
            StopShakeScreen();
            PlayAnimation("Player", "Idle Holding Book");
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Somehow, I'm still alive!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The book's pages are empty aside from a single word,\n\"EARTH\".", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("We need to get out of here! The cave could collapse at any moment!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }  
        else if (++i == cutsceneStep) {
            FlipDirection("Player");
            PlayAnimation("Player", "Idle Holding Book Flipped");
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Player", "Walk While Holding Book Flipped", -536, 1675, 5f);
            MoveEverythingExceptPlayer(1000, 0, 5f);
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep) {
            EndLastStageOfWorld();
        }
    }

    private void DoForest1Step(){   
        int i = 0;
        if (++i == cutsceneStep) {
            PlayAnimation("Player", "Walk While Holding Book Flipped");
            MoveObject("Player", -78, 1935, 3f);
            AdvanceConditionWait(3f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Idle Holding Book Flipped");
            AdvanceConditionDialogue_PlayerTalking("Huff... Huff... Phew!", DialogueStep.Emotion.Surprised);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I really need to get in better shape!", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Especially if you keep putting me in dangerous situations!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Can you tell me what the heck happened in that cave??", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("At first, the pages are blank.", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("After a moment, ink slowly forms on the paper.", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("\"YOU NOW WIELD THE POWER OF THE <earth>ELEMENT OF EARTH<>\"", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Earth?? That's incredible!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Just from touching that glowing cave rock?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The book quickly responds, \"YES\".", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("First <water>water<> and that <healing>healing<>, and now <earth>earth<>!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I've never heard of someone commanding three elements before.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I wonder what would happen if we got Eldric to touch that rock too...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Maybe we'll worry about that after we save the continent, huh?", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("For now I should focus on getting us through this forest!", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
            HideEnemyChathead();
        }  
        else if (++i == cutsceneStep){ 
            AdvanceConditionWait(1.5f);
            PlayAnimation("Player", "Put Away Book");
        }   
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It really is beautiful here...", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Even if that cyclops was a crazy necromancer, he still cared about protecting the undisturbed state of the forest.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I should try to get through quickly and leave no trace.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I'll have to just pick a direction and move carefully and quietly.", DialogueStep.Emotion.Normal);
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }  
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Player", "Walk", -178, 1435, 5f);
            AdvanceConditionWait(2f);
        }     
        else if (++i == cutsceneStep){
            EndNormalStage();
        }
    }

    private void DoForest2Step(){   
        int i = 0;
        if (++i == cutsceneStep){
            AdvanceConditionWait(2f);
        }        
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Huff... Huff...", DialogueStep.Emotion.Surprised);
            GameObject.Destroy(GetObjectFromName("Starting area"));
            //StaticVariables.WaitTimeThenCallFunction(2f, GameObject.Destroy, GetObjectFromName("Starting area"));
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I can't outrun these rabbits forever!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I need to find that other human, and fast!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("If I were a magical human in a magical forest, where would I live?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Huff...", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Wait a second, I am a magical human in a magical forest!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("That's not exactly helpful; I don't live here!", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Okay, Spellbook...", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionWait(1.5f);
            PlayAnimation("Player", "Take Out Book While Walking");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Please help me out here!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Do you feel anything with your magic radar thing?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The book immediately responds,\n\"THERE IS A STRONG MAGICAL AURA RADIATING FROM BELOW\".", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Great!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I should look for some kind of tunnel entrance, or big tree stump, or...", DialogueStep.Emotion.Surprised);
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_NobodyTalking(true);
        //}  
        else if (++i == cutsceneStep){
            HideChatheads();
            DisplayNobodyTalking();
            advanceCondition = Cond.externalTrigger;
            List<CutsceneTreeGenerator> treeGenerators = new() {
                GetObjectFromName("Tree Spawner 1").GetComponent<CutsceneTreeGenerator>(),
                GetObjectFromName("Tree Spawner 2").GetComponent<CutsceneTreeGenerator>(),
                GetObjectFromName("Tree Spawner 3").GetComponent<CutsceneTreeGenerator>(),
                GetObjectFromName("Tree Spawner 4").GetComponent<CutsceneTreeGenerator>(),
                GetObjectFromName("Tree Spawner 5").GetComponent<CutsceneTreeGenerator>(),
                GetObjectFromName("Tree Spawner 6").GetComponent<CutsceneTreeGenerator>()
            };
            foreach(CutsceneTreeGenerator treeGenerator in treeGenerators)
                treeGenerator.BeginSlowdown();
        } 
        else if (++i == cutsceneStep){
            CutsceneTreeMimic mimic = GetObjectFromName("Tree Synchronizer").transform.GetChild(0).GetComponent<CutsceneTreeMimic>();
            mimic.gameObject.SetActive(true);
            mimic.originalTree = GetObjectFromName("Tree Spawner 3").transform.GetChild(1);

            //print(externalTriggerParameter);
            AdvanceConditionWait(externalTriggerParameter - 0.5f);
        } 
        else if (++i == cutsceneStep){
            MoveObject("Player", -96, 2213, 0.5f);
            AdvanceConditionWait(0.5f);
        } 
        else if (++i == cutsceneStep){
            //DOTween.KillAll();
            PlayAnimation("Player", "Idle Holding Book");
            CutsceneTreeMimic mimic = GetObjectFromName("Tree Synchronizer").transform.GetChild(0).GetComponent<CutsceneTreeMimic>();
            StaticVariables.WaitTimeThenCallFunction(0.1f, mimic.DestroyScript);
            AdvanceConditionWait(0.5f);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("A trapdoor!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }  
        else if (++i == cutsceneStep){
            StartScreenShake();
            AdvanceConditionWait(0.8f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Rabbit Cycles").SetActive(true);
            //start rabbit running
            AdvanceConditionWait(3.5f);
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Phew, that was close!", DialogueStep.Emotion.Surprised);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Well, this must be it.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Nothing says \"possibly-evil lair of a possibly-evil forest wizard\" like a trapdoor.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Let's see what's inside...", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }  
        else if (++i == cutsceneStep){
            StartCutsceneImageTransition(forest2_pt2);
            advanceCondition = Cond.BackgroundChange;
        }
        else if (++i == cutsceneStep){
            StopShakeScreen();
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Wow!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I wasn't expecting a high-tech laboratory!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("There's so much bubbling and buzzing.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("That magical object must be around here somewhere...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("If I can find it, I might be able to figure out what's really going on in this forest!", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }  
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Player", "Walk", -198, 2004, 1f);
            AdvanceConditionWait(1.3f);
        }
        else if (++i == cutsceneStep){
            FlipDirection("Player");
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            FlipDirection("Player");
            AdvanceConditionWait(0.4f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Player", "Walk", -127, 1640, 1.5f);
            MoveEverythingExceptPlayer(-205, 0, 1.5f);
            AdvanceConditionWait(1.7f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Player", "Walk", -213, 1914, 1.5f);
            MoveEverythingExceptPlayer(-360, 0, 1.5f);
            AdvanceConditionWait(1.7f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Player", "Walk", 100, 1914, 1f);
            AdvanceConditionWait(1.2f);
        }
        else if (++i == cutsceneStep){
            FlipDirection("Player");
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Player", "Walk", -213, 1914, 1f);
            AdvanceConditionWait(1.2f);
        }
        else if (++i == cutsceneStep){
            FlipDirection("Player");
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Player", "Walk", -173, 1756, 2f);
            MoveEverythingExceptPlayer(-635, 0, 2f);
            AdvanceConditionWait(2.5f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Idle");
            AdvanceConditionDialogue_PlayerTalking("This looks pretty important!\nI wonder if -", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("A surprise visitor! How exciting!", "Wizard", DialogueStep.Emotion.Mystery, "???");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh!!", DialogueStep.Emotion.Worried);
            FlipDirection("Player");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        } 
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Wizard").transform.parent.GetComponent<RectTransform>().anchoredPosition = (new Vector2(-800, 343));
            PlayAnimationAndMoveThenIdle("Wizard", "Walk", -474, 343, 1.5f);
            AdvanceConditionWait(1.5f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Welcome to my laboratory!", "Wizard", DialogueStep.Emotion.Happy, "Wizard");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I'd offer you something to drink, but I'm afraid everything here is quite toxic to humans.", "Wizard", DialogueStep.Emotion.Questioning, "Wizard");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Is that a threat??", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("No, but please do keep your voice down.", "Wizard", DialogueStep.Emotion.Normal, "Wizard");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I am delighted to finally meet you!", "Wizard", DialogueStep.Emotion.Happy, "Wizard");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I've heard whispers of your movements around the forest in recent days.", "Wizard", DialogueStep.Emotion.Normal, "Wizard");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I've spent a considerable amount of my time here looking for you, actually.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I'm hoping you could tell me what's really going on here...", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I'd be happy to answer any questions!", "Wizard", DialogueStep.Emotion.Happy, "Wizard");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("But first, allow me to introduce myself.\nMy name is Mustrum, I'm 138 years old, and I'm...", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I'm...", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I'm sorry, Miss... " + StaticVariables.playerName + "? Are you wearing a name tag?", "Wizard", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh yeah, it's a funny story!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Here in the forest, everyone sees me as just another human.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("They're all suspicious of you, so they're suspicious of me too.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I thought a name tag might make me a little more personable.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Come to think of it, that's not a funny story. It's been a little dehumanizing.", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Oh, that is unfortunate! You have my apologies.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Acutally, that brings me to my first question!", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Why is everyone suspicious of you, anyway?", DialogueStep.Emotion.Questioning);
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_EnemyTalking("Since I've come here, my magical prowess has been steadily growing.", "Wizard", DialogueStep.Emotion.Normal);
        //}
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_EnemyTalking("Meanwhile, the strength of the forest wanes.", "Wizard", DialogueStep.Emotion.Normal);
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Ah, I can only assume they've noticed that the forest is weaker than ever now.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The local magic has been diminishing slowly for the entire time that I've lived here.", "Wizard", DialogueStep.Emotion.Normal);
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_EnemyTalking("Really, the whole time that I've lived here, the local magic has been diminishing.", "Wizard", DialogueStep.Emotion.Normal);
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Some creatures have even lost their magical intelligence altogether!", "Wizard", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh, like that big fluffy monster, by the huge tree!", DialogueStep.Emotion.Surprised); //player didnt previously know it as an owlbear?
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Precisely! That tree, the Quercus giganteum, lies at the heart of the forest.", "Wizard", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("It is what I call a \"catalyst\", a sentient object that contains the powers of a school of magic.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I've come to discover that it's tied to the very essence of wild magic in these lands.", "Wizard", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Its roots spread throughout the forest, intertwining with nearly every living thing.", "Wizard", DialogueStep.Emotion.Normal);
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_EnemyTalking("One such root is right above you, steeping in that cauldron.", "Wizard", DialogueStep.Emotion.Normal);
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Interesting...", DialogueStep.Emotion.Questioning);
            FlipDirection("Player");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("So this tree is the reason all the forest critters can think and talk and have an organized police force?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Yes, it would seem so.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("That's amazing! Are these overhead roots part of this \"catalyst\" too?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Yes! Although I may have to find a new root to work with soon. I doubt this sample will survive much longer.", "Wizard", DialogueStep.Emotion.Normal);
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_EnemyTalking("Although I doubt it will survive much longer. I may have to find a new root to work with soon...", "Wizard", DialogueStep.Emotion.Normal);
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("What??", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("What are you doing to the roots??", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Please, don't shout...", "Wizard", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("What are you doing to the tree??", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking(StaticVariables.playerName + "!", "Wizard", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("What are you doing to the whole forest??", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
            StartScreenShake();        
        }  
        else if (++i == cutsceneStep){
            GetObjectFromName("tiny roots").GetComponent<CutsceneBranchOrganizer>().StartDrops();
            AdvanceConditionWait(0.5f);
        }       
        else if (++i == cutsceneStep){
            Transform cluster = GetObjectFromName("Root Cluster").transform;
            cluster.DORotate(new Vector3(0,0,-6), 1.3f);
            cluster.DOLocalMoveY(-609, 1.3f);
            AdvanceConditionWait(1.3f);
        }         
        else if (++i == cutsceneStep){      
            Transform cluster = GetObjectFromName("Root Cluster").transform;
            foreach (Transform t in cluster){
                t.GetChild(0).gameObject.SetActive(false);
                t.GetChild(1).gameObject.SetActive(true);
            }
            AdvanceConditionWait(0.3f);
        }      
        else if (++i == cutsceneStep){
            Transform cluster = GetObjectFromName("Root Cluster").transform;
            cluster.GetComponent<CutsceneBranchOrganizer>().StartDrops();
            AdvanceConditionWait(4.7f);
        }   
        else if (++i == cutsceneStep){
            StopShakeScreen();
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Oh, dear...", "Wizard", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("That's it, no more questions!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I'm putting a stop to your mad science here and now!", DialogueStep.Emotion.Angry);
        }    
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        } 
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Cast Spell");
            AdvanceConditionWait(0.33f);
        }
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Player").transform.parent.GetChild(2).gameObject.SetActive(true);
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            EndNormalStage();
            AdvanceConditionWait(0.75f);
        }
        //something breaks on the rock impact
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Player").transform.parent.GetChild(2).gameObject.SetActive(false);
            AdvanceConditionWait(0.1f);
        }        
        //else if (++i == cutsceneStep){
        //    StaticVariables.hasCompletedStage = true;
        //    StaticVariables.FadeOutThenLoadScene(StaticVariables.lastVisitedStage.worldName);
        //}

        /*


        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Plus, that old cyclops necromancer said you were tampering with the forest!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Cyclops? Are you referring to the so-called \"Guardian of the Forest\"?", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I don't know where you got it in your head that he's a necromancer.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("He was studying the Necronomicon!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("You must be mistaken. The Necronomicon isn't real.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I thought so too, but then I saw it with my own eyes!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I assure you, that cyclops is no necromancer. Merely a humble historian.", "Wizard", DialogueStep.Emotion.Normal);
        }
        */
    }

    private void DoForest3Step(){   
        int i = 0;
        //player should be to the left, wizard to the right in front of cauldron
        if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("You want to tell me the truth? Fine, out with it!!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("And if you try anything funny, I'll knock you out again!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            //AdvanceConditionDialogue_NobodyTalking(true);
            AdvanceConditionDialogue_EnemyTalking("Sigh...\nAs you wish.", "Wizard", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Ninety-nine years ago, I graduated from The Academy in Duskvale with a degree in biochemistry.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Shortly thereafter, I came to the Enchanted Forest to research the nature of magic.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Year after year, hypothesis after hypothesis...", "Wizard", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I became a better practitioner of the sciences, and a much more powerful pyromancer!", "Wizard", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("But I also came to discover that the forest was slowly dying.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("And you weren't just stealing the magic of the tree?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking(StaticVariables.playerName +", it's impossible to \"steal\" magic.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Where do you think magic comes from?", "Wizard", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I can't say I've ever given it much thought...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("My magic comes from a talking book!", DialogueStep.Emotion.Normal);
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_NobodyTalking();
        //}  
        //else if (++i == cutsceneStep){
        //    AdvanceConditionWait(1.5f);
        //    PlayAnimation("Player", "Take Out Book");
        //}
        //player takes out book
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Could my spellbook be one of those catalysts you mentioned earlier?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Precisely! I'd guess it's the catalyst for the <water>element of water<>.", "Wizard", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("When someone comes into contact with such a catalyst, they may gain control over an elemental power.", "Wizard", DialogueStep.Emotion.Normal);
            //when someone comes into contact with such a catalyst, they gain control over the associated elemental power
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But... I'm the only one that can use the powers of the book!", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I let some friends give it a test, with no results.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I've come across a few other magical people, and none of them had a spellbook either.", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Would you mind telling me a bit about them?", "Wizard", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Well most recently, there's you. You live in a forest and control <fire>fire magic<>. You studied at the Academy, and you experiment on a sentient tree.", DialogueStep.Emotion.Normal);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Then before you, there was the cyclops in the grasslands. He threw <earth>magical rocks<>, spoke very slowly, and is probably a necromancer.", DialogueStep.Emotion.Normal);                
            }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I have reason to believe he isn't a necromancer, but please continue.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("The cyclops had an apprentice who had his own face on his torso. He couldn't do any magic, but I guess he was in training?", DialogueStep.Emotion.Normal);                
            }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Before him, there was this retired adventurer in my hometown who used to have control over the wind.", DialogueStep.Emotion.Normal);                
            }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Plus there's me! I have a magical book that lets me <water>summon waves<>, <healing>cure wounds<>, and <earth>toss rocks at people<>.", DialogueStep.Emotion.Happy);                
            }
        //i've also heard the lich king uses magic, although i'm not sure what kind.
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Why does this matter anyway?", DialogueStep.Emotion.Questioning);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Magic is magic, the people just use it.", DialogueStep.Emotion.Normal);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("After a century of research in the field, I can assure you that is not the case.", "Wizard", DialogueStep.Emotion.Normal);        
            }        
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Is there something that makes these people special or worthy somehow?", DialogueStep.Emotion.Questioning);                
            }   
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Eldric went on epic adventures! You and the cyclops have lived a long time; I bet you've done some pretty cool stuff.", DialogueStep.Emotion.Questioning);                
            }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But that doesn't make sense! I'm just a librarian, and I can use magic too!", DialogueStep.Emotion.Surprised);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("And I've never heard of anyone else who can control three different elements!", DialogueStep.Emotion.Surprised);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("What's special about me? I just like books...", DialogueStep.Emotion.Defeated);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("...", DialogueStep.Emotion.Questioning);        
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_PlayerTalking("That's it! I like to read books!", DialogueStep.Emotion.Happy);        
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Does magic come from reading?", DialogueStep.Emotion.Questioning);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Ah! Not just reading. We have arrived at the crux of the matter.", "Wizard", DialogueStep.Emotion.Happy);    
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Each branch of magic is tied to a unique field of study.", "Wizard", DialogueStep.Emotion.Normal);    
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("For example, <fire>fire<> is the element of science and discovery.", "Wizard", DialogueStep.Emotion.Happy);    
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Great <fire>pyromancers<> are biologists, chemists, and physicists, seeking to understand the fundamentals of our universe.", "Wizard", DialogueStep.Emotion.Normal);    
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("<earth>Earth<> is the element of history. <earth>Historians and archeologists<> uncover, record, and preserve the past from the sands of time.", "Wizard", DialogueStep.Emotion.Normal);    
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("<water>Language<> is dynamic. It flows and changes as the generations pass, much like the <water>element of water<> you carry.", "Wizard", DialogueStep.Emotion.Normal);    
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Care to take a guess about your <healing>power of healing<>?", "Wizard", DialogueStep.Emotion.Questioning);    
        }
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_EnemyTalking("I'm sure you can guess what your <healing>power of healing<> is tied to.", "Wizard", DialogueStep.Emotion.Questioning);    
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Actually, I was our town's doctor for a time!", DialogueStep.Emotion.Surprised);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But I think that was because nobody else could read medical textbooks.", DialogueStep.Emotion.Normal);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Or maybe I was the only one who cared to try.", DialogueStep.Emotion.Defeated);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking(StaticVariables.playerName + ", I think that is exactly why you are capable of wielding so many elements.", "Wizard", DialogueStep.Emotion.Normal);    
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Magic doesn't come from having knowledge, but from the pursuit of it.", "Wizard", DialogueStep.Emotion.Happy);    
        }  
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_EnemyTalking("And you have a passion for every discipline.", "Wizard", DialogueStep.Emotion.Normal);    
        //}  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("And you'll study anything you can get your hands on!", "Wizard", DialogueStep.Emotion.Happy);    
        }  
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_PlayerTalking("So that explains why there are so few magical people in the world!", DialogueStep.Emotion.Surprised);        
        //}
        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_PlayerTalking("You need to find a catalyst and have the right interests for its magic too!", DialogueStep.Emotion.Surprised);        
        //}
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I'm willing to bet you are also capable of harnessing <fire>fire<>!", "Wizard", DialogueStep.Emotion.Questioning);    
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("You think so??", DialogueStep.Emotion.Questioning);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Would you like to find out?", "Wizard", DialogueStep.Emotion.Questioning);    
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Absolutely! Who wouldn't want to shoot <fire>fire<> from their fingertips??", DialogueStep.Emotion.Surprised);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Spectacular!", "Wizard", DialogueStep.Emotion.Happy);    
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Oh, um...", "Wizard", DialogueStep.Emotion.Worried);    
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Would you mind if I attach some sensors to you first?", "Wizard", DialogueStep.Emotion.Questioning);    
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I've never witnessed someone acquiring the power of a new element before.", "Wizard", DialogueStep.Emotion.Normal);    
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The data would certainly help with my studies.", "Wizard", DialogueStep.Emotion.Happy);    
        }   
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh, why not. You're not going to electrocute me or anything, I hope?", DialogueStep.Emotion.Happy);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I wouldn't dream of it, " + StaticVariables.playerName + ".", "Wizard", DialogueStep.Emotion.Normal);    
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }  
        else if (++i == cutsceneStep){
            PlayAnimation("Wizard", "Walk");
            MoveObject("Wizard", -105, 399, 1f);
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Just this one here...", "Wizard", DialogueStep.Emotion.Normal);    
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Just this one here...\nAnd this one over here...", "Wizard", DialogueStep.Emotion.Normal);    
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("How many of these things are you putting on me??", DialogueStep.Emotion.Angry);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Only a few dozen more!", "Wizard", DialogueStep.Emotion.Happy);    
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Hurry it up, these things are itchy!", DialogueStep.Emotion.Worried);        
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("And...", "Wizard", DialogueStep.Emotion.Normal);    
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("And...\nDone!", "Wizard", DialogueStep.Emotion.Happy);  
            PlayAnimation("Wizard", "Idle");  
        } 
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Now, " + StaticVariables.playerName + ". Are you ready to become a <fire>pyromancer<>?", "Wizard", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Yes! Hand me that catalyst so I can take thee things off!!", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Of course, of course!", "Wizard", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }   
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Wizard", "Walk", -7, 399, 1f);
            AdvanceConditionWait(1f);
        } 
        else if (++i == cutsceneStep){ 
            PlayAnimation("Wizard", "Take Out Catalyst");
            AdvanceConditionWait(0.66f);
        } 
        else if (++i == cutsceneStep){
            GetObjectFromName("Catalyst").SetActive(true);
            AdvanceConditionWait(0.5f);
        } 
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Book Catch");
            AdvanceConditionWait(1f);
        } 
        else if (++i == cutsceneStep){
            StartScreenShake();
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Fire Spin 1").SetActive(true);
            AdvanceConditionWait(1.5f);
        }
        else if (++i == cutsceneStep){
            Transform t = GetObjectFromName("Fire Spin 1").transform;
            t.DOScale(t.localScale * 1.5f, 2f);
            AdvanceConditionWait(0.8f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Fire Spin 2").SetActive(true);
            AdvanceConditionWait(0.75f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Fire Spin 3").SetActive(true);
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            MagicFlash flash = GetObjectFromName("Magic Flash").GetComponent<MagicFlash>();
            flash.gameObject.SetActive(true);
            flash.StartProcess(StaticVariables.firePowerupColor);
            AdvanceConditionWait(flash.GetTotalTime() - flash.fadeTime);
        }
        else if (++i == cutsceneStep){
            StopShakeScreen();
            GetObjectFromName("Fire Spin 1").SetActive(false);
            GetObjectFromName("Fire Spin 2").SetActive(false);
            GetObjectFromName("Fire Spin 3").SetActive(false);
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Idle");
            GetObjectFromName("Catalyst").SetActive(false);
            PlayAnimation("Wizard", "Put Away Catalyst");
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("That was incredible! Back up, I want to test this out!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Please, hold your fire until you're well clear of the forest! It is already struggling for survival as-is.", "Wizard", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Oh, yes! I have to get going!", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Which way to the Sunscorched Desert?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("If you continue along the path up above, it should only be another day of travel.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Would you like to come with me? Dragons have taken over the continent and someone has to put a stop to it.", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Oh, that sounds like a desperate situation indeed...", "Wizard", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Unfortunately, I fear the Quercus giganteum does not have much time left.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I think I should stay here for the time being.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("I'm sorry for attacking you and destroying your lab.", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("That's quite alright. You just provided me with some truly invaluable data!", "Wizard", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I have an inkling that I might be close to a breakthrough!", "Wizard", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("But don't let me keep you! Those dragons aren't going to stop themselves.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }    
        else if (++i == cutsceneStep){
            FlipDirection("Player");
            AdvanceConditionWait(2f);
            PlayAnimationAndMoveThenIdle("Player", "Walk", -999, 1915, 5f);
        }
        else if (++i == cutsceneStep){
            EndLastStageOfWorld();
        }




        //stuff i thought of including:
        //wizard says he has been trying to figure out how to communicate with the tree
        //wizard says he isnt 100p sure what type of magic the tree is a catalyst for, but its probably some kind of knowledge/wisdom magic
        //wizard says he isnt 100p sure what field of study the tree is a catalyst for



        //do other magic users have something they interact with to use magic?
        //you're on the right track!
        //in my century of research, i have come to believe that magical powers are contained within objects spread throughout the world.
        //i call these objects \"catalysts\"
        //your spellbook is likely the catalyst for the element of water!

        //a necromancer?
        //yes, i saw he had the necronomicon in his study
        //you must be mistaken, the necronomicon isnt real
        //that's what i thought too, but then i saw it with my own eyes!
        //i can assure you, that cyclops in no necromancer.
        //please continue telling me about these other magical people!


        /*
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Tell me, what do you know of the scientific method?", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("What? Um...", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("In Posterior Analytics, Aristotle describes it as \"something\".", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("One time I tested how different factors affected the growth of tomato plants.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("The children liked that lesson!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Much more than the time we cataloged the library books, anyway...", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I see.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("You and I are not so different, Rebecca.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Ugh, you really shouldn't say things like that if you're trying not to sound like a villain.", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("You doubt my intentions still?", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("If you'd care to take a look in that broken cabinet behind you...", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("You should find all the proof you need.", "Wizard", DialogueStep.Emotion.Normal);
        }
        //player goes over to cabinet
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("\"Year 96. Day 118.\nExperiments with a 7kg bark sample submerged in compound yielded no results.\"", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("\"It is statistically unlikely that the corruption inhabits the organelle. The compound is five parts water to one part...\"", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Are these your research notes?", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("There are dozens of pages in here!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Thousands, actually. I've been here for a lifetime.", "Wizard", DialogueStep.Emotion.Normal);
        }
        */

        //do you have any proof you weren't just stealing the magic of the tree?

        //else if (++i == cutsceneStep){
        //    AdvanceConditionDialogue_PlayerTalking("But that doesn't exactly prove you aren't trying to steal the magic of the forest.", DialogueStep.Emotion.Normal);
        //}
        /*
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Rebecca, it's impossible to \"steal\" magic.", "Wizard", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Where do you think magic comes from?", "Wizard", DialogueStep.Emotion.Normal);
        }
        */
        //i cant say ive ever given it much thought...
        //my magic comes from this talking book here...
        //player takes out book
        //do other magic users have something they interact with to use magic?
        //you're on the right track!
        //in my century of research, i have come to believe that magical powers are contained within objects spread throughout the world.
        //i call these objects \"catalysts\"
        //your spellbook is likely the catalyst for the element of water!
        //when someone comes into contact with such a catalyst, they gain control over the associated elemental power
        //but... i'm the only one that is able to use the powers of the book!
        //i've let some friends give it a test, with no results!
        //and i've come across a few other magical people, and none of them had a spellbook either.
        //would you mind telling me a bit about them?
        //well most recently, there's you. you live in a forest and control fire magic. you studied at the academy, and you perform scientific experiments on trees.
        //then before you, there's the cyclops in the grasslands. he threw magical rocks, spoke very slowly, and is probably a necromancer

        //a necromancer?
        //yes, i saw he had the necronomicon in his study
        //you must be mistaken, the necronomicon isnt real
        //that's what i thought too, but then i saw it with my own eyes!
        //i can assure you, that cyclops in no necromancer.

        //please continue telling me about these other magical people!
        //the cyclops had an apprentice, a blemmyae, who had his face attached to his torso. he was good at throwing rocks, but not much else.
        //then there was a retired adventurer in my hometown, by the name of Eldric. he used to have control over the wind.
        //plus there's me! i have a magical book that lets me make waves, heal the injured, and toss rocks about.
        //i've also heard the lich king uses magic, although i'm not sure what kind.
        //why does this matter anyway?
        //magic is magic, the people just use it.
        //from a century of research in the field, i can assure you that is not the case.     
        //there's something about all of these people that makes them special or worthy somehow?
        //old man eldric went on epic adventures. the cyclops and the wizard have lived a long time; i bet they've done some pretty cool stuff.
        //but that doesnt make sense! im just a librarian, and i can use magic too!
        //plus i can control three different elements, which is pretty unheard of!
        //what's special about me? i just like to read books.
        //...
        //that's it! i like to read books!
        //does magic come from reading? (player) 
        //ah! not just reading. each branch of magic is tied to a unique field of study.
        //for example, fire is the element of science and discovery. (hide player chathead)
        //great pyromancers are biologists, chemists, and physicists, who seek to understand the fundamentals of our universe.
        //earth is the element of history. Historians and archeologists uncover, record, and preserve the past from the sands of time.
        //language is dynamic. it flows and changes as the generations pass, much like the element of water you carry.
        //i'm sure you can guess what your power of healing is tied to.
        //i did serve as our town's doctor, but that was mainly because i was the only one who could read medical textbooks.
        //or maybe i was the only one who cared to try.
        //rebecca, i think that is exactly why you are capable of weilding the power of so many elements.
        //magic doesn't come from having knowledge, but from the pursuit of it.
        //and you have a passion for every discipline.
        //i'm willing to bet you are already capable of harnessing the power of fire.
        //would you like to try?
        //i have the catalyst of fire here with me!



        //if you'll allow me to go fetch it...
        //sure, sure. just remember i'll blast you if y

        //take a minute to think on it. if you'll allow me to go fetch something for a moment?
        //sure, sure. just remember i'll blast you if you're up to something
        //wizard exits stage right
        //people don't just use magic?


        //wizard returns, holding a box

        


        //wizard offers player to touch object
        //explains the objects can convey the elemental power to a person if they are sufficiently ("worthy")
        //magical objects have a kind of sentience, too. like this (fire thing).
        //and my book!
        //and also the big forest tree?
        //precisely. i haven't been able to communicate with the quercus giganteum yet, but i suspect its magic is related to (botany, agriculture?)

        //thats why magical abilities are so rare. you have to be (worthy) of the magic, and then also come into contact with the (related object)

        //not just reading. each branch of magic is tied to an academnic pursuit

        //you have a thirst for knowledge. a passion for every discipline.


        //as i pursued my scientific education, i began to develop control over fire.





        //cooperative or semi cooperative mechanics
        //luck based gameplay
        //party games
        //replayability


        //take a minute to think on it. if you'll allow me to go fetch something for a moment?
        //sure, sure. just remember i'll blast you if you're up to something
        //wizard exits stage right
        //let me think about the magical people i know...
        //well there's me, this wizard, eldric, the cyclops...
        //that torso guy is still in training, plus i've heard the lich king uses magic
        

        //magical objects have a kind of sentience, too. like this (fire thing).
        //and my book!
        //and also the big forest tree?
        //precisely. i haven't been able to communicate with the quercus giganteum yet, but i suspect its magic is related to (botany?)
    }
    
    private void DoDesert1Step(){   
        int i = 0;
        if (++i == cutsceneStep){
            MoveObject("Player", 190, 2137, 3f);
            AdvanceConditionWait(3f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Idle");
            AdvanceConditionDialogue_PlayerTalking("Well that was quite a journey, but we finally made it to the edge of the forest!", DialogueStep.Emotion.Happy);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I can see the desert sands up ahead... And then our adventure might be over soon.", DialogueStep.Emotion.Normal);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }  
        else if (++i == cutsceneStep){
            AdvanceConditionWait(1.5f);
            PlayAnimation("Player", "Take Out Book");
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("None of this would have been possible without your help.", DialogueStep.Emotion.Happy);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Thanks to you, I can use four different types of magic now! Four!!", DialogueStep.Emotion.Happy);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("For the first time, words of praise appear, \"WELL DONE. FEW IN HISTORY HAVE CONTROLLED FOUR ELEMENTS\".", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("To be honest, I'm getting a little concerned about managing all of these different powers.", DialogueStep.Emotion.Worried);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I want to use each one to its fullest potential!", DialogueStep.Emotion.Normal);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But I've been thinking about the nature of magic, and I might have a solution.", DialogueStep.Emotion.Normal);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Each element is connected to a field of study, right?", DialogueStep.Emotion.Questioning);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Do you think I could strengthen my <fire>fire<> abilities by reading a book about <fire>science<>?", DialogueStep.Emotion.Questioning);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I have plenty of books for the other disciplines, too.", DialogueStep.Emotion.Normal);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("They should all still be in that magic pouch Eldric gave me.", DialogueStep.Emotion.Normal);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("What do you think?", DialogueStep.Emotion.Questioning);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The book pauses for a minute, then spells out,\n\"THAT IS A PLAUSIBLE IDEA WORTH TESTING\".", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I'm glad you think so!", DialogueStep.Emotion.Happy);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I'll have to pick something good to read...", DialogueStep.Emotion.Questioning);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But in the meantime, we should keep going. That dragon-killing sword is somewhere in this desert!", DialogueStep.Emotion.Normal);
        } 
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }    
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Put Away Book");
            AdvanceConditionWait(1.6f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionWait(2f);
            PlayAnimationAndMoveThenIdle("Player", "Walk", 900, 2137, 5f);
        }
        else if (++i == cutsceneStep){
            EndNormalStage();
        }
        //else if (++i == cutsceneStep){
        //    DisplayNobodyTalking();
        //    PlayAnimation("Player", "Put Away Book");
        //    StaticVariables.hasCompletedStage = true;
        //    StaticVariables.FadeOutThenLoadScene(StaticVariables.lastVisitedStage.worldName);
        //}
    }

    private void DoDesert2Step(){   
        int i = 0;
        if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Player", "Walk", -494, 2137, 1.5f);
            AdvanceConditionWait(1.5f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimation("Player", "Idle");
            AdvanceConditionDialogue_PlayerTalking("Alright, that knight has to be around here somewhere...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("My Dear " + StaticVariables.playerName + "!", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Knight NPC", "Walk", 456, 673, 1.5f);
            AdvanceConditionWait(1.5f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Good of you to finally arrive! You see, I have been -", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            GameObject group1 = GetObjectFromName("Undead Group 1");
            GameObject group2 = GetObjectFromName("Undead Group 2");
            group1.SetActive(true);
            group2.SetActive(true);
            MoveObject("Undead Group 1", 456, 673, 2f);
            MoveObject("Undead Group 2", 456, 673, 2f);
            foreach (Transform guy in group1.transform)
                guy.GetComponent<Animator>().Play("Walk");
            foreach (Transform guy in group2.transform)
                guy.GetComponent<Animator>().Play("Walk");
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep) {
            GameObject group1 = GetObjectFromName("Undead Group 1");
            GameObject group2 = GetObjectFromName("Undead Group 2");
            foreach (Transform guy in group1.transform) {
                guy.localScale = new Vector2(guy.localScale.x * -1, guy.localScale.y);
                guy.GetComponent<Animator>().Play("Idle");
            }
            foreach (Transform guy in group2.transform) {
                guy.localScale = new Vector2(guy.localScale.x * -1, guy.localScale.y);
                guy.GetComponent<Animator>().Play("Idle");
            }
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Gah!", "Knight NPC", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Knight NPC", "Walk", 919, 673, 2f);
            MoveObject("Undead Group 1", 919, 673, 2f);
            MoveObject("Undead Group 2", 919, 673, 2f);
            GameObject group1 = GetObjectFromName("Undead Group 1");
            GameObject group2 = GetObjectFromName("Undead Group 2");
            foreach (Transform guy in group1.transform)
                guy.GetComponent<Animator>().Play("Walk");
            foreach (Transform guy in group2.transform)
                guy.GetComponent<Animator>().Play("Walk");
            AdvanceConditionWait(2.1f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Knight NPC", "Walk", 456, 673, 1f);
            GameObject.Destroy(GetObjectFromName("Undead Group 1"));
            GameObject.Destroy(GetObjectFromName("Undead Group 2"));
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("All these undead abominations, they -", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            GameObject group1 = GetObjectFromName("Undead Group 3");
            GameObject group2 = GetObjectFromName("Undead Group 4");
            group1.SetActive(true);
            group2.SetActive(true);
            MoveObject("Undead Group 3", 456, 673, 2f);
            MoveObject("Undead Group 4", 456, 673, 2f);
            foreach (Transform guy in group1.transform)
                guy.GetComponent<Animator>().Play("Walk");
            foreach (Transform guy in group2.transform)
                guy.GetComponent<Animator>().Play("Walk");
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep) {
            GameObject group1 = GetObjectFromName("Undead Group 3");
            GameObject group2 = GetObjectFromName("Undead Group 4");
            foreach (Transform guy in group1.transform)
                guy.localScale = new Vector2(guy.localScale.x * -1, guy.localScale.y);
            foreach (Transform guy in group2.transform)
                guy.localScale = new Vector2(guy.localScale.x * -1, guy.localScale.y);
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Knight NPC", "Walk", 919, 673, 2f);
            MoveObject("Undead Group 3", 919, 673, 2f);
            MoveObject("Undead Group 4", 919, 673, 2f);
            AdvanceConditionWait(2.1f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Knight NPC", "Walk", 302, 673, 1.1f);
            GameObject.Destroy(GetObjectFromName("Undead Group 3"));
            GameObject.Destroy(GetObjectFromName("Undead Group 4"));
            AdvanceConditionWait(1.1f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Agh!!! I can't do this! I need your help!", "Knight NPC", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            GameObject group1 = GetObjectFromName("Undead Group 5");
            GameObject group2 = GetObjectFromName("Undead Group 6");
            group1.SetActive(true);
            group2.SetActive(true);
            MoveObject("Undead Group 5", 302, 673, 2f);
            MoveObject("Undead Group 6", 302, 673, 2f);
            foreach (Transform guy in group1.transform)
                guy.GetComponent<Animator>().Play("Walk");
            foreach (Transform guy in group2.transform)
                guy.GetComponent<Animator>().Play("Walk");
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep) {
            GameObject group1 = GetObjectFromName("Undead Group 5");
            GameObject group2 = GetObjectFromName("Undead Group 6");
            foreach (Transform guy in group1.transform)
                guy.GetComponent<Animator>().Play("Idle");
            foreach (Transform guy in group2.transform)
                guy.GetComponent<Animator>().Play("Idle");
            AdvanceConditionDialogue_PlayerTalking("Sigh... Here we go.", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep) {
            PlayAnimation("Player", "Cast Spell");
            AdvanceConditionWait(0.33f);
        }
        else if (++i == cutsceneStep) {
            GetAnimatorFromName("Player").transform.parent.GetChild(2).gameObject.SetActive(true);
            AdvanceConditionWait(1.33f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimation("Knight NPC", "Damage");
            GameObject group1 = GetObjectFromName("Undead Group 5");
            GameObject group2 = GetObjectFromName("Undead Group 6");
            foreach (Transform guy in group1.transform)
                guy.GetComponent<Animator>().Play("Die");
            foreach (Transform guy in group2.transform)
                guy.GetComponent<Animator>().Play("Die");
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep) {
            GetAnimatorFromName("Player").transform.parent.GetChild(2).gameObject.SetActive(false);
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Oh, My Dear " + StaticVariables.playerName + "! That was incredible!!", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Ugh, come on, get behind me. There are more of them approaching.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Player", "Walk", -301, 2137, 0.5f);
            AdvanceConditionWait(0.2f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Knight NPC", "Walk", -500, 605, 2f);
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep) {
            FlipDirection("Knight NPC");
            GetObjectFromName("Undead Horde Controller").GetComponent<CutsceneUndeadHorde>().Setup(playerAnimator, GetObjectFromName("Undead Horde Whole Groups").transform, GetObjectFromName("Undead Horde Top Half Groups").transform, GetObjectFromName("Undead Horde Bottom Half Groups").transform, this);
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("So let me take a wild guess about what happened here...", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("You wanted to act all brave, so you crashed this business party and attacked everyone.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I broke their piñata!", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("And when they all got mad, you couldn't defend yourself?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Er, I'm not sure I would use those words exactly.", "Knight NPC", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("You're lucky these guys can regenerate, or I wouldn't be helping you.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Have you learned anything from this experience?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Mainly that I'm a-afraid of sk-sk-skeletons!", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("And... I might not be as tough as I thought.", "Knight NPC", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("...", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("And?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Sigh...", "Knight NPC", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("And I owe you an apology.", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I've treated you with nothing but disrespect since we've met.", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("You have my deepest, sincerest apologies, My Dear " + StaticVariables.playerName + ".", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Wow. That's much better.", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Okay, you can count on me. I'll get you out of this mess.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("But we need to set some ground rules!", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Yes, yes! Anything! Just keep fighting them off, please!", "Knight NPC", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("First, you can't talk down to women all the time. We're fully grown-adults, and we deserve to be respected.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Which means you're not going to call me \"Milady\", or \"My Dear\", or anything else! Just address me by my actual name.", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Uh-huh.", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("I understand you want to be a brave knight one day, but for the time being, you're not, at all.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("So let me do the fighting. And the talking too. And don't do anything dangerous.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Do we have a deal?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Yes Ma'am! Uh... yes, " + StaticVariables.playerName + ".", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Great. Now can you tell me why you're so desperate to get in that pyramid?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I suppose you deserve to know. About three months ago, I had a dream. A... vision.", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("A glowing purple sword called out to me.", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Ah.", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("The dreams kept coming up until I left the Academy and set out for the desert.", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("My memory is a little fuzzy now, but the sword is definitely in that pyramid somewhere.", "Knight NPC", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Some kind of great calamity is coming soon, and I'm the only one who can stop it!", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Oh, dear.", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("What?", "Knight NPC", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("I'm not sure how to tell you this, but...", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("That calamity, it already happened.", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("What??", "Knight NPC", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("A group of dragons destroyed Duskvale, and they're on their way to taking over the continent!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("You're looking for the sword of dragonslaying. I am too.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Dragons!", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Are you telling me I've already failed in my quest?", "Knight NPC", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Look, as long as I'm still alive, the fight's not over.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("It's not too late to find that sword and put a stop to their conquest.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("You're right, " + StaticVariables.playerName + ".", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Perhaps we could sneak into the pyramid and look around?", "Knight NPC", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Okay, yeah, good idea! Finally using your brain for once.", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("MMMP MMT HMMPH", "Far-off voice", DialogueStep.Emotion.Normal);
            CutsceneUndeadHorde cuh = GetObjectFromName("Undead Horde Controller").GetComponent<CutsceneUndeadHorde>();
            cuh.CancelEverything();
            cuh.StopHordeMovement();
        }
        else if (++i == cutsceneStep) {
            HideEnemyChathead();
            AdvanceConditionDialogue_PlayerTalking("What was that?", DialogueStep.Emotion.Normal);
            FlipDirection("Table Mummy");
            FlipDirection("Zoo Skeleton 1");
            FlipDirection("Zoo Skeleton 2");
            CutsceneUndeadHorde cuh = GetObjectFromName("Undead Horde Controller").GetComponent<CutsceneUndeadHorde>();
            cuh.TurnHordeAround();
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking(); 
        }
        else if (++i == cutsceneStep) {
            GetObjectFromName("Undead Horde Controller").GetComponent<CutsceneUndeadHorde>().WalkHordeOffScreen();
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Table Mummy", "Walk", 831, 1164, 2f);
            PlayAnimationAndMoveThenIdle("Zoo Mummy", "Walk", -854, 1129, 1f);
            FlipDirection("Zoo Mummy");
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Table Skeleton", "Walk", 831, 1164, 2.5f);
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Zoo Skeleton 1", "Walk", 800, 1010, 3f);
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Zoo Skeleton 2", "Walk", 816, 939, 3.5f);
            AdvanceConditionWait(1.5f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("There's something going on up ahead.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Shall we investigate, " + StaticVariables.playerName + "?", "Knight NPC", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Yeah, but let's keep out of sight.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep) {
            GetAnimatorFromName("Table Skeleton").gameObject.SetActive(false);
            GetAnimatorFromName("Table Mummy").gameObject.SetActive(false);
            GetAnimatorFromName("Zoo Skeleton 1").gameObject.SetActive(false);
            GetAnimatorFromName("Zoo Skeleton 2").gameObject.SetActive(false);
            GetAnimatorFromName("Zoo Mummy").gameObject.SetActive(false);
            GetObjectFromName("Undead Horde Controller").GetComponent<CutsceneUndeadHorde>().HideHorde();
            PlayAnimationAndMoveThenIdle("Player", "Walk", -365, 2137, 4f);
            PlayAnimationAndMoveThenIdle("Knight NPC", "Walk", -548, 605, 4f);
            MoveEverythingExceptPlayerAndOther("Knight NPC", -1200, 0, 4f);
            AdvanceConditionWait(4f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Quick, hide behind that table! Before anyone turns around.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Knight NPC", "Walk", -373, 1245, 2f);
            AdvanceConditionWait(0.3f);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Player", "Walk", -414, 2557, 2f);
            GetObjectFromName("Player").transform.parent.SetAsLastSibling();
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Mmph mph mph mmt -", "Announcer Mummy", DialogueStep.Emotion.Custom1);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Unwrap your mouth!", "Crowd", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Mph moh yes, thank you.", "Announcer Mummy", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("As I was saying...", "Announcer Mummy", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("It's my pleasure to introduce your CEO!", "Announcer Mummy", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Put your bones together for... Edwin! Manticore!!", "Announcer Mummy", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Man-ti-core! Man-ti-core!", "Crowd", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Manticore", "Walk", 664, 800, 1.5f);
            AdvanceConditionWait(1.5f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Stay quiet, I think this might be the guy responsible for creating all of these undead.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            dialogueManager.HidePlayerChathead();
            AdvanceConditionDialogue_EnemyTalking("Ahem!", "Manticore", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("We have gathered today in celebration.", "Manticore", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Willing or not, you all have dedicated your afterlives to me and my vision for MantiCORP.", "Manticore", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Over a mere fifty days, we have built one of the greatest commercial empires in history!", "Manticore", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Woo-woo! Yeah we did!", "Crowd", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Yes, the future of MantiCORP is bright.", "Manticore", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("You may be wondering what's next. What shall we do, now that we've conquered the desert?", "Manticore", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I will sate your curiousity, but first allow me to share a little story.", "Manticore", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("For the last few decades, a precious artifact sat in our headquarters, under my care.", "Manticore", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Fifty-one days ago, a thief came to steal it.", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("You may have heard of him. He is the self-proclaimed ruler of the continent... the vile Lich King.", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Boo!!", "Crowd", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("He tried to sweet-talk me, to claim he was working for the greater good.", "Manticore", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I attacked the scoundrel!", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Woo! Yeah! Down with the Lich King!", "Crowd", DialogueStep.Emotion.Normal);
        } 
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("He met my lightning strikes with some kind of pitch-black dark magic.", "Manticore", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I saw the Lich King for what he truly is. A twisted abomination, reanimated by eldritch sorcery.", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("A mockery of life itself.", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Ultimately, his heist was successful. He managed to steal the artifact and escape.", "Manticore", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("How could I let a monster like that win?", "Manticore", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("The next day, I vowed to do better.", "Manticore", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("To raise the dead, the right way. No curses, no witchcraft, just honest work.", "Manticore", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Today, we stand in a conquered desert. We have brought towns to ruin and revived their citizens.", "Manticore", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Tomorrow, we march on Duskvale.", "Manticore", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Yeah! Woo-hoo!!", "Crowd", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("We will raze their precious city and show that Lich King what true necromancy is capable of!!", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Are you with me??", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Yeah!!!", "Crowd", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("He really knows how to work a crowd.", "Shelton", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Right? It's like he's telling us exactly what we want to hear!", "Bubby", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Yeah, so he must be the good guy!", "Shelton", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("For MantiCORP!!", "Crowd", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Now, hang on a minute!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking(StaticVariables.playerName + ", what are you doing? D-do you want to get z-zombified??", "Knight NPC", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Let's just go! The sword is in Duskvale anyway.", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("No, sometimes it's worth doing something brave and stupid. Watch.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            HideEnemyChathead();
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep) {
            PlayAnimationAndMoveThenIdle("Player", "Walk", -414, 2137, 2f);
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Are you all insane??", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Your boss just openly admitted to killing real people!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Worse, he brought you all back as his servants!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Now, he wants to march an army to the most populated city in the continent and do it all again!", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("What?", "Welton", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("She has a point.", "Lully", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_PlayerTalking("Plus, they're recovering from a disaster right now.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Is Edwin Manticore the bad guy?", "Felton", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Down with MantiCORP!", "Zuzzy", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("What is a human doing here??", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Boo!! Down with MantiCORP!!!", "Crowd", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Where is that bouncer??", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("\"That bouncer\"? Erm, you mean the Pharaoh?", "Announcer Mummy", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("His majesty... got beat up. He is currently reforming.", "Announcer Mummy", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Bah! I didn't bring him back to be worshipped, I brought him back to work for me!", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("He's fired!", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("I'd advise against firing the Pharaoh. It would be a PR nightmare.", "Announcer Mummy", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            if (StaticVariables.allowProfanities)
                AdvanceConditionDialogue_EnemyTalking("To hell with PR! I want his head!", "Manticore", DialogueStep.Emotion.Angry);
            else
                AdvanceConditionDialogue_EnemyTalking("To heck with PR! I want his head!", "Manticore", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Now he's threatening the Pharaoh!", "Elton", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("Down with MantiCORP!!", "Crowd", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            EndNormalStage();
        }
    }

    private void DoDesert3Step(){   
        int i = 0;
        if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Hey, spellbook.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Am I right in thinking that there are two catalysts inside the pyramid?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("One for <lightning>lightning<>, the other for <necromancy>necromancy<>?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("\"THERE ARE INDEED TWO STRONG MAGICAL SOURCES NEARBY.\"", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("\"THE SPECIFIC SCHOOLS OF MAGIC ARE NOT IDENTIFIABLE AT A DISTANCE.\"", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh! Interesting...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I have absolutely no desire to become a <necromancy>necromancer<>.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But <lightning>lightning<> would be cool! Could you guide me to one of them, and we'll see which one it is?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("\"CERTAINLY. PROCEED FORWARD.\"", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Walk While Holding Book");
            MoveObject("Player", 488, 2055, 5f);
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep){
            StartCutsceneImageTransition(desert3_pt2);
            advanceCondition = Cond.BackgroundChange;
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Idle Holding Book");
            AdvanceConditionDialogue_EnemyTalking("\"WARMER.\"", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Walk While Holding Book");
            MoveObject("Player", -129, 2321, 3f);
            MoveEverythingExceptPlayer(0, -100f, 3f);
            AdvanceConditionWait(3f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Idle Holding Book");
            AdvanceConditionDialogue_EnemyTalking("\"WARMER...\"", "Magic Book", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking();
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Walk While Holding Book");
            MoveEverythingExceptPlayer(-19.3f, -874.9f, 3f);
            MoveObject("Player", -10.4f, 1870, 3f);
            AdvanceConditionWait(3f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Idle Holding Book");
            AdvanceConditionDialogue_PlayerTalking("Hmmm... A very conspicuous lightning rod, at the top of a pyramid in the middle of a cloudless desert.", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("If this turns out to be the <necromancy>necromancy catalyst<>, I'd be absolutely shocked.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Not literally shocked, of course! At least I hope not...", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("What field of study could be related to <lightning>lightning magic<> anyway?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("That manticore didn't give any hints about his hobbies between all the self-praise and plans for domination.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It'll be a long journey to get to Duskvale. I'm sure I'll have plenty of time to figure it out.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh well. Let's get this new power and start heading back.", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Put Away Book");
            AdvanceConditionWait(1.5f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Walk");
            //MoveObject("Player", 2.5f, 2676, 1f);
            MoveObject("Player", -24f, 1976, 1f);
            //MoveEverythingExceptPlayer(0, -500f, 2f);
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Idle");
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Book Catch");
            AdvanceConditionWait(1f);
        } 
        else if (++i == cutsceneStep){
            StartScreenShake();
            AdvanceConditionWait(0.8f);
        }
        else if (++i == cutsceneStep){
            GetAnimatorFromName("Lightning Strike").transform.parent.gameObject.SetActive(true);
            AdvanceConditionWait(1f);
        }
        else if (++i == cutsceneStep){
            MagicFlash flash = GetObjectFromName("Magic Flash").GetComponent<MagicFlash>();
            flash.gameObject.SetActive(true);
            flash.StartProcess(StaticVariables.lightningPowerupColor);
            AdvanceConditionWait(flash.GetTotalTime() - flash.fadeTime);
        }
        else if (++i == cutsceneStep){
            StopShakeScreen();
            StartCutsceneImageTransition(desert3_pt3);
            advanceCondition = Cond.BackgroundChange;
        }
        else if (++i == cutsceneStep){
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Well, well. You speak the truth, " + StaticVariables.playerName + ". This stuff is great!", "Knight NPC", DialogueStep.Emotion.Custom1);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Isn't it?", DialogueStep.Emotion.Surprised_Spa);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("For the first time in my life, I'm aware of my pores!", "Knight NPC", DialogueStep.Emotion.Custom1);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Which is... good, I think?", "Knight NPC", DialogueStep.Emotion.Custom1);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I can feel my skin doing something!", "Knight NPC", DialogueStep.Emotion.Custom1);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Ha! Your knight friend isn't so bad after all.", "Nymph - sick", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It's amazing how a little humility can change a person.", DialogueStep.Emotion.Normal_Spa);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I'm glad you've stopped pretending you're such a brave macho man.", DialogueStep.Emotion.Surprised_Spa);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Ugh, honestly, it has been quite exhausting.", "Knight NPC", DialogueStep.Emotion.Custom1);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Maybe it's time to stop taking myself so seriously, huh?", "Knight NPC", DialogueStep.Emotion.Custom1);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Some peaceful relaxation here would do me good.", "Knight NPC", DialogueStep.Emotion.Custom1);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh yeah, we should really try to rest while we can!", DialogueStep.Emotion.Surprised_Spa);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I know a shortcut that'll get us to Duskvale ahead of the undead army.", DialogueStep.Emotion.Normal_Spa);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It'll save us a ton of time, but the path might be a little... rapid.", DialogueStep.Emotion.Surprised_Spa);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I don't quite understand what you mean.", "Knight NPC", DialogueStep.Emotion.Custom1);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh, you'll see soon enough!", DialogueStep.Emotion.Surprised_Spa);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep){
            StaticVariables.StartFadeDarken(0.5f);
            AdvanceConditionWait(0.5f);
        }
        else if (++i == cutsceneStep){
            GetObjectFromName("Desert background stuff").SetActive(false);
            GetObjectFromName("Desert foreground stuff").SetActive(false);
            GetAnimatorFromName("Player").transform.parent.gameObject.SetActive(false);
            GetAnimatorFromName("Knight NPC").transform.parent.gameObject.SetActive(false);
            GetAnimatorFromName("Nymph - sick").transform.parent.gameObject.SetActive(false);
            GetObjectFromName("Forest people").transform.DOLocalMoveX(-250, 4f);
            StaticVariables.StartFadeLighten(0.5f);
            AdvanceConditionWait(4f);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Why would you take us... Hah... this way? Are you crazy??", "Knight NPC", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Why wouldn't you take your... Huff... armor off first? Are YOU crazy??", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("This armor... Phew... represents the Academy... Hah... and my future as a knight!", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It might represent... Huff... your corpse if you keep wearing it everywhere!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("You went to a place called the \"Sunscorched... Hah... Desert\", and you really thought a suit of... Whew... armor was appropriate??", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Can we... Ahh... cut the chit-chat?", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Yeah, maybe now isn't the time.",  DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep){
            Transform people = GetObjectFromName("Forest people").transform;
            people.DOLocalMoveX(-1000, 3f);
            people.GetChild(2).DOKill();
            AdvanceConditionWait(2f);
        }
        else if (++i == cutsceneStep){
            StartCutsceneImageTransition(desert3_pt4);
            advanceCondition = Cond.BackgroundChange;
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("So it turns out he's been trying to cure the Quercus giganteum the entire time!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("That Is Im Press Ive.", "Cyclops", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Per Haps I Judged Him Too Harsh Ly.", "Cyclops", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Not just you! Everyone in the forst is suspicious of him!", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("I May Send My App Ren Tice To Pro Vide A Ssist Ance.", "Cyclops", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Such An Arr Ange Ment Could Be Mu Tu Al Ly Be Ne Fi Cial.", "Cyclops", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Could you please stop speaking gibberish??", "Knight NPC", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Ah! Lit Tle Knight.", "Cyclops", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("You Bear The I Con Og Ra Phy Of The A Ca Dem Y.", "Cyclops", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("The A.. Cademy? Uh, yeah, I'm a student there.", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("And You Now Tra Vel With The Most Gif Ted Mag I Cal Prac Ti Tion Er Of Our Age.", "Cyclops", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Are you trying to make me blush?", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Do Not Squan Der Your Opp Or Tu Ni Ties. You Have Much To Learn.", "Cyclops", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Um, yes! Yes. You're right.", "Knight NPC", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Thank you! For your wise, uh... wisdom.", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_NobodyTalking(true);
        }
        else if (++i == cutsceneStep){
            StartCutsceneImageTransition(desert3_pt5);
            advanceCondition = Cond.BackgroundChange;
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Actually, Eldric is out of town for the moment!", "Blond Lady", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Apparently adventure called, and he answered!", "Brown Hair Lady No Hat", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh, wow! Do you have any idea which way he went?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Not a clue. He just disappeared one night.", "Blond Lady", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("He left behind a goodbye note, and the kids were able to read it.", "Blond Lady", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("That's incredible!", DialogueStep.Emotion.Surprised);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Yeah, I told them about those magic letters in your magic book! They've been excited about reading ever since!", "Brown Hair Lady No Hat", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I should have a talk with those kids after this is all over...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Come to think of it, I saw you talking to Eldric on your way out of town!", "Blacksmith", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Something you said must have resonated with him?", "Blacksmith", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Let me think...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("He saw me talking to my book, and I mentioned I'd heal his hands when he got back, then he said he'd...", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Ah! I think I know what might've happened.", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Well don't be coy about it! Tell us!", "Brown Hair Lady No Hat", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Maybe he'll tell you himself, when he returns.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But that reminds me, I should get going too...", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I didn't find the sword, but it's likely in Duskvale.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("Let's all head over there and grab it!", "Blacksmith", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("No, no, no! That's a very bad idea.", DialogueStep.Emotion.Worried);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Dragons, magic, the undead, all of it seems to be converging in Duskvale.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("The city has been a warzone for a few weeks now, and it'll just continue to get worse.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("You can trust me, I'll get that sword! Just try to stay safe while I'm gone.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            EndLastStageOfWorld();
        }
        //have the kids approach and say they started reading, after hearing the magic book was filled with magic letters? or do that in the epilogue
        //in a duskvale cutscene, something about the existence of "actual literal personified death"
    }
    private void DoCity1Step(){   
        int i = 0;
        if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Well, we've made it to Duskvale.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_EnemyTalking("But it doesn't look like there's anything here!", "Knight NPC", DialogueStep.Emotion.Angry);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It looks like we've reached the end of the playtest.", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("The sword must be here somewhere...", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It must be, right?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But we won't know for sure until the next time the game gets updated.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("What else is there to do now?", "Knight NPC", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Too bad that big button on the bottom of the main menu doesn't work. It's supposed to be something called \"Endless Mode\"!", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh well, in the meantime at least I can play around with all my fun powerups!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It looks like this next enemy is harmless and has a load of health, too! What a great coincidence!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh, I guess we never explained how the <lightning>Power of Lightning<> works, did we?", DialogueStep.Emotion.Questioning);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("It's pretty straightforward, honestly. It just stuns an enemy and delays their next attack.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep) {
            AdvanceConditionDialogue_EnemyTalking("That sounds like fun!", "Knight NPC", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("I'm sure it will be.", DialogueStep.Emotion.Normal);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("But maybe it won't be the most fun to try on a harmless enemy.", DialogueStep.Emotion.Defeated);
        }
        else if (++i == cutsceneStep){
            AdvanceConditionDialogue_PlayerTalking("Oh, well! I bet the animations will look cool at least!", DialogueStep.Emotion.Happy);
        }
        else if (++i == cutsceneStep){
            EndNormalStage();
        }
    }

    private void MoveEverythingExceptPlayer(float xDistance, float yDistance, float duration){
        foreach (Transform t in backgroundParent){
            if (t.name != "Player (Overworld)"){
                t.DOLocalMove(new Vector2(t.localPosition.x + xDistance, t.localPosition.y + yDistance), duration);
            }
        }
    }
    
    private void MoveEverythingExceptPlayerAndOther(string otherName, float xDistance, float yDistance, float duration){
        foreach (Transform t in backgroundParent){
            if ((t.name != "Player (Overworld)") && (t.name != otherName)){
                t.DOLocalMove(new Vector2(t.localPosition.x + xDistance, t.localPosition.y + yDistance), duration);
            }
        }
    }

    private void AdvanceConditionWaitThenClick(float duration){
        advanceCondition = Cond.Wait;
        StaticVariables.WaitTimeThenCallFunction(duration, EnableClick);
    }

    private void EnableClick(){
        advanceCondition = Cond.Click;
        CheckButtonAvailability();
    }

    private void AdvanceConditionWait(float duration){
        advanceCondition = Cond.Wait;
        StaticVariables.WaitTimeThenCallFunction(duration, AdvanceCutsceneStep);
    }

    private void ToggleButton(bool value){
        dialogueManager.realButton.SetActive(value);
    }

    private void SetupDialogueManager(){
        dialogueManager.isInBattle = false;
        dialogueManager.isInOverworld = false;
        dialogueManager.isInCutscene = true;
        dialogueManager.cutsceneManager = this;
        dialogueManager.ClearDialogue();
        dialogueManager.SetStartingValues();
        dialogueManager.TransitionToShowing();
        ToggleButton(false);
    }

    private void HideChatheads(){
        dialogueManager.HideChatheads(dialogueManager.transitionDuration);
    }

    private void HideEnemyChathead(){
        dialogueManager.HideEnemyChathead(dialogueManager.transitionDuration);
    }
    
    private void AdvanceConditionDialogue_PlayerTalking(string s, DialogueStep.Emotion emotion){
        advanceCondition = Cond.Click;
        DisplayPlayerTalking(s, emotion);
    }

    private void AdvanceConditionDialogue_NobodyTalking(bool alsoHideChatheads = false){
        if (alsoHideChatheads)
            HideChatheads();
        DisplayNobodyTalking();
        AdvanceConditionWait(0.5f);
    }

    private void DisplayPlayerTalking(string s, DialogueStep.Emotion emotion){
        dialogueManager.ShowPlayerTalking(emotion);
        dialogueManager.dialogueTextBox.text = TextFormatter.FormatString(s);

    }

    private void DisplayEnemyTalking(string s, EnemyData enemyData, DialogueStep.Emotion emotion, string nameOverride = ""){
        dialogueManager.ShowEnemyTalking(enemyData, emotion, nameOverride);
        dialogueManager.dialogueTextBox.text = TextFormatter.FormatString(s);

    }

    private void AdvanceConditionDialogue_EnemyTalking(string s, string enemyName, DialogueStep.Emotion emotion, string nameOverride = ""){
        advanceCondition = Cond.Click;
        DisplayEnemyTalking(s, enemyName, emotion, nameOverride);

    }

    private void DisplayEnemyTalking(string s, string enemyName, DialogueStep.Emotion emotion, string nameOverride = ""){
        DisplayEnemyTalking(s, GetAnimatorFromName(enemyName).GetComponent<EnemyData>(), emotion, nameOverride);
    }

    private void DisplayNobodyTalking(){
        dialogueManager.ShowNobodyTalking();
    }

    private Animator GetAnimatorFromName(string name){
        if (name == "Player")
            return playerAnimator;
        foreach (Animator anim in animatedObjectsInCutscene){
            if (anim != null){
                if (anim.gameObject.name == name)
                    return anim;
            }
        }
        //print("No animator found with the name [" + name + "]");
        return null;
    }

    public GameObject GetObjectFromName(string name){
        if (name == "Player")
            return playerAnimator.gameObject;
        foreach (GameObject go in searchableObjectsInCutscene)
            if (go != null){
                if (go.name == name)
                    return go;
            }
        //print("No gameObject found with the name [" + name + "]");
        return null;
    }
    

    //private void ButtonText(string s){
    //    dialogueManager.SetButtonText(s);
    //}

    public void PressedNextButton(){
        if (advanceCondition == Cond.Click)
            AdvanceCutsceneStep();
    }

    private void ToggleObject(string name, bool enabled){
        //every animated object is made the child of an empty gameobject, so we want to toggle the parent of the animator
        Animator anim = GetAnimatorFromName(name);
        //the same is not true of searchable gameobjects, we want to toggle those directly
        GameObject go = GetObjectFromName(name);
        if (anim != null)
            anim.transform.parent.gameObject.SetActive(enabled);
        else if (go != null)
            go.SetActive(enabled);
        else{
            print("no object found with name [" + name + "] to toggle");
        }
    }
    
    private void StartScreenShake(float duration = -9999){
        shakeTimer = duration;
        ShakeScreen();
    }

    private void ShakeScreen(){
        float singleShakeDuration = 0.15f;
        if (shakeTimer != -9999)
            shakeTimer -= singleShakeDuration;
        if ((shakeTimer != -9999) && (shakeTimer <= 0)){
            screenshakeTransform.DOAnchorPos(Vector2.zero, singleShakeDuration);
            return;
        }
        Vector2 newSpot = new Vector2 (StaticVariables.rand.Next(-50, 50), StaticVariables.rand.Next(-50, 50));
        screenshakeTransform.DOAnchorPos(newSpot, singleShakeDuration).OnComplete(ShakeScreen);
    }

    private void StopShakeScreen(){
        shakeTimer = 0 ;
    }

    private void PlayAnimationAndMoveThenIdle(string objectName, string animationName, float xPos, float yPos, float duration){
        PlayAnimationThenIdle(objectName, animationName, duration);
        MoveObject(objectName, xPos, yPos, duration);
    }

    private void PlayAnimationThenIdle(string objectName, string animationName, float duration){
        PlayAnimation(objectName, animationName);
        StaticVariables.WaitTimeThenCallFunction(duration, PlayIdle, objectName);
    }

    private void PlayAnimationAndMoveThenDisable(string objectName, string animationName, float xPos, float yPos, float duration){
        PlayAnimationThenDisable(objectName, animationName, duration);
        MoveObject(objectName, xPos, yPos, duration);
    }

    private void PlayAnimationThenDisable(string objectName, string animationName, float duration){
        PlayAnimation(objectName, animationName);
        StaticVariables.WaitTimeThenCallFunction(duration, Disable, objectName);
    }

    private void PlayIdle(string name){
        PlayAnimation(name, "Idle");
    }

    private void Disable(string name){
        GetAnimatorFromName(name).gameObject.SetActive(false);
    }
    
    private void PlayAnimation(string objectName, string animationName){
        GetAnimatorFromName(objectName).Play(animationName);
    }

    private void MoveObject(string objectName, float xPos, float yPos, float duration){
        //every animated object is made the child of an empty gameobject, so we want to move the parent of the animator
        Animator anim = GetAnimatorFromName(objectName);
        //the same is not true of searchable gameobjects, we want to move those directly
        GameObject go = GetObjectFromName(objectName);
        if (anim != null)
            anim.transform.parent.GetComponent<RectTransform>().DOAnchorPos(new Vector2(xPos, yPos), duration);
        else if (go != null)
            go.GetComponent<RectTransform>().DOAnchorPos(new Vector2(xPos, yPos), duration);
        else{
            print("no object found with name [" + name + "] to move");
        }
    }

    private void FlipDirection(string objectName){
        Animator anim = GetAnimatorFromName(objectName);
        anim.transform.parent.localScale = new Vector2(anim.transform.parent.localScale.x * -1, anim.transform.parent.localScale.y);
    }

    private void StartCutsceneImageTransition(GameObject bg){
        nextBackground = bg;
        StaticVariables.StartFadeDarken(0.5f);
        StaticVariables.WaitTimeThenCallFunction(0.5f, MidCutsceneImageTransition);
    }

    private void MidCutsceneImageTransition(){
        SetCutsceneBackground(nextBackground);
        StaticVariables.StartFadeLighten(0.5f);
        StaticVariables.WaitTimeThenCallFunction(0.5f, EndCutsceneImageTransition);
        switch (advanceCondition){
            case (Cond.BackgroundChange):
                AdvanceCutsceneStep();
                break;
        }
    }

    private void SetCutsceneBackground(GameObject prefab){

        animatedObjectsInCutscene = new List<Animator>();
        searchableObjectsInCutscene = new List<GameObject>();
        Transform backgroundPrefab = prefab.transform.GetChild(1).transform;

        foreach (Transform t in backgroundParent)
            Destroy(t.gameObject);
        foreach(Transform t in backgroundPrefab){

            Animator a = t.gameObject.GetComponent<Animator>();
            if (a != null){
                GameObject parent = Instantiate(emptyGameObject, backgroundParent);
                parent.transform.localPosition = t.localPosition;
                parent.name = t.name;
                parent.SetActive(t.gameObject.activeSelf);
                GameObject go = Instantiate(t.gameObject, parent.transform.position, Quaternion.identity, parent.transform);
                go.name = t.name;
                parent.transform.localRotation = t.localRotation;
                go.SetActive(true);
                animatedObjectsInCutscene.Add(go.GetComponent<Animator>());
                AddToSearchableListIfAppropriate(go);
            }
            else{
                GameObject go = Instantiate(t.gameObject, backgroundParent);
                go.name = t.gameObject.name;
                if (t.name.Contains("Player"))
                    playerAnimator = go.transform.GetChild(0).GetComponent<Animator>();
                AddToSearchableListIfAppropriate(go);
            }
        }
    }

    private void AddToSearchableListIfAppropriate(GameObject go){
        if (go.tag == "ScriptSearchable")
            searchableObjectsInCutscene.Add(go);
    }

    private void EndCutsceneImageTransition(){
    }

    public void ExternalTrigger(float optionalParameter = -999){
        if (advanceCondition == Cond.externalTrigger){
            if (optionalParameter != -999)
                externalTriggerParameter = optionalParameter;
            AdvanceCutsceneStep();
        }
    }

    public void ClickedBackButtonl() {
        StaticVariables.hasCompletedStage = false;
        StaticVariables.FadeOutThenLoadScene(StaticVariables.lastVisitedStage.worldName);
    }
    
    private void EndLastStageOfWorld(){
        //if you havent beaten this world before
        if (StaticVariables.lastVisitedStage == StaticVariables.highestBeatenStage.nextStage){
            StaticVariables.highestBeatenStage = StaticVariables.highestBeatenStage.nextStage;
            StaticVariables.hasTalkedToNewestEnemy = false;
            StaticVariables.hasCompletedStage = false;
            StaticVariables.lastVisitedStage = StaticVariables.highestBeatenStage.nextStage;
            SaveSystem.SaveGame();
            StaticVariables.FadeOutThenLoadScene(StaticVariables.lastVisitedStage.worldName);
        }
        else{
            StaticVariables.hasCompletedStage = true;
            SaveSystem.SaveGame();
            StaticVariables.FadeOutThenLoadScene(StaticVariables.lastVisitedStage.worldName);
        }
    }
    
    private void EndNormalStage(){
        StaticVariables.hasCompletedStage = true;
        SaveSystem.SaveGame();
        StaticVariables.FadeOutThenLoadScene(StaticVariables.lastVisitedStage.worldName);
    }
}


