using System;
using System.Collections.Generic;
using System.IO;

namespace Day_4
{
    class Program
    {
        public enum EventTypes { BEGINSHIFT, ASLEEP, WAKEUP };
        static void Main(string[] args)
        {
            List<string> lines = readInput();
            Dictionary<string, int[]> guardMinutes = new Dictionary<string, int[]>();

            lines.Sort();

            string lastGuardEncountered = "";
            int lastMinuteFellAsleep = 0;
            foreach (string line in lines)
            {
                string[] lineWords = line.Split(' ');
                // lineWords[1] = hours:min] (XX:YY])
                // lineWords[2] = Guard|falls|wakes
                // lineWords[3] = Guard=>#XXX
                string minuteString = lineWords[1].Substring(3, 2);
                switch (lineWords[2])
                {
                    case "Guard":
                        lastGuardEncountered = lineWords[3];
                        break;
                    case "falls":
                        lastMinuteFellAsleep = Int32.Parse(minuteString);
                        break;
                    case "wakes":
                        int minute = Int32.Parse(minuteString);
                        int[] minuteArray;
                        if (!guardMinutes.TryGetValue(lastGuardEncountered, out minuteArray))
                            minuteArray = new int[60];
                        for (int i = lastMinuteFellAsleep; i != minute; i++)
                        {
                            if (i == 60)
                            { // Wrap-around minute
                                i = -1;
                                continue;
                            }
                            minuteArray[i]++;
                        }
                        guardMinutes.Remove(lastGuardEncountered);
                        guardMinutes.Add(lastGuardEncountered, minuteArray);
                        break;
                    default:
                        throw new Exception("Invalid input");

                }
            }

            // Part1. Get guard with most minutes asleep
            string sleepiestGuard = "";
            int sleepiestGuardMinutes = 0;
            foreach (KeyValuePair<string, int[]> guardMinute in guardMinutes)
            {
                int minutesAsleep = 0;
                foreach (int minutes in guardMinute.Value)
                    minutesAsleep += minutes;
                if (minutesAsleep > sleepiestGuardMinutes)
                {
                    sleepiestGuard = guardMinute.Key;
                    sleepiestGuardMinutes = minutesAsleep;
                }
            }
            // Get minute he slept the most
            int[] minuteSleptMost;
            if (!guardMinutes.TryGetValue(sleepiestGuard, out minuteSleptMost))
                minuteSleptMost = new int[60];
            int biggest = 0;
            for (int i = 0; i < minuteSleptMost.Length; i++)
                if (minuteSleptMost[i] > minuteSleptMost[biggest])
                    biggest = i;

            Console.WriteLine("Guard " + sleepiestGuard + " slept " + sleepiestGuardMinutes + " and most at minute " + biggest);

            // Part.2. Get guard who slept most at a given minute
            sleepiestGuard = "";
            int sleepiestGuardMinute = 0;
            int biggestSoFar = 0;
            foreach (KeyValuePair<string, int[]> guardMinute in guardMinutes)
            {
                for (int i = 0; i < guardMinute.Value.Length; i++)
                    if (guardMinute.Value[i] > biggestSoFar)
                    {
                        biggestSoFar = guardMinute.Value[i];
                        sleepiestGuard = guardMinute.Key;
                        sleepiestGuardMinute = i;
                    }
            }
            Console.WriteLine("Guard " + sleepiestGuard + " slept " + biggestSoFar + " times at minute " + sleepiestGuardMinute);

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
