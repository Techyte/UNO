using System.Collections.Generic;

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
    }
}