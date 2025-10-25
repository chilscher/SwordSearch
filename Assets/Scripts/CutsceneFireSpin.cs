using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneFireSpin : MonoBehaviour{

    
    public enum StartingPosition{Left, Bottom, Right, Top};
    public enum RotationDirection{Clockwise, Counterclockwise};
    public StartingPosition startingPosition;
    public RotationDirection rotationDirection;
    public Image image;
    private Vector3 leftPos = new(-1,0,0);
    private Vector3 rightPos = new(1,0,0);
    private Vector3 topPos = new(0,1,0);
    private Vector3 bottomPos = new(0,-1,0);

    private void Start() {
        FadeIn();
        SetStartingPos();
        NextSpinStep();
    }
    
    private void FadeIn(){
        Color c = Color.white;
        c.a = 0;
        image.color = c;
        image.DOColor(Color.white, 0.5f);
    }
    
    private void SetStartingPos(){
        float offset = 0;
        float xPos = image.transform.localPosition.x;
        float yPos = image.transform.localPosition.y;
        float x2 = Mathf.Abs(xPos);
        float y2 = Mathf.Abs(yPos);
        offset = x2;
        if (y2 > x2)
            offset = y2;
        leftPos *= offset;
        rightPos *= offset;
        topPos *= offset;
        bottomPos *= offset;
        switch (startingPosition){
            case (StartingPosition.Left):
                image.transform.localPosition = leftPos;
                break;
            case (StartingPosition.Right):
                image.transform.localPosition = rightPos;
                break;
            case (StartingPosition.Top):
                image.transform.localPosition = topPos;
                break;
            case (StartingPosition.Bottom):
                image.transform.localPosition = bottomPos;
                break;
        }
    }
    
    private void NextSpinStep(){
        //assumes this function is only called when the flame is at one of the cardinal points
        StartingPosition currentPosition = StartingPosition.Left;
        float leftDist = Vector3.Distance(image.transform.localPosition, leftPos);
        float rightDist = Vector3.Distance(image.transform.localPosition, rightPos);
        float topDist = Vector3.Distance(image.transform.localPosition, topPos);
        float bottomDist = Vector3.Distance(image.transform.localPosition, bottomPos);
        if (leftDist < rightDist && leftDist < topDist && leftDist < bottomDist)
            currentPosition = StartingPosition.Left;
        if (rightDist < leftDist && rightDist < topDist && rightDist < bottomDist)
            currentPosition = StartingPosition.Right;
        if (topDist < rightDist && topDist < leftDist && topDist < bottomDist)
            currentPosition = StartingPosition.Top;
        if (bottomDist < rightDist && bottomDist < topDist && bottomDist < leftDist)
            currentPosition = StartingPosition.Bottom;
        switch (currentPosition){
            case (StartingPosition.Left):
                if (rotationDirection == RotationDirection.Clockwise)
                    MoveToSide(topPos, false);
                else
                    MoveToSide(bottomPos, false);
                break;
            case (StartingPosition.Right):
                if (rotationDirection == RotationDirection.Clockwise)
                    MoveToSide(bottomPos, false);
                else
                    MoveToSide(topPos, false);
                break;
            case (StartingPosition.Top):
                if (rotationDirection == RotationDirection.Clockwise)
                    MoveToSide(rightPos, true);
                else
                    MoveToSide(leftPos, true);
                break;
            case (StartingPosition.Bottom):
                if (rotationDirection == RotationDirection.Clockwise)
                    MoveToSide(leftPos, true);
                else
                    MoveToSide(rightPos, true);
                break;
        }

    }
    
    private void MoveToSide(Vector3 side, bool flipSines){
        //image.transform.DOShapeCircle
        Ease e1 = Ease.InSine;
        Ease e2 = Ease.OutSine;
        if (flipSines){
            e1 = Ease.OutSine;
            e2 = Ease.InSine;
        }
        image.transform.DOLocalMoveX(side.x, 0.5f).SetEase(e1);
        image.transform.DOLocalMoveY(side.y, 0.5f).SetEase(e2).OnComplete(NextSpinStep);
    }

}
