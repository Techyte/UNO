using Riptide;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UNO.Enums;
using UNO.General;
using UNO.Multiplayer;

namespace UNO.Client
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private NetworkManager _networkManager;

        [Header("Prefabs")] [SerializeField] private CardPrefabManager cardPrefabManagerBase;

        [Space] [Header("Assignments")] [SerializeField]
        private GameObject cardHolder;
        [SerializeField] private GameObject colourPickDisplay;

        [SerializeField] private CardPrefabManager currentCardDisplay;

        private UNOPlayer player;

        private Card topCard;
        private CardColour currentColour;

        private void Awake()
        {
            Instance = this;
            _networkManager = FindObjectOfType<NetworkManager>();
        }

        private void Start()
        {
            player = new UNOPlayer(_networkManager.Client.Id);
        }
        
        // TODO: Send the name message

        private void UpdateCards()
        {
            foreach (var card in cardHolder.GetComponentsInChildren<CardPrefabManager>())
            {
                Destroy(card.gameObject);
            }

            for (int i = 0; i < player.Hand.Count; i++)
            {
                CardPrefabManager card = Instantiate(cardPrefabManagerBase.GetComponent<CardPrefabManager>(),
                    cardHolder.transform);

                int index = i;
                
                card.GetComponent<Button>().onClick.AddListener(delegate { PlayCard(index); });

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
                    case Enums.CardType.SHUFFLE:
                        card.numberText.text = "SHUFFLE";
                        break;
                }
            }
            
            switch (currentColour)
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
        }

        private void OtherPlayerPlayed(int otherPlayerId)
        {
            // TODO: Link proper other player animation/feedback
            
            Debug.Log("Someone else played: " + otherPlayerId);
        }

        private void LocalPlayed(ushort cardIndex)
        {
            // TODO: Link proper local played animations/feedback

            CardLogic.HandleCardLogic(player.Hand[cardIndex], this);
            
            Debug.Log("Played: " + cardIndex);
        }

        private void PlayCard(int cardIndex)
        {
            Message message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageId.Move);
            message.AddUShort((ushort)cardIndex);

            _networkManager.Client.Send(message);
        }

        public void DrawCard()
        {
            Message message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageId.Draw);
            _networkManager.Client.Send(message);
        }

        public void ChooseNewColour()
        {
            colourPickDisplay.SetActive(true);
        }

        public void NewColourChosen(int colourId)
        {
            colourPickDisplay.SetActive(false);
            Message message = Message.Create(MessageSendMode.Reliable, ClientToServerMessageId.ColourSelectResult);
            message.AddUShort((ushort)colourId);
            _networkManager.Client.Send(message);
        }

        private void NewTurn(int newTurnIndex)
        {
            // TODO: Tell the player if they can play or not when the turn upadates
            
            Debug.Log("New Turn: " + newTurnIndex);
        }

        [MessageHandler((ushort)ServerToClientMessageId.Cards)]
        private static void UpdatedHandMessage(Message message)
        {
            Debug.Log("Received card message");
            
            if (Instance != null)
            {
                Debug.Log("Received card message and actually doing something with it");
                List<Card> cards = message.GetCards();
                Card topCard = message.GetCard();
                CardColour currentColour = (CardColour)message.GetUShort();

                Instance.player.Hand = cards;
                Instance.topCard = topCard;
                Instance.currentColour = currentColour;
                Instance.UpdateCards();
            }
        }

        [MessageHandler((ushort)ServerToClientMessageId.PlayerPlayed)]
        private static void LocalPlayerPlayed(Message message)
        {
            Instance.LocalPlayed(message.GetUShort());
        }

        [MessageHandler((ushort)ServerToClientMessageId.OtherPlayerPlayed)]
        private static void OtherPlayerPlayedMessage(Message message)
        {
            Instance.OtherPlayerPlayed(message.GetUShort());
        }

        [MessageHandler((ushort)ServerToClientMessageId.NewTurn)]
        private static void NewTurnMessage(Message message)
        {
            Instance.NewTurn(message.GetUShort());
        }
    }
}