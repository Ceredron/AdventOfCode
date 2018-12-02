using System;
using System.Collections.Generic;
using System.IO;

namespace Day_2
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader reader;
            List<int[]> occurences = new List<int[]>();
            List<string> lines = new List<string>();

            try {
                reader = new StreamReader("input");
            } catch(Exception e) {
                Console.WriteLine("File could not be read:\n" + e.Message);
                return;
            }

            // Count occurrences of each letter in each line
            while(reader.Peek() != -1){
                string line = reader.ReadLine();
                int[] lineOccurences = CountOccurrences(line);
                occurences.Add(lineOccurences);        
                lines.Add(line); 
            }

            // Count ones with double and triples
            List<Tuple<bool,bool>> doublesAndTriples = CountDoublesAndTriples(occurences);

            int checksum = CalculateChecksum(doublesAndTriples);

            Console.WriteLine("The checksum is: " + checksum);

            // Part.2 - Find the two ID's that only deviate by one letter, and find their overlapping letters
            string FindOverlapInLines(){
                string result;
                for(int i = 0; i < lines.Count; i++)
                    for(int j = 0; j < lines.Count; j++){
                        result = GetOverlap(lines[i], lines[j]);
                        if(result.Length == (lines[i].Length-1))
                            return result;
                    }
                return "";
            }
            string overlap = FindOverlapInLines();

            Console.WriteLine("Overlap is : " + overlap);
        }

        static int[] CountOccurrences(string line){
            int[] lineOccurences = new int[26];
            foreach(char letter in line){
                int position = letter-97; // To convert from the letter to the correct index in range {0,25}
                lineOccurences[position]++;
            }
            return lineOccurences;
        }

        static List<Tuple<bool, bool>> CountDoublesAndTriples(List<int[]> lineOccurences){
            
            List<Tuple<bool, bool>> DoublesAndTriples = new List<Tuple<bool, bool>>();

            foreach(int[] letterOccurences in lineOccurences){
                bool doubleLetter = false;
                bool tripleLetter = false;

                foreach(int occurences in letterOccurences){
                    if (occurences == 2) doubleLetter = true;
                    if (occurences == 3) tripleLetter = true;
                }

                DoublesAndTriples.Add(new Tuple<bool, bool>(doubleLetter, tripleLetter));
            }
            return DoublesAndTriples;
        }

        static int CalculateChecksum(List<Tuple<bool, bool>> doublesAndTriples){
            int doubles = 0;
            int triples = 0;

            foreach(Tuple<bool, bool> doubleAndTriple in doublesAndTriples){
                doubles += doubleAndTriple.Item1?1:0;
                triples += doubleAndTriple.Item2?1:0;
            }

            return doubles*triples;
        }

        static string GetOverlap(string line1, string line2){
            char[] line1array = line1.ToCharArray();
            char[] line2array = line2.ToCharArray();
            List<char> resultList = new List<char>();

            for(int i = 0; i < line1array.Length; i++){
                if(line1array[i] == line2array[i])
                    resultList.Add(line1array[i]);
            }

            return new string(resultList.ToArray());
        }
    }
}
