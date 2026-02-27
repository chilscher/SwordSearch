//for Sword Search, copyright Fancy Bus Games 2026

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

public class SceneChangerVisuals : MonoBehaviour {

    public SceneChanger.Scene thisScene;
    public RectTransform canvas;
    public GameObject clickBlocker;   
    public SceneHeader sceneHeader;
    [Header("Only for the Homepage Scene")]
    public RectTransform settingsFolder;
    public RectTransform settingsPage;
    public RectTransform atlasSmall;
    public RectTransform atlasBig;
    public RectTransform atlasBigWorlds;
    [Header("Overworlds and Atlas")]
    public OverworldSceneManager overworldManager;
    [Header("Only for the Settings Scene")]
    public RectTransform settingsPaper1;
    public RectTransform settingsPaper2;
    public RectTransform settingsPaper3;
    public RectTransform settingsPaper4;
    public RectTransform settingsPaper5;
    public RectTransform settingsPaper6;
    [Header("Only for the Credits Scene")]
    public RectTransform creditsPaper;

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

    public void AnimateComingIntoScene(){
        clickBlocker.SetActive(true);
        if (CheckSceneChange(null, SceneChanger.Scene.Settings)){
            //assume all settings papers are the same width (should be 1440, the min screen width)
            float horizOffset = (settingsPaper1.rect.width / 2) + (canvasWidth / 2);
            settingsPaper1.localPosition = new Vector2(-horizOffset, settingsPaper1.localPosition.y);
            settingsPaper2.localPosition = new Vector2(horizOffset, settingsPaper2.localPosition.y);
            settingsPaper3.localPosition = new Vector2(-horizOffset, settingsPaper3.localPosition.y);
            settingsPaper4.localPosition = new Vector2(-horizOffset, settingsPaper4.localPosition.y);
            settingsPaper5.localPosition = new Vector2(horizOffset, settingsPaper5.localPosition.y);
            settingsPaper6.localPosition = new Vector2(-horizOffset, settingsPaper6.localPosition.y);
            MoveObjectToX0(settingsPaper1);
            AudioManager.PlaySound(AudioManager.library.settingsScrapMoveIn);
            StaticVariables.WaitTimeThenCallFunction(0.1f, MoveObjectToX0, settingsPaper2);
            StaticVariables.WaitTimeThenCallFunction(0.1f, AudioManager.PlaySound, AudioManager.library.settingsScrapMoveIn);
            StaticVariables.WaitTimeThenCallFunction(0.2f, MoveObjectToX0, settingsPaper3);
            StaticVariables.WaitTimeThenCallFunction(0.2f, AudioManager.PlaySound, AudioManager.library.settingsScrapMoveIn);
            StaticVariables.WaitTimeThenCallFunction(0.3f, MoveObjectToX0, settingsPaper4);
            StaticVariables.WaitTimeThenCallFunction(0.3f, AudioManager.PlaySound, AudioManager.library.settingsScrapMoveIn);
            StaticVariables.WaitTimeThenCallFunction(0.4f, MoveObjectToX0, settingsPaper5);
            StaticVariables.WaitTimeThenCallFunction(0.4f, AudioManager.PlaySound, AudioManager.library.settingsScrapMoveIn);
            StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToX0, settingsPaper6);
            StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.settingsScrapMoveIn);
            StaticVariables.WaitTimeThenCallFunction(0.5f, sceneHeader.SlideIn);
            StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.headerMoveIn);
            StaticVariables.WaitTimeThenCallFunction(1f, EnableClicks);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Settings, SceneChanger.Scene.Homepage)){
            settingsPage.gameObject.SetActive(true);
            float pos = settingsPage.localPosition.x;
            settingsPage.localPosition = new Vector2(0, settingsPage.localPosition.y);
            settingsPage.DOLocalMoveX(pos, 0.5f).SetEase(Ease.InSine);
            AudioManager.PlaySound(AudioManager.library.settingsPageMoveOut);
            customVal1 = settingsFolder.localPosition.x;
            float horizOffset = (settingsFolder.rect.width / 2) + (canvasWidth / 2);
            settingsFolder.localPosition = new Vector2(horizOffset, settingsFolder.localPosition.y);
            StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToXCustom1OutSine, settingsFolder);
            StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.settingsFolderMoveIn);
            StaticVariables.WaitTimeThenCallFunction(1f, EnableClicks);
            return;
        }
        else if (CheckSceneChange(null, SceneChanger.Scene.Credits)){
            creditsPaper.localPosition = new Vector2(creditsPaper.localPosition.x, -creditsPaper.rect.height);
            creditsPaper.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutSine);
            AudioManager.PlaySound(AudioManager.library.creditsPaperMoveIn);
            StaticVariables.WaitTimeThenCallFunction(0.5f, sceneHeader.SlideIn);
            StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.headerMoveIn);
            StaticVariables.WaitTimeThenCallFunction(1f, EnableClicks);
            return;
        }
        else if (CheckSceneChange(null, SceneChanger.Scene.Atlas)){
            StaticVariables.WaitTimeThenCallFunction(3f, sceneHeader.SlideIn);
            StaticVariables.WaitTimeThenCallFunction(3f, AudioManager.PlaySound, AudioManager.library.headerMoveIn);
            StaticVariables.WaitTimeThenCallFunction(3.5f, EnableClicks);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Atlas, SceneChanger.Scene.Homepage)){
            atlasBig.gameObject.SetActive(true);
            float pos = atlasBig.localPosition.x;
            atlasBig.localPosition = new Vector2(0, atlasBig.localPosition.y);
            atlasBig.DOLocalMoveX(pos, 0.5f).SetEase(Ease.InSine);
            AudioManager.PlaySound(AudioManager.library.settingsPageMoveOut);
            customVal1 = atlasSmall.localPosition.x;
            float horizOffset = (atlasSmall.rect.width / 2) + (canvasWidth / 2);
            atlasSmall.localPosition = new Vector2(-horizOffset, atlasSmall.localPosition.y);
            StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToXCustom1OutSine, atlasSmall);
            StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.settingsFolderMoveIn);
            StaticVariables.WaitTimeThenCallFunction(1f, EnableClicks);
            return;
        }
        else if (CheckSceneChange(null, SceneChanger.Scene.Hometown)){
            StaticVariables.WaitTimeThenCallFunction(3f, sceneHeader.SlideIn);
            StaticVariables.WaitTimeThenCallFunction(3f, AudioManager.PlaySound, AudioManager.library.headerMoveIn);
            StaticVariables.WaitTimeThenCallFunction(3.5f, EnableClicks);
            //atlasBig.gameObject.SetActive(true);
            //float pos = atlasBig.localPosition.x;
            //atlasBig.localPosition = new Vector2(0, atlasBig.localPosition.y);
            //atlasBig.DOLocalMoveX(pos, 0.5f).SetEase(Ease.InSine);
            //AudioManager.PlaySound(AudioManager.library.settingsPageMoveOut);
            //customVal1 = atlasSmall.localPosition.x;
            //float horizOffset = (atlasSmall.rect.width / 2) + (canvasWidth / 2);
            //atlasSmall.localPosition = new Vector2(-horizOffset, atlasSmall.localPosition.y);
            //StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToXCustom1OutSine, atlasSmall);
            //StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.settingsFolderMoveIn);
            //StaticVariables.WaitTimeThenCallFunction(1f, EnableClicks);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Hometown, SceneChanger.Scene.Homepage)){
            ///settingsPage.gameObject.SetActive(true);
            //float pos = settingsPage.localPosition.x;
            //settingsPage.localPosition = new Vector2(0, settingsPage.localPosition.y);
            //settingsPage.DOLocalMoveX(pos, 0.5f).SetEase(Ease.InSine);
            //AudioManager.PlaySound(AudioManager.library.settingsPageMoveOut);
            //customVal1 = settingsFolder.localPosition.x;
            //float horizOffset = (settingsFolder.rect.width / 2) + (canvasWidth / 2);
            //settingsFolder.localPosition = new Vector2(horizOffset, settingsFolder.localPosition.y);
            //StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToXCustom1OutSine, settingsFolder);
            //StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.settingsFolderMoveIn);
            StaticVariables.WaitTimeThenCallFunction(0.1f, EnableClicks);
            return;
        }
    }

    public void AnimateLeavingScene(){
        clickBlocker.SetActive(true);
        if (CheckSceneChange(SceneChanger.Scene.Homepage, SceneChanger.Scene.Settings)){
            settingsPage.gameObject.SetActive(true);
            float horizOffset = (settingsFolder.rect.width / 2) + (canvasWidth / 2);
            settingsFolder.DOLocalMoveX(horizOffset, 0.5f).SetEase(Ease.InSine);
            AudioManager.PlaySound(AudioManager.library.settingsFolderMoveOut);
            StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToX0OutSine, settingsPage);
            StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.settingsPageMoveIn);
            StaticVariables.WaitTimeThenCallFunction(1f, TriggerSceneChange);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Settings, null)){
            sceneHeader.SlideOut();
            AudioManager.PlaySound(AudioManager.library.headerMoveOut);
            float horizOffset = (settingsPaper1.rect.width / 2) + (canvasWidth / 2);
            customVal1 = -horizOffset;
            customVal2 = horizOffset;
            MoveObjectToXCustom1(settingsPaper1);
            AudioManager.PlaySound(AudioManager.library.settingsScrapMoveOut);
            StaticVariables.WaitTimeThenCallFunction(0.1f, MoveObjectToXCustom2, settingsPaper2);
            StaticVariables.WaitTimeThenCallFunction(0.1f, AudioManager.PlaySound, AudioManager.library.settingsScrapMoveOut);
            StaticVariables.WaitTimeThenCallFunction(0.15f, MoveObjectToXCustom1, settingsPaper3);
            StaticVariables.WaitTimeThenCallFunction(0.15f, AudioManager.PlaySound, AudioManager.library.settingsScrapMoveOut);
            StaticVariables.WaitTimeThenCallFunction(0.2f, MoveObjectToXCustom1, settingsPaper4);
            StaticVariables.WaitTimeThenCallFunction(0.2f, AudioManager.PlaySound, AudioManager.library.settingsScrapMoveOut);
            StaticVariables.WaitTimeThenCallFunction(0.25f, MoveObjectToXCustom2, settingsPaper5);
            StaticVariables.WaitTimeThenCallFunction(0.25f, AudioManager.PlaySound, AudioManager.library.settingsScrapMoveOut);
            StaticVariables.WaitTimeThenCallFunction(0.3f, MoveObjectToXCustom1, settingsPaper6);
            StaticVariables.WaitTimeThenCallFunction(0.3f, AudioManager.PlaySound, AudioManager.library.settingsScrapMoveOut);
            StaticVariables.WaitTimeThenCallFunction(0.8f, TriggerSceneChange);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Credits, null)){
            sceneHeader.SlideOut();
            AudioManager.PlaySound(AudioManager.library.headerMoveOut);
            creditsPaper.DOLocalMoveY(-creditsPaper.rect.height, 0.5f).SetEase(Ease.InSine);
            AudioManager.PlaySound(AudioManager.library.creditsPaperMoveOut);
            StaticVariables.WaitTimeThenCallFunction(0.5f, TriggerSceneChange);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Homepage, SceneChanger.Scene.Atlas)){
            atlasBig.gameObject.SetActive(true);
            float horizOffset = (atlasSmall.rect.width / 2) + (canvasWidth / 2);
            atlasSmall.DOLocalMoveX(-horizOffset, 0.5f).SetEase(Ease.InSine);
            AudioManager.PlaySound(AudioManager.library.atlasSmallMoveOut);
            StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToX0OutSine, atlasBig);
            StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.atlasBigMoveIn);
            StaticVariables.WaitTimeThenCallFunction(1f, TriggerSceneChange);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Atlas, SceneChanger.Scene.Homepage)){
            sceneHeader.SlideOut();
            AudioManager.PlaySound(AudioManager.library.headerMoveOut);
            foreach (OverworldSpace os in overworldManager.overworldSpaces){
                foreach (PathStep ps in os.steps)
                    ps.HideStep(0.5f);
            }
            overworldManager.FadeOutPlayer(0.5f);
            StaticVariables.WaitTimeThenCallFunction(0.5f, TriggerSceneChange);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Atlas, SceneChanger.Scene.Hometown)){
            sceneHeader.SlideOut();
            AudioManager.PlaySound(AudioManager.library.headerMoveOut);
            foreach (OverworldSpace os in overworldManager.overworldSpaces){
                foreach (PathStep ps in os.steps)
                    ps.HideStep(0.5f);
            }
            overworldManager.FadeOutPlayer(0.5f);
            StaticVariables.WaitTimeThenCallFunction(0.5f, TriggerSceneChange);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Hometown, SceneChanger.Scene.Homepage)){
            sceneHeader.SlideOut();
            AudioManager.PlaySound(AudioManager.library.headerMoveOut);
            foreach (OverworldSpace os in overworldManager.overworldSpaces){
                foreach (PathStep ps in os.steps)
                    ps.HideStep(0.5f);
            }
            overworldManager.FadeOutPlayer(0.5f);
            StaticVariables.WaitTimeThenCallFunction(0.5f, TriggerSceneChange);
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
        atlasBig.localPosition = new Vector2(- ((canvasWidth /2) + (atlasBig.rect.width/2)), 0);
        for (int i = StaticVariables.highestBeatenStage.nextStage.world; i < atlasBigWorlds.childCount; i++)
            atlasBigWorlds.GetChild(i).gameObject.SetActive(false);
    }

    private void MoveObjectToX0(RectTransform obj){
        obj.DOLocalMoveX(0, 0.5f);
    }

    private void MoveObjectToY0(RectTransform obj){
        obj.DOLocalMoveY(0, 0.5f);
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

    private void MoveObjectToYCustom1(RectTransform obj){
        obj.DOLocalMoveY(customVal1, 0.5f);
    }

    private void MoveObjectToXCustom1InSine(RectTransform obj){
        obj.DOLocalMoveX(customVal1, 0.5f).SetEase(Ease.InSine);
    }

    private void MoveObjectToXCustom1OutSine(RectTransform obj){
        obj.DOLocalMoveX(customVal1, 0.5f).SetEase(Ease.OutSine);
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

    private bool CheckSceneChange(SceneChanger.Scene? from, SceneChanger.Scene? to){
        if (from == null)
            return (to == SceneChanger.goingTo);
        if (to == null)
            return (from == SceneChanger.goingFrom);
        return ((from == SceneChanger.goingFrom) && (to == SceneChanger.goingTo));
    }
}