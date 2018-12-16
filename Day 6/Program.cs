using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_6
{
    class Program
    {
        // The border vertices, if sufficiently offset, indicates the infinite coordinates, and must include every coordinate
        static void Main(string[] args)
        {
            List<string> lines = readInput();
            List<Point> points = new List<Point>();

            foreach(string line in lines){
                string[] coordStrings = line.Split(',');
                points.Add(new Point(Int32.Parse(coordStrings[0]), Int32.Parse(coordStrings[1])));
            }

            // Get upper dimension
            int upperX = 0;
            int upperY = 0;
            foreach(Point point in points){
                if(point.x > upperX)
                    upperX = point.x;
                if(point.y > upperY)
                    upperY = point.y;
            }
            upperY++;
            upperX++;

            int[,] coordMap = new int[upperX, upperY]; // Int points to index of closest point in List<Point> points
            int[] sizes = new int[points.Count];
            for(int x = 0; x < upperX; x++)
                for(int y = 0; y < upperY; y++){
                    Point currentCoord = new Point(x,y);
                    int closestCoord = 0;
                    int closestDistance = upperX+upperY;
                    bool foundEqualPoint = false;
                    for(int i = 0; i < points.Count; i++){
                        int thisDistance = points[i].GetManhattanDistance(currentCoord);
                        if(thisDistance < closestDistance){
                            closestDistance = thisDistance;
                            closestCoord = i;
                            foundEqualPoint = false;
                        } else if(thisDistance == closestDistance && thisDistance != 0){
                            foundEqualPoint = true;
                        }
                    }
                    if(foundEqualPoint)
                        coordMap[x,y] = -1;
                    else{
                        coordMap[x,y] = closestCoord;
                        sizes[closestCoord]++;
                    }
                }

            foreach(int a in sizes)
                Console.WriteLine(a);

            // Null out size of all the infinite ones
            for(int i = 0; i < coordMap.GetLength(0); i++){
                int index1 = coordMap[i,0];
                int index2 = coordMap[i,upperY-1];
                if(index1 != -1)
                    sizes[index1] = 0;
                if(index2 != -1)
                    sizes[index2] = 0;
            }

            for(int i = 0; i < coordMap.GetLength(1); i++){
                int index1 = coordMap[0,i];
                int index2 = coordMap[coordMap.GetLength(0)-1,i];
                if(index1 != -1)
                    sizes[index1] = 0;
                if(index2 != -1)
                    sizes[index2] = 0;
            }

            Console.WriteLine("Biggest size is : " + sizes.Max());

            // Part.2
            coordMap = new int[upperX, upperY];
            for(int x = 0; x < coordMap.GetLength(0); x++){
                for(int y = 0; y < coordMap.GetLength(1); y++){
                    foreach(Point p in points)
                        coordMap[x,y] += p.GetManhattanDistance(new Point(x,y));
                }
            }

            int partTwoArea = 0;
            for(int x = 0; x < coordMap.GetLength(0); x++)
                for(int y = 0; y < coordMap.GetLength(1); y++)
                    if(coordMap[x,y] < 10000)
                        partTwoArea++;

            Console.WriteLine("Part 2 area: " + partTwoArea);
        }

        private class Point{
            public int x { get; set; }
            public int y { get; set; }
            public Point(int x, int y){
                this.x = x;
                this.y = y;
            }
            public int GetManhattanDistance(Point other){
                return (Math.Abs(this.x-other.x)+Math.Abs(this.y-other.y));
            }
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
