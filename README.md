# CardRacing

The rules are fairly straight forward.

 [u][b]Rules:[/u][/b]
 Standard 52 card deck.
 Arrange the aces in a horizontal line face up.
 Shuffle the deck.
 Pull out 5 cards, face down, and arrange them vertically (perpendicular) to the aces.  Start one row above the aces.
 Each turn consists of: 
 -- pulling the top card from the draw pile, 
 -- revealing the card
 -- move the corresponding suit's ace up one row.
 
 The win condition: first ace to cross the line wins.
 
 [u][b]Extra rules:[/u][/b]
 When one ace is the last in a given row, and there is a face down row card, show that card.
 Move that corresponding suit's ace BACK one spot.
 Discard the card.

=================
 Posted 25 September 2021 - 06:17 PM 
 
 
[u][b]Software[/u][/b]
Visual Studio 2019

[u][b]Concepts[/u][/b]
c#
.NET Core 3.1
modulus

[u][b]Github Link:[/u][/b] https://github.com/modi1231/CardRacing/

This spawned from a video of a card game seen here: https://imgur.com/gallery/CfzqrTn    

The rules seem fairly straight forward.

 [u][b]Rules:[/u][/b]
 Standard 52 card deck.
 Arrange the aces in a horizontal line face up.
 Shuffle the deck.
 Pull out 5 cards, face down, and arrange them vertically (perpendicular) to the aces.  Start one row above the aces.
 Each turn consists of: 
 -- pulling the top card from the draw pile, 
 -- revealing the card
 -- move the corresponding suit's ace up one row.
 
 The win condition: first ace to cross the line wins.
 
 [u][b]Extra rules:[/u][/b]
 When one ace is the last in a given row, and there is a face down row card, show that card.
 Move that corresponding suit's ace BACK one spot.
 Discard the card.
 
 [u][b]Key Concept - implied values[/u][/b]

 The application of the rules was a bit tricky, but ultimately this was a fun exercise.  It leans heavily into the concept that a list of 52 integers can singularly represent all the cards in a deck for value and suit.

 The face value is determined using 'mod 13', and the suit by comparing the value for every 13 values.

 Examples:
 Integer 0 mod 13 is 0 so we know that's an Ace.  Same as 13 mod 13 is 0, 26 mod 13 is 0 and 39 mod 13 is 0.

 34 mod 13 is 8; the face value.
 49 mod 13 is 11; a Jack!

 The suit is all about between.

 0 >= 0 and 0 < 13 so we can determine that 0 is a club.
 22 >= 13 and 22 < 26 so that plunks 22 down in Diamonds territory.
 34 >= 16 and 34 < 39 so that plunks 34 down in Hearts territory.
 45 >= 39 and 45 < 52 so that plunks 45 down in Spades territory.

 The fun part is this concept is transportable to most any other 52 card based games, like War, Solitaire, poker, Spades, etc.

 [u][b]Design[/u][/b]

 The design is a little broad, but not too deep.

 Everything is controlled in the backend by the numerical 0-51 values, and only displayed pretty in the print statements.  This is all housed in the Utilities_Cards class, and, for hte most part, can be plunked down in any future project.

[spoiler]
[code]
    public static class Utilities_Cards
    {
        public enum Suit
        {
            NONE,
            Clubs,
            Diamonds,
            Hearts,
            Spades
        }

        /// <summary>
        /// Since any card value can be found by mod-13, return the number or face value.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>Face value string</returns>
        public static string GetCardValue(int val)
        {
            int temp = val % 13;

            if (temp == 0)
                return "Ace";
            else if (temp == 10)
                return "Jack";
            else if (temp == 11)
                return "Queen";
            else if (temp == 12)
                return "King";
            else return (temp + 1).ToString();
        }

        /// <summary>
        /// Groups of cards can be found by ranges of 13.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>Enum suit name or 'NONE' if out of bounds</returns>
        public static Suit GetCardSuit(int val)
        {
            if (val < 0)
                return Suit.NONE;
            if (val < 13)
                return Suit.Clubs;
            else if (val < 26)
                return Suit.Diamonds;
            else if (val < 39)
                return Suit.Hearts;
            else if (val < 52)
                return Suit.Spades;
            else return Suit.NONE;
        }

        /// <summary>
        /// Get the first character of a suit for a given card ID.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>Char of suit.. so C,D,H,S or N if out of bounds.</returns>
        public static string GetSuitChar(int val)
        {
            return GetCardSuit(val).ToString().Substring(0, 1);
        }

        /// <summary>
        /// Get the first character of a suit
        /// </summary>
        /// <param name="val"></param>
        /// <returns>Char of suit.. so C,D,H,S or N if out of bounds.</returns>
        private static string GetSuitChar(Suit val)
        {
            return val.ToString().Substring(0, 1);
        }

        /// <summary>
        /// Pretty print (and numerical ID) of a card.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>String of SUIT : VALUE (id: ##)</returns>
        public static string PrintCard(int val)
        {
            return $"{GetCardSuit(val)} : {GetCardValue(val)}\t(id: {val.ToString().PadLeft(2, '0')})";
        }

        /// <summary>
        /// Short hand of suit and card. 10's are represented as 1
        /// </summary>
        /// <param name="val"></param>
        /// <returns>Single char suit, and single value. Ex: SA Ace of Spades, DK King of Diamonds, H1 ten of Hearts.</returns>
        public static string GetTwoCharCard(int val)
        {
            string suitChar = GetSuitChar(val);
            string faceValue = GetCardValue(val);

            return $"{suitChar}{faceValue.Trim().Substring(0, 1)}";
        }
    }
