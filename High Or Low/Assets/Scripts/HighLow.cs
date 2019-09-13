using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 
/// </summary>
public class HighLow : MonoBehaviour
{
    private bool isGameFinished = false;

    UIButtonHandler buttonHandler;

    Deck deck;
    private List<Card> discardPile;

    // Start is called before the first frame update
    void Start()
    {
        buttonHandler = GameObject.FindGameObjectWithTag("uicanvas1").GetComponent<UIButtonHandler>();
        discardPile = new List<Card>();

        StartCoroutine(PlayGame());
    }

    /// <summary>
    /// Start the game of High Low
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayGame()
    {
        deck = GameObject.FindGameObjectWithTag("deck").GetComponent<Deck>();

        Card currentCard = null;
        Card nextCard = null;
        Animator anim = null;
        Animator anim2 = null;

        while (true)
        {
         
            if (isGameFinished == false){
                currentCard = deck.DrawCard();
                anim = currentCard.GetComponent<Animator>();
                anim.enabled = true;
                anim.SetTrigger("DealCardUp");

                while (buttonHandler.GetIsLower() == false && buttonHandler.GetIsHigher() == false)
                {                  
                    yield return null;
                }

                nextCard = deck.DrawCard();
                anim2 = nextCard.GetComponent<Animator>();
                anim2.enabled = true;
                anim2.SetTrigger("DealCardFaceDown");

                //tie
                if (nextCard.FaceValue == currentCard.FaceValue){
                    Debug.Log("tie");                   
                    isGameFinished = true;
                }

                //card is higher
                else if (nextCard.FaceValue > currentCard.FaceValue)
                {
                    if (buttonHandler.GetIsHigher() == true){
                        //Debug.Log("Correct prediction");
                        buttonHandler.ResetButtons();              
                    }

                    else{
                       //Debug.Log("incorrect prediction");                   
                        isGameFinished = true;
                    }
                }
                //card is lower
                else{
                    if (buttonHandler.GetIsLower() == true) {
                        //Debug.Log("Correct prediction");
                    }

                    else{
                       // Debug.Log("incorrect guess");
                        isGameFinished = true;
                    }
                }

                //hard coded fix to allow animations to finish
                yield return new WaitForSeconds(3);

                anim.SetTrigger("DiscardCardFaceUp");
                anim2.SetTrigger("DiscardCardFaceDown");

                yield return new WaitForSeconds(1);

                //yield return new WaitForSeconds(anim2.GetCurrentAnimatorStateInfo(0).length + anim2.GetCurrentAnimatorStateInfo(0).normalizedTime);
                buttonHandler.ResetButtons();
                UpdateDebugLog(currentCard, nextCard);
            }

            //game is over when there is a tie or wrong guess
            if (isGameFinished == true){
                GameObject menu = GameObject.FindGameObjectWithTag("gameovermenu");
                GameObject child = menu.transform.GetChild(0).gameObject;
                child.SetActive(true);
                while (buttonHandler.GetIsNewGame() == false){                 
                    yield return null;
                }
                buttonHandler.Reset();
                isGameFinished = false;
                Text debugLog = GameObject.FindGameObjectWithTag("debuglog").GetComponent<Text>();
                debugLog.text = "";
            }
        }
    }

    /// <summary>
    /// Updates the Debug Log with information about the currently played cards
    /// </summary>
    /// <param name="card"></param>
    /// <param name="card2"></param>
    public void UpdateDebugLog(Card card, Card card2)
    {
        string debugText = "Card 1: " + card.gameObject.name + " Card 2: " + card2.gameObject.name + "\n";
        
        if (card2.FaceValue > card.FaceValue)
        {
            debugText += "2nd card was higher in numerical value";
        }

        else if (card2.FaceValue < card.FaceValue)
        {
            debugText += "2nd card was lower in numerical value";
        }

        else if (card2.FaceValue == card.FaceValue)
        {
            debugText += "2nd card was equal in numerical value";
        }

        //compare suit values base on heirarchy
        //Spades > Hearts > Clubs > Diamonds, with Spades being the high suit.
        if (card2.SuitValue > card.SuitValue){
            debugText += "\n2nd card was higher in suit value";
        }

        else if (card2.SuitValue < card.SuitValue){
            debugText += "\n2nd card was lower in suit value";
        }

        else if (card2.SuitValue == card.SuitValue){
            debugText += "\n2nd card was equal in suit value";
        }

        Text debugLog = GameObject.FindGameObjectWithTag("debuglog").GetComponent<Text>();
        debugLog.text = debugText;
    }


}
