using System;
using System.Collections.Generic;

namespace Day_9
{
    class Program
    {
        // Arg1: Last marble
        // Arg2: Amount of players
        static void Main(string[] args)
        {
            int amountOfPlayers = Convert.ToInt32(args[0]);
            int lastMarbleInt = Convert.ToInt32(args[1]);
            if (amountOfPlayers == 0)
            {
                Console.WriteLine("Must be at least one player playing.");
                return;
            }
            
            UInt64 lastMarble = (UInt64)lastMarbleInt;
            MarbleRing<UInt64> marbleRing = new MarbleRing<UInt64>(0);
            Ring<(int playerNumber, UInt64 playerScore)> players = new Ring<(int playerNumber, UInt64 playerScore)>();

            LinkedListNode<(int playerNumber, UInt64 playerScore)> currentPlayer = players.AddFirst((1, 0));

            // Add players
            for (int i = 2; i <= amountOfPlayers; i++)
                currentPlayer = players.AddAfter(currentPlayer, (i, 0));

            // Play game
            for (UInt64 i = 1; i <= lastMarble; i++)
                // Add a marble
                if (i % 23 == 0)
                {
                    LinkedListNode<UInt64> removedMarble = marbleRing.MarbleSpecialMove();

                    // Increment player score
                    (int playerNumber, UInt64 playerScore) currentPlayerValue = currentPlayer.Value;
                    currentPlayerValue.playerScore += i;
                    currentPlayerValue.playerScore += removedMarble.Value;
                    currentPlayer.Value = currentPlayerValue;

                    currentPlayer = players.GetNextInRing(currentPlayer);
                }
                else
                {
                    marbleRing.MarbleBasicMove(i);
                    currentPlayer = players.GetNextInRing(currentPlayer);
                }

            // Tally scores
            UInt64 topScore = 0;
            foreach ((int playerNumber, UInt64 playerScore) player in players)
                if (player.playerScore > topScore)
                    topScore = player.playerScore;

            Console.WriteLine("Top score was: " + topScore);
        }
    }
    class Ring<T> : LinkedList<T>
    {
        public LinkedListNode<T> GetNextInRing(LinkedListNode<T> current)
        {
            if (current.Next == null)
                return First;
            return current.Next;
        }
        public LinkedListNode<T> GetPreviousInRing(LinkedListNode<T> current)
        {
            if (current.Previous == null)
                return Last;
            return current.Previous;
        }
    }
    class MarbleRing<T> : Ring<T>
    {

        LinkedListNode<T> CurrentMarble;

        public MarbleRing(T first)
        {
            CurrentMarble = AddFirst(first);
        }

        public void MarbleBasicMove(T newElement)
        {
            LinkedListNode<T> nextMarble = GetNextInRing(CurrentMarble);
            CurrentMarble = AddAfter(nextMarble, newElement);
        }

        public LinkedListNode<T> MarbleSpecialMove()
        {
            LinkedListNode<T> previousNode = CurrentMarble;
            for (int i = 0; i < 7; i++)
                previousNode = GetPreviousInRing(previousNode);

            CurrentMarble = previousNode.Next;
            Remove(previousNode);
            return previousNode;
        }
    }

}

