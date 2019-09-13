# High Or Low

The game starts with a card being drawn. The user decides if the next card to be drawn will be higher or lower in numeric value then current face up card
on the game board. The game is over if the player guesses wrong or if the next card is equal to the current card in numeric value. If I were to be asked to not allow
ties, in the event that two cards have the same numeric value I would add an extra check that would compare their suit value. 

Class Heiarchy follows the design as described in the PDF in this folder


## Getting Started

This game was created in Unity using C#


# Gameplay flow:
1. One card is draw from the deck face up, a second card is draw when the user makes his/her guess
2. To the bottom left of the game screen a text will output wheter the 2nd card drawn is higher,lower, or equal in numeric/suit value after a user makes a guess.

# Requirements
1. After a player loses the game, the deck will be reshuffled. If the game is ran in the Unity IDE you can see the list of cards in the deck in the correct order, in
   the Unity Heirarchy so it can be confirmed after a new game is started that list will change.

2. I developed two working algoritmns that make all Heart suit cards 2x as likely and the Ace of Spades 3x as likely to be drawn compared to cards. In the
   Probabilities Test folder you will my method of testing the algorithmns.I developed a matlab script that takes in a .csv file exported from Unity that will graph 
   the test results. You can find more information on this in the HighLowArchitecture .pdf

3. In the game two cards cannot appear twice as they are discarded and removed from the deck after being drawn

# Optional

1. Animations can be found in the Unity Assets folder.
  
## Bugs And Other Known Issues
- When cards are discarded they dont stack nicely on top of each other. They lay as one card.

       

## Authors

* **Brandon Wade** 

