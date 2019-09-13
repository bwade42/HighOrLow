using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class UIButtonHandler : MonoBehaviour
{
    private bool isHigher = false; 
    private bool isLower = false;
    private bool isNewGame = false;

    /// <summary>
    /// Function is Called when the Higher Button is click
    /// </summary>
    public void SetHigher()
    {
      isHigher = true;     
    }

    /// <summary>
    /// Function is called when the lower button is clicked
    /// </summary>
    public void SetLower()
    {
      isLower = true;    
    }

    /// <summary>
    /// Returns the current state of the Higher Button
    /// </summary>
    /// <returns></returns>
    public bool GetIsHigher()
    {
      return isHigher;
    }

    /// <summary>
    /// Returns the current state of the Lower Button
    /// </summary>
    /// <returns></returns>
    public bool GetIsLower()
    {
      return isLower;
    }

    /// <summary>
    /// Returns true if the game has ended
    /// </summary>
    /// <returns></returns>
    public bool GetIsNewGame()
    {
      return isNewGame;
    }
    /// <summary>
    /// Function is called in HighLow.cs when the game has ended
    /// Displays a gameover menus which gives the player the option to start the game over
    /// </summary>
    public void NewGame()
    {
      GameObject menu = GameObject.FindGameObjectWithTag("gameovermenu");
      GameObject child = menu.transform.GetChild(0).gameObject;
      Deck deck = GameObject.FindGameObjectWithTag("deck").GetComponent<Deck>();
      deck.ResetDeck();
      child.SetActive(false);
      isNewGame = true;
    }

    /// <summary>
    //
    /// </summary>
    public void Reset()
    {
      isNewGame = false;
    }

    /// <summary>
    /// Reset the button states after a choice has been made
    /// </summary>
    public void ResetButtons()
    { 
      isLower = false;
      isHigher = false;
    }

}

