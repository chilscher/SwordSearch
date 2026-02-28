//for Sword Search, copyright Fancy Bus Games 2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;

public class OverworldSceneManager : MonoBehaviour{

    [Header("Scene References")]
    public RectTransform overworldView;
    public RectTransform playerParent;
    public Animator playerAnimator;
    public OverworldSpace[] overworldSpaces;
    public RectTransform sceneHeader;
    public SceneChangerVisuals sceneChangerVisuals;
    //public GeneralSceneManager generalSceneManager;


    [Header("Timing Configurations")]
    public float playerStepDuration = 0.2f; //time it takes to move 1 overworld space step
    //public float playerWalkSpeed = 500f;
    //public float minTimeToMove = 1f;

    [Header("Overworld Settings")]
    public int thisWorldNum; // 0 means this is the atlas/map scene
    public int powerupsPerPuzzle = 3;
    public bool healActive = false;
    public bool waterActive = false;
    public bool earthActive = false;
    public bool fireActive = false;
    public bool lightningActive = false;
    public bool darkActive = false;
    public bool swordActive = false;
    

    [HideInInspector]
    public bool isPlayerMoving = false;
    [HideInInspector]
    public OverworldSpace currentPlayerSpace = null;
    [HideInInspector]
    public EnemyData currentEnemyData;
    public InteractOverlayManager interactOverlayManager;
    public DialogueManager dialogueManager;
    private List<GameObject> stepsToNextSpace;
    public bool revealLastEnemySlowly = false;
    //private bool changePlayerDirectionAtNextStep = false;
    //private bool playerDestinationIsNextStep = false;


    void Start(){
        //GetComponent<GeneralSceneManager>().Setup();
        //generalSceneManager.Setup();
        interactOverlayManager.gameObject.SetActive(true);
        dialogueManager.gameObject.SetActive(true);
        SetupOverworldSpaces();
        PickReadingBookOptions();
        if (thisWorldNum == 0){
            PlacePlayerAtPosition(StaticVariables.lastVisitedStage.world);
            ShowAtlasProgress();
        }
        else{
            SetPowerupAvailability();
            //StaticVariables.lastVisitedStage = StaticVariables.highestBeatenStage.nextStage; //for testing fade-in for next enemy
            //StaticVariables.hasCompletedStage = true; //for testing fade-in for next enemy
            PlacePlayerAtPosition(StaticVariables.lastVisitedStage.stage);
            int lastStageToQuickReveal = StaticVariables.highestBeatenStage.nextStage.index;
            CheckIfCompletedStage();
            ShowOverworldProgress(lastStageToQuickReveal);
        }
        interactOverlayManager.Setup();
        //generalSceneManager.FadeIn();
    }

    private void CheckIfCompletedStage(){
        if (StaticVariables.lastVisitedStage == StaticVariables.highestBeatenStage.nextStage){
            if (StaticVariables.hasCompletedStage){
                StaticVariables.highestBeatenStage = StaticVariables.highestBeatenStage.nextStage;
                StaticVariables.hasTalkedToNewestEnemy = false;
                if (StaticVariables.highestBeatenStage.nextStage.world != thisWorldNum)
                    ImmediatelyStartNextWorld();
                else
                    revealLastEnemySlowly = true; //UnlockNextEnemy();
                SaveSystem.SaveGame();
            }
        }
        StaticVariables.hasCompletedStage = false;
    }

    private void ImmediatelyStartNextWorld(){
        StaticVariables.lastVisitedStage = StaticVariables.highestBeatenStage.nextStage;
        SceneManager.LoadScene(StaticVariables.lastVisitedStage.worldName);
    }

