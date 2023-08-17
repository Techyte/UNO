using System.Collections.Generic;
using UNO.Enums;
using UNO.General;
using Random = System.Random;

namespace UNO.Server
{
    public class Deck
    {
        private List<Card> cards = new List<Card>()
        {
            new Card(CardColour.NONE, CardType.WILD, CardType.WILD),
            new Card(CardColour.NONE, CardType.WILD, CardType.WILD),
            new Card(CardColour.NONE, CardType.WILD, CardType.WILD),
            new Card(CardColour.NONE, CardType.WILD, CardType.WILD),
            new Card(CardColour.NONE, CardType.WILD, CardType.DRAWFOUR),
            new Card(CardColour.NONE, CardType.WILD, CardType.DRAWFOUR),
            new Card(CardColour.NONE, CardType.WILD, CardType.DRAWFOUR),
            new Card(CardColour.NONE, CardType.WILD, CardType.DRAWFOUR),
            new Card(CardColour.NONE, CardType.WILD, CardType.SHUFFLE),
            new Card(CardColour.RED, CardType.ZERO),
            new Card(CardColour.RED, CardType.ONE),
            new Card(CardColour.RED, CardType.ONE),
            new Card(CardColour.RED, CardType.TWO),
            new Card(CardColour.RED, CardType.TWO),
            new Card(CardColour.RED, CardType.THREE),
            new Card(CardColour.RED, CardType.THREE),
            new Card(CardColour.RED, CardType.FOUR),
            new Card(CardColour.RED, CardType.FOUR),
            new Card(CardColour.RED, CardType.FIVE),
            new Card(CardColour.RED, CardType.FIVE),
            new Card(CardColour.RED, CardType.SIX),
            new Card(CardColour.RED, CardType.SIX),
            new Card(CardColour.RED, CardType.SEVEN),
            new Card(CardColour.RED, CardType.SEVEN),
            new Card(CardColour.RED, CardType.EIGHT),
            new Card(CardColour.RED, CardType.EIGHT),
            new Card(CardColour.RED, CardType.NINE),
            new Card(CardColour.RED, CardType.NINE),
            new Card(CardColour.RED, CardType.SKIP),
            new Card(CardColour.RED, CardType.SKIP),
            new Card(CardColour.RED, CardType.DRAWTWO),
            new Card(CardColour.RED, CardType.DRAWTWO),
            new Card(CardColour.RED, CardType.REVERSE),
            new Card(CardColour.RED, CardType.REVERSE),
            new Card(CardColour.GREEN, CardType.ZERO),
            new Card(CardColour.GREEN, CardType.ONE),
            new Card(CardColour.GREEN, CardType.ONE),
            new Card(CardColour.GREEN, CardType.TWO),
            new Card(CardColour.GREEN, CardType.TWO),
            new Card(CardColour.GREEN, CardType.THREE),
            new Card(CardColour.GREEN, CardType.THREE),
            new Card(CardColour.GREEN, CardType.FOUR),
            new Card(CardColour.GREEN, CardType.FOUR),
            new Card(CardColour.GREEN, CardType.FIVE),
            new Card(CardColour.GREEN, CardType.FIVE),
            new Card(CardColour.GREEN, CardType.SIX),
            new Card(CardColour.GREEN, CardType.SIX),
            new Card(CardColour.GREEN, CardType.SEVEN),
            new Card(CardColour.GREEN, CardType.SEVEN),
            new Card(CardColour.GREEN, CardType.EIGHT),
            new Card(CardColour.GREEN, CardType.EIGHT),
            new Card(CardColour.GREEN, CardType.NINE),
            new Card(CardColour.GREEN, CardType.NINE),
            new Card(CardColour.GREEN, CardType.SKIP),
            new Card(CardColour.GREEN, CardType.SKIP),
            new Card(CardColour.GREEN, CardType.DRAWTWO),
            new Card(CardColour.GREEN, CardType.DRAWTWO),
            new Card(CardColour.GREEN, CardType.REVERSE),
            new Card(CardColour.GREEN, CardType.REVERSE),
            new Card(CardColour.BLUE, CardType.ZERO),
            new Card(CardColour.BLUE, CardType.ONE),
            new Card(CardColour.BLUE, CardType.ONE),
            new Card(CardColour.BLUE, CardType.TWO),
            new Card(CardColour.BLUE, CardType.TWO),
            new Card(CardColour.BLUE, CardType.THREE),
            new Card(CardColour.BLUE, CardType.THREE),
            new Card(CardColour.BLUE, CardType.FOUR),
            new Card(CardColour.BLUE, CardType.FOUR),
            new Card(CardColour.BLUE, CardType.FIVE),
            new Card(CardColour.BLUE, CardType.FIVE),
            new Card(CardColour.BLUE, CardType.SIX),
            new Card(CardColour.BLUE, CardType.SIX),
            new Card(CardColour.BLUE, CardType.SEVEN),
            new Card(CardColour.BLUE, CardType.SEVEN),
            new Card(CardColour.BLUE, CardType.EIGHT),
            new Card(CardColour.BLUE, CardType.EIGHT),
            new Card(CardColour.BLUE, CardType.NINE),
            new Card(CardColour.BLUE, CardType.NINE),
            new Card(CardColour.BLUE, CardType.SKIP),
            new Card(CardColour.BLUE, CardType.SKIP),
            new Card(CardColour.BLUE, CardType.DRAWTWO),
            new Card(CardColour.BLUE, CardType.DRAWTWO),
            new Card(CardColour.BLUE, CardType.REVERSE),
            new Card(CardColour.BLUE, CardType.REVERSE),
            new Card(CardColour.YELLOW, CardType.ZERO),
            new Card(CardColour.YELLOW, CardType.ONE),
            new Card(CardColour.YELLOW, CardType.ONE),
            new Card(CardColour.YELLOW, CardType.TWO),
            new Card(CardColour.YELLOW, CardType.TWO),
            new Card(CardColour.YELLOW, CardType.THREE),
            new Card(CardColour.YELLOW, CardType.THREE),
            new Card(CardColour.YELLOW, CardType.FOUR),
            new Card(CardColour.YELLOW, CardType.FOUR),
            new Card(CardColour.YELLOW, CardType.FIVE),
            new Card(CardColour.YELLOW, CardType.FIVE),
            new Card(CardColour.YELLOW, CardType.SIX),
            new Card(CardColour.YELLOW, CardType.SIX),
            new Card(CardColour.YELLOW, CardType.SEVEN),
            new Card(CardColour.YELLOW, CardType.SEVEN),
            new Card(CardColour.YELLOW, CardType.EIGHT),
            new Card(CardColour.YELLOW, CardType.EIGHT),
            new Card(CardColour.YELLOW, CardType.NINE),
            new Card(CardColour.YELLOW, CardType.NINE),
            new Card(CardColour.YELLOW, CardType.SKIP),
            new Card(CardColour.YELLOW, CardType.SKIP),
            new Card(CardColour.YELLOW, CardType.DRAWTWO),
            new Card(CardColour.YELLOW, CardType.DRAWTWO),
            new Card(CardColour.YELLOW, CardType.REVERSE),
            new Card(CardColour.YELLOW, CardType.REVERSE),
        };

        public Card Draw()
        {
            Card returnCard = new Card(cards[cards.Count - 1].colour, cards[cards.Count - 1].type,
                cards[cards.Count - 1].secondaryType);
            cards.RemoveAt(cards.Count - 1);
            return returnCard;
        }

        public void Shuffle()
        {
            cards.Shuffle();
        }
    }

    static class MyExtensions
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}