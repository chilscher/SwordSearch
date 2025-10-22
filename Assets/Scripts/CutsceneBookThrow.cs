using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneBookThrow : MonoBehaviour{

    private bool keepThrowing = true;

    public Sprite magicBook;
    public List<Sprite> normalBooks;

    public float timeBetweenBooks = 0.5f;

    public GameObject tossedBookPrefab;
    public List<GameObject> booksToShowInOrder;
    public Transform booksInMotionParent;
    public Transform defaultBookDestination;
    public List<TossedBook> booksToShowInOrder2;
    private int bookIndex = 0;
    
    public void Start(){
        foreach (TossedBook book in booksToShowInOrder2)
            book.Hide();
    }

    public void StartThrow() {
        keepThrowing = true;
        ThrowRandomBook();
        StaticVariables.WaitTimeThenCallFunction(timeBetweenBooks, ThrowRandomBook);
        //the magic book is always the third book thrown out
        StaticVariables.WaitTimeThenCallFunction(timeBetweenBooks * 2, ThrowMagicBook);
        //then, start the process that continuously throws out random books until stopped
        StaticVariables.WaitTimeThenCallFunction(timeBetweenBooks * 3, ThrowBookAndQueueNextBook);
    }

    public void StopThrow(){
        keepThrowing = false;
    }

    private void ThrowRandomBook(){
        Sprite bookSprite = normalBooks[StaticVariables.rand.Next(normalBooks.Count)];
        bool isFlipped = StaticVariables.rand.Next(100) > 50;
        ThrowBook(bookSprite, isFlipped);
    }

    private void ThrowBook(Sprite bookSprite, bool isFlipped){
        if (bookIndex > booksToShowInOrder2.Count - 1) {
            GameObject newBook = Instantiate(tossedBookPrefab, booksInMotionParent);
            TossedBook book = newBook.GetComponent<TossedBook>();
            book.destroySelfAfter = true;
            book.StartThrow(bookSprite, isFlipped);
            return;
        }
        TossedBook nextBook = booksToShowInOrder2[bookIndex];
        nextBook.StartThrow(bookSprite, isFlipped);

        bookIndex++;
    }

    private void ThrowMagicBook(){
        ThrowBook(magicBook, false);
    }

    private void ThrowBookAndQueueNextBook(){
        if (!keepThrowing)
            return;
        ThrowRandomBook();
        StaticVariables.WaitTimeThenCallFunction(timeBetweenBooks, ThrowBookAndQueueNextBook);
    }
}
