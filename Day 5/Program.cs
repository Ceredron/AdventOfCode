using System;
using System.Collections.Generic;
using System.IO;

namespace Day_5
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = readInput();
            if(input == null)
                return;
            
            List<char> inputList = new List<char>(input.ToCharArray());
            List<Char> processedString = RemovePolymers(inputList);

            Console.WriteLine("Input was " + input.Length + " long. Is now " + processedString.Count + ".");

            ShortestPolymerWithCharRemoved(inputList);
        }

        static string readInput()
        {
            StreamReader reader;

            try
            {
                reader = new StreamReader("input");
            }
            catch (Exception e)
            {
                Console.WriteLine("File could not be read:\n" + e.Message);
                return null;
            }
            return reader.ReadLine();
        }

        static List<char> RemovePolymers(List<char> inputList){
            bool reiterate = true;
            while(reiterate){
                reiterate = false;
                for(int i = 0; i < (inputList.Count-1); i++){
                    bool match = false;
                    if(Char.ToUpper(inputList[i]) == Char.ToUpper(inputList[i+1]))
                        match = Char.IsUpper(inputList[i])^Char.IsUpper(inputList[i+1]);
        
                    if(match){
                        inputList.RemoveAt(i+1);
                        inputList.RemoveAt(i);
                        reiterate=true;
                    }
                }            
            }
            return inputList;
        }

        static void ShortestPolymerWithCharRemoved(List<char> inputList){
            for(int i = 0; i < 26; i++){
                List<char> inputListCharRemoved = new List<char>(inputList);
                char lower = (char)(97+i);
                char upper = Char.ToUpper(lower);
                inputListCharRemoved.RemoveAll(element => element == lower);
                inputListCharRemoved.RemoveAll(element => element == upper);
                List<char> processedList = RemovePolymers(inputListCharRemoved);
                Console.WriteLine("For " + lower + " there were " + processedList.Count);
            }    
        }
    }
}
