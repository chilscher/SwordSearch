//for Sword Search, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class SceneChangerVisuals : MonoBehaviour {

    public SceneChanger.Scene thisScene;
    public RectTransform canvas;
    public RectTransform settingsPage;
    public GameObject clickBlocker;   
    public SceneHeader sceneHeader;
    [Header("Only for the Homepage Scene")]
    public RectTransform settingsScroll;
    [Header("Only for the Settings Scene")]
    public RectTransform settingsPaper1;
    public RectTransform settingsPaper2;
    public RectTransform settingsPaper3;
    public RectTransform settingsPaper4;
    public RectTransform settingsPaper5;
    public RectTransform settingsPaper6;

    private float canvasWidth;
    private float canvasHeight;
    private float customVal1;
    private float customVal2;


    public void Start(){
        SceneChanger.visuals = this;
        SetCanvasSizes();
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
        if (SceneChanger.goingTo == SceneChanger.Scene.Settings){ //presumably only happening from the homepage
            settingsPage.gameObject.SetActive(true);
            float horizOffset = (settingsScroll.rect.width / 2) + (canvasWidth / 2);
            settingsScroll.DOLocalMoveX(horizOffset, 0.5f).SetEase(Ease.OutSine);
            //slide out the settings scroll first
            StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToX0OutSine, settingsPage);
            StaticVariables.WaitTimeThenCallFunction(1f, TriggerSceneChange);
            //settingsPage.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutSine);
            return;
        }
        else if ((SceneChanger.goingTo == SceneChanger.Scene.Homepage) && (SceneChanger.goingFrom == SceneChanger.Scene.Settings)){
            sceneHeader.SlideOut();
            float horizOffset = (settingsPaper1.rect.width / 2) + (canvasWidth / 2);
            customVal1 = -horizOffset;
            customVal2 = horizOffset;
            MoveObjectToXCustom1(settingsPaper1);
            StaticVariables.WaitTimeThenCallFunction(0.1f, MoveObjectToXCustom2, settingsPaper2);
            StaticVariables.WaitTimeThenCallFunction(0.15f, MoveObjectToXCustom1, settingsPaper3);
            StaticVariables.WaitTimeThenCallFunction(0.2f, MoveObjectToXCustom2, settingsPaper4);
            StaticVariables.WaitTimeThenCallFunction(0.25f, MoveObjectToXCustom1, settingsPaper5);
            StaticVariables.WaitTimeThenCallFunction(0.3f, MoveObjectToXCustom2, settingsPaper6);
            StaticVariables.WaitTimeThenCallFunction(0.8f, TriggerSceneChange);
            return;
        }
    }

    public void AnimateComingIntoScene(){
        clickBlocker.SetActive(true);
        if (SceneChanger.goingTo == SceneChanger.Scene.Settings){
            //assume all settings papers are the same width (should be 1440, the min screen width)
            float horizOffset = (settingsPaper1.rect.width / 2) + (canvasWidth / 2);
            settingsPaper1.localPosition = new Vector2(-horizOffset, settingsPaper1.localPosition.y);
            settingsPaper2.localPosition = new Vector2(horizOffset, settingsPaper2.localPosition.y);
            settingsPaper3.localPosition = new Vector2(-horizOffset, settingsPaper3.localPosition.y);
            settingsPaper4.localPosition = new Vector2(horizOffset, settingsPaper4.localPosition.y);
            settingsPaper5.localPosition = new Vector2(-horizOffset, settingsPaper5.localPosition.y);
            settingsPaper6.localPosition = new Vector2(horizOffset, settingsPaper6.localPosition.y);
            MoveObjectToX0(settingsPaper1);
            StaticVariables.WaitTimeThenCallFunction(0.1f, MoveObjectToX0, settingsPaper2);
            StaticVariables.WaitTimeThenCallFunction(0.2f, MoveObjectToX0, settingsPaper3);
            StaticVariables.WaitTimeThenCallFunction(0.3f, MoveObjectToX0, settingsPaper4);
            StaticVariables.WaitTimeThenCallFunction(0.4f, MoveObjectToX0, settingsPaper5);
            StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToX0, settingsPaper6);
            StaticVariables.WaitTimeThenCallFunction(0.5f, sceneHeader.SlideIn);
            StaticVariables.WaitTimeThenCallFunction(1f, EnableClicks);
            return;
        }
        else if ((SceneChanger.goingTo == SceneChanger.Scene.Homepage) && (SceneChanger.goingFrom == SceneChanger.Scene.Settings)){
            settingsPage.gameObject.SetActive(true);
            float pos = settingsPage.localPosition.x;
            settingsPage.localPosition = new Vector2(0, settingsPage.localPosition.y);
            settingsPage.DOLocalMoveX(pos, 0.5f).SetEase(Ease.InSine);
            customVal1 = settingsScroll.localPosition.x;
            float horizOffset = (settingsScroll.rect.width / 2) + (canvasWidth / 2);
            settingsScroll.localPosition = new Vector2(horizOffset, settingsScroll.localPosition.y);
            StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToXCustom1InSine, settingsScroll);
            StaticVariables.WaitTimeThenCallFunction(1f, EnableClicks);
            return;
        }
    }

    private void SetCanvasSizes(){
        canvasHeight = canvas.rect.height;
        canvasWidth = canvas.rect.width;
        
    }

    private void SetUIElementSizes(){
        settingsPage.sizeDelta = new Vector2(canvasWidth + 500, canvasHeight);
        settingsPage.localPosition = new Vector2(canvasWidth + 500, 0);
    }

    private void MoveObjectToX0(RectTransform obj){
        obj.DOLocalMoveX(0, 0.5f);
    }

    private void MoveObjectToX0OutSine(RectTransform obj){
        obj.DOLocalMoveX(0, 0.5f).SetEase(Ease.OutSine);
    }

    private void MoveObjectToX0InSine(RectTransform obj){
        obj.DOLocalMoveX(0, 0.5f).SetEase(Ease.InSine);
    }

    private void MoveObjectToXCustom1(RectTransform obj){
        obj.DOLocalMoveX(customVal1, 0.5f);
    }

    private void MoveObjectToXCustom1InSine(RectTransform obj){
        obj.DOLocalMoveX(customVal1, 0.5f).SetEase(Ease.InSine);
    }

    private void MoveObjectToXCustom2(RectTransform obj){
        obj.DOLocalMoveX(customVal2, 0.5f);
    }

    private void EnableClicks(){
        clickBlocker.SetActive(false);
    }

    private void TriggerSceneChange(){
        SceneChanger.LoadScene();
    }
}