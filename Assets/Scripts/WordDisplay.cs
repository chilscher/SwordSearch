using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordDisplay : MonoBehaviour {

    public Text text;
    [HideInInspector]
    public List<LetterSpace> letterSpacesForWord = new List<LetterSpace>(){};

    private LetterSpace lastLetterSpace = null;
    private LetterSpace secondToLastLetterSpace = null;

    public BattleManager battleManager;
    Color validWordColor;
    Color invalidWordColor;
    Color validButtonColor;
    Color invalidButtonColor;

    public GameObject sendWordButton;

    void Start() {
        text.text = "";
        ColorUtility.TryParseHtmlString("#4B4B4B", out validWordColor);
        ColorUtility.TryParseHtmlString("#C8C8C8", out invalidWordColor);
        ColorUtility.TryParseHtmlString("#8DE1FF", out validButtonColor);
        ColorUtility.TryParseHtmlString("#B34A50", out invalidButtonColor);
        
        CheckIfWordIsValid();
    }
    public void AddLetter(LetterSpace ls){
        text.text += ls.letter;
        letterSpacesForWord.Add(ls);
        if (lastLetterSpace != null){
            lastLetterSpace.nextLetterSpace = ls;
            ls.previousLetterSpace = lastLetterSpace;
        }
        UpdateNeighborsForLastTwoLetterSpaces();
        CheckIfWordIsValid();
    }

    public void RemoveLetter(LetterSpace ls){
        //assumes the last letter in the list is the provided letter
        if (text.text.Length < 2)
            text.text = "";
        else
            text.text = text.text.Substring(0, (text.text.Length - 1));
        letterSpacesForWord.Remove(ls);
        if (secondToLastLetterSpace != null){
            secondToLastLetterSpace.nextLetterSpace = null;
            lastLetterSpace.previousLetterSpace = null;
        }
        UpdateNeighborsForLastTwoLetterSpaces();
        CheckIfWordIsValid();
    }

    private void UpdateNeighborsForLastTwoLetterSpaces(){
        SetLastTwoLetterSpaces();
        if (lastLetterSpace != null)
            lastLetterSpace.ShowDirectionsToNeighbors();
        if (secondToLastLetterSpace != null)
            secondToLastLetterSpace.ShowDirectionsToNeighbors();
    }
    private void SetLastTwoLetterSpaces(){
        lastLetterSpace = null;
        secondToLastLetterSpace = null;
        if (letterSpacesForWord.Count > 0)
            lastLetterSpace = letterSpacesForWord[letterSpacesForWord.Count - 1];
        if (letterSpacesForWord.Count > 1)
            secondToLastLetterSpace = letterSpacesForWord[letterSpacesForWord.Count - 2];

    }

    public bool CanAddLetter(LetterSpace letterSpace){
        if (letterSpacesForWord.Contains(letterSpace))
            return false;
        if (letterSpacesForWord.Count == 0)
            return true;
        if (letterSpacesForWord.Count > 8) //decide on some limit, based on screen / text size?
            return false;
        if (letterSpace.IsAdjacentToLetterSpace(lastLetterSpace))
            return true;
        return false;
    }

    public bool CanRemoveLetter(LetterSpace letterSpace){
        if (letterSpacesForWord.Count == 0)
            return false;
        return (lastLetterSpace == letterSpace);
    }

    public void ClearWord(){
        foreach (LetterSpace ls in letterSpacesForWord){
            ls.StopDisplayingLetter();
            ls.previousLetterSpace = null;
            ls.nextLetterSpace = null;
        }
        letterSpacesForWord = new List<LetterSpace>();
        SetLastTwoLetterSpaces();
        text.text = "";
    }

    private void CheckIfWordIsValid(){
        if (battleManager.IsValidWord()){
            text.color = validWordColor;
            sendWordButton.GetComponent<Image>().color = validButtonColor;
            //sendWordButton.SetActive(true);
        }
        else{
            text.color = invalidWordColor;
            sendWordButton.GetComponent<Image>().color = invalidButtonColor;
            //sendWordButton.SetActive(false);
        }

        //if valid, start a dotween to pulse the color
    }


}
