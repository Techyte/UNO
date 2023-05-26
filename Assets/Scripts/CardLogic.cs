﻿using UnityEngine;
using UNO.Enums;
using ClientGameManager = UNO.Client.GameManager;
using ServerGameManager = UNO.Server.GameManager;

namespace UNO.General
{
     public static class CardLogic
     {
    //     #region ClientCardLogic
    //
    //     public static void Skip(ClientGameManager manager)
    //     {
    //         Debug.Log("");
    //         // TODO: Logic for skipping turn client
    //     }
    //
    //     public static void DrawTwo(ClientGameManager manager)
    //     {
    //         int nextTurn = manager.NextTurn();
    //
    //         UNOPlayer playerToDraw2 = manager.Players[nextTurn];
    //         playerToDraw2.AddCard(manager.Deck.Draw());
    //         playerToDraw2.AddCard(manager.Deck.Draw());
    //         // TODO: Logic for drawing 2 client
    //     }
    //
    //     public static void DrawFour(ClientGameManager manager)
    //     {
    //         int nextTurn = manager.NextTurn();
    //
    //         UNOPlayer playerToDraw2 = manager.Players[nextTurn];
    //         playerToDraw2.AddCard(manager.Deck.Draw());
    //         playerToDraw2.AddCard(manager.Deck.Draw());
    //         playerToDraw2.AddCard(manager.Deck.Draw());
    //         playerToDraw2.AddCard(manager.Deck.Draw());
    //         
    //         manager.ChooseNewColour();
    //         // TODO: Logic for drawing 4 client
    //     }
    //
    //     public static void Wild(ClientGameManager manager)
    //     {
    //         manager.ChooseNewColour();
    //         // TODO: Logic for wild cards client
    //     }
    //
    //     public static void Shuffle(ClientGameManager manager)
    //     {
    //         manager.ShuffleAllHands();
    //         // TODO: Logic for shuffle client
    //     }
    //
    //     public static void Reverse(ClientGameManager manager)
    //     {
    //         manager.turnDirection = manager.turnDirection == TurnDirection.FORWARD
    //             ? TurnDirection.BACKWARD
    //             : TurnDirection.FORWARD;
    //         // TODO: Logic for reversing client
    //     }

        //#endregion

        #region ServerCardLogic

        public static void Skip(ServerGameManager manager)
        {
            manager.NewTurn(manager.NextTurn(manager.NextTurn(manager.turnIndex)));
        }

        public static void DrawTwo(ServerGameManager manager)
        {
            int nextTurn = manager.NextTurn(manager.turnIndex);
            
            Debug.Log("Next turn: " + nextTurn);
            
            manager.NewTurn(manager.NextTurn(manager.NextTurn(manager.turnIndex)));

            // plus one because 0 is not a client id and turn indexs start from 1
            UNOPlayer playerToDraw2 = manager.Players[(ushort)(nextTurn+1)];
            playerToDraw2.AddCard(manager.Deck.Draw());
            playerToDraw2.AddCard(manager.Deck.Draw());
        }

        public static void DrawFour(ServerGameManager manager)
        {
            int nextTurn = manager.NextTurn(manager.turnIndex);
            
            Debug.Log("Next turn: " + nextTurn);


            // plus one because 0 is not a client id and turn indexs start from 1
            UNOPlayer playerToDraw4 = manager.Players[(ushort)(nextTurn+1)];
            playerToDraw4.AddCard(manager.Deck.Draw());
            playerToDraw4.AddCard(manager.Deck.Draw());
            playerToDraw4.AddCard(manager.Deck.Draw());
            playerToDraw4.AddCard(manager.Deck.Draw());
            
            manager.ChooseNewColour();
        }

        public static void Wild(ServerGameManager manager)
        {
            manager.ChooseNewColour();
        }

        public static void Shuffle(ServerGameManager manager)
        {
            manager.ShuffleAllHands();
        }

        public static void Reverse(ServerGameManager manager)
        {
            manager.turnDirection = manager.turnDirection == TurnDirection.FORWARD
                ? TurnDirection.BACKWARD
                : TurnDirection.FORWARD;
        }

        #endregion
    }
}