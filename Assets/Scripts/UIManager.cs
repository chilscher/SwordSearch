using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Dynamic;
using TMPro;
using MyBox;
//using System.Numerics;

public class UIManager : MonoBehaviour {
    private Transform enemyObject;
    [HideInInspector] public Animator enemyAnimator;
    [HideInInspector] public List<Animator> enemyHordeAnimators;
    private float waterDrainDuration;
    private Color waterPowerupStrengthColor;
    private float floodHeight;
    private bool movingBook = false;
    private List<GameObject> animatedObjectsInWindow = new List<GameObject>();
    private bool turningPage = false;
    //private List<Image> wordStrengthIconImages = new List<Image>();
    private Image enemyTimerBarImage;
    private Color defaultEnemyTimerBarColor;
    [HideInInspector] public BoulderGroup shownBoulders = null;

    [Header("Submit Word Button")]
    public TextMeshProUGUI wordDisplay;
    public Image countdownNumber;
    public GameObject countdownDivider;
    public Image strengthIcon1Digit;
    public Image strengthIcon2DigitOnes;
    public Image strengthIcon2DigitTens;
    public Image strengthIcon3DigitOnes;
    public Image strengthIcon3DigitTens;
    public Image strengthIcon3DigitHundreds;
    public GameObject wordStrengthDivider;
    //public List<GameObject> wordStrengthIconGameObjects;
    public GameObject wordStrengthIcon;

    public List<AnimatedPowerupBackground> animatedPowerupBackgrounds;


    [Header("Colors")]
    public Color textColorForValidWord = Color.white;
    public Color textColorForInvalidWord = Color.gray;
    public Color usedLetterColor;
    public List<PowerupDisplayData> powerupDisplayDataList;
    [Header("Numbers")]
    public Sprite[] numberSprites;

    [Header("Player and Enemy")]
    public Animator playerAnimator;
    public Transform playerObject;
    public GameObject playerHealTripleDigitPrefab;
    public GameObject playerHealDoubleDigitPrefab;
    public GameObject playerHealSingleDigitPrefab;
    public GameObject playerDamageTripleDigitPrefab;
    public GameObject playerDamageDoubleDigitPrefab;
    public GameObject playerDamageSingleDigitPrefab;
    public Transform enemyParentParent;
    public GameObject enemyHealTripleDigitPrefab;
    public GameObject enemyHealDoubleDigitPrefab;
    public GameObject enemyHealSingleDigitPrefab;
    public GameObject enemyDamageTripleDigitPrefab;
    public GameObject enemyDamageDoubleDigitPrefab;
    public GameObject enemyDamageSingleDigitPrefab;

    [Header("Health Display")]
    public Image playerHP4DigitThousands;
    public Image playerHP4DigitHundreds;
    public Image playerHP4DigitTens;
    public Image playerHP4DigitOnes;
    public Image playerHP3DigitHundreds;
    public Image playerHP3DigitTens;
    public Image playerHP3DigitOnes;
    public Image playerHP2DigitTens;
    public Image playerHP2DigitOnes;
    public Image playerHP1DigitOnes;
    public Image enemyHP4DigitThousands;
    public Image enemyHP4DigitHundreds;
    public Image enemyHP4DigitTens;
    public Image enemyHP4DigitOnes;
    public Image enemyHP3DigitHundreds;
    public Image enemyHP3DigitTens;
    public Image enemyHP3DigitOnes;
    public Image enemyHP2DigitTens;
    public Image enemyHP2DigitOnes;
    public Image enemyHP1DigitOnes;

    [Header("Status Bar")]
    public Transform enemyTimerBarImageParent;
    public Transform enemyTimerBar;
    public Transform enemyStunBar;
    public Animator enemyStunBarAnimation;
    public RectTransform copycatBar;

    [Header("Enemy Attacks")]
    public Transform boulderGroupsParent;
    public RectTransform necromancyHand1;
    public RectTransform necromancyHand2;
    public RectTransform necromancyHand3;
    public RectTransform necromancyHand4;
    public RectTransform necromancyHand5;
    public RectTransform necromancyHandSpacer12;
    public RectTransform necromancyHandSpacer23;
    public RectTransform necromancyHandSpacer34;
    public RectTransform necromancyHandSpacer45;

    [Header("Misc")]
    public BattleManager battleManager;
    public Transform playerAttackAnimationParent;
    public RectTransform waterBuffBottom;
    public RectTransform waterBuffTop;
    public RectTransform earthBuffLeft;
    public RectTransform earthBuffRight;
    public RectTransform earthBuffTop;
    public RectTransform earthBuffBottom;
    public float waterFillDuration = 2f;
    public float waterFloatDuration = 3f;
    public Transform backgroundParent;
    public Transform foregroundParent;
    public RectTransform book;
    public RectTransform pauseButton;
    public GameObject puzzlePage;
    public GameObject endGameDisplay;
    public Text endGameTitleText;
    public Text endGameButtonText;
    public Animator pulseAnimatorClock;
    public GameObject pageTurnGameObject;
    public Animator pageTurnAnimator;
    public GameObject enemyParentPrefab;
    public DialogueManager dialogueManager;

