using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour {

    [HideInInspector]
    public AttackData inProgressWord;
    [HideInInspector]
    public List<AttackData> attackQueue = new();
    [HideInInspector]
    public List<LetterSpace> letterSpacesForWord = new List<LetterSpace>(){};
    private LetterSpace lastLetterSpace = null;
    private LetterSpace secondToLastLetterSpace = null;
    [HideInInspector]
    public int enemyHealth = 0;
    [HideInInspector]
    public int hordeStartingHealth = 0;
    [HideInInspector]
    public int startingHordeEnemyCount = 0;
    [HideInInspector]
    public int currentHordeEnemyCount = 0;
    [HideInInspector]
    public EnemyData firstEnemyInHorde;
    [HideInInspector]
    public int playerHealth = 0;
    [HideInInspector]
    public int countdownToRefresh;
    public enum PowerupTypes{None, Water, Fire, Heal, Dark, Earth, Lightning, Pebble, Sword};
    private bool hasSwipedOffALetter = false;
    private bool stopNextAttack = false;
    [HideInInspector]
    public EnemyData enemyData;
    [HideInInspector]
    public EnemyAttackAnimatorFunctions enemyAttackAnimatorFunctions;
    [HideInInspector]
    public List<EnemyAttackAnimatorFunctions> enemyHordeAttackAnimatorFunctions;
    [HideInInspector]
    public bool isWaterInPuzzleArea = false;
    private bool isGamePaused = false;
    private int enemyAttackIndex = 0;
    [HideInInspector]
    public int copycatBuildup = 0;
    private int maxBurnedLetters = 20;
    [HideInInspector]
    public int maxInfectedLetters = 20;
    private bool hasEarthBuff = false;
    [HideInInspector]
    public int[] necromancyHandsHeights = new int[] { 0, 0, 0, 0, 0 };
    private bool isEnemyLightningCharging = false;
    //private bool hasPlayerStoppedEnemyLightning = false;
    private int enemyLightningChargeColumn = 0;

    [Header("Game Variables")]
    private readonly int startingPlayerHealth = 100;
    public int maxHealth = 999; //for display purposes
    public int minCheckingWordLength = 3;
    public int maxPuzzleCountdown = 3;
    public int selfDamageFromDarkAttack = 5;
    public float lightningStunDuration = 15f;
    public float darkPowerupDamageMultiplier = 2.5f;
    public float swordPowerupDamageMultiplier = 2f;
    public float swordPowerupDamageMultiplierVsDragons = 5f;
    public BattleData defaultBattleData;
    public int selfDamageFromBurnedLetters = 2;
    public int selfDamageFromInfectedLetters = 3;
    public int maxCopycatStacks = 5;

    [Header("Scripts")]
    public UIManager uiManager;
    public PuzzleGenerator puzzleGenerator;
    public PlayerAnimatorFunctions playerAnimatorFunctions;
    private float enemyDifficultyHealthModifier = 1f;
    //public GeneralSceneManager generalSceneManager;

    public virtual void Start() {
        GetComponent<GeneralSceneManager>().Setup();
        //generalSceneManager.Setup();
        countdownToRefresh = maxPuzzleCountdown;
        if (StaticVariables.battleData == null)
            StaticVariables.battleData = defaultBattleData;
        uiManager.AddEnemyToScene(StaticVariables.battleData.enemyPrefab);
        if (enemyData.isHorde) {
            startingHordeEnemyCount = enemyHordeAttackAnimatorFunctions.Count;
            currentHordeEnemyCount = startingHordeEnemyCount;
            firstEnemyInHorde = enemyHordeAttackAnimatorFunctions[0].GetComponent<EnemyData>();
            hordeStartingHealth = firstEnemyInHorde.startingHealth * startingHordeEnemyCount;
            enemyHealth = hordeStartingHealth;
        }
        else
            enemyHealth = enemyData.startingHealth;
        switch (StaticVariables.difficultyMode) {
            case StaticVariables.DifficultyMode.Story:
            //    enemyDifficultyHealthModifier = -1;
                enemyHealth = 1;
                break;
            case StaticVariables.DifficultyMode.Puzzle:
                enemyDifficultyHealthModifier = 5;
                //enemyHealth *= 5;
                break;
            case StaticVariables.DifficultyMode.Easy:
                enemyDifficultyHealthModifier = 0.7f;
                //float a = (enemyHealth * 0.7f);
                //if (a < 1)
                //    a = 1;
                //enemyHealth = (int)a;
                break;
            case StaticVariables.DifficultyMode.Hard:
                enemyDifficultyHealthModifier = 1.3f;
                //float b = enemyHealth * 1.3f;
                //enemyHealth = (int)b;
                break;
        }
        enemyHealth = ApplyEnemyDifficultyHealthModifier(enemyHealth);
        playerHealth = startingPlayerHealth;
        uiManager.ApplyBackground(StaticVariables.battleData.backgroundPrefab);

        inProgressWord = new(this);
        uiManager.SetStartingValues();
        uiManager.DisplayHealths(playerHealth, enemyHealth);
        uiManager.DisplayWord(inProgressWord, countdownToRefresh);
        StaticVariables.FadeIntoScene();
        StaticVariables.WaitTimeThenCallFunction(StaticVariables.sceneFadeDuration, QueueEnemyAttack);
        puzzleGenerator.Setup();
        //generalSceneManager.FadeIn();
    }

    public void Update() {
        CheckIfQueuedAttackCanBeUsed();
    }

    private void CheckIfQueuedAttackCanBeUsed() {
        if (attackQueue.Count == 0)
            return;
        if (playerAnimatorFunctions.attackInProgress != null)
            return;
        if (enemyHealth <= 0)
                return;
        if (playerHealth <= 0)
            return;
        if (enemyData.isHorde) {
            if (!uiManager.enemyHordeAnimators[0].GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                return;
            }
        else if (!uiManager.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            return;
        if (!uiManager.playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            return;
        AttackWithFirstWordInQueue();
    }
    
    private int ApplyEnemyDifficultyHealthModifier(int originalEnemyHealth){
        float newAmt = (originalEnemyHealth * enemyDifficultyHealthModifier);
        if (newAmt < 1)
            newAmt = 1;
        return (int)newAmt;
    }

    public virtual void DamageEnemyHealth(int amount){
        enemyHealth -= amount;
        if (enemyHealth < 0)
            enemyHealth = 0;
        CalculateHordeEnemiesLeft();
        uiManager.ShowEnemyTakingDamage(amount, enemyHealth > 0);
        uiManager.DisplayHealths(playerHealth, enemyHealth);
        if (enemyHealth <= 0){
            stopNextAttack = true;
            uiManager.PauseEnemyAttackBar();
            uiManager.PauseWaterDrain();
            uiManager.FadeOutWaterOverlay();
            uiManager.RemoveRocksFromEdgeOfPuzzle();
            uiManager.PauseBoulderDebuff();
            uiManager.PausePageTurn();
            ClearWord(false);
        }
    }

    private void CalculateHordeEnemiesLeft(){
        if (enemyHealth == 0){
            currentHordeEnemyCount = 0;
            return;
        }
        currentHordeEnemyCount = 1;
        for (int i = 1; i < startingHordeEnemyCount; i++){
            if (enemyHealth > (i * ApplyEnemyDifficultyHealthModifier(firstEnemyInHorde.startingHealth)))
                currentHordeEnemyCount ++;
        }
    }

    public virtual void ApplyEnemyAttackDamage(int amount) {
        switch (StaticVariables.difficultyMode) {
            case StaticVariables.DifficultyMode.Story:
                DamagePlayerHealth(1);
                break;
            case StaticVariables.DifficultyMode.Puzzle:
                DamagePlayerHealth(0);
                break;
            case StaticVariables.DifficultyMode.Easy:
                float a = (amount * 0.7f);
                if ((a < 1) && (a > 0))
                    a = 1;
                DamagePlayerHealth((int)a);
                break;
            case StaticVariables.DifficultyMode.Hard:
                float b = amount * 1.3f;
                DamagePlayerHealth((int)b);
                break;
            default:
                DamagePlayerHealth(amount);
                break;
        }
    }

    public virtual void DamagePlayerHealth(int amount) {
        playerHealth -= amount;
        if (playerHealth < 0)
            playerHealth = 0;
        uiManager.ShowPlayerTakingDamage(amount, playerHealth > 0);
        uiManager.DisplayHealths(playerHealth, enemyHealth);
        if (playerHealth == 0) {
            uiManager.PauseEnemyAttackBar();
            uiManager.PauseWaterDrain();
            uiManager.FadeOutWaterOverlay();
            uiManager.RemoveRocksFromEdgeOfPuzzle();
            uiManager.PauseBoulderDebuff();
            uiManager.PausePageTurn();
            ClearWord(false);
        }
    }

    public void PauseEverything(){
        isGamePaused = true;
        uiManager.PauseEnemyAttackBar();
        uiManager.PauseWaterDrain();
        uiManager.PauseBoulderDebuff();
        uiManager.SetAllAnimationStates(false);
    }

    public void ResumeEverything(){
        isGamePaused = false;
        uiManager.ResumeEnemyAttackBar();
        uiManager.ResumeWaterDrain();
        uiManager.ResumeBoulderDebuff();
        uiManager.SetAllAnimationStates(true);
    }

    private void StartPlayingPlayerAttackAnimation(PowerupTypes type) {
        uiManager.PauseEnemyAttackBar();
        if (type == PowerupTypes.Heal)
            uiManager.StartPlayerHealAnimation();
        else if (type == PowerupTypes.Sword)
            uiManager.StartPlayerSwordAnimation();
        else
            uiManager.StartPlayerCastAnimation();
    }

    public void DoEnemyAttackEffect(EnemyAttackAnimatorFunctions enemy){
        EnemyAttack ea = null;
        if (enemyData.isHorde){
            if (enemy.data == firstEnemyInHorde)
                ea = firstEnemyInHorde.attackOrder.Value[enemyAttackIndex];
        }
        else
            ea = enemyData.attackOrder.Value[enemyAttackIndex];

        if (ea == null)
            return;
        if (enemyData.isHorde)
            ApplyEnemyAttackDamage(ea.attackDamage * currentHordeEnemyCount);
        else if (enemyData.isCopycat) {
            int newDamage = ea.attackDamage * copycatBuildup; //at 0 stacks, copycat does 0 damage
            if (newDamage < 0)
                newDamage = 0;
            ApplyEnemyAttackDamage(newDamage);
            print("copycat is attacking! base damage is " + ea.attackDamage + ", buildup is " + copycatBuildup + ", so total damage is " + newDamage);
        }
        else if (ea.isSpecial && ea.specialType == EnemyAttack.EnemyAttackTypes.HealsSelf) {
            int amount = playerHealth;
            ApplyEnemyAttackDamage(ea.attackDamage);
            amount -= playerHealth;
            if (amount > 0)
                HealEnemyHealth(amount);
        }
        else if (ea.isSpecial && ea.specialType == EnemyAttack.EnemyAttackTypes.Lightning1)
            DoEnemyLightningCharge();
        //else if (ea.isSpecial && ea.specialType == EnemyAttack.EnemyAttackTypes.Lightning2)
        //    DoEnemyLightningStrike(ea.attackDamage);
        else if (ea.isSpecial && ea.specialType == EnemyAttack.EnemyAttackTypes.Necromancy){
            ApplyEnemyAttackDamage(0);
            RaiseNecromancyHands();
        }

        else
            ApplyEnemyAttackDamage(ea.attackDamage);

        if (ea.isSpecial){
            switch (ea.specialType){
                case EnemyAttack.EnemyAttackTypes.ThrowRocks:
                    uiManager.CoverPageWithBoulders();
                    break;
                case EnemyAttack.EnemyAttackTypes.BurnLetters:
                    BurnRandomLetters(3);
                    break;
                case EnemyAttack.EnemyAttackTypes.Infect:
                    InfectRandomLetters(1);
                    break;
            }
        }
        IncrementAttackIndex();
    }

    private void IncrementAttackIndex(){
        enemyAttackIndex = GetNextAttackIndex();
    }

    private int GetNextAttackIndex(){
        int val = enemyAttackIndex;
        val ++;
        int attackCount;
        if (enemyData.isHorde)
            attackCount = firstEnemyInHorde.attackOrder.Value.Length;
        else
            attackCount = enemyData.attackOrder.Value.Length;
        if (val >= attackCount)
            val = 0;
        return val;
    }

    public void AttackHitsEnemy(AttackData attackData){
        if (enemyData.isCopycat){
            if ((copycatBuildup < maxCopycatStacks) && (attackData.type != PowerupTypes.Heal)){
                copycatBuildup ++;
                uiManager.ShowCopycatBuildup();
            }
        }
        switch (attackData.type){
            case PowerupTypes.Water:
                if (enemyData.waterHealsIt)
                    HealEnemyHealth(attackData.strength);
                else
                    DamageEnemyHealth(attackData.strength);
                ApplyBuffForWaterAttack();
                break;
            case PowerupTypes.Fire:
                DamageEnemyHealth(attackData.strength);
                break;
            case PowerupTypes.Lightning:
                DamageEnemyHealth(attackData.strength);
                ApplyEnemyAttackTimeDebuffFromLightning();
                break;
            case PowerupTypes.Dark:
                DoDarkAttack(attackData.strength);
                break;
            case PowerupTypes.Earth:
                if (enemyData.earthHealsIt)
                    HealEnemyHealth(attackData.strength);
                else
                    DamageEnemyHealth(attackData.strength);
                ApplyEarthBuff();
                break;
            case PowerupTypes.Sword:
                DoSwordAttack(attackData.strength);
                break;
            default: //poweruptypes.heal and none
                DamageEnemyHealth(attackData.strength);
                break;
        }
    }

    private void ApplyEarthBuff() {
        if ((enemyHealth <= 0) || (playerHealth <= 0))
            return;
        if (hasEarthBuff)
            return;
        hasEarthBuff = true;
        inProgressWord.AddEarthBuff();
        uiManager.AddRocksToEdgeOfPuzzle();
        UpdateSubmitVisuals();
    }

    private void RemoveEarthBuff() {
        if (!hasEarthBuff)
            return;
        hasEarthBuff = false;
        inProgressWord.RemoveEarthBuff();
        uiManager.RemoveRocksFromEdgeOfPuzzle();
    }

    private void DoSwordAttack(int strength){
        float mult = swordPowerupDamageMultiplier;
        if (enemyData.isDraconic)
            mult = swordPowerupDamageMultiplierVsDragons;
        int enemyDamage = (int)(strength * mult);
        DamageEnemyHealth(enemyDamage);
    }

    public virtual void WaterDrainComplete(){
        isWaterInPuzzleArea = false;
        inProgressWord.RemoveWaterBuff();
        UpdateSubmitVisuals();
    }

    private void ApplyBuffForWaterAttack(){
        if ((enemyHealth <= 0) || (playerHealth <= 0))
            return;
        isWaterInPuzzleArea = true;
        uiManager.FillPuzzleAreaWithWater(StaticVariables.waterFloodDuration);
        inProgressWord.AddWaterBuff();
        UpdateSubmitVisuals();
    }

    private void ApplyEnemyAttackTimeDebuffFromLightning(){
        uiManager.StopEnemyAttackTimer();
        float stunTime = lightningStunDuration;
        if (enemyData.isHorde)
            stunTime /= currentHordeEnemyCount;
        uiManager.ActivateEnemyStunBar(stunTime);
    }

    public void DamagePlayerForDarkAttack(){
        //doesn't play animation for self damage
        //self damage can't kill you
        int prevHP = playerHealth;
        playerHealth -= selfDamageFromDarkAttack;
        if (playerHealth < 1)
            playerHealth = 1;
        uiManager.ShowPlayerTakingDamage((prevHP - playerHealth), playerHealth > 0, false);
        uiManager.DisplayHealths(playerHealth, enemyHealth);
    }

    private void DoDarkAttack(int strength){
        int enemyDamage = (int)(strength * darkPowerupDamageMultiplier);
        DamageEnemyHealth(enemyDamage);
    }

    public virtual void ApplyHealToSelf(AttackData attackData){
        int healAmount = attackData.strength * StaticVariables.healMultiplier;
        HealPlayerHealth(healAmount);
        ClearDebuffsViaHealing();
    }
        
    public void BurnRandomLetters(int amount){
        for (int i = 0; i < amount; i++){
            if (puzzleGenerator.burnedLetters.Count >= maxBurnedLetters)
                return;
            LetterSpace toBurn = puzzleGenerator.PickRandomSpaceWithoutModifier();
            if(toBurn != null)
                toBurn.ApplyBurn();
        }
    }
        
    public void InfectRandomLetters(int amount){
        for (int i = 0; i < amount; i++){
            if (puzzleGenerator.infectedLetters.Count >= maxInfectedLetters)
                return;
            LetterSpace toInfect = puzzleGenerator.PickRandomSpaceWithoutModifier(false);
            if(toInfect != null)
                toInfect.ApplyInfection();
        }
    }

    private void DoEnemyLightningCharge(){
        enemyLightningChargeColumn = StaticVariables.rand.Next(1, 6);
        puzzleGenerator.letterSpaces[0,enemyLightningChargeColumn-1].ShowCharged();
        StaticVariables.WaitTimeThenCallFunction(0.1f,puzzleGenerator.letterSpaces[1,enemyLightningChargeColumn-1].ShowCharged);
        StaticVariables.WaitTimeThenCallFunction(0.2f,puzzleGenerator.letterSpaces[2,enemyLightningChargeColumn-1].ShowCharged);
        StaticVariables.WaitTimeThenCallFunction(0.3f,puzzleGenerator.letterSpaces[3,enemyLightningChargeColumn-1].ShowCharged);
        StaticVariables.WaitTimeThenCallFunction(0.4f,puzzleGenerator.letterSpaces[4,enemyLightningChargeColumn-1].ShowCharged);
        StaticVariables.WaitTimeThenCallFunction(0.5f,puzzleGenerator.letterSpaces[5,enemyLightningChargeColumn-1].ShowCharged);
        StaticVariables.WaitTimeThenCallFunction(0.6f,puzzleGenerator.letterSpaces[6,enemyLightningChargeColumn-1].ShowCharged);
        isEnemyLightningCharging = true;
    }

    private void DoEnemyLightningStrike(int damage){
            ApplyEnemyAttackDamage(damage);
    }

    /*
    private void DoEnemyLightningAttack(int damage){
        if (isEnemyLightningCharging){
            ApplyEnemyAttackDamage(damage);
            ClearLightningCharging();
        }
        else{
            enemyLightningChargeColumn = StaticVariables.rand.Next(1, 6);
            puzzleGenerator.letterSpaces[0,enemyLightningChargeColumn-1].ShowCharged();
            StaticVariables.WaitTimeThenCallFunction(0.1f,puzzleGenerator.letterSpaces[1,enemyLightningChargeColumn-1].ShowCharged);
            StaticVariables.WaitTimeThenCallFunction(0.2f,puzzleGenerator.letterSpaces[2,enemyLightningChargeColumn-1].ShowCharged);
            StaticVariables.WaitTimeThenCallFunction(0.3f,puzzleGenerator.letterSpaces[3,enemyLightningChargeColumn-1].ShowCharged);
            StaticVariables.WaitTimeThenCallFunction(0.4f,puzzleGenerator.letterSpaces[4,enemyLightningChargeColumn-1].ShowCharged);
            StaticVariables.WaitTimeThenCallFunction(0.5f,puzzleGenerator.letterSpaces[5,enemyLightningChargeColumn-1].ShowCharged);
            StaticVariables.WaitTimeThenCallFunction(0.6f,puzzleGenerator.letterSpaces[6,enemyLightningChargeColumn-1].ShowCharged);
            isEnemyLightningCharging = true;
        }
    }
    */
    
    private void RaiseNecromancyHands(){
        //first, decide which heights to raise the new hands to
        for (int i = 0; i < necromancyHandsHeights.Length; i++){
            if (necromancyHandsHeights[i] == 0){ //if at 0 height, raise by 1 or 2
                if (StaticVariables.rand.Next(100) > 50)
                    necromancyHandsHeights[i] ++;
            }
            necromancyHandsHeights[i]++;
            if (necromancyHandsHeights[i] >= 7) //max height is 6
                necromancyHandsHeights[i] = 6;
        }
        uiManager.ShowNecromancyHandHeights();
    }
    
    public void ClearRandomBurnedLetters(int amount){
        for (int i = 0; i < amount; i++){
            LetterSpace toClear = puzzleGenerator.PickRandomBurnedSpace();
            if (toClear != null)
                toClear.RemoveBurn();
        }
    }
    
    public void ClearRandomInfectedLetters(int amount){
        for (int i = 0; i < amount; i++){
            LetterSpace toClear = puzzleGenerator.PickRandomInfectedSpace();
            if (toClear != null)
                toClear.RemoveInfection(true);
        }
    }

    private void ClearAllBurnedLetters() {
        List<LetterSpace> letters = new(puzzleGenerator.burnedLetters); //we dont want to modify the list as we are iterating on it, so we make a copy
        foreach (LetterSpace ls in letters)
            ls.RemoveBurn();
    }

    private void ClearAllInfectedLetters() {
        List<LetterSpace> letters = new(puzzleGenerator.infectedLetters); //we dont want to modify the list as we are iterating on it, so we make a copy
        foreach (LetterSpace ls in letters)
            ls.RemoveInfection(false);
    }

    public void ClearDebuffsViaHealing(){
        if (uiManager.shownBoulders != null)
            uiManager.ClearBouldersOnPage();
        if (enemyData.isCopycat) {
            copycatBuildup -= 2;
            if (copycatBuildup < 0)
                copycatBuildup = 0;
            uiManager.ShowCopycatBuildup();
        }
        if (enemyData.weirdAnimalDisease)
            ClearRandomInfectedLetters(1);
    }
    
    public void HideAllDebuffVisuals(){
        if (uiManager.shownBoulders != null)
            uiManager.ClearBouldersOnPage();
        if (puzzleGenerator.burnedLetters.Count > 0)
            ClearAllBurnedLetters();
        if (puzzleGenerator.infectedLetters.Count > 0)
            ClearAllInfectedLetters();
    }

    private void HealPlayerHealth(int amount) {
        playerHealth += amount;
        if (playerHealth > maxHealth)
            playerHealth = maxHealth;
        uiManager.ShowPlayerGettingHealed(amount);
        uiManager.DisplayHealths(playerHealth, enemyHealth);
    }

    private void HealEnemyHealth(int amount){
        enemyHealth += amount;
        if (enemyHealth > maxHealth)
            enemyHealth = maxHealth;
        uiManager.ShowEnemyGettingHealed(amount);
        uiManager.DisplayHealths(playerHealth, enemyHealth);
    }

    public void DecrementRefreshPuzzleCountdown(){
        countdownToRefresh --;
        if (countdownToRefresh < 0)
            countdownToRefresh = 0;
    }

    public virtual void AddLetter(LetterSpace ls) {
        letterSpacesForWord.Add(ls);
        inProgressWord.AddLetter(ls.letter);
        if (lastLetterSpace != null) {
            lastLetterSpace.nextLetterSpace = ls;
            ls.previousLetterSpace = lastLetterSpace;
        }
        SetLastTwoLetterSpaces();
        UpdateSubmitVisuals();
        if (ls.isBurned)
            DamagePlayerHealth(selfDamageFromBurnedLetters);
        //if (ls.isInfected)
        //    DamagePlayerHealth(selfDamageFromInfectedLetters);

    }

    public void RemoveLetter(LetterSpace ls){
        //assumes the last letter in the list is the provided letter
        letterSpacesForWord.Remove(ls);
        inProgressWord.RemoveLastLetter();
        ls.ShowAsNotPartOfWord();
        if (secondToLastLetterSpace != null){
            secondToLastLetterSpace.nextLetterSpace = null;
            lastLetterSpace.previousLetterSpace = null;
        }
        SetLastTwoLetterSpaces();
        UpdateSubmitVisuals();
    }

    public virtual void UpdateSubmitVisuals(){
        uiManager.powerupType = inProgressWord.type;
        uiManager.UpdateVisualsForLettersInWord(letterSpacesForWord);
        uiManager.DisplayWord(inProgressWord, countdownToRefresh);
        uiManager.ShowPowerupBackground(inProgressWord.type);
    }

    private void SetLastTwoLetterSpaces(){
        lastLetterSpace = null;
        secondToLastLetterSpace = null;
        if (letterSpacesForWord.Count > 0)
            lastLetterSpace = letterSpacesForWord[letterSpacesForWord.Count - 1];
        if (letterSpacesForWord.Count > 1)
            secondToLastLetterSpace = letterSpacesForWord[letterSpacesForWord.Count - 2];
    }

    public virtual bool CanAddLetter(LetterSpace letterSpace){
        if (letterSpace.letter == '=')
            return false;
        if (letterSpace.letter == '-')
            return false;
        if (letterSpace.letter == ' ')
            return false;
        if ((playerHealth == 0) || (enemyHealth == 0) || (isGamePaused))
            return false;
        if (letterSpace.hasBeenUsedInAWordAlready)
            return false;
        if (letterSpacesForWord.Contains(letterSpace))
            return false;
        if ((uiManager.shownBoulders != null) && (uiManager.shownBoulders.coveredLetters.Contains(letterSpace)))
            return false;
        if (IsLetterCoveredByHands(letterSpace))
            return false;
        if (letterSpacesForWord.Count == 0)
            return true;
        if (letterSpacesForWord.Count >= 15) //hard limit of 15 letters per word, for UI reasons
            return false;
        if (letterSpace.IsAdjacentToLetterSpace(lastLetterSpace))
            return true;
        return false;
    }
    
    private bool IsLetterCoveredByHands(LetterSpace letterSpace){
        if (!enemyData.usesNecromancy)
            return false;
        int row = (int)letterSpace.position[0];
        int height = 7 - row;
        int col = (int)letterSpace.position[1];
        if (necromancyHandsHeights[col] > height)
            return true;
        return false;
    }

    public virtual bool CanRemoveLetter(LetterSpace letterSpace){
        if (letterSpacesForWord.Count == 0)
            return false;
        return (lastLetterSpace == letterSpace);
    }

    public void ClearWord(bool markLettersAsUsed, bool applyChar = false){
        foreach (LetterSpace ls in letterSpacesForWord){
            ls.previousLetterSpace = null;
            ls.nextLetterSpace = null;
            if (markLettersAsUsed)
                ls.hasBeenUsedInAWordAlready = true;
            if (applyChar)
                ls.ApplyChar();
            ls.ShowAsNotPartOfWord();
        }
        letterSpacesForWord = new List<LetterSpace>();
        SetLastTwoLetterSpaces();
        inProgressWord.UpdateWord("");
        UpdateSubmitVisuals();
    }

    public void ProcessFingerDown(){
        SetLetterSpaceActiveBeforeFingerDown();
        hasSwipedOffALetter = false;
    }

    public void ProcessBeginSwipe(){
    }

    public void ProcessBeginSwipeOnLetterSpace(LetterSpace space){
        if (CanAddLetter(space))
            AddLetter(space);
    }

    public void ProcessSwipeOffLetterSpace(LetterSpace space){      
        hasSwipedOffALetter = true;  
        if (CanAddLetter(space))
            AddLetter(space);
    }

    public void ProcessSwipeOnLetterSpace(LetterSpace space){
        if (CanAddLetter(space))
            AddLetter(space);
        else if (space == secondToLastLetterSpace){
            if (CanRemoveLetter(lastLetterSpace))
                RemoveLetter(lastLetterSpace);
        }
    }

    public void ProcessFingerDownOnLetter(LetterSpace space){
        if (CanAddLetter(space))
            AddLetter(space);
    }

    public void ProcessSwipeReleaseOnLetterSpace(LetterSpace space){
        if ((space.wasActiveBeforeFingerDown) && (CanRemoveLetter(space)) && (!hasSwipedOffALetter))
            RemoveLetter(space);
        else if ((inProgressWord.word.Length == 1) && (hasSwipedOffALetter) && (CanRemoveLetter(space)))
            RemoveLetter(space);
    }

    public void ProcessTapReleaseOnLetterSpace(LetterSpace space){
        if ((space.wasActiveBeforeFingerDown) && (CanRemoveLetter(space)))
            RemoveLetter(space);
    }

    public virtual void ProcessFingerRelease(){
        if (inProgressWord.isValidWord)
            SubmitWord();
        else
            ClearWord(false);
    }

    public void SubmitWord(){
        if ((playerHealth == 0) || (enemyHealth == 0) || (isGamePaused))
            return;
        if (inProgressWord.isValidWord) {
            bool applyChar = (inProgressWord.type == PowerupTypes.Fire);
            attackQueue.Add(inProgressWord);
            inProgressWord = new(this);
            DecrementRefreshPuzzleCountdown();
            RemoveInfectionsFromCurrentWord();
            LowerNecromancyHandsFromCurrentWord();
            DischargeLightningFromCurrentWord();
            ClearWord(true, applyChar);
            RemoveEarthBuff();
            if (isWaterInPuzzleArea && enemyData.canBurn)
                ClearRandomBurnedLetters(2);
        }
    }

    private void RemoveInfectionsFromCurrentWord() {
        foreach (LetterSpace letter in letterSpacesForWord) {
            if (letter.isInfected)
                letter.RemoveInfection(true);
        }
    }
    
    private void LowerNecromancyHandsFromCurrentWord(){
        if (!enemyData.usesNecromancy)
            return;
        foreach (LetterSpace ls in letterSpacesForWord) {
            int row = (int)ls.position[0];
            int height = 7 - row;
            int col = (int)ls.position[1];
            if (necromancyHandsHeights[col] == height)
                necromancyHandsHeights[col]--;
        }
        uiManager.ShowNecromancyHandHeights();
    }

    private void DischargeLightningFromCurrentWord(){
        if (!enemyData.usesLightning)
            return;
        if (!isEnemyLightningCharging)
            return;
        //if (hasPlayerStoppedEnemyLightning)
        //    return;
        foreach (LetterSpace ls in letterSpacesForWord) {
            int col = (int)ls.position[1];
            if (col == enemyLightningChargeColumn- 1){
                ClearLightningCharging();
                uiManager.CancelEnemyAttackAndQueueNext();
                return;
            }
        }
    }

    public void ClearLightningCharging(){
        foreach (LetterSpace ls in puzzleGenerator.letterSpaces)
            ls.DissipateCharge();
        isEnemyLightningCharging = false;
    }

    public void QueueAttackAfterCancel(){
        IncrementAttackIndex();
        QueueEnemyAttack();
    }
    
    private void DamagePlayerFromInfectedLetters(){
        int damage = puzzleGenerator.infectedLetters.Count * selfDamageFromInfectedLetters;
        if (damage > 0)
            DamagePlayerHealth(damage);
    }

    //private void RemoveInfectionsAndDamagePlayer() {
    //    int damage = 0;
    //    foreach (LetterSpace letter in letterSpacesForWord) {
    //        if (letter.isInfected) {
    //            letter.RemoveInfection(true);
    //            damage += selfDamageFromInfectedLetters;
    //        }
    //    }
    //    if (damage > 0)
    //        DamagePlayerHealth(selfDamageFromInfectedLetters, false);
    //}
    
    private void AttackWithFirstWordInQueue() {
        if (attackQueue.Count == 0)
            return;
        AttackData attackData = attackQueue[0];
        attackQueue.RemoveAt(0);
        playerAnimatorFunctions.attackInProgress = attackData;
        StartPlayingPlayerAttackAnimation(attackData.type);
    }

    public virtual void PressWordArea() {
        if ((inProgressWord.word.Length == 0) && (countdownToRefresh == 0)) {
            DamagePlayerFromInfectedLetters();
            puzzleGenerator.GenerateNewPuzzle();
            countdownToRefresh = maxPuzzleCountdown;
            ClearWord(true);
            uiManager.ShowPageTurn();
        }
    }

    private void SetLetterSpaceActiveBeforeFingerDown(){
        foreach (LetterSpace ls in puzzleGenerator.letterSpaces){
            ls.wasActiveBeforeFingerDown = false;
            if (letterSpacesForWord.Contains(ls))
                ls.wasActiveBeforeFingerDown = true;
        }
    }

    public virtual void QueueEnemyAttack(){
        if (playerHealth <= 0)
            return;
        if (enemyHealth <= 0)
            return;
        EnemyAttack ea;
        if (enemyData.isHorde)
            ea = firstEnemyInHorde.attackOrder.Value[enemyAttackIndex];
        else
            ea = enemyData.attackOrder.Value[enemyAttackIndex];
        if (ea.isSpecial)
            uiManager.StartEnemyAttackTimer(ea.attackSpeed, ea.specialColor, ea.specialType);
        else
            uiManager.StartEnemyAttackTimer(ea.attackSpeed);
    }

    public void PlayerAttackAnimationFinished(GameObject attackObject) {
        //destroys the gameobject, then resumes the enemy attack timer
        Destroy(attackObject);
        playerAnimatorFunctions.attackInProgress = null;
        if (enemyHealth <= 0)
            return;
        if (attackQueue.Count > 0)
            return;
        uiManager.ResumeEnemyAttackBar();
    }

    public virtual void TriggerEnemyAttack(){
        if (!stopNextAttack){
            DecrementRefreshPuzzleCountdown();
            UpdateSubmitVisuals();
            uiManager.StartEnemyAttackAnimation(enemyData.attackOrder.Value[enemyAttackIndex].specialType);
        }
    }

    public void EnemyStunWearsOff(){
        uiManager.DeactivateEnemyStunBar();
        if (!stopNextAttack){
            QueueEnemyAttack();
        }
    }

    public virtual void EnemyDeathAnimationFinished(){
        if (enemyData.isHorde){
            if (enemyHealth == 0)
                uiManager.ShowVictoryPage();
            else{
                for (int i = currentHordeEnemyCount; i < startingHordeEnemyCount; i++){
                    if (enemyHordeAttackAnimatorFunctions[i].gameObject.activeSelf){
                        enemyHordeAttackAnimatorFunctions[i].gameObject.SetActive(false);
                    }
                }
            }
        }
        else{
            uiManager.ShowVictoryPage();
        }
    }

    public virtual void TurnPageEnded(){

    }

    public virtual void WaterReachedTopOfPage(){

    }

    public int GetNumberOfEnemies() {
        if (enemyData.isHorde)
            return currentHordeEnemyCount;
        return 1;
    }

}
public class AttackData {
    public BattleManager.PowerupTypes type = BattleManager.PowerupTypes.None;
    public string word = "";
    public bool hasEarthBuff = false;
    public bool hasWaterBuff = false;
    public bool isValidWord = false;
    public int strength = 0;
    private readonly BattleManager battleManager;

    public AttackData(BattleManager battleManager) {
        this.battleManager = battleManager;
        if (battleManager.isWaterInPuzzleArea)
            AddWaterBuff();
    }

    public void AddLetter(char letter) {
        UpdateWord(word + letter);
    }

    public void RemoveLastLetter() {
        if (word.Length < 2)
            UpdateWord("");
        else
            UpdateWord(word[..^1]);

    }

    public void UpdateWord(string newWord) {
        word = newWord;
        SetIsValidWord();
        SetPowerupType();
        SetWordStrength();
        //PrintAttackData();
    }

    public void AddWaterBuff() {
        hasWaterBuff = true;
        SetWordStrength();
    }

    public void RemoveWaterBuff() {
        hasWaterBuff = false;
        SetWordStrength();
    }

    public void AddEarthBuff() {
        hasEarthBuff = true;
        SetWordStrength();
    }

    public void RemoveEarthBuff() {
        hasEarthBuff = false;
        SetWordStrength();
    }

    public void SetIsValidWord() {
        if (word.Length < battleManager.minCheckingWordLength)
            isValidWord = false;
        else
            isValidWord = SearchLibraryForWord(word);
    }

    private bool SearchLibraryForWord(string word) {
        //returns true if the library contains the word
        if (StaticVariables.allowProfanities)
            return (System.Array.BinarySearch<string>(StaticVariables.allWordLibrary, word.ToLower()) > -1);
        else
            return (System.Array.BinarySearch<string>(StaticVariables.allWordNoSwearsLibrary, word.ToLower()) > -1);
        //int result = System.Array.BinarySearch<string>(StaticVariables.wordLibraryForChecking, word.ToLower());
        //return (result > -1);
    }

    public void SetWordStrength(){
        if (word.Length < battleManager.minCheckingWordLength) {
            strength = 0;
            return;
        }
        int len = word.Length;
        if (hasEarthBuff)
            len += 2;
        int powerupCount = 0;
        foreach (LetterSpace ls in battleManager.letterSpacesForWord) {
            if (ls.powerupType != BattleManager.PowerupTypes.None)
                powerupCount++;
        }
        if (powerupCount > 1)
            len += (powerupCount - 1);
        int str = Mathf.FloorToInt(Mathf.Pow((len - 2), 2));
        if (hasWaterBuff) {
            str += (StaticVariables.waterFloodDamageBonus * battleManager.GetNumberOfEnemies());
            if (battleManager.enemyData.isNearWater)
                str += StaticVariables.riverDamageBonus;
        }
        if (type == BattleManager.PowerupTypes.Fire) {
            str *= StaticVariables.fireDamageMultiplier;
            if (battleManager.enemyData.weakToFire)
                str = (int)(str * 1.5f);
        }
        strength = str;
        //strength = 130;
    }

    public void SetPowerupType() {
        type = BattleManager.PowerupTypes.None;
        if (battleManager.letterSpacesForWord.Count == 0)
            return;
        foreach (LetterSpace ls in battleManager.letterSpacesForWord) {
            if (ls.powerupType != BattleManager.PowerupTypes.None) {
                type = ls.powerupType;
                return;
            }
        }
    }

    private void PrintAttackData() {
        Debug.Log("attack data- type(" + type + ") str(" + strength + ") length(" + word.Length + ") flooded(" + hasWaterBuff + ") rocked(" + hasEarthBuff + ")");
    }
}
