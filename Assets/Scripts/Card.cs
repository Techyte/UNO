using UnityEngine;
using UNO.Enums;

namespace UNO.General
{
    public class Card
    {
        public Card(CardColour colour, CardType type)
        {
            this.colour = colour;
            this.type = type;
        }
        
        public Card(CardColour colour, CardType type, CardType secondaryType)
        {
            this.colour = colour;
            this.type = type;
            this.secondaryType = secondaryType;
        }
        
        public CardColour colour;
        public CardType type;
        public CardType secondaryType;
    }
}