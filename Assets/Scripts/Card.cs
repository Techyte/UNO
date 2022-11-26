using System;
using UNO.Enums;

namespace UNO.General
{
    [Serializable]
    public class Card
    {
        public Card(CardColour colour, CardType type)
        {
            this.colour = colour;
            this.type = type;
            secondaryType = CardType.NONE;
        }

        public Card()
        {
            colour = CardColour.NONE;
            type = CardType.ONE;
            secondaryType = CardType.NONE;
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