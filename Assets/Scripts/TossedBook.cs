using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TossedBook : MonoBehaviour {
    public Transform origin;
    public Transform peak; //only used for its height
    public Transform destination;
    private float horizontalSpeed = 500f;
    public Transform book;
    private float duration = 0f;
    public Image image;
    public bool destroySelfAfter = false;
    
    public void Hide(){
        origin.gameObject.SetActive(false);
        peak.gameObject.SetActive(false);
        destination.gameObject.SetActive(false);
        book.gameObject.SetActive(false);
    }
    
    public void StartThrow(Sprite bookSprite, bool isFlipped){
        book.gameObject.SetActive(true);
        image.sprite = bookSprite;
        book.localPosition = origin.localPosition;
        float startingRotation = StaticVariables.rand.Next(270, 450);
        book.Rotate(new Vector3 (0,0,startingRotation));
        if (isFlipped)
            book.transform.localScale = new Vector3(-1, 1, 1);
        
        float xDistance = origin.localPosition.x - destination.localPosition.x;
        duration = xDistance / horizontalSpeed;

        book.DOLocalMoveX(destination.localPosition.x, duration).SetEase(Ease.Linear);
        book.DOLocalMoveY(peak.localPosition.y, duration * .35f).SetEase(Ease.OutSine).OnComplete(MakeFall);

        int rotation = 720;
        book.DORotate(new Vector3(0, 0, rotation), duration, RotateMode.FastBeyond360);
    }
    
    private void MakeFall(){
        book.DOLocalMoveY(destination.localPosition.y, duration * .65f).SetEase(Ease.InSine).OnComplete(DestroySelf);
    }

    private void DestroySelf() {
        if (destroySelfAfter)
            Destroy(gameObject);
    }
}
