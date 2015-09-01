using System;
using System.Collections.Generic;
using AntsAlg.AntsAlgorithm.Algorithm;
using AntsAlg.AntsAlgorithm.Graph;

namespace AntsAlg.QapAlg
{
    public interface IQapAntAlgorithm : IAlgorithm<QapGraph, IList<IVertex>>
    {
        double GetDislocationCoefficient();
        int GetPermutationalIndex(int low, int high);
        void GeneratePath(int[] path);
        void CreateAnts();
        IList<IVertex> AntsTravel();
        IVertex GetNextNode(Tuple<int, int[], int[], int[]> tuple);
        void LocalSearch(QapAnt ant);
        int ComputeMoveCost(int r, int s, IList<IVertex> path);
        int ComputePathCost(QapAnt ant);
        bool IsNewBestPath(QapAnt ant);
        void UpdateGeneration();
        bool IsFinished();
        void Run();
        void NewResultUpdate();
    }
}