    [HideInInspector]
    public BattleManager.PowerupTypes powerupType;


    public void SetStartingValues() {
        puzzlePage.SetActive(true);
        endGameDisplay.SetActive(false);
        pageTurnGameObject.SetActive(false);
        waterPowerupStrengthColor = GetPowerupDisplayDataWithType(BattleManager.PowerupTypes.Water).backgroundColor;
        floodHeight = waterBuffTop.anchoredPosition.y;
        waterBuffTop.anchoredPosition = new Vector2(waterBuffTop.anchoredPosition.x, waterBuffBottom.anchoredPosition.y);
        enemyTimerBarImage = enemyTimerBar.GetComponent<Image>();
        defaultEnemyTimerBarColor = enemyTimerBarImage.color;

        SetOriginalPowerupBackgroundTransparencies();
        if (battleManager.enemyData.isCopycat)
            ShowCopycatBuildup();
    }

    public void DisplayHealths(int playerHealth, int enemyHealth) {
        Vector4 playerHP = GetDigitsFromInt(playerHealth);
        Vector4 enemyHP = GetDigitsFromInt(enemyHealth);

        playerHP1DigitOnes.sprite = numberSprites[(int)playerHP[0]];
        playerHP2DigitOnes.sprite = numberSprites[(int)playerHP[0]];
        playerHP2DigitTens.sprite = numberSprites[(int)playerHP[1]];
        playerHP3DigitOnes.sprite = numberSprites[(int)playerHP[0]];
        playerHP3DigitTens.sprite = numberSprites[(int)playerHP[1]];
        playerHP3DigitHundreds.sprite = numberSprites[(int)playerHP[2]];
        playerHP4DigitOnes.sprite = numberSprites[(int)playerHP[0]];
        playerHP4DigitTens.sprite = numberSprites[(int)playerHP[1]];
        playerHP4DigitHundreds.sprite = numberSprites[(int)playerHP[2]];
        playerHP4DigitThousands.sprite = numberSprites[(int)playerHP[3]];
        playerHP1DigitOnes.transform.parent.gameObject.SetActive(playerHealth <= 9);
        playerHP2DigitOnes.transform.parent.gameObject.SetActive((playerHealth >= 10) && (playerHealth <= 99));
        playerHP3DigitOnes.transform.parent.gameObject.SetActive((playerHealth >= 100) && (playerHealth <= 999));
        playerHP4DigitOnes.transform.parent.gameObject.SetActive(playerHealth >= 1000);

        enemyHP1DigitOnes.sprite = numberSprites[(int)enemyHP[0]];
        enemyHP2DigitOnes.sprite = numberSprites[(int)enemyHP[0]];
        enemyHP2DigitTens.sprite = numberSprites[(int)enemyHP[1]];
        enemyHP3DigitOnes.sprite = numberSprites[(int)enemyHP[0]];
        enemyHP3DigitTens.sprite = numberSprites[(int)enemyHP[1]];
        enemyHP3DigitHundreds.sprite = numberSprites[(int)enemyHP[2]];
        enemyHP4DigitOnes.sprite = numberSprites[(int)enemyHP[0]];
        enemyHP4DigitTens.sprite = numberSprites[(int)enemyHP[1]];
        enemyHP4DigitHundreds.sprite = numberSprites[(int)enemyHP[2]];
        enemyHP4DigitThousands.sprite = numberSprites[(int)enemyHP[3]];
        enemyHP1DigitOnes.transform.parent.gameObject.SetActive(enemyHealth <= 9);
        enemyHP2DigitOnes.transform.parent.gameObject.SetActive((enemyHealth >= 10) && (enemyHealth <= 99));
        enemyHP3DigitOnes.transform.parent.gameObject.SetActive((enemyHealth >= 100) && (enemyHealth <= 999));
        enemyHP4DigitOnes.transform.parent.gameObject.SetActive(enemyHealth >= 1000);
    }

    public void ShowPlayerTakingDamage(int amount, bool stillAlive) {
        if (amount > 0) //if dealt 0 damage, don't show hitsplat
            ShowNumbersAsChild(playerDamageSingleDigitPrefab, playerDamageDoubleDigitPrefab, playerDamageTripleDigitPrefab, playerObject, amount);
        if (stillAlive)
            playerAnimator.SetTrigger("TakeDamage");
        else
            playerAnimator.SetTrigger("Die");
    }

    public void ShowPlayerGettingHealed(int amount) {
        ShowNumbersAsChild(playerHealSingleDigitPrefab, playerHealDoubleDigitPrefab, playerHealTripleDigitPrefab, playerObject, amount);
    }

