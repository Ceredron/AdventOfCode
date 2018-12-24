using System;

namespace Day_11
{
    class Program
    {
        // Brute force
        // Arg0: Grid Serial Number
        static void Main(string[] args)
        {
            int gridSerialNumber = Convert.ToInt32(args[0]);
            int[,] fuelCellGrid = new int[301, 301];
            for(int x = 1; x <= 300; x++)
                for(int y = 1; y <= 300; y++)
                    fuelCellGrid[x,y] = GetFuelCellPower(x,y,gridSerialNumber);

            int[,] fuelCellSquares = new int[299,299];
            for(int x = 1; x <= 298; x++)
                for(int y = 1; y <= 298; y++)
                    fuelCellSquares[x,y] = GetFuelSquarePower(x, y, fuelCellGrid);

            (int x, int y) maxCoord = (1,1);
            int maxValue = fuelCellSquares[1,1];            
            for(int x = 1; x <= 298; x++)
                for(int y = 1; y <= 298; y++)
                    if(fuelCellSquares[x,y] > maxValue){
                        maxCoord = (x,y);
                        maxValue = fuelCellSquares[x,y];
                    }

            Console.WriteLine("Part1: x: " + maxCoord.x + " y: " + maxCoord.y + " Value: " + maxValue);

            (int x, int y, int size) maxCoordWithSize = (1,1,1);
            maxValue = fuelCellGrid[1,1];
            int[, ,] fuelCellVariableSquares = new int[299,299,298];            
            for(int x = 1; x <= 298; x++)
                for(int y = 1; y <= 298; y++){
                    int maxSize = Math.Min(298-x,298-y);
                    for(int size = 1; size < maxSize; size++){
                        int squarePowerLevel = 0;
                        for(int squareX = x; squareX < (x+size); squareX++)
                            for(int squareY = y; squareY < (y+size); squareY++)
                                squarePowerLevel += fuelCellGrid[squareX,squareY];
                        fuelCellVariableSquares[x,y,size] = squarePowerLevel;
                        if(squarePowerLevel > maxValue){
                            maxValue = squarePowerLevel;
                            maxCoordWithSize = (x,y,size);
                        }
                    }
                }
            Console.WriteLine("Part2: x: " + maxCoordWithSize.x + " y: " + maxCoordWithSize.y + " Size: " + maxCoordWithSize.size + " Value: " + maxValue);

        }

        static int GetFuelCellPower(int x, int y, int gridSerialNumber){
            int rackID = x+10;
            int powerLevel = rackID*y;
            powerLevel += gridSerialNumber;
            powerLevel *= rackID;
            powerLevel = (powerLevel % 1000) / 100; // Get hundredth digit
            powerLevel -= 5;
            return powerLevel;
        }

        static int GetFuelSquarePower(int x, int y, int[,] fuelCellGrid){
            int squarePowerLevel = 0;
            for(int squareX = 0; squareX < 3; squareX++)
                for(int squareY = 0; squareY < 3; squareY++)
                    squarePowerLevel += fuelCellGrid[x+squareX,y+squareY];
            return squarePowerLevel;
        }
    }
}
