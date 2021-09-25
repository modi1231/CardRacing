using System;
using System.Collections.Generic;
using System.Text;

namespace CardRacing
{
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
}
