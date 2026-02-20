//for Cityscapes, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SceneChangerVisuals : MonoBehaviour {

    public SceneChanger.Scene thisScene;
    public RectTransform canvas;
    public RectTransform settingsPage;
    public GameObject clickBlocker;   

    public void Start(){
        SceneChanger.visuals = this;
        SetUIElementSizes();
        if (SceneChanger.goingTo == SceneChanger.Scene.None){
            settingsPage.gameObject.SetActive(false);
            clickBlocker.SetActive(false);
        }
        else
            AnimateComingIntoScene();
    }

    public void AnimateLeavingScene(){
        clickBlocker.SetActive(true);
        switch (SceneChanger.goingTo){
            case SceneChanger.Scene.Settings:
                settingsPage.gameObject.SetActive(true);
                settingsPage.DOLocalMoveX(0, 1f).SetEase(Ease.OutSine);
                return;//there is no animation going to the settings scene
            //case SceneChanger.Scene.Homepage:
                //something will be added, just not yet
            //    return;
        }
    }

    public void AnimateComingIntoScene(){
        clickBlocker.SetActive(true);
        switch (SceneChanger.goingTo){
            case SceneChanger.Scene.Settings:
                clickBlocker.SetActive(false);
                return;//there is no animation going to the settings scene
            //case SceneChanger.Scene.Homepage:
                //something will be added, just not yet
           //     return;
        }
    }

    public void SetUIElementSizes(){
        float fullHeight = canvas.rect.height;
        float fullWidth = canvas.rect.width;
        settingsPage.sizeDelta = new Vector2(fullWidth + 500, fullHeight);
        settingsPage.localPosition = new Vector2(fullWidth + 500, 0);
        
    }
}