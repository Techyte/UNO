using UnityEngine;
using UNO.Enums;
using ClientGameManager = UNO.Client.GameManager;
using ServerGameManager = UNO.Server.GameManager;

namespace UNO.General
{
     public static class CardLogic
     {
         #region ClientCardLogic
    
         public static void HandleCardLogic(Card card, ClientGameManager manager)
         {
             bool isWild = card.type == CardType.WILD;
            
             if (isWild)
             {
                 switch (card.secondaryType)
                 {
                     case CardType.WILD:
                         Wild(manager);
                         break;
                     case CardType.DRAWFOUR:
                         DrawFour(manager);
                         break;
                     case CardType.SHUFFLE:
                         Shuffle(manager);
                         break;
                 }
             }
             else
             {
                 switch (card.type)
                 {
                     case CardType.SKIP:
                         Skip(manager);
                         break;
                     case CardType.DRAWTWO:
                         DrawTwo(manager);
                         break;
                     case CardType.REVERSE:
                         Reverse(manager);
                         break;
                 }
             }
         }
         
         private static void Skip(ClientGameManager manager)
         {
             // TODO: Logic for skipping turn client
         }
    
         private static void DrawTwo(ClientGameManager manager)
         {
             // TODO: Logic for drawing 2 visuals client
         }
    
         private static void DrawFour(ClientGameManager manager)
         {
             Wild(manager);
             // TODO: Logic for drawing 4 visuals client
         }
    
         private static void Wild(ClientGameManager manager)
         {
             manager.ChooseNewColour();
             // TODO: Logic for wild cards visuals client
         }
    
         private static void Shuffle(ClientGameManager manager)
         {
             Wild(manager);
             // TODO: Logic for shuffle visuals client
         }
    
         private static void Reverse(ClientGameManager manager)
         {
             // TODO: Logic for reversing visuals client
         }

        #endregion

        #region ServerCardLogic

        public static bool HandleCardLogic(Card card, ServerGameManager manager)
        {
            bool skipped = false;
            
            bool isWild = card.type == CardType.WILD;
            
            if (isWild)
            {
                switch (card.secondaryType)
                {
                    case CardType.WILD:
                        Wild(manager);
                        break;
                    case CardType.DRAWFOUR:
                        DrawFour(manager);
                        break;
                    case CardType.SHUFFLE:
                        Shuffle(manager);
                        break;
                }
            }
            else
            {
                switch (card.type)
                {
                    case CardType.SKIP:
                        skipped = true;
                        Skip(manager);
                        break;
                    case CardType.DRAWTWO:
                        skipped = true;
                        DrawTwo(manager);
                        break;
                    case CardType.REVERSE:
                        Reverse(manager);
                        break;
                }
            }

            return skipped;
        }
        
        private static void Skip(ServerGameManager manager)
        {
            manager.NewTurn(manager.NextTurn(manager.NextTurn(manager.turnIndex)));
        }

        private static void DrawTwo(ServerGameManager manager)
        {
            int nextTurn = manager.NextTurn(manager.turnIndex);
            
            Debug.Log("Next turn: " + nextTurn);
            
            manager.NewTurn(manager.NextTurn(manager.NextTurn(manager.turnIndex)));

            // plus one because 0 is not a client id and turn indexs start from 1
            UNOPlayer playerToDraw2 = manager.Players[(ushort)(nextTurn+1)];
            playerToDraw2.AddCard(manager.Deck.Draw());
            playerToDraw2.AddCard(manager.Deck.Draw());
        }

        private static void DrawFour(ServerGameManager manager)
        {
            int nextTurn = manager.NextTurn(manager.turnIndex);
            
            Debug.Log("Next turn: " + nextTurn);


            // plus one because 0 is not a client id and turn indexs start from 1
            UNOPlayer playerToDraw4 = manager.Players[(ushort)(nextTurn+1)];
            playerToDraw4.AddCard(manager.Deck.Draw());
            playerToDraw4.AddCard(manager.Deck.Draw());
            playerToDraw4.AddCard(manager.Deck.Draw());
            playerToDraw4.AddCard(manager.Deck.Draw());
            
            Wild(manager);
        }

        private static void Wild(ServerGameManager manager)
        {
            Debug.Log("wild");
            manager.ChooseNewColour();
        }

        private static void Shuffle(ServerGameManager manager)
        {
            manager.ShuffleAllHands();
            Wild(manager);
        }

        private static void Reverse(ServerGameManager manager)
        {
            manager.turnDirection = manager.turnDirection == TurnDirection.FORWARD
                ? TurnDirection.BACKWARD
                : TurnDirection.FORWARD;
        }

        #endregion
    }
}