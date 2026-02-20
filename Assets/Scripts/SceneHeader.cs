using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneHeader : MonoBehaviour{
    //private float pos;

    //public void Start(){
        //SlideIn();
        //RectTransform rt = GetComponent<RectTransform>();
        //pos = rt.localPosition.y;
        //print(pos);
        //transform.localPosition = new Vector2(transform.localPosition.x, pos + rt.rect.height);
        //transform.DOLocalMoveY(pos, 0.5f);
    //}

    public void SlideIn(){
        
    }

    public void SlideOut(){
        
    }

    public void GoToHomepage(){
        SceneChanger.GoHome();
        //StaticVariables.FadeOutThenLoadScene("Homepage");
    }
    public void GoToAtlas(){
        StaticVariables.FadeOutThenLoadScene(StaticVariables.mapName);
    }
}
