using System;
using System.Collections.Generic;
using System.IO;

namespace Day_8
{
    class Program
    {
        static void Main(string[] args)
        {
            StreamReader reader;

            try
            {
                reader = new StreamReader("input");
            }
            catch (Exception e)
            {
                Console.WriteLine("File could not be read:\n" + e.Message);
                return;
            }
            
            string line = reader.ReadLine();
            string[] numberStrings = line.Split(' ');
            List<int> numbers = new List<int>();

            foreach(string numberString in numberStrings){
                int number = Convert.ToInt32(numberString);
                numbers.Add(number);
            }

            (int, int) partOneResult = GetMetadataSumAndNodeLength(numbers, 0);
            Console.WriteLine("Metadata sum is: " + partOneResult.Item1);

            (int, int) partTwoResult = GetValueOfMetadataReferencedChildNodes(numbers, 0);
            Console.WriteLine("Part two result is: " + partTwoResult.Item1);
        }
        

        private static (int, int) GetMetadataSumAndNodeLength(List<int> numbers, int startIndex){
            
            int childNodeQuantity = numbers[startIndex];
            int metadataQuantity = numbers[startIndex+1];
            int sum = 0;

            // Basecase
            if(childNodeQuantity == 0){ 
                for(int i = 0; i < metadataQuantity; i++)
                    sum += numbers[startIndex+2+i];
                
                return (sum, 2+metadataQuantity);
            }
            
            // Inductive step
            int totalNodeLength = 2;
            int totalNodeSum = 0;
            for(int i = 0; i < childNodeQuantity; i++){
                (int nodeSum, int nodeLength) result = GetMetadataSumAndNodeLength(numbers, startIndex+totalNodeLength);
                totalNodeLength += result.nodeLength;
                totalNodeSum += result.nodeSum;
            }
            for(int i = 0; i < metadataQuantity; i++)
                totalNodeSum += numbers[i+startIndex+totalNodeLength];

            totalNodeLength += metadataQuantity;
                    

            return (totalNodeSum,  totalNodeLength);
        }

        private static (int, int) GetValueOfMetadataReferencedChildNodes(List<int> numbers, int startIndex){
            int childNodeQuantity = numbers[startIndex];
            int metadataQuantity = numbers[startIndex+1];
            int sum = 0;

            // Basecase
            if(childNodeQuantity == 0){ 
                for(int i = 0; i < metadataQuantity; i++)
                    sum += numbers[startIndex+2+i];
                
                return (sum, 2+metadataQuantity);
            }

            // Inductive step
            int totalNodeLength = 2;
            int totalNodeSum = 0;
            int[] childNodeScores = new int[childNodeQuantity+1];
            for(int i = 1; i <= childNodeQuantity; i++){
                (int childNodeSum, int childNodeLength) = GetValueOfMetadataReferencedChildNodes(numbers, startIndex+totalNodeLength);
                childNodeScores[i] = childNodeSum;
                totalNodeLength += childNodeLength;
            }

            for(int i = 0; i < metadataQuantity; i++){
                int metadata = numbers[startIndex+totalNodeLength+i];
                if(metadata >= childNodeScores.Length || metadata == 0)
                    continue;
                totalNodeSum += childNodeScores[metadata];
            }

            totalNodeLength += metadataQuantity;

            Console.WriteLine("Child nodes: " + childNodeQuantity + " Metadata: " + metadataQuantity + " Total length: " + totalNodeLength);
            return (totalNodeSum, totalNodeLength);
        }
    }
}
