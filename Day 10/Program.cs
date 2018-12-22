using System;
using System.Collections.Generic;
using System.IO;

namespace Day_10
{
    class Program
    {
        // Store all coordinates and their velocities in a data structure
        // Make function that simulates their positions after a given amount of time
        // Calculate the dimension of the coordinate map after a given amount of time
        // Simulate the transition until it reaches its minimum dimension
        // Print the minimum dimension map
        // Written clumsily
        static void Main(string[] args)
        {
            List<string> lines = readInput();
            List<(Scalar, Scalar)> inputData = new List<(Scalar, Scalar)>();

            foreach(string line in lines){
                string[] dataPoints = new string[4];
                dataPoints[0] = line.Substring(10, 6);
                dataPoints[1] = line.Substring(18, 6);
                dataPoints[2] = line.Substring(36, 2);
                dataPoints[3] = line.Substring(40, 2);
                inputData.Add((
                    new Scalar(Convert.ToInt32(dataPoints[0]), Convert.ToInt32(dataPoints[1])),
                    new Scalar(Convert.ToInt32(dataPoints[2]), Convert.ToInt32(dataPoints[3]))
                    ));
            }
            CoordinateVelocityMap coordVelocityMap = new CoordinateVelocityMap(inputData);

            int seconds = 0;
            bool isIncreasing = false;
            int previousDistance = CoordinateVelocityMap.GetMapSize(coordVelocityMap.GetFutureMap(seconds));
            while(!isIncreasing){
                seconds++;
                int newDistance = CoordinateVelocityMap.GetMapSize(coordVelocityMap.GetFutureMap(seconds));
                if(newDistance > previousDistance){
                    isIncreasing = true;
                    seconds--;
                } 
                previousDistance = newDistance;
            }

            Console.WriteLine("Smallest after " + seconds + " seconds.");
            CoordinateVelocityMap.ShowMap(coordVelocityMap.GetFutureMap(seconds));
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

    class CoordinateVelocityMap{
        public List<(Scalar, Scalar)> signals;

        public CoordinateVelocityMap(List<(Scalar, Scalar)> signals){
            this.signals = signals;
        }
        public List<Scalar> GetFutureMap(int secondsInFuture){
            List<Scalar> futureMap = new List<Scalar>();
            foreach((Scalar, Scalar) point in signals)
                futureMap.Add(new Scalar(point.Item1.x+point.Item2.x*secondsInFuture, point.Item1.y+point.Item2.y*secondsInFuture));
            return futureMap;
        }

        public static int GetMapSize(List<Scalar> map){
            (Scalar lowest, Scalar highest) dim = GetMapDimensions(map);

            int xDistance = dim.highest.x - dim.lowest.x;
            int yDistance = dim.highest.y - dim.lowest.y;

            return Math.Abs(xDistance)+Math.Abs(yDistance);
        }

        public static (Scalar, Scalar) GetMapDimensions(List<Scalar> map){
            int lowestX = map[0].x;
            int highestX = map[0].x;
            int lowestY = map[0].y;
            int highestY = map[0].y;

            foreach(Scalar point in map){
                if(point.x < lowestX)
                    lowestX = point.x;
                if(point.y < lowestY)
                    lowestY = point.y;
                if(point.x > highestX)
                    highestX = point.x;
                if(point.y > highestY)
                    highestY = point.y;
            }

            return (new Scalar(lowestX, lowestY), new Scalar(highestX, highestY));
        }

        public static void ShowMap(List<Scalar> map){
            // Normalize then show
            (Scalar lowest, Scalar highest) dim = GetMapDimensions(map);
            Console.WriteLine("Lowest: x: " + dim.lowest.x + " y: " + dim.lowest.y);
            Console.WriteLine("HIghest: x: " + dim.highest.x + " y: " + dim.highest.y);
            for(int y = 0; y <= (dim.highest.y-dim.lowest.y); y++){
                for(int x = 0; x <= (dim.highest.x-dim.lowest.x); x++)
                    if(map.Contains(new Scalar(x+dim.lowest.x,
                                               y+dim.lowest.y)))
                        Console.Write("*");
                    else   
                        Console.Write(" ");
                Console.WriteLine();
            }

        }
    }

    struct Scalar{
        public int x {get; set;}
        public int y {get; set;}

        public Scalar(int x, int y){
            this.x = x;
            this.y = y;
        }
    }
}
