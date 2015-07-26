using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Algorithm.Models;
using AntsLibrary.Classes;
using Grsu.Lab.Aoc.Contracts;

namespace Algorithm.HelpMethods
{
    public static class Helper
    {
        public static List<List<int>> SplitListIntoLists(List<int> ints, int number)
        {
            return ints.Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / number)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static int[] ConvertMatrixToArray(int n, int[,] ints)
        {
            List<int> lineInts = new List<int>();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    var elem = ints[i, j];
                    lineInts.Add(elem);
                }
            }
            return lineInts.ToArray();
        }

        public static void SetGraphMatrices(IAlgorithm algorithm, GraphViewModel matrix)
        {
            algorithm.Graph.Edges.Clear();

            int n = algorithm.Graph.Nodes.Count;
            int pheromone = ((AntAlgorithm)algorithm).Pheromone;

            for (int i = 0; i < matrix.DistGraph.Count(); i++)
            {
                var elem = matrix.DistGraph[i];
                ((Graph)algorithm.Graph).DistanceMatrix[i / n, i % n] = elem;
                algorithm.Graph.Edges.Add(new Edge
                {
                    Begin = i / n,
                    End = i % n,
                    HeuristicInformation = elem,
                    Pheromone = pheromone
                });
            }
            for (int i = 0; i < matrix.FlowGraph.Count(); i++)
            {
                var elem = matrix.FlowGraph[i];
                ((Graph)algorithm.Graph).FlowMatrix[i / n, i % n] = elem;
            }
        }
        
    }
}