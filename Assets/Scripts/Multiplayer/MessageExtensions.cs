using Riptide;
using System.Collections.Generic;
using UnityEngine;
using UNO.General;

namespace UNO.Multiplayer
{
    public static class MessageExtensions
    {
        public static void AddCards(this Message message, List<Card> cards)
        {
            ushort[] colours = new ushort[cards.Count];
            ushort[] types = new ushort[cards.Count];
            ushort[] secondaryTypes = new ushort[cards.Count];

            for (int i = 0; i < cards.Count; i++)
            {
                colours[i] = (ushort)cards[i].colour;
            }
            for (int i = 0; i < cards.Count; i++)
            {
                types[i] = (ushort)cards[i].type;
            }
            for (int i = 0; i < cards.Count; i++)
            {
                secondaryTypes[i] = (ushort)cards[i].secondaryType;
            }

            message.AddUShorts(colours);
            message.AddUShorts(types);
            message.AddUShorts(secondaryTypes);
        }

        public static List<Card> GetCards(this Message message)
        {
            ushort[] colours = message.GetUShorts();
            ushort[] types = message.GetUShorts();
            ushort[] secondaryTypes = message.GetUShorts();

            List<Card> cards = new List<Card>();

            for (int i = 0; i < colours.Length; i++)
            {
                cards.Add(new Card((Enums.CardColour)colours[i], (Enums.CardType)types[i], (Enums.CardType)secondaryTypes[i]));  
            }

            return cards;
        }

        public static void AddCard(this Message message, Card card)
        {
            message.AddUShort((ushort)card.colour);
            message.AddUShort((ushort)card.type);
            message.AddUShort((ushort)card.secondaryType);
        }

        public static Card GetCard(this Message message)
        {
            return new Card((Enums.CardColour)message.GetUShort(), (Enums.CardType)message.GetUShort(), (Enums.CardType)message.GetUShort());
        }
    }
}
