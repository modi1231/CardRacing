using System;
using System.Collections.Generic;
using System.Text;

namespace CardRacing
{
    /// <summary>
    /// Handles the interaction of the components, and the main game loop.
    /// </summary>
    class TheGame
    {
        private List<int> deck;//where you draw cards
        private List<int> discard;// where cards end up.
        private Random _r;

        private MyBoard _myBoard;//handles position and movement.

        public TheGame()
        {
            //init all the things.
            deck = new List<int>();
            discard = new List<int>();
            _r = new Random();

            //populate the deck
            FillDeck(deck);

            RemovedAces(deck);

            ShuffleDeck(deck);

            //draw the hidden row cards.
            List<int> slotCards = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                int temp = GetTop(deck);// TopToDiscard(deck, discard);

                slotCards.Add(temp);
            }

            //shovel everything into the board.
            _myBoard = new MyBoard(slotCards);

            //can remove.  Just shows the board setup for verification
            _myBoard.Print();

            _myBoard.showSlotCard = false;
        }

        public void RunTheGame()
        {

            Console.WriteLine($"Press any key for the turn;\n");
            Console.ReadKey();

            int _maxDeck = deck.Count;

            //run until the draw deck is empty or win condition hits.
            for (int i = 0; i < _maxDeck; i++)
            {
                Console.Clear();

                //info
                Console.WriteLine($"Round: {i + 1}");
                Console.WriteLine($"************");

                //1.  discard
                int turn = TopToDiscard(deck, discard);
                Console.WriteLine($"Discard: {Utilities_Cards.PrintCard(turn)}");

                //2.  apply discard to the board.
                _myBoard.MoveForward(Utilities_Cards.GetCardSuit(turn));

                //3.  check if row card needs flipped
                int rowToFlip = _myBoard.RowSlotToFlip();
                if (rowToFlip > -1)
                {
                    int flippedCard = _myBoard.FlipRowSlot(rowToFlip);
                    discard.Add(flippedCard);
                    _myBoard.Print();
                    Console.WriteLine($"Row card - Move Back: {Utilities_Cards.PrintCard(flippedCard)}");//RowCard
                    _myBoard.MoveBackward(Utilities_Cards.GetCardSuit(flippedCard));
                }

                //4.  info and print
                Console.WriteLine($"Deck Remaining: {deck.Count}");
                _myBoard.Print();

                //5.  win condition
                int winner = _myBoard.CheckWinner();
                if (winner > -1)
                {
                    Console.WriteLine($"Winner is: {Utilities_Cards.GetCardSuit(winner)}");
                    break;
                }

                Console.WriteLine($"Press any key for the turn;\n");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Stop at each card, and swap it out with another card spot in the deck.
        /// </summary>
        /// <param name="deck"></param>
        private void ShuffleDeck(List<int> deck)
        {
            int temp = -1;
            int newSpot = -1;
            for (int i = 0; i < deck.Count; i++)
            {
                temp = deck[i];
                newSpot = _r.Next(deck.Count);

                deck[i] = deck[newSpot];
                deck[newSpot] = temp;
            }
        }

        /// <summary>
        /// Remove top (index 0) from one list, add to the back of another.
        /// </summary>
        /// <param name="deck"></param>
        /// <param name="discard"></param>
        /// <returns>Card id discarded</returns>
        private int TopToDiscard(List<int> deck, List<int> discard)
        {
            int top = -1;
            if (deck.Count > 1)
            {
                top = deck[0];
                discard.Add(top);
                deck.RemoveAt(0);

                // Console.WriteLine($"DiscardTop: {PrintCard(top)}");
            }
            return top;
        }

        /// <summary>
        /// Remove top (index 0) from one list
        /// </summary>
        /// <param name="deck"></param>
        /// <returns>Card id discarded</returns>
        private int GetTop(List<int> deck)
        {
            int top = -1;
            if (deck.Count > 1)
            {
                top = deck[0];
                deck.RemoveAt(0);

                // Console.WriteLine($"DiscardTop: {PrintCard(top)}");
            }
            return top;
        }

        /// <summary>
        /// Removes every 13th card - aka the aces.
        /// </summary>
        /// <param name="deck"></param>
        private void RemovedAces(List<int> deck)
        {
            int temp = 0;
            for (int i = 0; i < 4; i++)
            {
                deck.Remove(temp);
                temp += 13;
            }
        }

        /// <summary>
        /// Pretty print of a deck.
        /// </summary>
        /// <param name="deck"></param>
        private void PrintDeck(List<int> deck)
        {
            foreach (int i in deck)
            {
                Console.WriteLine($"{Utilities_Cards.PrintCard(i)}");
            }
        }

        /// <summary>
        /// If someone wants to review the discarded cards in series of they were played.
        /// </summary>
        public void PrintDiscard()
        {
            Console.WriteLine("");
            Console.WriteLine("Printing Discard Pile");
            Console.WriteLine("---------------------");

            PrintDeck(discard);
        }


        private void FillDeck(List<int> deck)
        {
            for (int i = 0; i < 52; i++)
            {
                deck.Add(i);
            }

        }
    }
}
