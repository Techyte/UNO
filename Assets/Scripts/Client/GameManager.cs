using Riptide;
using System.Collections.Generic;
using UnityEngine;
using UNO.General;
using UNO.Multiplayer;

namespace UNO.Client
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Prefabs")]
        [SerializeField] private CardPrefabManager cardPrefabManagerBase;
        [Space]
        [Header("Assignments")]
        [SerializeField] private GameObject cardHolder;

        private UNOPlayer player;

        private void Awake()
        {
            Instance = this;
        }

        private void UpdateCards()
        {
            foreach (var card in cardHolder.GetComponentsInChildren<CardPrefabManager>())
            {
                Destroy(card.gameObject);
            }

            for (int i = 0; i < player.Hand.Count; i++)
            {
                CardPrefabManager card = Instantiate(cardPrefabManagerBase.GetComponent<CardPrefabManager>());
                card.transform.parent = cardHolder.transform;

                switch (player.Hand[i].colour)
                {
                    case Enums.CardColour.NONE:
                        card.cardImage.color = Color.black;
                        break;
                    case Enums.CardColour.RED:
                        card.cardImage.color = Color.red;
                        break;
                    case Enums.CardColour.GREEN:
                        card.cardImage.color = Color.green;
                        break;
                    case Enums.CardColour.BLUE:
                        card.cardImage.color = Color.blue;
                        break;
                    case Enums.CardColour.YELLOW:
                        card.cardImage.color = Color.yellow;
                        break;
                }

                switch (player.Hand[i].type)
                {
                    case Enums.CardType.ZERO:
                        card.numberText.text = "0";
                        break;
                    case Enums.CardType.ONE:
                        card.numberText.text = "1";
                        break;
                    case Enums.CardType.TWO:
                        card.numberText.text = "2";
                        break;
                    case Enums.CardType.THREE:
                        card.numberText.text = "3";
                        break;
                    case Enums.CardType.FOUR:
                        card.numberText.text = "4";
                        break;
                    case Enums.CardType.FIVE:
                        card.numberText.text = "5";
                        break;
                    case Enums.CardType.SIX:
                        card.numberText.text = "6";
                        break;
                    case Enums.CardType.SEVEN:
                        card.numberText.text = "7";
                        break;
                    case Enums.CardType.EIGHT:
                        card.numberText.text = "8";
                        break;
                    case Enums.CardType.NINE:
                        card.numberText.text = "9";
                        break;
                    case Enums.CardType.SKIP:
                        card.numberText.text = "SKIP";
                        break;
                    case Enums.CardType.REVERSE:
                        card.numberText.text = "REVERSE";
                        break;
                    case Enums.CardType.WILD:
                        card.numberText.text = "WILD";
                        break;
                    case Enums.CardType.DRAWTWO:
                        card.numberText.text = "DRAW 2";
                        break;
                }

                switch (player.Hand[i].secondaryType)
                {
                    case Enums.CardType.DRAWFOUR:
                        card.numberText.text = "DRAW 4";
                        break;
                    case Enums.CardType.SHUFFEL:
                        card.numberText.text = "SHUFFEL";
                        break;
                }
            }
        }

        [MessageHandler((ushort)ServerToClientMessageId.Cards)]
        private static void UpdatedHandMessage(Message message)
        {
            List<Card> cards = message.GetCards();

            Instance.player.Hand = cards;
            Instance.UpdateCards();
        }
    }
}