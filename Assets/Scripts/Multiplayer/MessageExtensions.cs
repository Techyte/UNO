using Riptide;
using System.Collections.Generic;
using UNO.General;

namespace UNO.Multiplayer
{
    public static class MessageExtensions
    {
        public static void AddCards(this Message message, List<Card> cards)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                message.AddUShort((ushort)cards[i].colour);
            }
            for (int i = 0; i < cards.Count; i++)
            {
                message.AddUShort((ushort)cards[i].type);
            }
            for (int i = 0; i < cards.Count; i++)
            {
                message.AddUShort((ushort)cards[i].secondaryType);
            }
        }

        public static List<Card> GetCards(this Message message)
        {
            ushort[] colours = message.GetUShorts();
            ushort[] types = message.GetUShorts();
            ushort[] secondaryTypes = message.GetUShorts();

            List<Card> cards = new List<Card>();

            for (int i = 0; i < colours.Length; i++)
            {
                cards.Add(new Card((UNO.Enums.CardColour)colours[i], (UNO.Enums.CardType)types[i], (UNO.Enums.CardType)secondaryTypes[i]));
            }

            return cards;
        }
    }
}
