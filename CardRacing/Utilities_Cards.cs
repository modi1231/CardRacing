using System;
using System.Collections.Generic;
using System.Text;

namespace CardRacing
{
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

}
