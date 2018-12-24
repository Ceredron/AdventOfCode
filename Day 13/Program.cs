using System;
using System.Collections.Generic;
using System.IO;

namespace Day_13
{
    // Exception throwing solution
    class Program
    {
        static char[,] TrackMap;
        static int ticks = 0;
        static SortedList<(int y, int x), (char, int)> Carts;

        static bool part1 = false;
        //Args0: part
        static void Main(string[] args)
        {
            if(args[0] == "1")
                part1 = true;
            else if(args[0] == "2")
                part1 = false;
            else   
                throw new Exception("Must be run with a part (1 or 2).");
            List<string> lines = readInput();
            TrackMap = new char[lines[0].Length, lines.Count];
            Carts = new SortedList<(int y, int x), (char, int)>();
            for (int y = 0; y < lines.Count; y++)
                for (int x = 0; x < lines[y].Length; x++)
                {
                    switch (lines[y][x])
                    {
                        case '^':
                            TrackMap[x,y] = '|';
                            Carts.Add((y,x),('^',1));
                            break;
                        case 'v':
                            TrackMap[x,y] = '|';
                            Carts.Add((y,x),('v', 1));
                            break;
                        case '>':
                            TrackMap[x,y] = '-';
                            Carts.Add((y,x),('>', 1));
                            break;
                        case '<':
                            TrackMap[x,y] = '-';
                            Carts.Add((y,x),('<', 1));
                            break;
                        default:
                            TrackMap[x, y] = lines[y][x];
                            break;
                    }
                }

            while (true){
                
                // Debugger
                for (int y = 25; y < 35; y++){
                    for (int x = 50; x < 60; x++)
                        if(Carts.ContainsKey((y,x)))
                            Console.Write(Carts.GetValueOrDefault((y,x)).Item1);
                        else
                            Console.Write(TrackMap[x,y]);
                    Console.WriteLine();
                }
                Console.WriteLine();
                SimulateTick();
            }
        }

        static void SimulateTick()
        {
            // For every cart, move it
            // Have to be ordered by y
            List<(int y, int x)> cartList = new List<(int, int)>();
            foreach((int y, int x) cart in Carts.Keys)
                cartList.Add(cart);
            foreach((int y, int x) cart in cartList)
                MoveCart(cart);
                
            ticks++;
            if(!part1 && Carts.Count == 1){{
                    throw new Exception("Final cart is at " + Carts.Keys[0].x + ", " + Carts.Keys[0].y);
                }
            }
            Console.WriteLine("Ticks: " + ticks + " Carts left: " + Carts.Count);
        }

        static void MoveCart((int y, int x) cart){
            (char direction, int state) cartValue = ('.',0);
            Carts.TryGetValue(cart, out cartValue);
            if(cartValue.state == 0)
                return; // Cart was crashed into previously in this tick

            var next = GetNextTrack(cart, cartValue.direction);
            // Check for collision
            if(Carts.ContainsKey(next.coord)){
                Carts.Remove(cart);
                Carts.Remove(next.coord);
                if(part1){
                    throw new Exception("Crashed at " + next.coord.x + ", " + next.coord.y);
                }
                return;
            }

            char newDirection;
            switch(next.trackPart){
                case 'X':
                    return;
                case '|':
                case '-':
                    Carts.Remove(cart);
                    Carts.Add((next.coord.y, next.coord.x), cartValue);
                    break;
                case '\\':
                case '/':
                    newDirection = GetNewCartDirection(cartValue.direction, next.trackPart, cartValue.state);
                    Carts.Remove(cart);
                    Carts.Add((next.coord.y, next.coord.x), (newDirection, cartValue.state));
                    break;
                case '+':
                    newDirection = GetNewCartDirection(cartValue.direction, next.trackPart, cartValue.state);
                    Carts.Remove(cart);
                    Carts.Add((next.coord.y, next.coord.x), (newDirection, cartValue.state+1));
                    break;
                default:
                    throw new Exception("Error in input: " + next.trackPart + " at (" + next.coord.x + ", " + next.coord.y + ")");
            }
        }

        static (char trackPart, (int y, int x) coord) GetNextTrack((int y, int x) cart, char cartDirection){
            switch(cartDirection){
                case 'X':
                    return ('X', (cart.y,cart.x));
                case '^':
                    return (TrackMap[cart.x,cart.y-1], (cart.y-1, cart.x));
                case 'v':
                    return (TrackMap[cart.x,cart.y+1], (cart.y+1, cart.x));
                case '>':
                    return (TrackMap[cart.x+1,cart.y], (cart.y, cart.x+1));
                case '<':
                    return (TrackMap[cart.x-1,cart.y], (cart.y, cart.x-1));   
            }
            throw new Exception("Invalid cart direction.");
        }

        static char GetNewCartDirection(char oldDirection, char newTrackpart, int state){
            if(newTrackpart == '+'){
                if (state % 3 == 2) // Straight ahead
                    return oldDirection;
                if (state % 3 == 1) // To left
                    switch(oldDirection){
                        case '^':
                            return '<';
                        case 'v':
                            return '>';
                        case '>':
                            return '^';
                        case '<':
                            return 'v';   
                    }
                
                if (state % 3 == 0) // To right
                    switch(oldDirection){
                        case '^':
                            return '>';
                        case 'v':
                            return '<';
                        case '>':
                            return 'v';
                        case '<':
                            return '^';   
                    }
            }


            if((oldDirection == '^' && newTrackpart == '\\') || (oldDirection == 'v' && newTrackpart == '/'))
                return '<';
            if((oldDirection == '^' && newTrackpart == '/') || (oldDirection == 'v' && newTrackpart == '\\'))
                return '>';
            if((oldDirection == '>' && newTrackpart == '\\') || (oldDirection == '<' && newTrackpart == '/'))
                return 'v';
            if((oldDirection == '<' && newTrackpart == '\\') || (oldDirection == '>' && newTrackpart == '/'))
                return '^';

            throw new Exception("Error in cart direction.");
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