[/code]
[/spoiler]

 TheGame class contains the lists (or "decks") for cards to draw, the discard pile as well as the board.  This is all tightly coupled to this specific game's rules.

[spoiler]
[code]
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
[/code]
[/spoiler]


 MyBoard class is all about tracking the aces, the hidden row cards, and the GUI display.  Again, this is tightly coupled with this specific game's display and rules.


[spoiler]
[code]
   public class MyBoard
    {
        const int _maxY = 7;//0 row is start, 6 row is finish line.
        const int _maxX = 4;
        const int _empty = -1;

        int[,] _board;
        int[] _rowSlots;//hold the hidden row cards
        bool[] _rowSlotFlipped;// quick way to determine if hidden card was flipped or not.

        public bool showSlotCard = true;//for debugging purposes.

        //default
        //public MyBoard()
        //{
        //    _board = new int[_maxX, _maxY];
        //    InitBoard(_board);

        //    _rowSlots = new int[_maxY];
        //    InitRowSlots(_rowSlots);

        //    _rowSlotFlipped = new bool[_maxY];
        //    InitRowSlotsFlipped(_rowSlotFlipped);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowCards">5 hidden row cards</param>
        public MyBoard(IList<int> rowCards)
        {
            _board = new int[_maxX, _maxY];
            InitBoard(_board);

            _rowSlots = new int[_maxY];
            InitRowSlots(_rowSlots, rowCards);

            _rowSlotFlipped = new bool[_maxY];
            InitRowSlotsFlipped(_rowSlotFlipped);
        }

        private void InitRowSlotsFlipped(bool[] rowSlotFlipped)
        {
            rowSlotFlipped[0] = true;//start is always 'flipped' true.. since no card will be there.
            rowSlotFlipped[_maxY - 1] = true;//finish is always 'flipped' true.. since no card will be there.
            for (int i = 1; i < _maxY - 1; i++)
            {
                rowSlotFlipped[i] = false;
            }

        }

        private void InitRowSlots(int[] rowSlots, IList<int> rowCards)
        {
            rowSlots[0] = 0;
            rowSlots[_maxY - 1] = 0;

            if (rowCards.Count == 5)
            {
                for (int i = 1; i < _maxY - 1; i++)
                {
                    rowSlots[i] = rowCards[i - 1];
                }
            }

        }

        //private void InitRowSlots(int[] rowSlots)
        //{
        //    rowSlots[0] = 0;
        //    for (int i = 1; i < _maxY; i++)
        //    {
        //        rowSlots[i] = 0;
        //    }
        //}

        private void InitBoard(int[,] board)
        {
            int temp = 0;
            for (int i = 0; i < _maxX; i++)
            {
                board[i, 0] = temp;
                temp += 13;
            }

            for (int y = 1; y < _maxY; y++)
            {
                for (int x = 0; x < _maxX; x++)
                {
                    board[x, y] = _empty;
                }
            }

        }

        public void Print()
        {
            for (int y = 0; y < _maxY; y++)
            {
                for (int x = 0; x < _maxX; x++)
                {
                    Console.Write(GetCleanBoard(_board[x, y]) + " ");
                }
                Console.WriteLine("| " + GetSlotCardForPrint(y, _rowSlots[y], _rowSlotFlipped[y]));

                if (y == 0)
                    Console.WriteLine($"----------------------");
                else if (y == _maxY - 2)
                    Console.WriteLine($"======================");

            }

            Console.WriteLine();
        }

        public string GetSlotCardForPrint(int position, int card, bool flipped)
        {
            if (position == 0)
                return "START";
            else if (position == _maxY - 1)
                return "FINISH";
            else
                return GetSlotCardForPrint(card, flipped);
        }

        private string GetSlotCardForPrint(int card, bool flipped)
        {
            if (flipped)
            {
                return "".PadLeft(2);
            }
            else
            {
                if (showSlotCard)
                    //return card.ToString().PadLeft(2);
                    return Utilities_Cards.GetTwoCharCard(card);
                else
                    return "*".ToString().PadLeft(2);
            }

        }

        private string GetCleanBoard(int spot)
        {
            if (spot == _empty)
                return "".PadLeft(2);
            else
                return Utilities_Cards.GetSuitChar(spot).ToString().PadLeft(2);
        }

        /// <summary>
        /// Every drawn/discarded card's suit advances that same suit's ace forward one space.
        /// </summary>
        /// <param name="suit"></param>
        public void MoveForward(Utilities_Cards.Suit suit)
        {
            int temp = GetAceColumn(suit);

            for (int i = 0; i < _maxY; i++)
            {
                if (_board[temp, i] != _empty)
                {
                    if (i + 1 < _maxY)
                    {
                        _board[temp, i + 1] = _board[temp, i];
                        _board[temp, i] = _empty;
                    }
                    else

                        Console.WriteLine($"Past Edge!");
                    break;
                }
            }
        }

        /// <summary>
        /// Move corresponding suit's ace back one space on the row's hidden card flip.
        /// </summary>
        /// <param name="suit"></param>
        public void MoveBackward(Utilities_Cards.Suit suit)
        {
            int temp = GetAceColumn(suit);

            for (int i = 0; i < _maxY; i++)
            {
                if (_board[temp, i] != _empty)
                {
                    if (i - 1 >= 0)
                    {
                        _board[temp, i - 1] = _board[temp, i];
                        _board[temp, i] = _empty;
                    }
                    else
                        Console.WriteLine($"Past Edge!");
                    break;
                }
            }
        }

        private int GetAceColumn(Utilities_Cards.Suit suit)
        {
            int ret = -1;
            switch (suit)
            {
                case Utilities_Cards.Suit.Clubs:
                    ret = 0;
                    break;
                case Utilities_Cards.Suit.Diamonds:
                    ret = 1;
                    break;
                case Utilities_Cards.Suit.Hearts:
                    ret = 2;
                    break;
                case Utilities_Cards.Suit.Spades:
                    ret = 3;
                    break;
                default:
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Winner's card; -1 no winner</returns>
        public int CheckWinner()
        {
            int ret = -1;

            for (int x = 0; x < _maxX; x++)
            {
                if (_board[x, _maxY - 1] != _empty)
                    return _board[x, _maxY - 1];
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Y index of row to flip; -1 no row</returns>
        public int RowSlotToFlip()
        {
            int ret = -1;

            int[] temp = new int[_maxY];

            //find position of all the aces
            //get the lowest spot where count == 1
            //if not flipped then return that row value
            //if flipped do nothing.


            for (int y = 0; y < _maxY; y++)
            {
                for (int x = 0; x < _maxX; x++)
                {
                    if (_board[x, y] != _empty)
                        temp[y] += 1;

                }
            }

            for (int i = 0; i < _maxY; i++)
            {
                if (temp[i] != 0)
                {
                    if (temp[i] == 1 && !_rowSlotFlipped[i])
                        return i;
                    else
                        return -1;
                }
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row"></param>
        /// <returns>Card to flip over</returns>
        public int FlipRowSlot(int row)
        {
            if (row >= 0 && row < _maxY)
            {
                _rowSlotFlipped[row] = true;
                return _rowSlots[row];
            }
            return -1;
        }
    }
[/code]
[/spoiler]


 The crux of the game rules are applied in the TheGame class's method 'RunTheGame'.

 Here is an example of a run:

The initial display:
[code]
  C  D  H  S | START
----------------------
            | C1
            | H3
            | H4
            | H2
            | D7
======================
            | FINISH
Press any key for the turn;

[/code]

[spoiler]

[code]
Round: 1
************
Discard: Diamonds : Jack        (id: 23)
Deck Remaining: 42
 C     H  S | START
----------------------
    D       |  *
            |  *
            |  *
            |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 2
************
Discard: Spades : Jack  (id: 49)
Deck Remaining: 41
 C     H    | START
----------------------
    D     S |  *
            |  *
            |  *
            |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 3
************
Discard: Hearts : 6     (id: 31)
Deck Remaining: 40
 C          | START
----------------------
    D  H  S |  *
            |  *
            |  *
            |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 4
************
Discard: Spades : Queen (id: 50)
Deck Remaining: 39
 C          | START
----------------------
    D  H    |  *
          S |  *
            |  *
            |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 5
************
Discard: Diamonds : 3   (id: 15)
Deck Remaining: 38
 C          | START
----------------------
       H    |  *
    D     S |  *
            |  *
            |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 6
************
Discard: Clubs : Jack   (id: 10)
Deck Remaining: 37
            | START
----------------------
 C     H    |  *
    D     S |  *
            |  *
            |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 7
************
Discard: Diamonds : 8   (id: 20)
Deck Remaining: 36
            | START
----------------------
 C     H    |  *
          S |  *
    D       |  *
            |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 8
************
Discard: Clubs : Queen  (id: 11)
            | START
----------------------
       H    |
 C        S |  *
    D       |  *
            |  *
            |  *
======================
            | FINISH

Row card - Move Back: Clubs : 10        (id: 09)
Deck Remaining: 35
            | START
----------------------
 C     H    |
          S |  *
    D       |  *
            |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 9
************
Discard: Diamonds : 9   (id: 21)
Deck Remaining: 34
            | START
----------------------
 C     H    |
          S |  *
            |  *
    D       |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 10
************
Discard: Hearts : 9     (id: 34)
Deck Remaining: 33
            | START
----------------------
 C          |
       H  S |  *
            |  *
    D       |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 11
************
Discard: Clubs : 5      (id: 04)
Deck Remaining: 32
            | START
----------------------
            |
 C     H  S |  *
            |  *
    D       |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 12
************
Discard: Hearts : Queen (id: 37)
Deck Remaining: 31
            | START
----------------------
            |
 C        S |  *
       H    |  *
    D       |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 13
************
Discard: Spades : 2     (id: 40)
            | START
----------------------
            |
 C          |
       H  S |  *
    D       |  *
            |  *
======================
            | FINISH

Row card - Move Back: Hearts : 3        (id: 28)
Deck Remaining: 30
            | START
----------------------
            |
 C     H    |
          S |  *
    D       |  *
            |  *
======================
            | FINISH

Press any key for the turn;

Round: 14
************
Discard: Diamonds : 10  (id: 22)
Deck Remaining: 29
            | START
----------------------
            |
 C     H    |
          S |  *
            |  *
    D       |  *
======================
            | FINISH

Press any key for the turn;

Round: 15
************
Discard: Clubs : 4      (id: 03)
Deck Remaining: 28
            | START
----------------------
            |
       H    |
 C        S |  *
            |  *
    D       |  *
======================
            | FINISH

Press any key for the turn;

Round: 16
************
Discard: Clubs : 8      (id: 07)
Deck Remaining: 27
            | START
----------------------
            |
       H    |
          S |  *
 C          |  *
    D       |  *
======================
            | FINISH

Press any key for the turn;

Round: 17
************
Discard: Spades : 5     (id: 43)
Deck Remaining: 26
            | START
----------------------
            |
       H    |
            |  *
 C        S |  *
    D       |  *
======================
            | FINISH

Press any key for the turn;

Round: 18
************
Discard: Clubs : 7      (id: 06)
Deck Remaining: 25
            | START
----------------------
            |
       H    |
            |  *
          S |  *
 C  D       |  *
======================
            | FINISH

Press any key for the turn;

Round: 19
************
Discard: Hearts : 8     (id: 33)
            | START
----------------------
            |
            |
       H    |
          S |  *
 C  D       |  *
======================
            | FINISH

Row card - Move Back: Hearts : 4        (id: 29)
Deck Remaining: 24
            | START
----------------------
            |
       H    |
            |
          S |  *
 C  D       |  *
======================
            | FINISH

Press any key for the turn;

Round: 20
************
Discard: Hearts : 7     (id: 32)
Deck Remaining: 23
            | START
----------------------
            |
            |
       H    |
          S |  *
 C  D       |  *
======================
            | FINISH

Press any key for the turn;

Round: 21
************
Discard: Clubs : 9      (id: 08)
Deck Remaining: 22
            | START
----------------------
            |
            |
       H    |
          S |  *
    D       |  *
======================
 C          | FINISH

Winner is: Clubs
[/code]
[/spoiler]


[size="1"]** Not for gambling or drinking purposes ** [/size]
