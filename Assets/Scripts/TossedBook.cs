using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TossedBook : MonoBehaviour{
    [HideInInspector]
    public CutsceneBookThrow cbt;

    public void DestroySelf(){
        cbt.ABookThrowEnded(GetComponent<Image>().sprite);
        Destroy(gameObject);
    }

}
