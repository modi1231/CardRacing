using System;

namespace CardRacing
{
    //2021.09.24
    //https://imgur.com/gallery/CfzqrTn    
    /* Rules:
     * Standard 52 card deck.
     * Arrange the aces in a horizontal line face up.
     * Shuffle the deck.
     * Pull out 5 cards, face down, and arrange them vertically (perpendicular) to the aces.  Start one row above the aces.
     * Each turn consists of: 
     * -- pulling the top card from the draw pile, 
     * -- revealing the card
     * -- move the corresponding suit's ace up one row.
     * 
     * The win condition: first ace to cross the line wins.
     * 
     * Extra rules:
     * When one ace is the last in a given row, and there is a face down row card, show that card.
     * Move that corresponding suit's ace BACK one spot.
     * Discard the card.
     * 
     * */

    class Program
    {
        static void Main(string[] args)
        {
            TheGame game = new TheGame();
            game.RunTheGame();

            //   game.PrintDiscard();

            Console.ReadLine();
        }
    }
}
