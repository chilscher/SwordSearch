using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAnimatorFunctions : MonoBehaviour{

    public BattleManager battleManager;
    public Animator animator;

    public List<GameObject> attacksInProgress = new List<GameObject>();

    [Header("Attack Animation Objects")]
    public GameObject DeathBubble;
    public GameObject basicFirePrefab;
    public GameObject powerFirePrefab;
    public GameObject basicWaterPrefab;
    public GameObject powerWaterPrefab;
    public GameObject basicDarkPrefab;
    public GameObject powerDarkPrefab;
    public GameObject basicEarthPrefab;
    public GameObject powerEarthPrefab;
    public GameObject basicLightningPrefab;
    public GameObject powerLightningPrefab;
    public GameObject powerHealPrefab;

    [Header("Health Change Animation Objects")]
    public GameObject playerHealSingleDigitPrefab;
    public GameObject playerHealDoubleDigitPrefab;


    

    void Start(){
        foreach (Transform t in transform)
            t.gameObject.SetActive(false);
    }

    public void CreateAttackAnimation(BattleManager.PowerupTypes type, int strength, int powerupLevel){
        GameObject o = basicFirePrefab;
        switch (type){
            case BattleManager.PowerupTypes.Fire:
                o = powerFirePrefab;
                break;
            case BattleManager.PowerupTypes.Water:
                o = powerWaterPrefab;
                break;
            case BattleManager.PowerupTypes.Heal:
                o = powerHealPrefab;
                break;
            case BattleManager.PowerupTypes.Dark:
                o = powerDarkPrefab;
                break;
            case BattleManager.PowerupTypes.Earth:
                o = powerEarthPrefab;
                break;
            case BattleManager.PowerupTypes.Lightning:
                o = powerLightningPrefab;
                break;
            default:
                o = ChooseAnimationForPowerupTypeNone();
                break;
        }

        o = powerFirePrefab;
        strength = 6;
        powerupLevel = StaticVariables.rand.Next(1,3);
        type = BattleManager.PowerupTypes.Fire;

        GameObject newAttack = Instantiate(o, battleManager.uiManager.playerAttackAnimationParent);
        newAttack.SetActive(false);
        newAttack.GetComponent<AttackAnimatorFunctions>().SetStats(type, strength, powerupLevel, battleManager);
        attacksInProgress.Add(newAttack);   
    }


    public void StartNextAttackAnimation(){
        GameObject go = attacksInProgress[0];
        go.SetActive(true);
        attacksInProgress.RemoveAt(0);
    }

    public GameObject ChooseAnimationForPowerupTypeNone(){
        int range = 5;
        int i = StaticVariables.rand.Next(0, range);
        switch (i){
            case 0:
                return basicFirePrefab;
            case 1:
                return basicWaterPrefab;
            case 2:
                return basicDarkPrefab;
            case 3:
                return basicEarthPrefab;
            default:
                return basicLightningPrefab;
        }
    }

    public void ShowDeathBubble(){
        DeathBubble.SetActive(true);
    }
}
