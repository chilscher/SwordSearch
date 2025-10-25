using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneBranchData : MonoBehaviour{

    public Vector2 droppedPosition;
    
    public void ShowPoof(){
        foreach (Transform t in transform){
            if (t.name.Contains("Poof")){
                GameObject poof = t.gameObject;
                poof.SetActive(true);
            }
        }
    }
}
