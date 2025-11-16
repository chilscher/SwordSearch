using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyAttackAnimatorFunctions : MonoBehaviour{

    public BattleManager battleManager;
    public EnemyData data;

    public void DoEnemyAttackEffect(){
        battleManager.DoEnemyAttackEffect(this);
    }

    public void QueueNextAttack(){
        if (battleManager.enemyData.isHorde){
            if (data == battleManager.firstEnemyInHorde)
                battleManager.QueueEnemyAttack();
        }
        else
            battleManager.QueueEnemyAttack();
    }

    public void ReturnedToIdle(){
    }

    public void DeathAnimationFinished(){
        if (battleManager == null)
            return;
        battleManager.EnemyDeathAnimationFinished();
    }
    
    public void StartDeathMovement(){
        Vector3 newPos = transform.parent.position + new Vector3(20,20,0);
        transform.parent.DOMove(newPos, 1f);
    }
    
}
