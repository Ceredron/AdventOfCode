using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_7
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> lines = readInput();

            string outputPartOne = PartOne(lines);
           
            Console.WriteLine("Sequence: " + outputPartOne);
            
            int outputPartTwo = PartTwo(lines, 5);

            Console.WriteLine("Took the 5 workers " + outputPartTwo + " seconds.");
        }

        static int PartTwo(List<string> lines, int workers){
            
            int secondsWorked = 0;
            
            SortedList<char, SortedSet<char>> precededBy = GetPredecessorList(lines);

            SortedSet<char> finishedChars = new SortedSet<char>();
            SortedList<char, int> secondsLeft = new SortedList<char, int>();
            foreach(KeyValuePair<char, SortedSet<char>> predecessorPair in precededBy)
                secondsLeft.Add(predecessorPair.Key, 60 + (predecessorPair.Key-65));
            
            while(finishedChars.Count < precededBy.Count){
                secondsWorked++;
                int workersAvailableThisIteration = workers;
                SortedSet<char> charsEligibileForThisLevel = new SortedSet<char>();
                foreach(KeyValuePair<char, SortedSet<char>> charPredecessorPair in precededBy)
                    if(!finishedChars.Contains(charPredecessorPair.Key))
                        if(charPredecessorPair.Value.Except(finishedChars).Count() == 0)
                            charsEligibileForThisLevel.Add(charPredecessorPair.Key);

                foreach(char eligibleChar in charsEligibileForThisLevel){
                    int newVal = 0;
                    if(secondsLeft.TryGetValue(eligibleChar, out newVal))
                        if(newVal != 0){
                            newVal--;
                            secondsLeft.Remove(eligibleChar);
                            secondsLeft.Add(eligibleChar, newVal);
                            workersAvailableThisIteration--;
                        } else
                            finishedChars.Add(eligibleChar);
                    else
                        throw new Exception("Seconds left not filled in properly.");
                    if(workersAvailableThisIteration == 0)
                        break;
                }
            }
            return secondsWorked;
        }

        static int PartTwoGetTimeForChar(char letter){
            return 60 + (letter-65);
        }

        static string PartOne(List<string> lines){
            SortedList<char, SortedSet<char>> precededBy = GetPredecessorList(lines);
            
            string output = "";
            while(output.Length < precededBy.Count){
                SortedSet<char> charsEligibileForThisLevel = new SortedSet<char>();
                foreach(KeyValuePair<char, SortedSet<char>> charPredecessorPair in precededBy)
                    if(!output.Contains(charPredecessorPair.Key))
                        if(charPredecessorPair.Value.Except(output).Count() == 0)
                            charsEligibileForThisLevel.Add(charPredecessorPair.Key);
                
                output += charsEligibileForThisLevel.First();
            }

            return output;
        }

        static SortedList<char, SortedSet<char>> GetPredecessorList(List<string> lines){
            SortedList<char, SortedSet<char>> precededBy = new SortedList<char, SortedSet<char>>();
            foreach (string line in lines)
                precededBy = AddPredecessor(line[36], line[5], precededBy);

            SortedSet<char> unprecededChars = new SortedSet<char>();
            foreach(KeyValuePair<char, SortedSet<char>> charPredecessorPair in precededBy)
                foreach(char unprecededCharacter in charPredecessorPair.Value)
                    if(!precededBy.Keys.Contains(unprecededCharacter))
                        unprecededChars.Add(unprecededCharacter);

            foreach(char unprecededChar in unprecededChars)
                precededBy.Add(unprecededChar, new SortedSet<char>());

            return precededBy;
        }
        static SortedList<char, SortedSet<char>> AddPredecessor(char thisChar, char predecessor, SortedList<char, SortedSet<char>> precededBy)
        {
            SortedSet<char> predecessors = new SortedSet<char>();
            if(precededBy.TryGetValue(thisChar, out predecessors)){
                predecessors.Add(predecessor);
                precededBy.Remove(thisChar);
                precededBy.Add(thisChar, predecessors);
            }else {
                predecessors = new SortedSet<char>();
                predecessors.Add(predecessor);                
                precededBy.Add(thisChar, predecessors);
            }

            return precededBy;   
        }

        static List<string> readInput()
        {
            StreamReader reader;
            List<string> lines = new List<string>();

            try
            {
                reader = new StreamReader("input");
            }
            catch (Exception e)
            {
                Console.WriteLine("File could not be read:\n" + e.Message);
                return null;
            }

            // Count occurrences of each letter in each line
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();
                lines.Add(line);
            }

            return lines;
        }
    }
}
