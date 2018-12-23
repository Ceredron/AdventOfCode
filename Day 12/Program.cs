using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_12
{
    class Program
    {
        // Assumption: non-contradicting rules
        // Arg0: Generations
        static void Main(string[] args)
        {
            Int64 generations = Convert.ToInt64(args[0]);
            List<string> lines = readInput();

            string initialStateString = lines[0].Substring(15);

            List<bool> rightCenterPots = new List<bool>();
            List<bool> leftPots = new List<bool>();
            for(int i = 0; i < initialStateString.Length; i++){
                rightCenterPots.Add(initialStateString[i] == '#'?true:false);
            }
            bool[] padding = new bool[4];
            rightCenterPots = rightCenterPots.Concat(padding).ToList();// To pad up
            
            for(int i = leftPots.Count; i < rightCenterPots.Count; i++)
                leftPots.Add(false);
            leftPots.RemoveAt(0); // Because rightCenterPots also includes center

            List<bool> pots = leftPots.Concat(rightCenterPots).ToList();

            // Transformation rule: [L,L,C,R,R,Result]
            List<(bool[], bool)> transformationRules = new List<(bool[], bool)>();
            for(int i = 2; i < lines.Count; i++){
                bool[] transformationRuleAntecedent = new bool[5];
                transformationRuleAntecedent[0] = lines[i][0] == '#';
                transformationRuleAntecedent[1] = lines[i][1] == '#';
                transformationRuleAntecedent[2] = lines[i][2] == '#';
                transformationRuleAntecedent[3] = lines[i][3] == '#';
                transformationRuleAntecedent[4] = lines[i][4] == '#';
                transformationRules.Add((transformationRuleAntecedent,lines[i][9] == '#'));
            }
            
            int[] deltaBuffer = new int[10];
            int deltaIterator = 0;
            int lastPlansum = 0;
            for(int i = 0; i < generations; i++){
                pots = ApplyTransformationRules(pots, transformationRules);
                int plantSumInner = 0;
                int potNumberInner = (pots.Count/2)*-1;
                for(int j = 0; j < pots.Count; j++){
                    if(pots[j])
                        plantSumInner += potNumberInner;
                    potNumberInner++;
                }
                Console.WriteLine("Plant sum after " + (i+1) + " generations: " + plantSumInner);

                // Part two solution, buffer deltas and apply linear function when 10 deltas are equal
                deltaBuffer[deltaIterator] = plantSumInner-lastPlansum;
                lastPlansum = plantSumInner;
                deltaIterator++;
                if(deltaIterator == 10)
                    deltaIterator = 0;
                if(i > 10 && deltaBuffer.Distinct().ToList().Count == 1){ // If we have gone through more than 10 elements, and there is only one distinct element in buffer, we can numerically calculate rest
                    UInt64 calculatedPlantSum = (UInt64)(plantSumInner) + (UInt64)((generations-(i+1))*deltaBuffer[0]);
                    Console.WriteLine("Plant sum after " + generations + " generations: " + calculatedPlantSum);
                    return;
                }
            }

            int plantSum = 0;
            int potNumber = (pots.Count/2)*-1;
            for(int i = 0; i < pots.Count; i++){
                if(pots[i])
                    plantSum += potNumber;
                potNumber++;
            }
            Console.WriteLine("Plant sum after " + generations + " generations: " + plantSum);
        }

        static List<bool> ApplyTransformationRules(List<bool> pots, List<(bool[], bool)> transformationRules){
            List<(int,bool)> changes = new List<(int,bool)>();
            for(int i = 0; i < pots.Count-5; i++)
                foreach((bool[], bool) transformationRule in transformationRules)
                    if(pots.GetRange(i,5).SequenceEqual(transformationRule.Item1)){
                        changes.Add((i+2,transformationRule.Item2));
                        break;
                    }
            
            foreach((int index, bool newValue) change in changes)
                pots[change.index] = change.newValue;

            //Resize if necessary
            if(pots[4] | pots[pots.Count-5]){
                pots.Add(false);
                pots.Add(false);
                pots.Insert(0,false);
                pots.Insert(0,false);
            } 

            return pots;
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
