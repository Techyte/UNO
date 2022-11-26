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

        public Dictionary<ushort, UNOPlayer> Players => players;

        private Dictionary<ushort, UNOPlayer> players = new Dictionary<ushort, UNOPlayer>();

        public Deck Deck => deck;
        
        [SerializeField] private Deck deck;
        
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
        public TurnDirection turnDirection;

        private void Awake()
        {
            Instance = this;
            _networkManager = FindObjectOfType<NetworkManager>();
        }

        private void Start()
        {
            deck.Shuffle();
            
            foreach (var player in _networkManager.Server.Clients)
            {
                players.Add(player.Id, new UNOPlayer(player.Id));
            }

            foreach (var player in players.Values)
            {
                List<Card> randomCards = new List<Card>();
                for (int j = 0; j < 5; j++)
                {
                    randomCards.Add(deck.Draw());
                }
                
                player.Hand = randomCards;
                
                if(player.networkClientId != 1)
                {
                    SendCardInformation(player.networkClientId);
                }
            }
            
            UpdateCards();
        }

        public void NewTurn(int index)
        {
            turnIndex = (index + index) % _networkManager.Server.ClientCount-1;
            if(turnIndex == 1)
            {
                // Hosts turn
            }
            
            SendGlobalTurnUpdate();
        }

        public int NextTurn()
        {
            return (turnIndex + 1) % _networkManager.Server.ClientCount;
        }

        private int ConvertClientIdIntoTurnIndex(ushort id)
        {
            int index = 0;
            foreach (var player in players.Values)
            {
                if (player.networkClientId == id)
                {
                    return index;
                }

                index++;
            }

            Debug.Log("Could not find a player with the specified id");
            return 0;
        }

        private void UpdateCards()
        {
            foreach (var card in cardHolder.GetComponentsInChildren<CardPrefabManager>())
            {
                Destroy(card.gameObject);
            }
            
            for (int i = 0; i < players[1].Hand.Count; i++)
            {
                CardPrefabManager card = Instantiate(cardPrefabManagerBase.GetComponent<CardPrefabManager>(), cardHolder.transform);

                int index = i;
                
                card.GetComponent<Button>().onClick.AddListener(delegate { PlayCard(1, index); });

                switch (players[1].Hand[i].colour)
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

                switch (players[1].Hand[i].type)
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

                switch (players[1].Hand[i].secondaryType)
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

        private void PlayCard(ushort playerId, int cardIndex)
        {
            if(ConvertClientIdIntoTurnIndex(playerId) != turnIndex) return;
            
            Debug.Log(players[playerId]);
            Debug.Log("card index:" + cardIndex);
            Debug.Log(players[playerId].Hand[cardIndex]);
            
            Card card = players[playerId].Hand[cardIndex];

            bool sameColour = card.colour == currentColour;
            bool sameType = card.type == topCard.type;
            bool isWild = card.type != CardType.WILD;
            
            bool canPlayCard = sameColour || sameType || isWild;

            if (canPlayCard)
            {
                bool skipped = false;
                
                if (isWild)
                {
                    switch (card.secondaryType)
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
                        case CardType.REVERSE:
                            CardLogic.Reverse(this);
                            break;
                    }
                }

                if (!skipped)
                {
                    NewTurn(1);
                }
                
                topCard = card;
                currentColour = card.colour;
                if (playerId != 1)
                {
                    ClientPlayed(playerId, (ushort)cardIndex);
                }
                else
                {
                    HostPlayed(card);
                }
                
                SendCardPlayedMessage(playerId, card);
                
                SendGlobalCardUpdate();
            }
        }

        private void SendGlobalTurnUpdate()
        {
            Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.NewTurn);
            message.AddUShort((ushort)turnIndex);
            
            _networkManager.Server.SendToAll(message, 1);
        }

        private void SendGlobalCardUpdate()
        {
            foreach (var player in players.Values)
            {
                if (player.networkClientId != 1)
                {
                    SendCardInformation(player.networkClientId);
                }
            }
        }

        private void SendCardPlayedMessage(ushort clientThatPlayed, Card playedCard)
        {
            Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.OtherPlayerPlayed);
            message.AddUShort(clientThatPlayed);
            message.AddCard(playedCard);
            
            foreach (var player in players.Values)
            {
                if (player.networkClientId != 1 && player.networkClientId != clientThatPlayed)
                {
                    _networkManager.Server.Send(message, player.networkClientId);
                }
            }
        }

        public void ShuffleAllHands()
        {
            
        }

        public void ChooseNewColour()
        {
            
        }

        private void ClientPlayed(ushort id, ushort index)
        {
            Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.PlayerPlayed);
            message.AddUShort(index);
            
            _networkManager.Client.Send(message);
        }

        private void HostPlayed(Card card)
        {
            
        }

        private void SendCardInformation(int playerId)
        {
            Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.Cards);
            message.AddCards(players[(ushort)playerId].Hand);

            _networkManager.Server.Send(message, players[(ushort)playerId].networkClientId);
        }
        
        // TODO: Receive name Messages

        [MessageHandler((ushort)ClientToServerMessageId.Move)]
        private static void PlayerMoved(ushort fromClientId, Message message)
        {
            Instance.PlayCard(fromClientId, message.GetUShort());
        }
    }
}
