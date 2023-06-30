using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CutsceneManager : MonoBehaviour{

    private int cutsceneStep = 0;
    private enum Cond{Click, Wait, BackgroundChange};
    private Cond advanceCondition;
    public enum Cutscene{Opening, SavedTown};
    private Cutscene cutsceneID;
    private List<Animator> animatedObjectsInCutscene = new List<Animator>();
    private float shakeTimer = 0f;
    private Animator playerAnimator;
    private GameObject nextBackground;

    public GeneralSceneManager generalSceneManager;
    public DialogueManager dialogueManager;
    public RectTransform backgroundParent;
    public GameObject emptyGameObject;
    public RectTransform screenshakeTransform;

    public GameObject openingBackground1;
    public GameObject openingBackground2;
    public GameObject openingBackground3;
    

    public void Start() {
        //manually start at a later cutscene step (put 1 less than the step you actually want to start on)
        //cutsceneStep = 31;
        SetCutsceneID();
        switch (cutsceneID){
            case (Cutscene.Opening):
                SetupOpening();
                break;
        }
        ButtonText("CONTINUE");

        SetupDialogueManager();

        generalSceneManager.Setup();
        StaticVariables.WaitTimeThenCallFunction(StaticVariables.sceneFadeDuration, AdvanceCutsceneStep);
    }

    private void SetCutsceneID(){
        cutsceneID = StaticVariables.cutsceneID;
    }

    private void SetupOpening(){
        SetCutsceneBackground(openingBackground1);

    }

    private void AdvanceCutsceneStep(){
        cutsceneStep ++;
        switch (cutsceneID){
            case (Cutscene.Opening):
                DoOpeningStep();
                break;
        }
        switch (advanceCondition){
            case (Cond.Click):
                ToggleButton(true);
                break;
            default:
                ToggleButton(false);
                break;
        }
    }

    private void DoOpeningStep(){   
        int i = 0;
        if (++i == cutsceneStep){
            DisplayEnemyTalking("Miss Player! Miss Player!", "Child 1", DialogueStep.Emotion.Excited);
            PlayAnimationAndMoveThenIdle("Child 1", "Walk", 26, 605, 2f);
            PlayAnimationAndMoveThenIdle("Child 2", "Walk", 85, 564, 2.3f);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(2.3f);
        }
        else if (++i == cutsceneStep){
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("What is it, children?", DialogueStep.Emotion.Happy);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("A newspaper just came in! Look!", "Child 2", DialogueStep.Emotion.Excited);
            PlayAnimation("Child 2", "Show Newspaper");
            advanceCondition = Cond.Wait;
            WaitThenAdvance(1.6f);
        }
        else if (++i == cutsceneStep){
            advanceCondition = Cond.Click;
        }        
        else if (++i == cutsceneStep){                        
            DisplayEnemyTalking("Can you read it? It looks important!", "Child 1", DialogueStep.Emotion.Excited);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("You're the only person in the whole town who knows how to read!", "Child 2", DialogueStep.Emotion.Excited);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("This does look important...", DialogueStep.Emotion.Worried);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("'Kingdom under new rule! The Lich King has been driven into hiding...'", DialogueStep.Emotion.Normal);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("Well that's great! I never liked His Lichness anyway.", DialogueStep.Emotion.Happy);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("There's more here...\n'The king has been driven away by --'", DialogueStep.Emotion.Questioning);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("ROOOOOOOAR!!!!", "Mystery Dragon", DialogueStep.Emotion.Angry);
            PlayAnimation("Child 2", "Put Away Newspaper");
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Red Dragon", "Fly");
            MoveObject("Red Dragon", 1200, 1377, 2f);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(1f);
        }
        else if (++i == cutsceneStep){
            FlipDirection("Child 1");
            FlipDirection("Child 2");
            advanceCondition = Cond.Wait;
            WaitThenAdvance(1f);
        }
        else if (++i == cutsceneStep){
            MoveObject("Red Dragon", 521, 1050, 2f);
            FlipDirection("Red Dragon");
            advanceCondition = Cond.Wait;
            WaitThenAdvance(2f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Red Dragon", "Land");
            MoveObject("Red Dragon", 521, 1000, 1.1f);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(0.6f);
        }
        else if (++i == cutsceneStep){
            StartScreenShake(1f);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(1f);
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("'-- a group of dragons!'", DialogueStep.Emotion.Worried);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("Run away!!!!", "Child 1", DialogueStep.Emotion.Excited);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(1f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Child 1", "Walk", -844, 605, 2f);
            PlayAnimationAndMoveThenIdle("Child 2", "Walk", -1096, 564, 2f);
            FlipDirection("Child 1");
            FlipDirection("Child 2");
            advanceCondition = Cond.Wait;
            WaitThenAdvance(2f);
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("Humans...\nTiny, tasty, delectable humans...", "Red Dragon", DialogueStep.Emotion.Angry);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("Come out little humans, and meet your fate!", "Red Dragon", DialogueStep.Emotion.Angry);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Blacksmith", "Walk", -172, 64, 1.3f);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(0.2f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Orange Shirt Black Woman", "Walk", -419, 244, 1.3f);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(0.1f);
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Redhead Woman", "Walk", 273, 540, 1.3f);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(1.3f);
        }
        else if (++i == cutsceneStep){
            FlipDirection("Redhead Woman");
            DisplayEnemyTalking("This town now belongs to the mighty King Dragon! Bow down to him and all of dragonkind!", "Red Dragon", DialogueStep.Emotion.Angry);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("Surrender all of your valuables, or be brought to ruin!", "Red Dragon", DialogueStep.Emotion.Angry);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("No! We won't be bossed around by some overgrown lizard!", "Blacksmith", DialogueStep.Emotion.Angry);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("We're not afraid of you!", "Redhead Woman", DialogueStep.Emotion.Angry);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("You're not afraid of me? Let me give you a little taste of fear!", "Red Dragon", DialogueStep.Emotion.Angry);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Red Dragon", "Prolonged Attack - Start");
            advanceCondition = Cond.Wait;
            WaitThenAdvance(1.5f);
        }
        else if (++i == cutsceneStep){
            StartCutsceneImageTransition(openingBackground2);
            advanceCondition = Cond.BackgroundChange;
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Red Dragon", "Prolonged Attack - End");
            advanceCondition = Cond.Wait;
            WaitThenAdvance(1.5f);
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("Ha ha ha... You humans are powerless!", "Red Dragon", DialogueStep.Emotion.Angry);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("Just try to protect your homes now... Goblins!", "Red Dragon", DialogueStep.Emotion.Angry);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            PlayAnimationAndMoveThenIdle("Wolf Rider", "Walk", 575, 451, 2f);
            PlayAnimationAndMoveThenIdle("Goblin 1", "Walk", -47, 348, 2f);
            PlayAnimationAndMoveThenIdle("Goblin 2", "Walk", 157, 348, 2f);
            PlayAnimationAndMoveThenIdle("Goblin 3", "Walk", 350, 348, 2f);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(2f);
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("Yes master?", "Wolf Rider", DialogueStep.Emotion.Normal);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("Kill them! And take everything they have!", "Red Dragon", DialogueStep.Emotion.Angry);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayEnemyTalking("Ha ha ha!!!", "Red Dragon", DialogueStep.Emotion.Angry);
            PlayAnimationAndMoveThenIdle("Red Dragon", "Fly", -1500, 1400, 2f);
            PlayAnimationAndMoveThenIdle("Goblin 1", "Walk", -244, 191, 2f);
            PlayAnimationAndMoveThenIdle("Goblin 2", "Walk", 21, 16, 2f);
            PlayAnimationAndMoveThenIdle("Goblin 3", "Walk", 446, 489, 2f);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(2f);
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("I don't know anything about fighting... I'll have to leave the town defense to the professionals.", DialogueStep.Emotion.Worried);
            dialogueManager.HideEnemyChathead(dialogueManager.transitionDuration);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("AH! The library is on fire!", DialogueStep.Emotion.Angry);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("All of our precious books are burning!", DialogueStep.Emotion.Worried);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("I'll have to toss some of the books into the sacred town well... I hope the goddess doesn't mind.", DialogueStep.Emotion.Questioning);
            FlipDirection("Player");
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Walk");
            MoveObject("Player", -75, 2137, 1.5f);
            FlipDirection("Player");
            advanceCondition = Cond.Wait;
            WaitThenAdvance(1.5f);
        }
        else if (++i == cutsceneStep){
            ToggleObject("Player", false);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(0.5f);
        }
        else if (++i == cutsceneStep){
            ToggleObject("Tossing Books", true);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(5f);
        }
        else if (++i == cutsceneStep){
            StartScreenShake(6);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(1.66f);
        }
        else if (++i == cutsceneStep){              
            DisplayPlayerTalking("What's happening now??", DialogueStep.Emotion.Questioning);
            ToggleObject("Player", true);
            ToggleObject("Tossing Books", false);
            PlayAnimationAndMoveThenIdle("Player", "Walk", -140, 1860, 1.5f);
            FlipDirection("Player");
            advanceCondition = Cond.Wait;
            WaitThenAdvance(2f);
        }
        else if (++i == cutsceneStep){             
            ToggleObject("Water Spray", true);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(3f);
        }
        else if (++i == cutsceneStep){             
            ToggleObject("Rain", true);
            MoveObject("Rain", -33, 630, 1.5f);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(5f);
        }
        else if (++i == cutsceneStep){
            StartCutsceneImageTransition(openingBackground3);
            advanceCondition = Cond.BackgroundChange;
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Water Spray", "Sustained Spray");
            DisplayPlayerTalking("There's a book coming out of the water!", DialogueStep.Emotion.Happy);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Water Spray", "End Spray");
            advanceCondition = Cond.Wait;
            WaitThenAdvance(0.7f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Walk");
            MoveObject("Player", 300, 1860, 1.1f);
            advanceCondition = Cond.Wait;
            WaitThenAdvance(0.6f);
        }
        else if (++i == cutsceneStep){
            PlayAnimation("Player", "Book Catch");
            advanceCondition = Cond.Wait;
            WaitThenAdvance(0.67f);
        }
        else if (++i == cutsceneStep){
            ToggleObject("Water Spray", false);
            DisplayPlayerTalking("Got it!", DialogueStep.Emotion.Happy);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("Hey, I remember this book! It was always empty, and no ink would stick to the pages...", DialogueStep.Emotion.Questioning);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("But now the pages have writing in them?", DialogueStep.Emotion.Questioning);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            DisplayPlayerTalking("And the letters react to my touch!", DialogueStep.Emotion.Happy);
            advanceCondition = Cond.Click;
        }
        else if (++i == cutsceneStep){
            StaticVariables.currentBattleWorld = 0;
            StaticVariables.currentBattleLevel = 1;
            StaticVariables.beatCurrentBattle = true;
            StaticVariables.FadeOutThenLoadScene(StaticVariables.GetCurrentWorldName());
        }
    }

    private void WaitThenAdvance(float duration){
        StaticVariables.WaitTimeThenCallFunction(duration, AdvanceCutsceneStep);
    }

    
    

    private void ToggleButton(bool value){
        dialogueManager.buttonText.transform.parent.gameObject.SetActive(value);
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
    
    private void DisplayPlayerTalking(string s, DialogueStep.Emotion emotion){
        dialogueManager.ShowPlayerTalking(emotion);
        dialogueManager.dialogueTextBox.text = s;
    }

    private void DisplayEnemyTalking(string s, EnemyData enemyData, DialogueStep.Emotion emotion){
        dialogueManager.ShowEnemyTalking(enemyData, emotion);
        dialogueManager.dialogueTextBox.text = s;

    }

    private void DisplayEnemyTalking(string s, string enemyName, DialogueStep.Emotion emotion){
        DisplayEnemyTalking(s, GetAnimatorFromName(enemyName).GetComponent<EnemyData>(), emotion);
    }

    private Animator GetAnimatorFromName(string name){
        if (name == "Player")
            return playerAnimator;
        foreach (Animator anim in animatedObjectsInCutscene){
            if (anim.gameObject.name == name)
                return anim;
        }
        print("No animator found with the name [" + name + "]");
        return null;
    }
    

    private void ButtonText(string s){
        dialogueManager.buttonText.text = s;
    }

    public void PressedNextButton(){
        if (advanceCondition == Cond.Click)
            AdvanceCutsceneStep();
    }

    private void ToggleObject(string name, bool enabled){
        //every animated object is made the child of an empty gameobject, so we want to toggle the parent of the animator
        GetAnimatorFromName(name).transform.parent.gameObject.SetActive(enabled);
    }
    
    private void StartScreenShake(float duration){
        shakeTimer = duration;
        ShakeScreen();
    }

    private void ShakeScreen(){
        float singleShakeDuration = 0.15f;
        shakeTimer -= singleShakeDuration;
        if (shakeTimer <= 0){
            screenshakeTransform.DOAnchorPos(Vector2.zero, singleShakeDuration);
            return;
        }
        Vector2 newSpot = new Vector2 (StaticVariables.rand.Next(-50, 50), StaticVariables.rand.Next(-50, 50));
        screenshakeTransform.DOAnchorPos(newSpot, singleShakeDuration).OnComplete(ShakeScreen);
    }

    private void PlayAnimationAndMoveThenIdle(string objectName, string animationName, float xPos, float yPos, float duration){
        PlayAnimationThenIdle(objectName, animationName, duration);
        MoveObject(objectName, xPos, yPos, duration);
    }

    private void PlayAnimationThenIdle(string objectName, string animationName, float duration){
        PlayAnimation(objectName, animationName);
        StaticVariables.WaitTimeThenCallFunction(duration, PlayIdle, objectName);
    }

    private void PlayIdle(string name){
        PlayAnimation(name, "Idle");
    }
    
    private void PlayAnimation(string objectName, string animationName){
        GetAnimatorFromName(objectName).Play(animationName);
    }

    private void MoveObject(string objectName, float xPos, float yPos, float duration){
        GetAnimatorFromName(objectName).transform.parent.GetComponent<RectTransform>().DOAnchorPos(new Vector2(xPos, yPos), duration);
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
        Transform backgroundPrefab = prefab.transform.GetChild(1).transform;

        foreach (Transform t in backgroundParent)
            Destroy(t.gameObject);
        foreach(Transform t in backgroundPrefab){

            Animator a = t.gameObject.GetComponent<Animator>();
            if (a != null){
                GameObject parent = Instantiate(emptyGameObject, backgroundParent);
                parent.transform.localPosition = t.localPosition;
                //parent.transform.localRotation = t.localRotation;
                parent.name = t.name;
                parent.SetActive(t.gameObject.activeSelf);
                GameObject go = Instantiate(t.gameObject, parent.transform.position, Quaternion.identity, parent.transform);
                go.name = t.name;
                parent.transform.localRotation = t.localRotation;
                go.SetActive(true);
                animatedObjectsInCutscene.Add(go.GetComponent<Animator>());
            }
            else{
                GameObject go = Instantiate(t.gameObject, backgroundParent);
                go.name = t.gameObject.name;
                if (t.name.Contains("Player"))
                    playerAnimator = go.transform.GetChild(0).GetComponent<Animator>();
            }
        }
    }

    private void EndCutsceneImageTransition(){
    }

}