    private void SetPowerupAvailability(){
        StaticVariables.healActive = healActive;
        StaticVariables.waterActive = waterActive;
        StaticVariables.fireActive = fireActive;
        StaticVariables.earthActive = earthActive;
        StaticVariables.lightningActive = lightningActive;
        StaticVariables.darkActive = darkActive;
        StaticVariables.swordActive = swordActive;
        StaticVariables.powerupsPerPuzzle = powerupsPerPuzzle;

        //clear the buffed type if it isnt available for this world
        if (thisWorldNum < 4)
            StaticVariables.buffedType = BattleManager.PowerupTypes.None;
        if ((thisWorldNum < 5) && (StaticVariables.buffedType == BattleManager.PowerupTypes.Lightning))
            StaticVariables.buffedType = BattleManager.PowerupTypes.None;
        if ((thisWorldNum < 7) && (StaticVariables.buffedType == BattleManager.PowerupTypes.Sword))
            StaticVariables.buffedType = BattleManager.PowerupTypes.None;

        //also set the buffed powerup type, if it's not set already
        if (StaticVariables.buffedType == BattleManager.PowerupTypes.None){//if the buffed type is not selected, pick the most recent unlocked one
            if (thisWorldNum == 4)
                StaticVariables.buffedType = BattleManager.PowerupTypes.Fire;
            if (thisWorldNum == 5)
                StaticVariables.buffedType = BattleManager.PowerupTypes.Lightning;
            if (thisWorldNum == 6)
                StaticVariables.buffedType = BattleManager.PowerupTypes.Lightning;
            if (thisWorldNum == 7)
                StaticVariables.buffedType = BattleManager.PowerupTypes.Sword;
        }
    }

    private void PickReadingBookOptions(){
        interactOverlayManager.chosenWaterBook = StaticVariables.readingWaterBooks[StaticVariables.rand.Next(StaticVariables.readingWaterBooks.Length)];
        interactOverlayManager.chosenHealingBook = StaticVariables.readingHealBooks[StaticVariables.rand.Next(StaticVariables.readingHealBooks.Length)];
        interactOverlayManager.chosenEarthBook = StaticVariables.readingEarthBooks[StaticVariables.rand.Next(StaticVariables.readingEarthBooks.Length)];
        interactOverlayManager.chosenFireBook = StaticVariables.readingFireBooks[StaticVariables.rand.Next(StaticVariables.readingFireBooks.Length)];
        interactOverlayManager.chosenLightningBook = StaticVariables.readingLightningBooks[StaticVariables.rand.Next(StaticVariables.readingLightningBooks.Length)];
        interactOverlayManager.chosenDarknessBook = StaticVariables.readingDarkBooks[StaticVariables.rand.Next(StaticVariables.readingDarkBooks.Length)];
        interactOverlayManager.chosenSwordBook = StaticVariables.readingSwordBooks[StaticVariables.rand.Next(StaticVariables.readingSwordBooks.Length)];
    }

    private void SetupOverworldSpaces(){
        for (int i = 0; i < overworldSpaces.Length; i++){
            OverworldSpace space = overworldSpaces[i];
            space.overworldSceneManager = this;
            space.originalSiblingIndex = space.transform.GetSiblingIndex();

            float startTime = (StaticVariables.rand.Next(0, 100)) / 100f;
            //print(startTime);

            //start the overworld space idle animation at a random time
            Transform overworldSpaceTransform = space.transform.GetChild(0).GetChild(0);

            if (space.type == OverworldSpace.OverworldSpaceType.Cutscene) //if it's a cutscene do this
                overworldSpaceTransform.GetComponent<Animator>().Play("Idle", 0, startTime);
            else if (space.type != OverworldSpace.OverworldSpaceType.Atlas){ //if it's a battle or tutorial do this
                if (overworldSpaceTransform.GetComponent<EnemyData>().isHorde){
                    foreach(Transform hordeTransform in overworldSpaceTransform){
                        hordeTransform.GetChild(0).GetComponent<Animator>().Play("Idle", 0, startTime);
                    }
                }
                else
                    overworldSpaceTransform.GetComponent<Animator>().Play("Idle", 0, startTime);
            }
        }
    }

