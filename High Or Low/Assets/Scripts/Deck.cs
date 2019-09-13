using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Deck.cs 
/// </summary>
public class Deck : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;

    private List<string> deckstring; //string representaion of the deck
    private List<Card> finaldeck;  // object representation of the deck
    private List<Card> discardPile; 

    private string[] suits = new string[] { "C", "D", "H", "S" };
    private string[] values = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    public List<Texture> cardTextures; //textures to be applied to card objects

    /// <summary>
    /// Constructs a shuffled deck of cards
    /// </summary>
    private void Awake()
    {
        deckstring = new List<string>();
        discardPile = new List<Card>();
        finaldeck = new List<Card>();

        deckstring = GenerateDeckString();
        deckstring = NaiveBiasedShuffle(deckstring);
        finaldeck = CreateFinalDeck();

        //ExportProbabilityData(); //uncomment to export csv containing probabillity data on shuffles
    }

    /// <summary>
    /// Destroys all card game objects and creates a new shuffled deck of cards
    /// </summary>
    public void ResetDeck()
    {
        foreach (Card g in finaldeck){
            Destroy(g.gameObject);
        }

        foreach (Card g in discardPile){
            Destroy(g.gameObject);
        }

        deckstring = new List<string>();
        finaldeck = new List<Card>();
        discardPile = new List<Card>();

        deckstring = GenerateDeckString();
        deckstring = NaiveBiasedShuffle(deckstring);
        finaldeck = CreateFinalDeck();
    }

    /// <summary>
    /// Shuffle the deck of cards where hearts are 2x likely to be drawn
    /// and the Ace of Spades is 3x likely to be drawn
    /// </summary>
    public List<string> Shuffle()
    {
        deckstring = NaiveBiasedShuffle(deckstring);
        finaldeck = CreateFinalDeck();
        return deckstring;
    }

    /// <summary>
    /// Returns the number of cards left in the deck
    /// </summary>
    /// <returns></returns>
    public int RemainingCards()
    {
        return finaldeck.Count;
    }

    /// <summary>
    /// Deals one card from the deck and adds it to the discard pile
    /// </summary>
    /// <returns></returns>
    public Card DrawCard()
    {
        Card card = finaldeck[0];
        finaldeck.Remove(card);
        discardPile.Add(card);
        return card;
    }

    //********************** Deck Creation **********************//

    /// <summary>
    /// Builds a string representation of the deck of cards
    /// </summary>
    /// <returns></returns>
    public List<string> GenerateDeckString()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits){
            foreach (string v in values){
                newDeck.Add(v + s);
            }
        }
        return newDeck;
    }

    /// <summary>
    /// Instaniate a list of Card game objects and returns a list
    /// containing each card
    /// </summary>
    /// <returns></returns>
    private List<Card> CreateFinalDeck()
    {
        float yOffset = -0.13f; // Hard coded value so cards stack nicely on top of each other
        List<Card> deck = new List<Card>();

        //creates an object representation of a Card from its string name
        foreach (string c in deckstring){

            Texture texture = StringToTexture(c);
            GameObject cardInstance = Instantiate(cardPrefab);

            Renderer rend = cardInstance.GetComponent<Renderer>();
            rend.material.SetTexture("_MainTex", texture);

            Card card = cardInstance.GetComponent<Card>(); //reference to card script
            card.gameObject.name = c;
            card.transform.position = new Vector3(-7.84f, yOffset, 11.64f);
            card.FaceValue = StringToFaceValue(card.gameObject.name);
            card.SuitValue = StringToSuitValue(card.gameObject.name);
            yOffset = yOffset + 0.01f;

            deck.Add(card);
        }
        return deck;
    }

    /// <summary>
    /// Returns the card value based on the following strings:
    /// Aces = 1
    /// Jacks = 11
    /// Queens = 12
    /// Kings = 13
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private int StringToFaceValue(string input)
    {
        for (int i = 2; i < 11; ++i){
            if (input.Contains(i.ToString())){
                return i;
            }
        }

        if(input.Contains("J")){
            return 11;
        }

        else if (input.Contains("Q")){
            return 12;
        }

        else if (input.Contains("K")){
            return 13;
        }

        else if (input.Contains("A")){
            return 1;
        }
        return 0;
    }

    /// <summary>
    /// Returns a value representing a suit based on the following hierarchy
    /// Spades > Hearts > Clubs > Diamonds, with Spades being the high suit
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private int StringToSuitValue(string input)
    {
        if (input.Contains("S")){
            return 4;
        }

        else if (input.Contains("H")){
            return 3;
        }

        else if (input.Contains("C")){
            return 2;
        }

        else if (input.Contains("D")){
            return 1;
        }

        return 0;
    }

    /// <summary>
    /// Returns a texture that matches a given card name
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    private Texture StringToTexture(string input)
    {
        Texture texture = null;

        foreach (Texture t in cardTextures){
            if (t.name.Equals(input)){
                texture = t;
            }
        }
        return texture;
    }

    //********************** Shuffling Algorithmns **********************//


    /// <summary>
    /// Shuffles a list such that all elements are equally likely to
    /// be choosen
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public void FisherYateShuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        //start from last element and loop down
        while (n > 1){   
            //pick a random index from 0 to the size of the list
            int k = random.Next(n);
            n--;
            //swap current element with element located at random index
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }


    /// <summary>
    /// Returns a shuffled deck where hearts are 2x as likely to be drawn vs any other card and
    /// the Ace of Spades is 3x as likely to pulled
    /// This version of the Biased Shuffle runs in O(n) please refer to external documentation
    /// for more information
    /// See SmartBiasedShuffle for a version that relys on Weighted Randomization
    /// </summary>
    public List<string> NaiveBiasedShuffle(List<string> deck)
    {
        List<string> shuffledDeck = new List<string>(); //
        List<string> clubs_diamonds_spades = new List<string>(); // contains 38 card
        List<string> hearts = new List<string>(); // 13 cards
        List<string> AofSpades = new List<string>(); // 1 card

        /* store each list in a stack, easier to maintain after shuffling */
        Stack<string> cds = new Stack<string>();
        Stack<string> ht = new Stack<string>();
        Stack<string> aofspades = new Stack<string>();

        float maxProb = (2.0f / 52.0f * 13.0f) + (3.0f / 52.0f * 1.0f) + (1.0f / 52.0f * 38.0f);// used to normalize values adds up to 1
        float heartProbability = (((2.0f / 52.0f) * 13.0f) / maxProb);  //hearts need to show up 2x as likely as any other card 
        float aceProbability = (((3.0f / 52.0f) * 1.0f) / maxProb);//ace of spades probability 3x as likely as any other       
        float basicProbability = (((1.0f / 52.0f) * 38.0f) / maxProb);//clubs diamons and spades probability

        //Seperate each card in the deck into seperate piles
        foreach (string c in deck){
            
            //clubs, diamonds, and 12 spade cards should have the same probability
            if (c.Contains("C") || c.Contains("D") || c.Contains("S")){
                //Ace of Spades
                if (c.Equals("AS")){
                    AofSpades.Add(c);
                }
                else{
                    clubs_diamonds_spades.Add(c);
                }
            }
            //Hearts
            if (c.Contains("H")){

                hearts.Add(c);
            }
        }

        //shuffle each list using fisher yates
        FisherYateShuffle(hearts);
        FisherYateShuffle(clubs_diamonds_spades);

        //populate stacks with shuffled list
        for (int i = 0; i < clubs_diamonds_spades.Count; i++){
            cds.Push(clubs_diamonds_spades[i]);
        }

        for (int i = 0; i < hearts.Count; i++){
            ht.Push(hearts[i]);
        }

        aofspades.Push(AofSpades[0]);

        //pop values from each stack based on probabilities
        for (int i = 0; i < 52; i++) {
            float random = Random.Range(0.0f, 1.0f);

            //draw a heart
            if (random <= heartProbability){
                // pull a card from the hearts stack

                if (ht.Count != 0){
                    shuffledDeck.Add(ht.Pop());
                }

                // if there are no more hearts
                else if (ht.Count == 0){
                    //pull a card from the diamonds,clubs,spade stack
                    if (cds.Count != 0) {
                        shuffledDeck.Add(cds.Pop());
                    }
                    // if clubs,diamonds,spades stack is empty pull the ace of spades
                    else if (cds.Count == 0){
                        shuffledDeck.Add(aofspades.Pop());
                    }
                }
               
            }

            //draw ace of spade
            else if (random > heartProbability && random <= basicProbability){
                //pull Ace of Spade card               
                if (aofspades.Count != 0){
                    shuffledDeck.Add(aofspades.Pop());
                }
                //if there isnt a Ace of Spade to pull
               else if (aofspades.Count == 0){
                    //pull from heart stack
                    if (ht.Count != 0){
                        shuffledDeck.Add(ht.Pop());
                    }
                    //if heart stack is empty pull from clubs,diamonds,spades stack
                    else if (ht.Count == 0){
                        shuffledDeck.Add(cds.Pop());
                    }
                }
               
            }

            //draw a club diamond or spade
            else if (random > basicProbability){            
                //if the is a club diamond spade draw it
                if (cds.Count != 0){
                    shuffledDeck.Add(cds.Pop());
                }
                // if there isnt any clubs,diamonds, or spades
               else if (cds.Count == 0){

                    //draw Ace of Spades
                    if (aofspades.Count != 0){
                        shuffledDeck.Add(aofspades.Pop());
                    }
                     //if there isnt any Ace of Spades left draw an heart
                    else if (aofspades.Count == 0){
                        shuffledDeck.Add(ht.Pop());
                    }

                }              
            }
        }
       return shuffledDeck;
    }

    /// <summary>
    /// Returns a shuffle deck of cards using Weighted Randomization, runs in O(n^2)
    /// Easier to maintain and read then NaiveBiasedShuffle, refer to external documentation
    /// for more information
    /// </summary>
    /// <param name="deck"></param>
    /// <returns></returns>
    public List<string> SmartBiasedShuffle(List<string> deck)
    {
        List<string> shuffledDeck = new List<string>();
        FisherYateShuffle(deck);
        //loop through an already shuffled deck of cards
        //add a randomly choosen card(based on probability)
        //to a the shuffled deck list.
        for (int i = 0; i < 52; i++){
            shuffledDeck.Add(AddCard(deck));
        }

        return shuffledDeck;
    }


    /// <summary>
    /// Returns a randomly selected card from alist based on its weight.
    /// </summary>
    /// <param name="deck"></param>
    /// <returns></returns>
    public string AddCard(List<string> deck) {
        string card = "";
        WeightedShuffle<string> cards = new WeightedShuffle<string>();

        float maxProb = (2.0f / 52.0f * 13.0f) + (3.0f / 52.0f * 1.0f) + (1.0f / 52.0f * 38.0f);// used to normalize values adds up to 1
        float heartProbability = (((2.0f / 52.0f) * 13.0f) / maxProb) * 3;  //hearts need to show up 2x as likely as any other card 
        float aceProbability = (((3.0f / 52.0f) * 1.0f) / maxProb);//ace of spades probability 3x as likely as any other       
        float basicProbability = (((1.0f / 52.0f) * 38.0f) / maxProb);//clubs diamons and spades probability

        //loop through the deck and give each card a weight
        //based on probabilities
        foreach (string c in deck)
        {
            if (c.Contains("C") || c.Contains("D") || c.Contains("S")){
                //A of spades 3x as likely
                if (c.Equals("AS")){
                    cards.AddEntry(c, aceProbability);
                }
                else{
                    cards.AddEntry(c, basicProbability);
                }
            }
            //Hearts 2x as likely
            if (c.Contains("H")){
                cards.AddEntry(c, heartProbability);
            }
        }
 
        card = cards.GetRandom();
        deckstring.Remove(card);
        return card;
    }
    //********************** Shuffle Testing **********************//

    private void ExportProbabilityData()
    {
        string result = "";
  
        for(int i = 0; i < 20; i++)
        {
           
            result += TestProbabilities();
           
        }

        // WriteAllText creates a file, writes the specified string to the file,
        // and then closes the file.    You do NOT need to call Flush() or Close().
        System.IO.File.WriteAllText(@"C:heartprob.csv", result);
    }
    private string TestProbabilities()
    {
        float regularAverage = 0;
        float naiveAverage = 0;
        float smartAverage = 0;

        List<float> regularShuffle = new List<float>();
        List<float> naiveShuffle = new List<float>();
        List<float> smartShuffle = new List<float>();

        string probabilitystring = "";
      
        for (int i = 0; i < 10; i++){
            List<float> prob = GenerateProbabilities();
            regularShuffle.Add(prob[0]);
            naiveShuffle.Add(prob[1]);
            smartShuffle.Add(prob[2]);
        }

        regularAverage = AverageList(regularShuffle);
        naiveAverage = AverageList(naiveShuffle);
        smartAverage = AverageList(smartShuffle);



        //Debug.Log(regularAverage);
        //Debug.Log(naiveAverage);
        //Debug.Log(smartAverage);
        
        probabilitystring = regularAverage + "," + naiveAverage + "," + smartAverage + "\n" ;
        Debug.Log(smartAverage);
        return probabilitystring;

    }

    /// <summary>
    /// Returns the average of a list of floats
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private float AverageList(List<float> list)
    {
        float sum = 0;

        foreach (float s in list){
            sum += s;
        }

        return sum / list.Count;
    }

    /// <summary>
    /// Returns a list containing the probability that a heart will be drawn
    /// using three different shuffling methods
    /// </summary>
    /// <returns></returns>
    private List<float> GenerateProbabilities()
    {
        List<List<string>> testDeck1 = new List<List<string>>();
        List<List<string>> testDeck2 = new List<List<string>>();
        List<List<string>> testDeck3 = new List<List<string>>();

        List<string> testDeckString1 = new List<string>();
        List<string> testDeckString2 = new List<string>();
        List<string> testDeckString3 = new List<string>();

        List<float> deckProbabilities = new List<float>();

        for (int i = 0; i < 52; i++){
            testDeckString1 = GenerateDeckString();
            testDeckString2 = GenerateDeckString();
            testDeckString3 = GenerateDeckString();


            FisherYateShuffle(testDeckString1);
            testDeckString2 = NaiveBiasedShuffle(testDeckString2);
            testDeckString3 = SmartBiasedShuffle(testDeckString3);

            testDeck1.Add(testDeckString1);
            testDeck2.Add(testDeckString2);
            testDeck3.Add(testDeckString3);
        }

        float cardcounter1 = 0f;
        float cardcounter2 = 0f;
        float cardcounter3 = 0f;

        for (int i = 0; i < testDeck1.Count; i++){
            cardcounter1 += CardCounter(testDeck1[i], "H");
            cardcounter2 += CardCounter(testDeck2[i], "H");
            cardcounter3 += CardCounter(testDeck3[i], "H");
        }

        deckProbabilities.Add(cardcounter1);
        deckProbabilities.Add(cardcounter2);
        deckProbabilities.Add(cardcounter3);

        //Debug.Log(heartcounter1 / 2704f);
        //Debug.Log(heartcounter2 / 2704f);
        //Debug.Log(cardcounter3);

        return deckProbabilities;
    }
     /// <summary>
     /// Returns 1 if the first card drawn matches the in suit:
     /// S = Spade
     /// D = Diamond
     /// AS = Ace of Spades
     /// C = Club
     /// H = Hearts
     /// </summary>
     /// <param name="deck"></param>
     /// <returns></returns>
    public int CardCounter(List<string> deck,string suit)
    {
        int heartCounter = 0;

        if (deck[0].Contains(suit)) {
            heartCounter++;
        }

        return heartCounter;
    }

    //********************** Inner Classes **********************//

    /// <summary>
    /// Constructs a data structure that contains and item and its accumulated weights
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class WeightedShuffle<T>
    {

        private struct Entry{
            public double accumulatedWeight;
            public T item;
        }

        private List<Entry> entries = new List<Entry>();
        private double accumulatedWeight;
        private System.Random rand = new System.Random();

        /// <summary>
        /// Add an item and a weight that is the sum of all the previous weights
        /// </summary>
        /// <param name="item"></param>
        /// <param name="weight"></param>
        public void AddEntry(T item, double weight){
            accumulatedWeight += weight;
            entries.Add(new Entry { item = item, accumulatedWeight = accumulatedWeight });
        }

        /// <summary>
        /// Remove item from an Entry
        /// </summary>
        /// <param name="item"></param>
        public void RemoveEntry(T item){
            foreach (Entry entry in entries){
                if (entry.item.Equals(item)){
                    entries.Remove(entry);
                }
            }

        }

        /// <summary>
        /// Generate a random float beteween 0 and the total weight of Entry
        /// and return an item thats is greater than or equal to the random number
        /// </summary>
        /// <returns></returns>
        public T GetRandom(){
            double r = rand.NextDouble() * accumulatedWeight;

            foreach (Entry entry in entries){
                if (entry.accumulatedWeight >= r) {
                    return entry.item;
                }
            }
            return default(T); //should only happen when there are no entries
        }   
    }

}
