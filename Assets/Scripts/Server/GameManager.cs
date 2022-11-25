using System.Collections.Generic;
using UnityEngine;
using UNO.General;
using UNO.Multiplayer;
using Riptide;
using UnityEngine.UI;
using UNO.Enums;

namespace UNO.Server
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
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

        [SerializeField] private Card topCard;
        [SerializeField] private CardColour currentColour;

        private void Awake()
        {
            Instance = this;
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

        private void NewTurn(int index)
        {
            turnIndex = (index + index) % _networkManager.Server.ClientCount;
            if(turnIndex == 0)
            {
                // Hosts turn
            }
            
            // TODO: Send new turn message
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

                card.GetComponent<Button>().onClick.AddListener(delegate { PlayCard(0, i); });

                switch (players[0].Hand[i].colour)
                {
                    case CardColour.NONE:
                        card.cardImage.color = Color.black;
                        break;
                    case CardColour.RED:
                        card.cardImage.color = Color.red;
                        break;
                    case CardColour.GREEN:
                        card.cardImage.color = Color.green;
                        break;
                    case CardColour.BLUE:
                        card.cardImage.color = Color.blue;
                        break;
                    case CardColour.YELLOW:
                        card.cardImage.color = Color.yellow;
                        break;
                }

                switch (players[0].Hand[i].type)
                {
                    case CardType.ZERO:
                        card.numberText.text = "0";
                        break;
                    case CardType.ONE:
                        card.numberText.text = "1";
                        break;
                    case CardType.TWO:
                        card.numberText.text = "2";
                        break;
                    case CardType.THREE:
                        card.numberText.text = "3";
                        break;
                    case CardType.FOUR:
                        card.numberText.text = "4";
                        break;
                    case CardType.FIVE:
                        card.numberText.text = "5";
                        break;
                    case CardType.SIX:
                        card.numberText.text = "6";
                        break;
                    case CardType.SEVEN:
                        card.numberText.text = "7";
                        break;
                    case CardType.EIGHT:
                        card.numberText.text = "8";
                        break;
                    case CardType.NINE:
                        card.numberText.text = "9";
                        break;
                    case CardType.SKIP:
                        card.numberText.text = "SKIP";
                        break;
                    case CardType.REVERSE:
                        card.numberText.text = "REVERSE";
                        break;
                    case CardType.WILD:
                        card.numberText.text = "WILD";
                        break;
                    case CardType.DRAWTWO:
                        card.numberText.text = "DRAW 2";
                        break;
                }

                switch (players[0].Hand[i].secondaryType)
                {
                    case CardType.DRAWFOUR:
                        card.numberText.text = "DRAW 4";
                        break;
                    case CardType.SHUFFLE:
                        card.numberText.text = "SHUFFLE";
                        break;
                }
            }
        }

        private void PlayCard(int playerId, int cardIndex)
        {
            Card card = players[playerId].Hand[cardIndex];

            bool sameColour = card.colour == currentColour;
            bool sameType = card.type == topCard.type;
            bool isWild = card.secondaryType != CardType.NONE;
            
            bool canPlayCard = sameColour || sameType || isWild;

            if (canPlayCard)
            {
                bool skipped = false;
                
                if (isWild)
                {
                    switch (card.type)
                    {
                        case CardType.WILD:
                            CardLogic.Wild(this);
                            break;
                        case CardType.DRAWFOUR:
                            CardLogic.DrawFour(this);
                            break;
                        case CardType.SHUFFLE:
                            CardLogic.Shuffle(this);
                            break;
                    }
                }
                else
                {
                    switch (card.type)
                    {
                        case CardType.SKIP:
                            skipped = true;
                            CardLogic.Skip(this);
                            break;
                        case CardType.DRAWTWO:
                            CardLogic.DrawTwo(this);
                            break;
                    }
                }

                if (!skipped)
                {
                    NewTurn(1);
                }
                
                // TODO: Set the top card to the new card and set the colour to the new colour
                // TODO: If the card was not played by the host, send a message to them saying it was
                // TODO: Send a message to everyone expect for the host and person that made played the card telling them about the move
                // TODO: Send a message to everyone about the new turn
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
        
        // TODO: Receive Move Messages

        [MessageHandler((ushort)ClientToServerMessageId.Move)]
        private static void PlayerMoved(Message message, ushort fromClientId)
        {
            //Instance.PlayCard();
        }
    }
}