    public void StartMovingPlayerToSpace(OverworldSpace space){
        bool reverse = false;
        int currentSpaceIndex = 0;
        int destinationSpaceIndex = 0;
        for (int i = 0; i < overworldSpaces.Length; i++){
            if (overworldSpaces[i] == currentPlayerSpace)
                currentSpaceIndex = i;
            if (overworldSpaces[i] == space)
                destinationSpaceIndex = i;
        }
        int earlierIndex = currentSpaceIndex;
        int laterIndex = destinationSpaceIndex;
        if (destinationSpaceIndex < currentSpaceIndex){
            earlierIndex = destinationSpaceIndex;
            laterIndex = currentSpaceIndex;
            reverse = true;
        }

        stepsToNextSpace = new List<GameObject>();
        for (int i = 0; i < overworldSpaces.Length; i++){
            if (i == earlierIndex)
                stepsToNextSpace.Add(overworldSpaces[i].playerDestination);
            if ((i > earlierIndex) && (i <= laterIndex)){
                foreach (Transform t in overworldSpaces[i].pathFromLastSpace)
                    stepsToNextSpace.Add(t.gameObject);
                stepsToNextSpace.Add(overworldSpaces[i].playerDestination);
            }
        }

        if (reverse)
            stepsToNextSpace.Reverse();

        playerAnimator.SetTrigger("WalkStart");
        isPlayerMoving = true;
        currentPlayerSpace.transform.GetChild(2).GetChild(0).gameObject.SetActive(true);
        MovePlayerToNextStep();
        PointPlayerTowardNextDestination();
    }

    private void MovePlayerToNextStep(){
        if (stepsToNextSpace.Count <= 1){
            EndPlayerWalk();
            return;
        }

        GameObject currentStep = stepsToNextSpace[0]; //the step or destination that the player is currently on
        OverworldSpace overworldSpaceForCurrentStep = GetSpaceThatStepBelongsTo(currentStep);
        //nt currentStepSiblingIndex = overworldSpaceForCurrentStep.GetSiblingIndex();

        stepsToNextSpace.RemoveAt(0);

        GameObject nextStep = stepsToNextSpace[0]; //the step or destination that the player will move toward during this iteration
        OverworldSpace overworldSpaceForNextStep = GetSpaceThatStepBelongsTo(nextStep);
        //int nextStepSiblingIndex = overworldSpaceForNextStep.GetSiblingIndex();

        PointPlayerTowardNextDestination();
        playerParent.DOMove(nextStep.transform.position, playerStepDuration).OnComplete(MovePlayerToNextStep);

        OverworldSpace spacePlayerShouldBeOnTopOf = overworldSpaceForNextStep;
        if (overworldSpaceForCurrentStep.originalSiblingIndex > overworldSpaceForNextStep.originalSiblingIndex)
            spacePlayerShouldBeOnTopOf = overworldSpaceForCurrentStep;
        playerParent.SetSiblingIndex(spacePlayerShouldBeOnTopOf.originalSiblingIndex + 1);
    }

    private void PointPlayerTowardNextDestination(){
        GameObject space = GetNextPlayerDestination();
        if (space != null) {
            Vector3 s = playerParent.localScale;
            s.x = 1;
            if (space.transform.position.x < playerParent.position.x)
                s.x = -1;
            playerParent.localScale = s;
            return;
        }
    }

    private GameObject GetNextPlayerDestination() {
        foreach (GameObject space in stepsToNextSpace) {
            if (space.name == "Player Destination")
                return space;
        }
        print("there is no next enemy space");
        return null;
    }

    private OverworldSpace GetSpaceThatStepBelongsTo(GameObject step) {
        OverworldSpace parent = step.transform.parent.GetComponent<OverworldSpace>();
        OverworldSpace grandparent = step.transform.parent.parent.GetComponent<OverworldSpace>();
        if (parent != null)
            return parent;
        else if (grandparent != null)
            return grandparent;
        return null;
    }

    private void PlacePlayerAtPosition(int stageNum){
        if (stageNum == 0)
            stageNum = 1;
        int index = stageNum -1;
        OverworldSpace space = overworldSpaces[index];
        GameObject newSpot = space.playerDestination;
        playerParent.transform.position = newSpot.transform.position;
        currentPlayerSpace = space;
        currentEnemyData = space.battleData.enemyPrefab.GetComponent<EnemyData>();
        PathStep currentPlayerPathStep = currentPlayerSpace.transform.GetChild(2).Find("Overworld Player Space Icon").GetComponent<PathStep>();
        currentPlayerPathStep.gameObject.SetActive(false);
        currentPlayerPathStep.isPlayerAtStep = true;
    }

