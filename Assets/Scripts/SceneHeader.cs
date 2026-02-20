using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneHeader : MonoBehaviour{
    //private float pos;

    //public void Start(){
    //    SlideIn();
    //}

    public void SlideIn(){
        RectTransform rt = GetComponent<RectTransform>();
        float pos = rt.localPosition.y;
        transform.localPosition = new Vector2(transform.localPosition.x, pos + rt.rect.height);
        transform.DOLocalMoveY(pos, 0.5f);
    }

    public void SlideOut(){
        RectTransform rt = GetComponent<RectTransform>();
        float pos = rt.localPosition.y;
        //transform.localPosition = new Vector2(transform.localPosition.x, pos + rt.rect.height);
        transform.DOLocalMoveY(pos + rt.rect.height, 0.5f);
        
    }

    public void GoToHomepage(){
        SceneChanger.GoHome();
        //StaticVariables.FadeOutThenLoadScene("Homepage");
    }
    public void GoToAtlas(){
        StaticVariables.FadeOutThenLoadScene(StaticVariables.mapName);
    }
}