    public void ShowEnemyTakingDamage(int amount, bool stillAlive) {
        ShowNumbersAsChild(enemyDamageSingleDigitPrefab, enemyDamageDoubleDigitPrefab, enemyDamageTripleDigitPrefab, enemyParentParent, amount);
        if (battleManager.enemyData.isHorde) {
            int i = 0;
            foreach (Animator anim in enemyHordeAnimators) {
                if (anim.gameObject.activeSelf) {
                    if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Die")) {
                        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage")) {
                            if (i < battleManager.currentHordeEnemyCount)
                                anim.SetTrigger("TakeDamage");
                            else
                                anim.SetTrigger("Die");
                        }
                    }
                }
                i++;
            }
        }
        else {
            if (!stillAlive)
                enemyAnimator.Play("Die");
            else if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("TakeDamage"))
                enemyAnimator.SetTrigger("TakeDamage");
        }
    }

    public void ShowEnemyGettingHealed(int amount) {
        ShowNumbersAsChild(enemyHealSingleDigitPrefab, enemyHealDoubleDigitPrefab, enemyHealTripleDigitPrefab, enemyParentParent, amount);
    }

    private void ShowNumbersAsChild(GameObject singleDigitPrefab, GameObject doubleDigitPrefab, GameObject tripleDigitPrefab, Transform parent, int amount) { 
        Vector4 digits = GetDigitsFromInt(amount);
        if (amount >= 100) {
            GameObject obj = Instantiate(tripleDigitPrefab, parent);
            obj.transform.Find("Ones").GetComponent<Image>().sprite = numberSprites[(int)digits[0]];
            obj.transform.Find("Tens").GetComponent<Image>().sprite = numberSprites[(int)digits[1]];
            obj.transform.Find("Hundreds").GetComponent<Image>().sprite = numberSprites[(int)digits[2]];
        }
        else if (amount >= 10) {
            GameObject obj = Instantiate(doubleDigitPrefab, parent);
            obj.transform.Find("Ones").GetComponent<Image>().sprite = numberSprites[(int)digits[0]];
            obj.transform.Find("Tens").GetComponent<Image>().sprite = numberSprites[(int)digits[1]];
        }
        else{
            GameObject obj = Instantiate(singleDigitPrefab, parent);
            obj.transform.Find("Ones").GetComponent<Image>().sprite = numberSprites[(int)digits[0]];
        }
    }


    public void StartPlayerCastAnimation() {
        playerAnimator.SetTrigger("StartCast");
    }

    public void StartPlayerHealAnimation() {
        playerAnimator.SetTrigger("StartHeal");
    }

    public void StartPlayerSwordAnimation() {
        playerAnimator.SetTrigger("StartSword");
    }

    public Color GetPowerupBackgroundColor() {
        foreach (PowerupDisplayData d in powerupDisplayDataList) {
            if (d.type == powerupType)
                return d.backgroundColor;
        }
        return Color.yellow; //definitely wrong and gross
    }

    public void UpdateVisualsForLettersInWord(List<LetterSpace> letterSpacesForWord) {
        foreach (LetterSpace ls in letterSpacesForWord)
            ls.ShowAsPartOfWord(GetPowerupBackgroundColor());
    }

    public void DisplayWord(AttackData wordData, int countdown) {
        if (wordData.isValidWord) {
            wordDisplay.text = wordData.word;
            wordDisplay.color = textColorForValidWord;
            wordStrengthDivider.SetActive(true);
            countdownDivider.SetActive(true);
            UpdateWordStrengthDisplay(wordData.strength);
            UpdateCountdownDisplay(countdown);
        }
        else if ((countdown == 0) && (wordData.word.Length == 0)) {
            wordDisplay.text = "TURN  PAGE";
            wordDisplay.color = textColorForInvalidWord;
            //submitWordButtonImage.color = canRefreshPuzzleColor;
            wordStrengthDivider.SetActive(true);
            countdownDivider.SetActive(true);
            UpdateWordStrengthDisplay(wordData.strength);
            UpdateCountdownDisplay(countdown);
        }
        else {
            wordDisplay.text = wordData.word;
            wordDisplay.color = textColorForInvalidWord;
            wordStrengthDivider.SetActive(false);
            countdownDivider.SetActive(false);
            UpdateWordStrengthDisplay(wordData.strength);
            UpdateCountdownDisplay(countdown);
        }
    }

    public void UpdateWordStrengthDisplay(int strength) {
        Vector4 digits = GetDigitsFromInt(strength);
        strengthIcon3DigitOnes.sprite = numberSprites[(int)digits[0]];
        strengthIcon3DigitTens.sprite = numberSprites[(int)digits[1]];
        strengthIcon3DigitHundreds.sprite = numberSprites[(int)digits[2]];
        strengthIcon2DigitOnes.sprite = numberSprites[(int)digits[0]];
        strengthIcon2DigitTens.sprite = numberSprites[(int)digits[1]];
        strengthIcon1Digit.sprite = numberSprites[(int)digits[0]];
        strengthIcon1Digit.transform.parent.gameObject.SetActive(strength <= 9);
        strengthIcon2DigitOnes.transform.parent.gameObject.SetActive((strength >= 10) && (strength <= 99));
        strengthIcon3DigitOnes.transform.parent.gameObject.SetActive(strength >= 100);
        
        Color numberColor = Color.white;
        //if ((battleManager.isWaterInPuzzleArea) && (strength > 0))
        //    numberColor = waterPowerupStrengthColor;
        strengthIcon1Digit.color = numberColor;
        strengthIcon2DigitOnes.color = numberColor;
        strengthIcon2DigitTens.color = numberColor;
        strengthIcon3DigitOnes.color = numberColor;
        strengthIcon3DigitTens.color = numberColor;
        strengthIcon3DigitHundreds.color = numberColor;
    }
    
    private Vector4 GetDigitsFromInt(int val){
        //returns in the order ones - tens - hundres - thousands
        int thousands = 0;
        int hundreds = 0;
        int tens = 0;
        int ones = 0;
        if (val > 9999)
            return new Vector4(9, 9, 9, 9);
        int remainder = val;
        if (val > 999) {
            thousands = remainder / 1000;
            remainder -= (thousands * 1000);
        }
        if (val > 99) {
            hundreds = remainder / 100;
            remainder -= (hundreds * 100);
        }
        if (val > 9) {
            tens = remainder / 10;
            remainder -= (tens * 10);
        }
        ones = remainder;
        return new Vector4(ones, tens, hundreds, thousands);
    }
    
    public void UpdateCountdownDisplay(int countdown) {
        if (countdown > 9)
            countdown = 9;
        countdownNumber.sprite = numberSprites[countdown];
    }

    public PowerupDisplayData GetPowerupDisplayDataWithType(BattleManager.PowerupTypes t) {
        foreach (PowerupDisplayData d in powerupDisplayDataList) {
            if (d.type == t)
                return d;
        }
        return null;
    }

    public void StopEnemyAttackTimer() {
        enemyTimerBar.localScale = Vector3.one;
        DOTween.Kill(enemyTimerBar);
    }

    public void ActivateEnemyStunBar(float duration) {
        enemyStunBar.gameObject.SetActive(true);
        enemyStunBarAnimation.gameObject.SetActive(true);
        enemyStunBar.localScale = Vector3.one;
        DOTween.Kill(enemyStunBar);
        enemyStunBar.DOScale(new Vector3(0, 1, 1), duration).SetEase(Ease.Linear).OnComplete(battleManager.EnemyStunWearsOff);
    }

    public void DeactivateEnemyStunBar() {
        enemyStunBar.gameObject.SetActive(false);
        enemyStunBarAnimation.SetTrigger("End");
    }

    public void CancelEnemyAttackAndQueueNext(){
        print("cancelling enemy attack");
        enemyTimerBar.DOKill();
        enemyTimerBar.DOScale(Vector3.one, 1f).SetEase(Ease.Linear).OnComplete(battleManager.QueueAttackAfterCancel);
        enemyTimerBarImage.color = defaultEnemyTimerBarColor; 
    }

    public void StartEnemyAttackTimer(float duration, Color? c = null, EnemyAttack.EnemyAttackTypes attackType = EnemyAttack.EnemyAttackTypes.None) {
        duration = ScaleAttackTimerWithDifficulty(duration, attackType);
        enemyTimerBar.localScale = Vector3.one;
        enemyTimerBar.DOScale(new Vector3(0, 1, 1), duration).SetEase(Ease.Linear).OnComplete(battleManager.TriggerEnemyAttack);
        if (c == null)
            enemyTimerBarImage.color = defaultEnemyTimerBarColor;
        else
            enemyTimerBarImage.color = (Color)c;
    }

    private float ScaleAttackTimerWithDifficulty(float normalTime, EnemyAttack.EnemyAttackTypes attackType){
        //modify attack speed for specific enemy special attacks (usually attacks that don't do damage on their own)
        if (attackType == EnemyAttack.EnemyAttackTypes.None)
            return normalTime;
        if (StaticVariables.difficultyMode == StaticVariables.DifficultyMode.Normal)
            return normalTime;
        if (attackType == EnemyAttack.EnemyAttackTypes.Necromancy){
            switch (StaticVariables.difficultyMode) {
                case StaticVariables.DifficultyMode.Easy:
                    return normalTime * 1.3f;
                case StaticVariables.DifficultyMode.Hard:
                    return normalTime * 0.7f;
            }
        }
        else if (attackType == EnemyAttack.EnemyAttackTypes.Lightning2){
            switch (StaticVariables.difficultyMode) {
                case StaticVariables.DifficultyMode.Easy:
                    return normalTime * 1.3f;
                case StaticVariables.DifficultyMode.Hard:
                    return normalTime * 0.7f;
            }
        }
        return normalTime;
    }

    public void AnimateEnemyAttackBarDisappearing() {
        foreach (Transform t in enemyTimerBarImageParent) {
            Image im = t.GetComponent<Image>();
            Color newColor = im.color;
            newColor.a = 0;
            im.DOColor(newColor, 1);
        }
    }

    public void StartEnemyAttackAnimation(EnemyAttack.EnemyAttackTypes attackType = EnemyAttack.EnemyAttackTypes.None) {
        if (battleManager.enemyData.isHorde) {
            foreach (Animator anim in enemyHordeAnimators)
                anim.SetTrigger("Attack");
        }
        else if (attackType == EnemyAttack.EnemyAttackTypes.Necromancy)
            enemyAnimator.SetTrigger("Attack_Necromancy");
        else if (attackType == EnemyAttack.EnemyAttackTypes.Lightning1)
            enemyAnimator.SetTrigger("Attack_Lightning_Charge");
        else if (attackType == EnemyAttack.EnemyAttackTypes.Lightning2){
            enemyAnimator.SetTrigger("Attack_Lightning_Strike");
            battleManager.ClearLightningCharging();
        }
        else
            enemyAnimator.SetTrigger("Attack");
        enemyTimerBar.DOScale(Vector3.one, 1f).SetEase(Ease.Linear);
        enemyTimerBarImage.color = defaultEnemyTimerBarColor;
    }

    public void AddEnemyToScene(GameObject enemyPrefab) {
        GameObject enemyParent = Instantiate(enemyParentPrefab, enemyParentParent);
        enemyParent.name = enemyPrefab.name;
        GameObject newEnemy = Instantiate(enemyPrefab, enemyParent.transform);
        newEnemy.name = enemyPrefab.name;
        enemyObject = newEnemy.transform;
        battleManager.enemyData = newEnemy.GetComponent<EnemyData>();
        if (battleManager.enemyData.isHorde) {
            foreach (Transform t in battleManager.enemyData.GetComponent<HordeOrder>().order) {
                enemyHordeAnimators.Add(t.GetChild(0).GetComponent<Animator>());
                EnemyAttackAnimatorFunctions temp = t.GetChild(0).GetComponent<EnemyAttackAnimatorFunctions>();
                battleManager.enemyHordeAttackAnimatorFunctions.Add(temp);
                temp.battleManager = battleManager;
                temp.data = temp.GetComponent<EnemyData>();
            }
        }
        else {
            enemyAnimator = newEnemy.GetComponent<Animator>();
            battleManager.enemyAttackAnimatorFunctions = enemyObject.GetComponent<EnemyAttackAnimatorFunctions>();
            battleManager.enemyAttackAnimatorFunctions.battleManager = battleManager;
        }
    }

    public void ApplyBackground(GameObject backgroundPrefab) {
        animatedObjectsInWindow = new List<GameObject>();
        Transform background = backgroundPrefab.transform.GetChild(0).transform;
        Transform foreground = backgroundPrefab.transform.GetChild(2).transform;
        foreach (Transform t in backgroundParent)
            GameObject.Destroy(t.gameObject);
        foreach (Transform t in foregroundParent)
            GameObject.Destroy(t.gameObject);
        foreach (Transform t in background) {
            GameObject go = Instantiate(t.gameObject, backgroundParent);
            if (go.GetComponent<Animator>() != null)
                animatedObjectsInWindow.Add(go);
        }
        foreach (Transform t in foreground) {
            GameObject go = Instantiate(t.gameObject, foregroundParent);
            if (go.GetComponent<Animator>() != null)
                animatedObjectsInWindow.Add(go);
        }
    }
    
    public void CoverPageWithBoulders() {
        //pick a random boulder from the children
        int r = StaticVariables.rand.Next(boulderGroupsParent.childCount);
        GameObject selection = boulderGroupsParent.GetChild(r).gameObject;

        //show the boulders on screen
        shownBoulders = selection.GetComponent<BoulderGroup>();
        selection.SetActive(true);
        //hownBoulders.ResetBouldersColor();
        shownBoulders.MoveBouldersIntoPosition();

        //boulders only persist for X seconds. do the tween on the first child, so that the timer can be paused by other functions
        shownBoulders.transform.GetChild(0).GetComponent<Image>().DOColor(Color.white, 17f).OnComplete(ClearBouldersOnPage);
    }

    public void ClearBouldersOnPage() {
        if (shownBoulders == null)
            return;
        foreach (Transform t in shownBoulders.transform) {
            Color c = Color.white;
            c.a = 0;
            t.GetComponent<Image>().DOColor(c, 0.5f).OnComplete(FinishClearingBoulders);
        }
    }

    private void FinishClearingBoulders() {
        if (shownBoulders == null)
            return;
        shownBoulders.gameObject.SetActive(false);
        shownBoulders = null;
    }
    
    public void ShowCopycatBuildup(){
        print("copycat buildup is " + battleManager.copycatBuildup);
        float frac = (float)battleManager.copycatBuildup / (float)battleManager.maxCopycatStacks;
        if (frac < 0)
            frac = 0;
        copycatBar.localScale = new Vector2(1, frac);
        copycatBar.gameObject.SetActive(true);
    }
    
    public void FillPuzzleAreaWithWater(float totalDuration) {
        CancelWaterDrain();
        waterDrainDuration = totalDuration - waterFillDuration - waterFloatDuration;
        waterBuffBottom.DOSizeDelta(new Vector2(waterBuffBottom.sizeDelta.x, (floodHeight - waterBuffBottom.anchoredPosition.y)), waterFillDuration);
        waterBuffTop.DOAnchorPos(new Vector2(0, floodHeight), waterFillDuration).OnComplete(FloatWater);
        waterBuffBottom.gameObject.SetActive(true);
        waterBuffTop.gameObject.SetActive(true);
    }
    
    public void AddRocksToEdgeOfPuzzle(){
        float temp = earthBuffBottom.rect.height;
        earthBuffBottom.SetPositionY(-temp);
        earthBuffBottom.DOAnchorPosY(0, 0.5f);
        temp = earthBuffTop.rect.height;
        earthBuffTop.SetPositionY(temp);
        earthBuffTop.DOAnchorPosY(0, 0.5f);
        temp = earthBuffLeft.rect.width;
        earthBuffLeft.SetPositionX(-temp);
        earthBuffLeft.DOAnchorPosX(0, 0.5f);
        temp = earthBuffRight.rect.width;
        earthBuffRight.SetPositionX(temp);
        earthBuffRight.DOAnchorPosX(0, 0.5f);
    }
    
    public void RemoveRocksFromEdgeOfPuzzle(){
        float temp = earthBuffBottom.rect.height;
        earthBuffBottom.DOAnchorPosY(-temp, 0.5f);
        temp = earthBuffTop.rect.height;
        earthBuffTop.DOAnchorPosY(temp, 0.5f);
        temp = earthBuffLeft.rect.width;
        earthBuffLeft.DOAnchorPosX(-temp, 0.5f);
        temp = earthBuffRight.rect.width;
        earthBuffRight.DOAnchorPosX(temp, 0.5f);
    }

    public void RemoveNecromancyHands(){
        battleManager.necromancyHandsHeights[0] = 0;
        battleManager.necromancyHandsHeights[1] = 0;
        battleManager.necromancyHandsHeights[2] = 0;
        battleManager.necromancyHandsHeights[3] = 0;
        battleManager.necromancyHandsHeights[4] = 0;
        ShowNecromancyHandHeights();
    }
    
    public void ShowNecromancyHandHeights(){
        int[] heights = { -1481, -1400, -1200, -993, -786, -579, -372, -165, -10 };
        necromancyHand1.DOAnchorPosY(heights[battleManager.necromancyHandsHeights[0]], 1f);
        necromancyHand2.DOAnchorPosY(heights[battleManager.necromancyHandsHeights[1]], 1f);
        necromancyHand3.DOAnchorPosY(heights[battleManager.necromancyHandsHeights[2]], 1f);
        necromancyHand4.DOAnchorPosY(heights[battleManager.necromancyHandsHeights[3]], 1f);
        necromancyHand5.DOAnchorPosY(heights[battleManager.necromancyHandsHeights[4]], 1f);
        necromancyHandSpacer12.DOAnchorPosY(heights[Mathf.Min(battleManager.necromancyHandsHeights[0], battleManager.necromancyHandsHeights[1])], 1f);
        necromancyHandSpacer23.DOAnchorPosY(heights[Mathf.Min(battleManager.necromancyHandsHeights[1], battleManager.necromancyHandsHeights[2])], 1f);
        necromancyHandSpacer34.DOAnchorPosY(heights[Mathf.Min(battleManager.necromancyHandsHeights[2], battleManager.necromancyHandsHeights[3])], 1f);
        necromancyHandSpacer45.DOAnchorPosY(heights[Mathf.Min(battleManager.necromancyHandsHeights[3], battleManager.necromancyHandsHeights[4])], 1f);
        //print("hand heights: (" + battleManager.necromancyHandsHeights[0] + ", "+ battleManager.necromancyHandsHeights[1] + ", "+ battleManager.necromancyHandsHeights[2] + ", "+ battleManager.necromancyHandsHeights[3] + ", "+ battleManager.necromancyHandsHeights[4] + ")");
    }

    private void FloatWater() {
        battleManager.WaterReachedTopOfPage();
        if (waterFloatDuration == -1)
            return;
        StaticVariables.WaitTimeThenCallFunction(waterFloatDuration, StartDrainingWater);
    }

    public void StartDrainingWaterSmallerPage() {
        float offset = 650f;
        waterBuffBottom.DOSizeDelta(new Vector2(waterBuffBottom.sizeDelta.x, offset), waterDrainDuration).SetEase(Ease.Linear);
        waterBuffTop.DOAnchorPos(new Vector2(0, (-waterBuffBottom.sizeDelta.y + waterBuffTop.anchoredPosition.y + offset)), waterDrainDuration).SetEase(Ease.Linear).OnComplete(DrainingComplete);
    }

    public void StartDrainingWater() {
        waterBuffBottom.DOSizeDelta(new Vector2(waterBuffBottom.sizeDelta.x, 0), waterDrainDuration).SetEase(Ease.Linear);
        waterBuffTop.DOAnchorPos(new Vector2(0, (-waterBuffBottom.sizeDelta.y + waterBuffTop.anchoredPosition.y)), waterDrainDuration).SetEase(Ease.Linear).OnComplete(DrainingComplete);
    }

    private void DrainingComplete() {
        waterBuffBottom.gameObject.SetActive(false);
        waterBuffTop.gameObject.SetActive(false);
        battleManager.WaterDrainComplete();
    }

    private void CancelWaterDrain() {
        DOTween.Kill(waterBuffBottom);
        DOTween.Kill(waterBuffTop);
    }

    public void PushedPauseButton() {
        if ((movingBook) || (turningPage) || (battleManager.playerHealth == 0) || (battleManager.enemyHealth == 0))
            return;
        movingBook = true;
        if (IsPuzzlePageShowing()) {
            book.DOLocalMoveX((book.localPosition.x + book.rect.width), 0.5f).OnComplete(MovingBookEnded);
            pauseButton.DOAnchorPosY(350f, 0.5f);
            battleManager.PauseEverything();
        }
        else {
            book.DOLocalMoveX((book.localPosition.x - book.rect.width), 0.5f).OnComplete(MovingBookEnded);
            pauseButton.DOAnchorPosY(0, 0.5f);
        }
    }

    private void MovingBookEnded() {
        movingBook = false;
        if (IsPuzzlePageShowing())
            battleManager.ResumeEverything();
    }

    private bool IsPuzzlePageShowing() {
        return (book.localPosition.x < book.rect.width);
    }

    public void SetAllAnimationStates(bool state) {
        ChangeAnimationStateIfObjectIsActive(waterBuffTop.gameObject, state);
        ChangeAnimationStateIfObjectIsActive(enemyStunBarAnimation, state);
        ChangeAnimationStateIfObjectIsActive(battleManager.playerAnimatorFunctions.deathBubble, state);
        ChangeAnimationStateIfObjectIsActive(pulseAnimatorClock, state);
        ChangeAnimationStateIfObjectIsActive(pageTurnAnimator, state);
        foreach (Transform t in playerAttackAnimationParent)
            ChangeAnimationStateIfObjectIsActive(t.gameObject, state);
        foreach (Transform t in playerAnimator.transform.parent)
            ChangeAnimationStateIfObjectIsActive(t.gameObject, state);
        foreach (GameObject go in animatedObjectsInWindow)
            ChangeAnimationStateIfObjectIsActive(go, state);
        foreach (LetterSpace ls in battleManager.puzzleGenerator.letterSpaces) {
            ChangeAnimationStateIfObjectIsActive(ls.waterIconAnimator, state);
            ChangeAnimationStateIfObjectIsActive(ls.healingIconAnimator, state);
            ChangeAnimationStateIfObjectIsActive(ls.earthIconAnimator, state);
            ChangeAnimationStateIfObjectIsActive(ls.fireIconAnimator, state);
            ChangeAnimationStateIfObjectIsActive(ls.lightningIconAnimator, state);
            ChangeAnimationStateIfObjectIsActive(ls.darkIconAnimator, state);
            ChangeAnimationStateIfObjectIsActive(ls.swordIconAnimator, state);
            ChangeAnimationStateIfObjectIsActive(ls.selectedSignifierAnimator, state);
            ChangeAnimationStateIfObjectIsActive(ls.chargedIconAnimator, state);
        }
        foreach (Transform t in enemyParentParent) {
            if (t.GetComponent<Animator>() != null)
                ChangeAnimationStateIfObjectIsActive(t.gameObject, state);
        }
        foreach (AnimatedPowerupBackground apb in animatedPowerupBackgrounds)
            ChangeAnimationStateIfObjectIsActive(apb.gameObject, state);
        if (battleManager.enemyData.isHorde) {
            foreach (Animator anim in enemyHordeAnimators)
                ChangeAnimationStateIfObjectIsActive(anim, state);
        }
        else 
            ChangeAnimationStateIfObjectIsActive(enemyAnimator, state);
        if (state)
            UpdateVisualsForLettersInWord(battleManager.letterSpacesForWord);
    }

    public void PauseEnemyAttackBar() {
        if (enemyStunBar.gameObject.activeSelf)
            DOTween.Pause(enemyStunBar);
        else
            DOTween.Pause(enemyTimerBar);
    }

    public void ResumeEnemyAttackBar() {
        if (enemyStunBar.gameObject.activeSelf)
            DOTween.Play(enemyStunBar);
        else
            DOTween.Play(enemyTimerBar);
    }

    public void PauseWaterDrain() {
        if (waterBuffTop.gameObject.activeSelf) {
            DOTween.Pause(waterBuffBottom);
            DOTween.Pause(waterBuffTop);
        }
    }

    public void ResumeWaterDrain() {
        if (waterBuffTop.gameObject.activeSelf) {
            DOTween.Play(waterBuffBottom);
            DOTween.Play(waterBuffTop);
        }
    }

    public void PauseBoulderDebuff() {
        if (shownBoulders != null) {
            foreach (Transform t in shownBoulders.transform)
                DOTween.Pause(t.GetComponent<Image>());
        }
    }

    public void ResumeBoulderDebuff() {
        if (shownBoulders != null) {
            foreach (Transform t in shownBoulders.transform)
                DOTween.Play(t.GetComponent<Image>());
        }
    }

    private void ChangeAnimationStateIfObjectIsActive(Animator anim, bool state) {
        if (anim.gameObject.activeSelf)
            anim.enabled = state;
    }

    private void ChangeAnimationStateIfObjectIsActive(GameObject go, bool state) {
        if (go.activeSelf)
            go.GetComponent<Animator>().enabled = state;
    }

    private void ChangeChildAnimationStateIfObjectIsActive(GameObject go, bool state) {
        if (go.activeSelf)
            go.transform.GetChild(0).GetComponent<Animator>().enabled = state;
    }

    public void PushedResumeButton() {
        //shows up while paused
        PushedPauseButton();
    }

    public void PushedQuitButton() {
        //shows up while paused
        StaticVariables.hasCompletedStage = false;
        StaticVariables.FadeOutThenLoadScene(StaticVariables.lastVisitedStage.worldName);
    }

    public void PushedEndGameButton() {
        //the button that appears after victory or defeat. its function varies depending on if you won or lost
        if (battleManager.enemyHealth <= 0) //you won, proceed through the victory dialogue
            dialogueManager.Setup(battleManager.enemyData.victoryDialogueSteps, StaticVariables.battleData);
        else { //you lost, quit
            StaticVariables.hasCompletedStage = false;
            StaticVariables.FadeOutThenLoadScene(StaticVariables.lastVisitedStage.worldName);
        }
    }

    public void EndDialogue() {
        StaticVariables.hasCompletedStage = true;
        StaticVariables.FadeOutThenLoadScene(StaticVariables.lastVisitedStage.worldName);
    }

    public void ShowVictoryPage() {
        battleManager.HideAllDebuffVisuals();
        ShowPageTurn(true);
        //FadeOutWaterOnBattleEnd();
        endGameDisplay.SetActive(true);
        endGameTitleText.text = "VICTORY";
        endGameButtonText.text = "CONTINUE";
        pauseButton.DOAnchorPosY(350f, 0.5f);
    }

    public void ShowDefeatPage() {
        battleManager.HideAllDebuffVisuals();
        ShowPageTurn(true);
        //FadeOutWaterOnBattleEnd();
        endGameDisplay.SetActive(true);
        endGameTitleText.text = "DEFEAT";
        endGameButtonText.text = "BACK TO MAP";
        pauseButton.DOAnchorPosY(350f, 0.5f);
    }

    public void FadeOutWaterOverlay() {
        Image bot = waterBuffBottom.GetComponent<Image>();
        Image top = waterBuffTop.GetComponent<Image>();
        Color botColor = bot.color;
        Color topColor = top.color;
        botColor.a = 0;
        topColor.a = 0;
        bot.DOColor(botColor, 0.5f);
        top.DOColor(topColor, 0.5f);
    }

    public float GetSynchronizedLetterAnimationFrame() {
        return pulseAnimatorClock.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public void SynchronizePulse(Animator animator, string nameOverride="") {
        string animationName = animator.name;
        if (nameOverride != "")
            animationName = nameOverride;
        animator.Play(animationName, 0, GetSynchronizedLetterAnimationFrame());
    }

    public void ShowPageTurn(bool hideLetters = false) {
        if (pageTurnGameObject.activeSelf)
            pageTurnAnimator.Play("Book Turn", 0, 0);
        pageTurnGameObject.SetActive(true);
        pageTurnGameObject.GetComponent<PageTurnAnimatorFunctions>().SetUpLetterSpaces();
        pageTurnGameObject.GetComponent<PageTurnAnimatorFunctions>().hidingLetters = hideLetters;
        turningPage = true;
    }

    public void PausePageTurn() {
        DOTween.Pause(pageTurnAnimator);
    }

    public void PageTurnEnded() {
        turningPage = false;
        battleManager.TurnPageEnded();
    }

    public void ShowPowerupBackground(BattleManager.PowerupTypes type) {
        foreach (AnimatedPowerupBackground apb in animatedPowerupBackgrounds)
            apb.ShowIfMatchesType(type);
    }

    private void SetOriginalPowerupBackgroundTransparencies() {
        foreach (AnimatedPowerupBackground apb in animatedPowerupBackgrounds)
            apb.SetOriginalTransparencyThenHide();
    }
}

[System.Serializable]
public class PowerupDisplayData{
    public BattleManager.PowerupTypes type;
    public Color textColor = Color.white;
    public Color backgroundColor = Color.white;
    public Sprite icon;
}
