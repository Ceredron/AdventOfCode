using System;
using System.Collections.Generic;
using System.IO;

namespace Day_1
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Read input file
            // 2. For part one
            //      * Sum up every line according to + or -
            //      * Print result
            // 3. For part two
            //      * Iterate through lines, add every line to a list, return first duplicate
            StreamReader sumReader;
            StreamReader duplicateReader;
            int frequency = 0;

            // 1.
            try {
                sumReader = new StreamReader("input");
                duplicateReader = new StreamReader("input");
            } catch(Exception e) {
                Console.WriteLine("File could not be read:\n" + e.Message);
                return;
            }

            // 2.
            try {
                frequency = SumLines(sumReader);
            } catch(Exception e) {
                Console.WriteLine(e.Message);
                return;
            }

            int firstDuplicate = DuplicateFrequency(duplicateReader);

            Console.WriteLine("The sum frequency is " + frequency);
            Console.WriteLine("The first duplicate is: " + firstDuplicate);
        }

        private static int SumLines(StreamReader reader){
            
            int frequency = 0;

            // Read every line
            while(reader.Peek() != -1){

                string line = reader.ReadLine();

                if(line[0] == '+')
                    frequency += Int32.Parse(line.Substring(1));

                else if(line[0] == '-')
                    frequency -= Int32.Parse(line.Substring(1));

                else
                    throw new Exception("Error in input-file: line was not prefixed with + or -.");
            }
            return frequency;
        }

        private static int DuplicateFrequency(StreamReader reader){
            
            List<string> lines = new List<string>();
            List<int> frequencies = new List<int>();
            int frequency = 0;

            // Read every line
            while(reader.Peek() != -1){
                lines.Add(reader.ReadLine());
            }

            // Find duplicate frequency
            while(true){              
                
                foreach(string line in lines){

                    if(frequencies.Contains(frequency))
                        return frequency;
                    frequencies.Add(frequency);

                    if(line[0] == '+')
                        frequency += Int32.Parse(line.Substring(1));

                    else if(line[0] == '-')
                        frequency -= Int32.Parse(line.Substring(1));

                }
            }
        }
    }
}