    public void PlayerArrivedAtDestination(){
        isPlayerMoving = false;
        currentPlayerSpace.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        playerParent.SetSiblingIndex(currentPlayerSpace.originalSiblingIndex + 1);
         Vector3 s = playerParent.localScale;
        s.x = 1;
        playerParent.localScale = s;

        if (currentPlayerSpace.type == OverworldSpace.OverworldSpaceType.Atlas){
            StaticVariables.lastVisitedStage = StaticVariables.GetStage(currentPlayerSpace.worldNumber, 1);
            SceneChanger.GoWorld(StaticVariables.lastVisitedStage.world);
            //StaticVariables.FadeOutThenLoadScene(StaticVariables.lastVisitedStage.worldName);
            return;

            
        //StaticVariables.lastVisitedStage = StaticVariables.GetStage(worldNum, 1);
        //StaticVariables.FadeOutThenLoadScene(StaticVariables.lastVisitedStage.worldName);
        }

        SetLastWorldStageVisited(currentPlayerSpace);
        interactOverlayManager.ShowInteractOverlay();
        if (currentPlayerSpace.type == OverworldSpace.OverworldSpaceType.Cutscene)
            return;    
        EnemyData ed = currentPlayerSpace.battleData.enemyPrefab.GetComponent<EnemyData>();
        interactOverlayManager.DisplayEnemyName(ed);
        interactOverlayManager.ConfigureInfoButton(ed);
        interactOverlayManager.ConfigureReadButton(ed);
        if ((IsCurrentEnemyNewestEnemy()) && (!StaticVariables.hasTalkedToNewestEnemy)){
            if ((currentPlayerSpace.type == OverworldSpace.OverworldSpaceType.Battle) || (currentPlayerSpace.type == OverworldSpace.OverworldSpaceType.Tutorial))
                dialogueManager.Setup(currentEnemyData.overworldDialogueSteps, currentPlayerSpace.battleData);
        }
    }

    private void EndPlayerWalk(){
        playerAnimator.SetTrigger("WalkEnd");
        PlayerArrivedAtDestination();
    }

    private bool IsCurrentEnemyNewestEnemy(){
        if (thisWorldNum == StaticVariables.highestBeatenStage.nextStage.world){
            if (GetStageNumOfSpace(currentPlayerSpace) == StaticVariables.highestBeatenStage.nextStage.stage)
                return true;
        }
        return false;
    }

    public void LoadBattleWithData(OverworldSpace space){
        StaticVariables.battleData = space.battleData;
        SetLastWorldStageVisited(space);
        StaticVariables.FadeOutThenLoadScene(StaticVariables.battleSceneName);
    }

    private void ShowOverworldProgress(int index){
        //create a list of all path steps the player has available, and hide the unavailable spaces
        List<PathStep> availableSteps = new();
        for (int i = 0; i < overworldSpaces.Length; i++){
            bool isUnlocked = (StaticVariables.GetStage(thisWorldNum, i + 1).index <= index);
            overworldSpaces[i].gameObject.SetActive(isUnlocked);
            if (isUnlocked){
                foreach (PathStep ps in overworldSpaces[i].steps)
                    availableSteps.Add(ps);
            }
        }
        foreach (PathStep step in availableSteps){
            if (step.isPlayerAtStep){
                playerParent.gameObject.SetActive(false);
                FadeInPlayer(1f);
            }
            else{
                step.HideStep(0);
                step.ShowStep(1f);
            }
            if (step.isDestination){
                step.overworldSpace.HideEnemy(0);
                step.overworldSpace.FadeInEnemy(1f);
            }
        }
        if (revealLastEnemySlowly)
            StaticVariables.WaitTimeThenCallFunction(1.5f, RevealNextEnemy); //FinishEnteringOverworld will eventually be called by RevealNextEnemy
        else
            StaticVariables.WaitTimeThenCallFunction(1f, sceneChangerVisuals.FinishEnteringOverworld);
    }

