using System;
using System.Collections.Generic;
using System.IO;

namespace Day_3
{
    class Program
    {
        static void Main(string[] args)
        {
            // Basic idea:
            // Find max dimension in each direction
            // Make a two-dimensional int array
            // Process each line
            // Increment corresponding element in two-dimensional array for every area
            // Count >1 elements in two-dimensional array

            StreamReader reader;
            List<FabricArea> areas = new List<FabricArea>();
            
            try {
                reader = new StreamReader("input");
            } catch(Exception e) {
                Console.WriteLine("File could not be read:\n" + e.Message);
                return;
            }

            while(reader.Peek() != -1)
                areas.Add(new FabricArea(reader.ReadLine()));

            // Find max dimensions in each direction
            int xMax = 0;
            int yMax = 0;
            foreach(FabricArea i in areas){
                if((i.xPos + i.xLength) > xMax) xMax = i.xPos + i.xLength;
                if((i.yPos + i.yLength) > yMax) yMax = i.yPos + i.yLength;
            }

            int[,] fabric = new int[xMax,yMax];

            foreach(FabricArea area in areas)
                for(int i = 0; i < area.xLength; i++)
                    for(int j = 0; j < area.yLength; j++)
                        fabric[area.xPos+i,area.yPos+j]++;

            int overlappingSquareInches = 0;
            for(int i = 0; i < xMax; i++)
                for(int j = 0; j < yMax; j++)
                    if(fabric[i,j] > 1) overlappingSquareInches++;

            Console.WriteLine("There are " + overlappingSquareInches + " squares inches of overlap.");
            
            foreach(FabricArea area in areas)
                if(area.OverlapInTotalArea(fabric))
                    Console.WriteLine("Area " + area.ID + " has no overlap.");
        }

        class FabricArea{
            
            public string ID { get; set; }
            public int xPos { get; set; }
            public int yPos { get; set; }
            public int xLength { get; set; }
            public int yLength { get; set; }

            public FabricArea(string line){
                // #1 @ 342,645: 25x20
                line = line.Replace(" ", "");
                line = line.Replace("#","");
                string[] lineSplitByAlpha = line.Split("@");
                ID = lineSplitByAlpha[0];
                string[] lineSplitByColon = lineSplitByAlpha[1].Split(":");
                string[] positions = lineSplitByColon[0].Split(",");
                xPos = Int32.Parse(positions[0]);
                yPos = Int32.Parse(positions[1]);
                string[] sizes = lineSplitByColon[1].Split("x");
                xLength = Int32.Parse(sizes[0]);
                yLength = Int32.Parse(sizes[1]);
            }

            public bool OverlapInTotalArea(int[,] totalArea){
                for(int i = 0; i < this.xLength; i++)
                    for(int j = 0; j < this.yLength; j++)
                        if(totalArea[this.xPos+i,this.yPos+j] > 1) return false;
                return true;
            }
        }
    }
}
