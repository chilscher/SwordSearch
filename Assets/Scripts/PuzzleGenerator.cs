using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PuzzleGenerator : MonoBehaviour {

    //representations of the game board as 2d arrays
    //the first dimension is height, second dimension is width. [0,2] represents the third element of the top row
    private LetterSpace[,] letterSpaces;
    private char[,] letters;

    public TextAsset randomLetterPoolFile;
    private char[] randomLetterPool;
    public WordDisplay wordDisplay;
    System.Random rand = new System.Random();

    bool useStartingLayout = false;
    private char[,] startingLayout = {{'-', '-', '-', '-', 'A'}, {'O', 'R', '-', 'L', 'V'}, {'W', 'S', 'D', 'S', 'I'}, {'-', 'A', 'R', 'R', 'U'}, {'-', '-', '-', 'P', 'Y'}, {'-', 'L', 'A', 'T', '-'}, {'P', '-', '-', '-', '-'}};

    public int maxAttemptsPerPuzzle = 3;

    public TextAsset wordLibraryForGenerationFile; //all words that can be used to generate the puzzle
    private string[] wordLibraryForGeneration;

    [Header("Word Rules")]
    public int wordCount = 3;
    public int minGenerationWordLength = 3;
    public int maxGenerationWordLength = 6;

    public BattleManager battleManager;

    void Start() {
        wordLibraryForGeneration = wordLibraryForGenerationFile.text.Split("\r\n");
        randomLetterPool = randomLetterPoolFile.text.ToCharArray();
        GetPuzzleDimensions();
        GenerateNewPuzzle();
    }

    public void GenerateNewPuzzle(){
        foreach (LetterSpace ls in letterSpaces)
            ls.hasBeenUsedInAWordAlready = false;
        ClearPuzzle();
        bool succeeded = false;
        while (!succeeded)
            succeeded = PickWordsAndAttemptToGenerateSolution();
        FillRestOfPuzzle();
        RenderLetters();   
        ClearAllPowerups();
        PickRandomSpaceForPowerup();    
        PickRandomSpaceForPowerup();  
        PickRandomSpaceForPowerup();   
    }

    private bool PickWordsAndAttemptToGenerateSolution(){
        string[] words = GetRandomWordsFromLibrary();
        return AttemptToGenerateSolution(words);
    }

    private string[] GetRandomWordsFromLibrary(){
        string[] result = new string[wordCount];
        for (int i = 0; i < wordCount; i++){
            bool searching = true;
            while (searching){
                string word = wordLibraryForGeneration[rand.Next(wordLibraryForGeneration.Length)];  
                if (DoesWordFitCriteria(word)){
                    searching = false;     
                    result[i] = word;
                }
            }
        }
        return result;
    }

    private bool DoesWordFitCriteria(string word){
        if (word.Length < minGenerationWordLength)
            return false;
        if (word.Length > maxGenerationWordLength)
            return false;
        return true;
    }

    private bool AttemptToGenerateSolution(string[] words){
        int attemptCount = 0;
        bool succeeded = true;

        while (attemptCount < maxAttemptsPerPuzzle){
            foreach (string word in words){
                bool couldGenerateWord = GenerateLetters(word);
                if (!couldGenerateWord){
                    succeeded = false;
                }
            }
            if (succeeded)
                attemptCount += maxAttemptsPerPuzzle;
            else
                ClearPuzzle();
            attemptCount++;
        }
        if (succeeded)
            print("puzzle generation completed on attempt " + (attemptCount - maxAttemptsPerPuzzle));
        else
            print("puzzle generation failed. attempts made: " + attemptCount);

        return succeeded;

    }


    private void GenerateLetters(){
        for (int i = 0; i < letterSpaces.GetLength(0); i++){
            for (int j = 0; j < letterSpaces.GetLength(1); j++){
                int temp = rand.Next(randomLetterPool.Length);
                letters[i,j] = randomLetterPool[temp];
            }
        }
    }

    private bool GenerateLetters(string requiredWord){

        string remainingWord = requiredWord.ToUpper();;
        Vector2 previousSpace = new Vector2(-1,-1);
        int remainingLength = requiredWord.Length;
        //Vector2 previousSpace = PlaceLetter(requiredWord[0], new Vector2(-1,-1), (requiredWord.Length - 1));
        bool cont = true;
        while (cont){
            previousSpace = PlaceLetter(remainingWord[0], previousSpace, remainingWord.Length);
            remainingWord = remainingWord.Substring(1);
            //print("remaining word is " + remainingWord);
            remainingLength = remainingWord.Length - 1;
            if (remainingLength == -1)
                cont = false;
            if ((previousSpace[0] == -1) && (previousSpace[1] == -1))
                return false;
        }
        return true;

    }

    private Vector2 PlaceLetter(char letter, Vector2 previousSpace, int numberOfSpacesRequiredForWordToFit){

        //get a list of all possible spaces this letter can go in
        List<Vector2> candidates1 = GetAllPossibleSpacesForLetter(previousSpace);

        //remove all candidates that are out of bounds, and all that already contain values
        List<Vector2> candidates2 = RemoveImpossibleSpaces(candidates1);

        //remove all candidates that are inside a closed loop, if the loop is too small
        List<Vector2> candidates3 = RemoveSpacesInsideSmallRegion(candidates2, numberOfSpacesRequiredForWordToFit);

        //print("picking the final spot from " + candidates3.Count + " possible candidates");

        //pick one at random
        if (candidates3.Count == 0){
            //print("there is no space to finish the word.");
            return new Vector2(-1,-1);
        }
        int temp = rand.Next(candidates3.Count);
        Vector2 selectedSpot = candidates3[temp];
        letters[(int)selectedSpot[0], (int)selectedSpot[1]] = letter;
        //print("placed letter (" + letter + ") in position [" + selectedSpot[0] + "," + selectedSpot[1] + "]");
        return selectedSpot;
    }


    private List<Vector2> GetAllPossibleSpacesForLetter(Vector2 previousSpace){
        List<Vector2> candidates1 = new List<Vector2>();
        if ((previousSpace[0] == -1) && (previousSpace[1] == -1)){
            //this the first letter of a new word. all tiles are possible candidates
            for (int i = 0; i < letterSpaces.GetLength(0); i++){
                for (int j = 0; j < letterSpaces.GetLength(1); j++)
                    candidates1.Add(new Vector2(i, j));
            }
            return candidates1;
        }

        //this letter is part of a pre-existing word. get all of the previous letter's neighbors        
        candidates1 = GetNeighbors(previousSpace);
        
        return candidates1;
    }

    private List<Vector2> RemoveImpossibleSpaces(List<Vector2> candidates1){
        List<Vector2> candidates2 = new List<Vector2>();
        foreach (Vector2 candidate in candidates1){
            if ((candidate[0] >= 0) && (candidate[0] < letterSpaces.GetLength(0)) && (candidate[1] >= 0) && (candidate[1] < letterSpaces.GetLength(1))){
                if (letters[(int)candidate[0], (int)candidate[1]].Equals('-')){
                    candidates2.Add(candidate);
                }
            }  
        }
        return candidates2;
    }

    private List<Vector2> GetNeighbors(Vector2 position){
        //returns a list of coordinates adjacent to the provided position. Includes out-of-bounds options
        List<Vector2> neighbors = new List<Vector2>();
        neighbors.Add(position + Vector2.up);
        neighbors.Add(position + Vector2.right + Vector2.up);
        neighbors.Add(position + Vector2.right);
        neighbors.Add(position + Vector2.right + Vector2.down);
        neighbors.Add(position + Vector2.down);
        neighbors.Add(position + Vector2.left + Vector2.down);
        neighbors.Add(position + Vector2.left);
        neighbors.Add(position + Vector2.left + Vector2.up);
        return neighbors;
    }

    private List<Vector2> RemoveSpacesInsideSmallRegion(List<Vector2> candidates2, int requiredSizeOfRegion){
        List<Vector2> candidates3 = new List<Vector2>();

        List<List<Vector2>> regions = GetSeparateRegions(GetListOfAllSpaces());
        foreach (List<Vector2> region in regions){
            if (region.Count >= requiredSizeOfRegion){
                foreach(Vector2 space in region)
                    foreach (Vector2 candidate in candidates2){
                        if (candidate == space)
                            candidates3.Add(space);
                    }
            }
        }

        if (requiredSizeOfRegion == 1)
            return candidates3;

        //check if placing the letter in a spot would create separate regions
        List<Vector2> candidates4 = new List<Vector2>();
        foreach (Vector2 candidate in candidates3){
            letters[(int)candidate[0], (int)candidate[1]] = '#';
            List<Vector2> regionContainingCandidate = regions[0];
            foreach (List<Vector2> region in regions){
                if (region.Contains(candidate))
                    regionContainingCandidate = region;
            }
            List<List<Vector2>> subRegions = GetSeparateRegions(regionContainingCandidate);
            bool isValid = false;
            foreach (List<Vector2> subRegion in subRegions){
                if (subRegion.Count >= requiredSizeOfRegion - 1)
                    isValid = true;
            }
            if (isValid)
                candidates4.Add(candidate);
            letters[(int)candidate[0], (int)candidate[1]] = '-';
        }


        return candidates4;
    }

    private List<Vector2> GetListOfAllSpaces(){
        List<Vector2> allSpaces = new List<Vector2>();
        for (int i = 0; i < letterSpaces.GetLength(0); i++){
            for (int j = 0; j < letterSpaces.GetLength(1); j++){
                allSpaces.Add(new Vector2(i, j));
            }
        }
        return allSpaces;
    }


    private List<List<Vector2>> GetSeparateRegions(List<Vector2> spacesToCheck){
        //creates a list of separate regions in the puzzle area
        //each region is bounded by the walls of the puzzle or filled spaces

        //start with a list of all unfilled spaces
        List<Vector2> allSpaces = new List<Vector2>();
        foreach (Vector2 s in spacesToCheck){
            if (letters[(int)s[0],(int)s[1]].Equals('-'))
                allSpaces.Add(s);
        }


        List<List<Vector2>> regions = new List<List<Vector2>>();
        if (allSpaces.Count == 0)
            return regions;
        List<Vector2> firstRegion = new List<Vector2>();
        firstRegion.Add(allSpaces[0]);
        regions.Add(firstRegion);
        allSpaces.RemoveAt(0);

        foreach(Vector2 space in allSpaces){ //35 iterations

            //check if any of its neighbors are in a region already
            List<int> regionsThisBelongsTo = new List<int>();
            int regionIndex = 0;
            List<Vector2> neighbors = GetNeighbors(space);
            foreach (List<Vector2> region in regions){ //low int number of regions
                //check if a neighbor is in the region
                bool isNeighborInRegion = false;
                foreach (Vector2 neighbor in neighbors){ //8 iterations
                    if (region.Contains(neighbor)) //35 iterations
                        isNeighborInRegion = true;
                }
                if (isNeighborInRegion){
                    regionsThisBelongsTo.Add(regionIndex);
                    if (regionsThisBelongsTo.Count == 1)
                        region.Add(space); //only add the space to the first region. the regions will be combined later
                }
                regionIndex++;
            }


            if (regionsThisBelongsTo.Count > 1){
                //add the contents of each other region to the first region that this space belongs to
                foreach (int i in regionsThisBelongsTo){
                    if (i != regionsThisBelongsTo[0]){
                        foreach(Vector2 newSpaceToAdd in regions[i]){
                            regions[regionsThisBelongsTo[0]].Add(newSpaceToAdd);
                        }
                    }
                }
                //remove the other regions. they have been combined and are no longer required
                for (int z = regionsThisBelongsTo.Count - 1; z > 0; z--){
                    regions.RemoveAt(regionsThisBelongsTo[z]);
                }
            }
            else if (regionsThisBelongsTo.Count == 0){
                List<Vector2> newRegion = new List<Vector2>();
                newRegion.Add(space);
                regions.Add(newRegion);
            }
        }

        return regions;

    }

    private void FillRestOfPuzzle(){
        for (int i = 0; i < letterSpaces.GetLength(0); i++){
            for (int j = 0; j < letterSpaces.GetLength(1); j++){
                if (letters[i,j].Equals('-')){
                    int index = rand.Next(randomLetterPool.Length);
                    char l = randomLetterPool[index];
                    letters[i,j] = l;
                    if (l.Equals('Q')){
                        Vector2 temp = PlaceLetter('U', new Vector2(i,j), 1);
                        if ((temp[0] == -1) && (temp[1] == -1)){ //could not place a U nearby
                            letters[i,j] = 'E';
                        }
                    }
                }
                
            }
        }
    }

    private void RenderLetters(){    
        for (int i = 0; i < letterSpaces.GetLength(0); i++){
            for (int j = 0; j < letterSpaces.GetLength(1); j++){
                LetterSpace ls = letterSpaces[i,j];
                ls.UpdateLetter(letters[i,j]);
                ls.ShowAsNotPartOfWord();
            }
        }

    }

    private void GetPuzzleDimensions(){
        int totalCount = transform.childCount;
        int width = Mathf.FloorToInt(GetComponent<GridLayoutGroup>().constraintCount);
        int height = Mathf.FloorToInt(totalCount / width);
        letterSpaces = new LetterSpace[height, width];
        letters = new char[height, width];
        for (int i = 0; i < height; i++){
            for (int j = 0; j < width; j++){
                letterSpaces[i, j] = transform.GetChild((i * width) + j).GetComponent<LetterSpace>();
                letterSpaces[i,j].wordDisplay = wordDisplay;
                letterSpaces[i,j].position = new Vector2(i,j);
            }
        }

    }

    private void PrintPuzzle(){
        string result = "";
        for (int i = 0; i < letterSpaces.GetLength(0); i++){
                for (int j = 0; j < letterSpaces.GetLength(1); j++)
                    result += letters[i, j] + " ";
            print(result);
            result = "";
        }
    }

    private void ClearPuzzle(){
        for (int i = 0; i < letters.GetLength(0); i++){
            for (int j = 0; j < letters.GetLength(1); j++){
                letters[i,j] = '-';
                if (useStartingLayout)
                    letters[i,j] = startingLayout[i,j];
            }
        }
    }

    private void ClearAllPowerups(){
        for (int i = 0; i < letters.GetLength(0); i++){
            for (int j = 0; j < letters.GetLength(1); j++){
                letterSpaces[i,j].SetPowerup(BattleManager.PowerupTypes.None);
            }
        }
    }

    private void PickRandomSpaceForPowerup(){
        int t1 = rand.Next(letters.GetLength(0));
        int t2 = rand.Next(letters.GetLength(1));
        LetterSpace ls = letterSpaces[t1,t2];
        if (ls.powerupType != BattleManager.PowerupTypes.None)
            PickRandomSpaceForPowerup();
        else
            letterSpaces[t1,t2].SetPowerup(SelectPowerupType());
    }
    
    private BattleManager.PowerupTypes SelectPowerupType(){
        int range = battleManager.powerupArray.Length - 1; //"None" is not an option we want to select

        int i = rand.Next(0,range) + 1;
        return (BattleManager.PowerupTypes)battleManager.powerupArray.GetValue(i);
    }
}