    private void ShowAtlasProgress(){
        //only used in the atlas scene to show available worlds
        for (int i = StaticVariables.highestBeatenStage.nextStage.world; i < overworldSpaces.Length; i++)
            overworldSpaces[i].gameObject.SetActive(false);

        //create a list of all path steps the player has available
        List<PathStep> availableSteps = new();
        for (int i = 0; i < StaticVariables.highestBeatenStage.nextStage.world; i++){
            OverworldSpace os = overworldSpaces[i];
            foreach (PathStep ps in os.steps){
                availableSteps.Add(ps);
            }
        }
        foreach (PathStep step in availableSteps){
            if (step.isPlayerAtStep){
                playerParent.gameObject.SetActive(false);
                FadeInPlayer(1f);
            }
            else{
                step.HideStep(0);
                step.ShowStep(1f);
            }
        }
    }

    private void FadeInPlayer(float duration){
        playerParent.gameObject.SetActive(true);
        Image im = playerAnimator.GetComponent<Image>();
        Color c1 = im.color;
        Color c2 = im.color;
        c1.a = 0;
        c2.a = 1;
        im.color = c1;
        im.DOColor(c2, duration);
    }

    public void FadeOutPlayer(float duration){
        Image im = playerAnimator.GetComponent<Image>();
        Color c1 = im.color;
        Color c2 = im.color;
        c1.a = 0;
        c2.a = 1;
        im.color = c2;
        im.DOColor(c1, duration);
    }

    private void RevealNextEnemy(){
        OverworldSpace nextSpace = GetFirstLockedEnemySpace();
        if (nextSpace != null){
            nextSpace.gameObject.SetActive(true);
            nextSpace.RevealStepsSlowly();
        }
    }

    private OverworldSpace GetFirstLockedEnemySpace(){
        for (int i = 0; i < overworldSpaces.Length; i++){
            OverworldSpace space = overworldSpaces[i];
            if (!space.gameObject.activeSelf)
                return space;
        }
        return null;
    }

    private void SetLastWorldStageVisited(OverworldSpace space){
        StaticVariables.lastVisitedStage = StaticVariables.GetStage(thisWorldNum, GetStageNumOfSpace(space));
    }

    private int GetStageNumOfSpace(OverworldSpace space){
        int i = 0;
        foreach (OverworldSpace s in overworldSpaces){
            i++;
            if (s == space)
                return i;
        }
        return -1;
    }

    public void StartBattle(){
        if (currentPlayerSpace.type == OverworldSpace.OverworldSpaceType.Battle){
            if (currentPlayerSpace.name == "Enemy 3 - No Powerups")
                StaticVariables.powerupsPerPuzzle = 0;
            LoadBattleWithData(currentPlayerSpace);
        }
        else if (currentPlayerSpace.type == OverworldSpace.OverworldSpaceType.Tutorial){
            StaticVariables.battleData = currentPlayerSpace.battleData;
            SetLastWorldStageVisited(currentPlayerSpace);
            StaticVariables.FadeOutThenLoadScene("Tutorial");
        }
    }

    public void StartCutscene(){
        if (currentPlayerSpace.type == OverworldSpace.OverworldSpaceType.Cutscene){
            StaticVariables.cutsceneID = currentPlayerSpace.cutsceneID;
            SetLastWorldStageVisited(currentPlayerSpace);
            StaticVariables.FadeOutThenLoadScene("Cutscene");
        }
    }

    public void BackToMap(){
        StaticVariables.FadeOutThenLoadScene(StaticVariables.mapName);
    }

    public void BackToHomepage(){
        StaticVariables.FadeOutThenLoadScene(StaticVariables.mainMenuName);
    }

    public void FinishedTalking(){
        if (IsCurrentEnemyNewestEnemy()) {
            StaticVariables.hasTalkedToNewestEnemy = true;
            SaveSystem.SaveGame();
        }
    }

    public void HideSceneHeader(float duration){
        sceneHeader.DOAnchorPos((sceneHeader.anchoredPosition + new Vector2(0, 400)), duration).OnComplete(DisableSceneHeader);
    }

    public void ShowSceneHeader(float duration){
        sceneHeader.gameObject.SetActive(true);
        sceneHeader.DOAnchorPos((sceneHeader.anchoredPosition + new Vector2(0, -400)), duration);
    }

    private void DisableSceneHeader(){
        sceneHeader.gameObject.SetActive(false);
    }
}