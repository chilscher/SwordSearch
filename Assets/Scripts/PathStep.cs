//for Sword Search, copyright Fancy Bus Games 2026

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using MyBox;

public class PathStep : MonoBehaviour{
    public Image backgroundImage;
    public Image arrowImage;
    public bool isDestination = false;
    [HideInInspector]
    public bool isPlayerAtStep = false;

    public void HideStep(float duration){
        Color im1Color = backgroundImage.color;
        Color im2Color = arrowImage.color;

        im1Color.a = 0;
        im2Color.a = 0;

        backgroundImage.DOColor(im1Color, duration);
        arrowImage.DOColor(im2Color, duration);
    }

    public void ShowStep(float duration){
        Color im1Color = backgroundImage.color;
        Color im2Color = arrowImage.color;

        im1Color.a = 1;
        im2Color.a = 1;

        backgroundImage.DOColor(im1Color, duration);
        arrowImage.DOColor(im2Color, duration);
    }

}

