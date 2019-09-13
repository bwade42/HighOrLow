using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class Card : MonoBehaviour
{
    // a cards value based on suit
    // Aces = 1
    // Jacks = 11
    // Queens = 12
    //Kings = 13
    public int FaceValue { get; set;}

    //the cards suit value
    // Spades = 5
    // Hearts = 4
    // Clubs = 3
    // Diamonds = 2
    public int SuitValue { get; set; }  
}
