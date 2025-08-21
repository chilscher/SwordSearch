using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneUndeadHorde : MonoBehaviour {

    private GameObject powerWater;
    private GameObject powerEarth;
    private GameObject powerFire;
    private Animator playerAnimator;
    private List<GameObject> undeadHordesWhole = new(); //all enemies in one chunk
    private List<GameObject> undeadHordesHalfTop = new();
    private List<GameObject> undeadHordesHalfBottom = new();
    private List<GameObject> currentHordes;
    private BattleManager.PowerupTypes powerToUse;
    private CutsceneManager cutsceneManager;
    public bool playerIsCasting = false;
    private bool cancelEverything = false;

    public void Setup(Animator playerAnimator, Transform wholeHordesParent, Transform topHalfHordesParent, Transform bottomHalfHordesParent, CutsceneManager cutsceneManager) {
        this.playerAnimator = playerAnimator;
        this.cutsceneManager = cutsceneManager;
        powerWater = playerAnimator.transform.parent.GetChild(2).gameObject;
        powerEarth = playerAnimator.transform.parent.GetChild(3).gameObject;
        powerFire = playerAnimator.transform.parent.GetChild(4).gameObject;
        foreach (Transform t in wholeHordesParent)
            undeadHordesWhole.Add(t.gameObject);
        foreach (Transform t in topHalfHordesParent)
            undeadHordesHalfTop.Add(t.gameObject);
        foreach (Transform t in bottomHalfHordesParent)
            undeadHordesHalfBottom.Add(t.gameObject);
        SendOutNewHorde();
    }

    private void SendOutNewHorde() {
        //determine power
        int val = StaticVariables.rand.Next(3);
        switch (val) {
            case (1):
                powerToUse = BattleManager.PowerupTypes.Earth;
                break;
            case (2):
                powerToUse = BattleManager.PowerupTypes.Fire;
                break;
            default:
                powerToUse = BattleManager.PowerupTypes.Water;
                break;
        }

        //send horde
        currentHordes = new();
        if (powerToUse == BattleManager.PowerupTypes.Fire)
            currentHordes.Add(undeadHordesWhole[StaticVariables.rand.Next(undeadHordesWhole.Count)]);
        else{
            int num = StaticVariables.rand.Next(undeadHordesHalfTop.Count);
            currentHordes.Add(undeadHordesHalfTop[num]);
            currentHordes.Add(undeadHordesHalfBottom[num]);
        }
        foreach (GameObject horde in currentHordes){
            horde.SetActive(true);
            horde.transform.localPosition = Vector2.zero;
            foreach (Transform guy in horde.transform)
                guy.GetComponent<Animator>().Play("Walk");
            horde.transform.DOLocalMoveX(-50, 6.5f).SetEase(Ease.Linear);
        }

        //prep attack
        StaticVariables.WaitTimeThenCallFunction(3f, StartPlayerAttack);
    }

    private void StartPlayerAttack() {
        if (cancelEverything)
            return;
        playerIsCasting = true;
        playerAnimator.Play("Cast Spell");
        StaticVariables.WaitTimeThenCallFunction(0.33f, StartPower);
    }

    private void StartPower() {
        float enemyHitTime;
        float animationEndedTime;
        GameObject power;
        switch (powerToUse) {
            case (BattleManager.PowerupTypes.Earth):
                power = powerEarth;
                enemyHitTime = 0.66f;
                animationEndedTime = 1.08f;
                break;
            case (BattleManager.PowerupTypes.Fire):
                power = powerFire;
                enemyHitTime = 1.5f;
                animationEndedTime = 1.91f;
                break;
            default:
                power = powerWater;
                enemyHitTime = 1.33f;
                animationEndedTime = 1.83f;
                break;
        }
        power.SetActive(true);
        StaticVariables.WaitTimeThenCallFunction(enemyHitTime, DamageEnemies);
        StaticVariables.WaitTimeThenCallFunction(animationEndedTime, EndAnimation);
    }

    private void DamageEnemies(){
        if (cancelEverything)
            return;
        foreach (GameObject horde in currentHordes){
            horde.transform.DOKill();
            foreach (Transform guy in horde.transform)
                guy.GetComponent<Animator>().Play("Die");
        }
    }

    private void EndAnimation() {
        if (cancelEverything)
            return;
        playerIsCasting = false;
        powerWater.SetActive(false);
        powerEarth.SetActive(false);
        powerFire.SetActive(false);
        foreach (GameObject horde in currentHordes) {
            horde.SetActive(false);
        }
        StaticVariables.WaitTimeThenCallFunction(2f, SendOutNewHorde);
    }

    public void CancelEverything(){
        cancelEverything = true;
        if (playerIsCasting) {
            Color c = Color.white;
            c.a = 0;
            powerWater.GetComponent<Image>().DOColor(c, 0.5f);
            powerEarth.GetComponent<Image>().DOColor(c, 0.5f);
            powerFire.GetComponent<Image>().DOColor(c, 0.5f);
        }
    }

    public void TurnHordeAround() {
        foreach (GameObject horde in currentHordes) {
            foreach (Transform guy in horde.transform)
                guy.localScale = new Vector2(guy.transform.localScale.x * -1, guy.transform.localScale.y);
        }
    }
    public void StopHordeMovement() {
        foreach (GameObject horde in currentHordes) {
            foreach (Transform guy in horde.transform)
                guy.GetComponent<Animator>().Play("Idle");
            horde.transform.DOKill();
        }
    }
    
    public void WalkHordeOffScreen(){
        foreach (GameObject horde in currentHordes){
            foreach (Transform guy in horde.transform)
                guy.GetComponent<Animator>().Play("Walk");
            horde.transform.DOLocalMoveX(0, 2.5f);
        }
    }
    
    public void HideHorde(){
        foreach (GameObject horde in currentHordes) {
            horde.SetActive(false);
        }
        
    }
}
