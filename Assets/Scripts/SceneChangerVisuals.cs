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
    public HomepageManager homepageManager;
    public RectTransform settingsFolder;
    public RectTransform settingsPage;
    public RectTransform atlasSmall;
    public RectTransform atlasBig;
    public RectTransform atlasBigWorlds;
    public Transform hometownPreview;
    public Transform grasslandsPreview;
    public Transform forestPreview;
    public Transform desertPreview;
    public Transform duskvalePreview;
    public Transform frostlandsPreview;
    public Transform cavernsPreview;
    public Transform endlessModeEnemies;
    [Header("Overworlds and Atlas")]
    public OverworldSceneManager overworldManager;
    public RectTransform fakeHomepage;
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
        if (SceneChanger.goingTo == SceneChanger.Scene.None){ //when launching the game for the first time
            clickBlocker.SetActive(false);
            homepageManager.DisplayProgress();
            homepageManager.ShowEndlessModeEnemies();
        }
        else{
            PrepComingIntoScene();
            StaticVariables.WaitTimeThenCallFunction(0.2f, AnimateComingIntoScene);
        }
    }

    private void PrepComingIntoScene(){
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
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Settings, SceneChanger.Scene.Homepage)){
            homepageManager.DisplayProgress();
            homepageManager.ShowEndlessModeEnemies();
            settingsPage.gameObject.SetActive(true);
            customVal2 = settingsPage.localPosition.x;
            settingsPage.localPosition = new Vector2(0, settingsPage.localPosition.y);
            customVal1 = settingsFolder.localPosition.x;
            float horizOffset = (settingsFolder.rect.width / 2) + (canvasWidth / 2);
            settingsFolder.localPosition = new Vector2(horizOffset, settingsFolder.localPosition.y);
            return;
        }
        else if (CheckSceneChange(null, SceneChanger.Scene.Credits)){
            creditsPaper.localPosition = new Vector2(creditsPaper.localPosition.x, -creditsPaper.rect.height);
            return;
        }
        else if (CheckSceneChange(null, SceneChanger.Scene.Atlas)){
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Atlas, SceneChanger.Scene.Homepage)){
            homepageManager.DisplayProgress();
            homepageManager.ShowEndlessModeEnemies();
            atlasBig.gameObject.SetActive(true);
            customVal2 = atlasBig.localPosition.x;
            atlasBig.localPosition = new Vector2(0, atlasBig.localPosition.y);
            customVal1 = atlasSmall.localPosition.x;
            float horizOffset = (atlasSmall.rect.width / 2) + (canvasWidth / 2);
            atlasSmall.localPosition = new Vector2(-horizOffset, atlasSmall.localPosition.y);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Homepage, SceneChanger.Scene.World)){
            fakeHomepage.gameObject.SetActive(true);
            overworldManager.HideProgress();
            customVal1 = overworldManager.stageIndexToQuickReveal;
            if (customVal1 == - 1)
                customVal1 = StaticVariables.highestBeatenStage.nextStage.index;
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Atlas, SceneChanger.Scene.World)){
            overworldManager.HideProgress();
            customVal1 = overworldManager.stageIndexToQuickReveal;
            if (customVal1 == - 1)
                customVal1 = StaticVariables.highestBeatenStage.nextStage.index;
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.World, SceneChanger.Scene.Homepage)){
            homepageManager.DisplayProgress();
            homepageManager.ShowEndlessModeEnemies();
            return;
        }
    }

    private void AnimateComingIntoScene(){
        if (CheckSceneChange(null, SceneChanger.Scene.Settings)){
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
            settingsPage.DOLocalMoveX(customVal2, 0.5f).SetEase(Ease.InSine);
            AudioManager.PlaySound(AudioManager.library.settingsPageMoveOut);
            StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToXCustom1OutSine, settingsFolder);
            StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.settingsFolderMoveIn);
            StaticVariables.WaitTimeThenCallFunction(1f, EnableClicks);
            return;
        }
        else if (CheckSceneChange(null, SceneChanger.Scene.Credits)){
            creditsPaper.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutSine);
            AudioManager.PlaySound(AudioManager.library.creditsPaperMoveIn);
            StaticVariables.WaitTimeThenCallFunction(0.5f, sceneHeader.SlideIn);
            StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.headerMoveIn);
            StaticVariables.WaitTimeThenCallFunction(1f, EnableClicks);
            return;
        }
        else if (CheckSceneChange(null, SceneChanger.Scene.Atlas)){
            StaticVariables.WaitTimeThenCallFunction(1f, sceneHeader.SlideIn);
            StaticVariables.WaitTimeThenCallFunction(1f, AudioManager.PlaySound, AudioManager.library.headerMoveIn);
            StaticVariables.WaitTimeThenCallFunction(1.5f, EnableClicks);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Atlas, SceneChanger.Scene.Homepage)){
            atlasBig.DOLocalMoveX(customVal2, 0.5f).SetEase(Ease.InSine);
            AudioManager.PlaySound(AudioManager.library.settingsPageMoveOut);
            StaticVariables.WaitTimeThenCallFunction(0.5f, MoveObjectToXCustom1OutSine, atlasSmall);
            StaticVariables.WaitTimeThenCallFunction(0.5f, AudioManager.PlaySound, AudioManager.library.settingsFolderMoveIn);
            StaticVariables.WaitTimeThenCallFunction(1f, EnableClicks);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Homepage, SceneChanger.Scene.World)){
            ScaleUpFakeHomepage();
            StaticVariables.WaitTimeThenCallFunction(1f, overworldManager.ShowOverworldProgress, (int)customVal1);
            //overworld scene manager will call FinishEnteringOverworld when everything is finished popping in
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Atlas, SceneChanger.Scene.World)){
            StaticVariables.WaitTimeThenCallFunction(1.5f, overworldManager.ShowOverworldProgress, (int)customVal1);
            //overworld scene manager will call FinishEnteringOverworld when everything is finished popping in
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.World, SceneChanger.Scene.Homepage)){
            if (homepageManager.continueHometown.activeSelf)
                FadeInChildren(hometownPreview, 1f);
            if (homepageManager.continueGrasslands.activeSelf)
                FadeInChildren(grasslandsPreview, 1f);
            if (homepageManager.continueForest.activeSelf)
                FadeInChildren(forestPreview, 1f);
            if (homepageManager.continueDesert.activeSelf)
                FadeInChildren(desertPreview, 1f);
            if (homepageManager.continueCity.activeSelf)
                FadeInChildren(duskvalePreview, 1f);
            if (homepageManager.continueFrostlands.activeSelf)
                FadeInChildren(frostlandsPreview, 1f);
            if (homepageManager.continueCaverns.activeSelf)
                FadeInChildren(cavernsPreview, 1f);
            FadeInChildren(endlessModeEnemies, 1f);
            StaticVariables.WaitTimeThenCallFunction(0.5f, EnableClicks);
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
        else if (CheckSceneChange(SceneChanger.Scene.Atlas, SceneChanger.Scene.World)){
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
        else if (CheckSceneChange(SceneChanger.Scene.World, SceneChanger.Scene.Homepage)){
            sceneHeader.SlideOut();
            AudioManager.PlaySound(AudioManager.library.headerMoveOut);
            foreach (OverworldSpace os in overworldManager.overworldSpaces){
                foreach (PathStep ps in os.steps)
                    ps.HideStep(0.5f);
                os.HideEnemy(0.5f);
            }
            overworldManager.FadeOutPlayer(0.5f);
            StaticVariables.WaitTimeThenCallFunction(0.5f, ScaleDownFakeHomepage);
            StaticVariables.WaitTimeThenCallFunction(1f, TriggerSceneChange);
            return;
        }
        else if (CheckSceneChange(SceneChanger.Scene.Homepage, SceneChanger.Scene.World)){
            if (homepageManager.continueHometown.activeSelf)
                FadeOutChildren(hometownPreview, 1f);
            if (homepageManager.continueGrasslands.activeSelf)
                FadeOutChildren(grasslandsPreview, 1f);
            if (homepageManager.continueForest.activeSelf)
                FadeOutChildren(forestPreview, 1f);
            if (homepageManager.continueDesert.activeSelf)
                FadeOutChildren(desertPreview, 1f);
            if (homepageManager.continueCity.activeSelf)
                FadeOutChildren(duskvalePreview, 1f);
            if (homepageManager.continueFrostlands.activeSelf)
                FadeOutChildren(frostlandsPreview, 1f);
            if (homepageManager.continueCaverns.activeSelf)
                FadeOutChildren(cavernsPreview, 1f);
            FadeOutChildren(endlessModeEnemies, 1f);
            StaticVariables.WaitTimeThenCallFunction(1f, TriggerSceneChange);
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

    public void FinishEnteringOverworld(){
        sceneHeader.SlideIn();
        AudioManager.PlaySound(AudioManager.library.headerMoveIn);
        fakeHomepage.gameObject.SetActive(false);
        StaticVariables.WaitTimeThenCallFunction(0.5f, EnableClicks);
    }

    private void FadeInChildren(Transform t, float duration){
        foreach (Transform t2 in t)
            FadeInSelfAndChildren(t2, duration);
    }

    private void FadeInSelfAndChildren(Transform t, float duration){
        Image im = t.GetComponent<Image>();
        if (im != null){
            Color c1 = im.color;
            Color c2 = im.color;
            c1.a = 0;

            im.color = c1;
            im.DOColor(c2, duration);
        }
        Animator anim = t.GetComponent<Animator>();
        if (anim != null){
            anim.enabled = false;
            StaticVariables.WaitTimeThenCallFunction(duration, EnableAnimator, anim);
        }
        foreach (Transform t2 in t)
            FadeInSelfAndChildren(t2, duration);
    }

    private void FadeOutChildren(Transform t, float duration){
        foreach (Transform t2 in t)
            FadeOutSelfAndChildren(t2, duration);
    }

    private void FadeOutSelfAndChildren(Transform t, float duration){
        Image im = t.GetComponent<Image>();
        if (im != null){
            Color c1 = im.color;
            Color c2 = im.color;
            c1.a = 0;

            im.color = c2;
            im.DOColor(c1, duration);
        }
        Animator anim = t.GetComponent<Animator>();
        if (anim != null)
            anim.enabled = false;
        foreach (Transform t2 in t)
            FadeOutSelfAndChildren(t2, duration);
    }

    private void ScaleUpFakeHomepage(){
        fakeHomepage.DOScale(Vector3.one * 6.6f, 1f).SetEase(Ease.InSine);
    }

    private void EnableAnimator(Animator animator){
        animator.enabled = true;
    }

    private void ScaleDownFakeHomepage(){
        fakeHomepage.gameObject.SetActive(true);
        fakeHomepage.localScale = Vector3.one * 13;
        fakeHomepage.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutSine);  
    }
}