using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneBranchOrganizer : MonoBehaviour{

    public float dropSpeed = 1200f;
    public float timeBetweenFalls = 0.2f;
    public List<CutsceneBranchData> branches;
    private int index = 0;

    public void StartDrops(){
        index = 0;
        DropNextBranch();
    }

    private void DropNextBranch(){
        if (index < branches.Count){
            DropBranch(branches[index]);
            index ++;
            StaticVariables.WaitTimeThenCallFunction(timeBetweenFalls, DropNextBranch);
        }
    }

    private void DropBranch(CutsceneBranchData branch){
        float totalDistance = (branch.droppedPosition - new Vector2(branch.transform.localPosition.x, branch.transform.localPosition.y)).magnitude;
        float totalDuration = totalDistance/dropSpeed;
        branch.transform.DOLocalMove(branch.droppedPosition, totalDuration).SetEase(Ease.Linear).OnComplete(branch.ShowPoof);
    }
}
