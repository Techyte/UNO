using System.Collections.Generic;
using System.Linq;
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
        
        private Deck deck = new Deck();
        
        [Header("Prefabs")]
        [SerializeField] private CardPrefabManager cardPrefabManagerBase;
        [Space]
        [Header("Assignments")]
        [SerializeField] private GameObject cardHolder;
        [SerializeField] private GameObject colourPickDisplay;
        [SerializeField] private Button drawButton;

        [SerializeField] private CardPrefabManager currentCardDisplay;
        [Space]
        [Header("Readouts")]
        public int turnIndex = 0;

        public Card TopCard => topCard;

        [SerializeField] private Card topCard;
        public TurnDirection turnDirection;

        private void Awake()
        {
            Instance = this;
            _networkManager = FindObjectOfType<NetworkManager>();
        }

        public Card GetTopCard()
        {
            return topCard;
        }

        private void Start()
        {
            deck.Shuffle();
            
            foreach (var player in _networkManager.Server.Clients)
            {
                UNOPlayer unoPlayer = new UNOPlayer(player.Id);
                
                players.Add(player.Id, unoPlayer);
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
            turnIndex = index;
            
            SendGlobalTurnUpdate();
        }

        public int NextTurn(int currentTurnIndex)
        {
            int nextTurnIndex = currentTurnIndex;
            
            if (turnDirection == TurnDirection.FORWARD)
            {
                nextTurnIndex += 1;
                if (nextTurnIndex >= _networkManager.Server.ClientCount)
                {
                    Debug.Log("hosts turn after turn change");
                    nextTurnIndex = 0;
                } 
            }
            else
            {
                nextTurnIndex -= 1;
                if (nextTurnIndex < 0)
                {
                    nextTurnIndex = _networkManager.Server.ClientCount-1;
                }
            }
            
            return nextTurnIndex;
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

            Debug.LogWarning("Could not find a player with the specified id, returning 0");
            return 0;
        }

        private ushort ConvertTurnIndexToClientId(int index)
        {
            // plus one because 0 is not a client id and turn indexs start from 1
            return players[(ushort)(index+1)].networkClientId;
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
            
            UpdateCurrentCardDisplay();
        }

        private void UpdateCurrentCardDisplay()
        {
            switch (topCard.colour)
            {
                case CardColour.NONE:
                    currentCardDisplay.cardImage.color = Color.black;
                    break;
                case CardColour.RED:
                    currentCardDisplay.cardImage.color = Color.red;
                    break;
                case CardColour.GREEN:
                    currentCardDisplay.cardImage.color = Color.green;
                    break;
                case CardColour.BLUE:
                    currentCardDisplay.cardImage.color = Color.blue;
                    break;
                case CardColour.YELLOW:
                    currentCardDisplay.cardImage.color = Color.yellow;
                    break;
            }
            
            switch (topCard.type)
            {
                case CardType.ZERO:
                    currentCardDisplay.numberText.text = "0";
                    break;
                case CardType.ONE:
                    currentCardDisplay.numberText.text = "1";
                    break;
                case CardType.TWO:
                    currentCardDisplay.numberText.text = "2";
                    break;
                case CardType.THREE:
                    currentCardDisplay.numberText.text = "3";
                    break;
                case CardType.FOUR:
                    currentCardDisplay.numberText.text = "4";
                    break;
                case CardType.FIVE:
                    currentCardDisplay.numberText.text = "5";
                    break;
                case CardType.SIX:
                    currentCardDisplay.numberText.text = "6";
                    break;
                case CardType.SEVEN:
                    currentCardDisplay.numberText.text = "7";
                    break;
                case CardType.EIGHT:
                    currentCardDisplay.numberText.text = "8";
                    break;
                case CardType.NINE:
                    currentCardDisplay.numberText.text = "9";
                    break;
                case CardType.SKIP:
                    currentCardDisplay.numberText.text = "SKIP";
                    break;
                case CardType.REVERSE:
                    currentCardDisplay.numberText.text = "REVERSE";
                    break;
                case CardType.WILD:
                    currentCardDisplay.numberText.text = "WILD";
                    break;
                case CardType.DRAWTWO:
                    currentCardDisplay.numberText.text = "DRAW 2";
                    break;
            }

            switch (topCard.secondaryType)
            {
                case CardType.DRAWFOUR:
                    currentCardDisplay.numberText.text = "DRAW 4";
                    break;
                case CardType.SHUFFLE:
                    currentCardDisplay.numberText.text = "SHUFFLE";
                    break;
            }

            if (players[1].CanPlayAnyCards(this))
            {
                drawButton.interactable = false;
            }
            else
            {
                drawButton.interactable = true;
            }
        }

        public bool IsPlayable(Card cardToPlay, Card cardOnTop)
        {
            bool sameColour = cardToPlay.colour == topCard.colour;
            bool sameType = cardToPlay.type == cardOnTop.type;
            bool isWild = cardToPlay.type == CardType.WILD;
            
            return sameColour || sameType || isWild;
        }

        private void PlayCard(ushort playerId, int cardIndex)
        {
            if(ConvertClientIdIntoTurnIndex(playerId) != turnIndex) return;
            
            Card card = players[playerId].Hand[cardIndex];
            
            bool isPlayable = IsPlayable(card, topCard);

            if (isPlayable)
            {
                bool cardManagesTurn = CardLogic.HandleCardLogic(card, this);

                if (!cardManagesTurn)
                {
                    Debug.Log("card does not manage turn so we are manually increasing it");
                    NewTurn(NextTurn(turnIndex));
                    Debug.Log(turnIndex);
                }

                players[playerId].Hand.Remove(card);
                
                topCard = card;
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
                UpdateCards();
            }
        }

        private void Draw(ushort playerId)
        {
            if(ConvertClientIdIntoTurnIndex(playerId) != turnIndex) return;

            if (players[playerId].CanPlayAnyCards(this))
            {
                return;
            }
            
            Debug.Log("drawing a card and we have checked and they can");
            
            if (players.TryGetValue(playerId, out UNOPlayer player))
            {
                player.AddCard(deck.Draw());
            }
            
            NewTurn(NextTurn(turnIndex));
            
            SendGlobalCardUpdate();
            UpdateCards();
        }

        public void ServerDrawButton(int playerId)
        {
            Draw((ushort)playerId);
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
             List<UNOPlayer> playersToShuffle = players.Values.ToList();
             List<Card[]> hands = new List<Card[]>();
            
             foreach (var player in playersToShuffle)
             {
                 hands.Add(player.Hand.ToArray());
             }
            
             hands.Shuffle();
            
             foreach (var hand in hands)
             {
                 hand.Shuffle();
             }
            
             // TODO: DONE Shuffle hands logic
        }

        public void ChooseNewColour()
        {
            Debug.Log("Gonna choose a new colour");

            // if host played
            if (turnIndex == 0)
            {
                colourPickDisplay.SetActive(true);
            }
            else
            {
                ushort clientPlayedId = ConvertTurnIndexToClientId(turnIndex);
                Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.CanSelectColour);
                _networkManager.Server.Send(message, clientPlayedId);
            }
        }

        public void NewColourChosen(int colourId)
        {
            topCard.colour = (CardColour)colourId;
            colourPickDisplay.SetActive(false);
            
            NewTurn(NextTurn(turnIndex));
            SendGlobalCardUpdate();
            UpdateCards();
        }

        private void ClientMadeColourSelection(ushort clientThatChose, int selectedColour)
        {
            if (ConvertClientIdIntoTurnIndex(clientThatChose) == turnIndex)
            {
                Debug.Log("Client chose a colour");
                topCard.colour = (CardColour)selectedColour;
            
                NewTurn(NextTurn(turnIndex));
                SendGlobalCardUpdate();
                UpdateCards();
            }
        }

        private void ClientPlayed(ushort id, ushort index)
        {
            Debug.Log("A client played");
            
            // TODO: Play animations for client playing
            
            UpdateCurrentCardDisplay();
            
            Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.PlayerPlayed);
            message.AddUShort(index);
            
            _networkManager.Server.Send(message, id);
        }

        private void HostPlayed(Card card)
        {
            Debug.Log("You Played");
            
            UpdateCards();
            
            // TODO: Play animations for host playing
        }

        private void SendCardInformation(int playerId)
        {
            Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.Cards);
            message.AddCards(players[(ushort)playerId].Hand);
            message.AddCard(topCard);

            _networkManager.Server.Send(message, players[(ushort)playerId].networkClientId);
        }
        
        // TODO: Receive name Messages

        [MessageHandler((ushort)ClientToServerMessageId.Move)]
        private static void PlayerMoved(ushort fromClientId, Message message)
        {
            Instance.PlayCard(fromClientId, message.GetUShort());
        }

        [MessageHandler((ushort)ClientToServerMessageId.Draw)]
        private static void Draw(ushort fromClientId, Message message)
        {
            Instance.Draw(fromClientId);
        }

        [MessageHandler((ushort)ClientToServerMessageId.ColourSelectResult)]
        private static void ColourSelectionResult(ushort fromClientId, Message message)
        {
            Instance.ClientMadeColourSelection(fromClientId, message.GetUShort());
        }
    }
}
