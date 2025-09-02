using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneMoveBackAndForth : MonoBehaviour{
    public Vector2 destination1;
    public Vector2 destination2;
    public float moveTime;
   // private Vector2 startingPoint;
    private bool isAt1 = true;

    void Start(){
        //Vector2 startingPoint = transform.localPosition;
        //transform.DOLocalMove(destination, moveTime);
        StartMove();
    }

    private void StartMove() {
        if (isAt1)
            transform.DOLocalMove(destination1, moveTime).SetEase(Ease.InOutSine).OnComplete(StartMove);
        else
            transform.DOLocalMove(destination2, moveTime).SetEase(Ease.InOutSine).OnComplete(StartMove);
        isAt1 = !isAt1;

    }
    
}
