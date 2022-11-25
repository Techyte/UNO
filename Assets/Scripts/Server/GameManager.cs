using System.Collections.Generic;
using UnityEngine;
using UNO.General;
using UNO.Multiplayer;
using Riptide;

namespace UNO.Server
{
    public class GameManager : MonoBehaviour
    {
        // Use this for getting data about things like players and stuff
        private NetworkManager _networkManager;

        private List<UNOPlayer> players = new List<UNOPlayer>();

        [SerializeField] private Deck deck = new Deck(true);
        
        [Header("Prefabs")]
        [SerializeField] private CardPrefabManager cardPrefabManagerBase;
        [Space]
        [Header("Assignments")]
        [SerializeField] private GameObject cardHolder;
        [Space]
        [Header("Readouts")]
        [SerializeField] private int turnIndex = 0;

        private void Awake()
        {
            _networkManager = FindObjectOfType<NetworkManager>();
        }

        private void Start()
        {   
            for (int i = 0; i < _networkManager.Server.ClientCount; i++)
            {
                players.Add(new UNOPlayer(_networkManager.Server.Clients[i].Id));
            }

            for (int i = 0; i < players.Count; i++)
            {
                List<Card> randomCards = new List<Card>();
                for (int j = 0; j < 5; j++)
                {
                    randomCards.Add(deck.Draw());
                }
                
                players[i].Hand = randomCards;
                
                if(players[i].networkClientId != 1)
                {
                    SendCardInformation(players[i].networkClientId, i);
                }
            }
            
            UpdateCards();
        }

        private void SetTurn(int index)
        {
            turnIndex = index;
            if(turnIndex == 0)
            {
                // Hosts turn
            }
        }

        public void PlayCard(int index)
        {

        }

        private void UpdateCards()
        {
            foreach (var card in cardHolder.GetComponentsInChildren<CardPrefabManager>())
            {
                Destroy(card.gameObject);
            }

            for (int i = 0; i < players[0].Hand.Count; i++)
            {
                CardPrefabManager card = Instantiate(cardPrefabManagerBase.GetComponent<CardPrefabManager>(), cardHolder.transform);

                switch (players[0].Hand[i].colour)
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

                switch (players[0].Hand[i].type)
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

                switch (players[0].Hand[i].secondaryType)
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

        private void SendCardInformation(ushort id, int playerId)
        {
            Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.Cards);
            message.AddCards(players[playerId].Hand);

            _networkManager.Server.Send(message, id);
        }

        private void Update()
        {
            if(turnIndex == 0)
            {
                // Hosts turn to play
            }
        }
    }
}
