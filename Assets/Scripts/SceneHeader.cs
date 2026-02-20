using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneHeader : MonoBehaviour{
    private float startingPos;

    public void Start(){
        startingPos = transform.localPosition.y;
        StartOut();
    }

    public void SlideIn(){
        RectTransform rt = GetComponent<RectTransform>();
        transform.localPosition = new Vector2(transform.localPosition.x, startingPos + rt.rect.height);
        transform.DOLocalMoveY(startingPos, 0.5f);
    }

    public void SlideOut(){
        RectTransform rt = GetComponent<RectTransform>();
        transform.DOLocalMoveY(startingPos + rt.rect.height, 0.5f);
    }
    
    private void StartOut(){
        RectTransform rt = GetComponent<RectTransform>();
        transform.localPosition = new Vector2(transform.localPosition.x, startingPos + rt.rect.height);
    }

    public void GoToHomepage(){
        SceneChanger.GoHome();
        //StaticVariables.FadeOutThenLoadScene("Homepage");
    }
    public void GoToAtlas(){
        StaticVariables.FadeOutThenLoadScene(StaticVariables.mapName);
    }
}
