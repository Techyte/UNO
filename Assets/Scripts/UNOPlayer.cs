using System.Collections.Generic;
using UNO.Server;
using ServerGameManager = UNO.Server.GameManager;
using ClientGameManager = UNO.Client.GameManager;

namespace UNO.General
{
    public class UNOPlayer
    {
        public string Username;
        public List<Card> Hand;
        public ushort networkClientId;

        public UNOPlayer(ushort clientId)
        {
            Hand = new List<Card>();
            Username = string.Empty;
            networkClientId = clientId;
        }

        public void AddCard(Card card)
        {
            Hand.Add(card);
        }

        public bool CanPlayAnyCards(ServerGameManager manager)
        {
            foreach (var card in Hand)
            {
                if (manager.IsPlayable(card, manager.TopCard))
                {
                    return true;
                }
            }

            return false;
        }

        public bool CanPlayAnyCards(ClientGameManager manager)
        {
            foreach (var card in Hand)
            {
                if (manager.IsPlayable(card, manager.TopCard))
                {
                    return true;
                }
            }

            return false;
        }
    }
}