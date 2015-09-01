using System;
using System.Collections.Generic;
using System.Linq;
using AntsAlg.AntsAlgorithm.Ants;
using AntsAlg.AntsAlgorithm.Graph;
using AntsAlg.AntsAlgorithm.Rules;

namespace AntsAlg.AntsAlgorithm.Algorithm
{
    public class StandartAlgorithm : IStandartAntAlgorithm
    {
        protected IList<IAnt> Ants;
        protected readonly IAntFactory AntFactory;
        protected readonly IDictionary<Type, ISelectRule> Rules;
        protected readonly IDictionary<Type, IIncrimentRule> UpdateRules;
        protected readonly IProber Prober;
        protected Path MinPathLength = new Path(double.MaxValue); //забыл даблмакс
        protected IList<IVertex> Result = new List<IVertex>();
        protected int CurrentIteration;
        protected int CurrentIterationNoChanges;

        public StandartAlgorithm(IAntFactory antFactory, IDictionary<Type, ISelectRule> rules, IProber prober,
            IDictionary<Type, IIncrimentRule> updateRules, int maxIterationsNoChanges, int maxIterations)
        {
            AntFactory = antFactory;
            Rules = rules;
            Prober = prober;
            UpdateRules = updateRules;
            MaxIterationsNoChanges = maxIterationsNoChanges;
            MaxIterations = maxIterations;
        }

        public IList<IVertex> Calculate(IGraph input)
        {
            SetDefaultPheramone(input);
            while (!IsFinished)
            {
                Ants = AntFactory.GeneratePapulation(input.Vertecies.Count);
                SetPositions(input);
                foreach (var ant in Ants)
                {
                    Travel(ant, input);
                }
                UpdateMinPath(input);
                UpdatePheramone(input);
                CurrentIteration++;
            }
            return Result;
        }

        protected void SetDefaultPheramone(IGraph input)
        {
            foreach (var edge in input.EdgesList)
            {
                edge.Mark.Pheromone = new Pheromone() {Value = 10};
            }
        }

        protected void UpdatePheramone(IGraph input)
        {
            foreach (var ant in Ants)
            {
                foreach (var item in input.GetEdgePath(ant.VisitedVetecies))
                {
                    item.Mark = UpdateRules[ant.GetType()].Proccess(item.Mark);
                }
            }
        }

        protected bool IsFinished
        {
            get
            {
                if (CurrentIteration == MaxIterations) return true;
                return CurrentIterationNoChanges >= MaxIterationsNoChanges;
            }
        }

        public int MaxIterationsNoChanges { get; private set; }

        public int MaxIterations { get; private set; }

        protected void SetPositions(IGraph input)
        {
            for (var i = 0; i < input.Vertecies.Count; i++)
            {
                Ants[i].VisitedVetecies.Add(input.Vertecies[i]);
            }
        }

        protected void Travel(IAnt ant, IGraph input)
        {
            while (true)
            {
                var allPossibleVertecies = input.GetSibilings(ant.VisitedVetecies.Last());
                foreach (var currentNode in ant.VisitedVetecies)
                {
                    if (allPossibleVertecies.ContainsKey(currentNode))
                    {
                        allPossibleVertecies.Remove(currentNode);
                    }
                }
                var probes = allPossibleVertecies.Keys.ToDictionary(vertex => vertex,
                    vertex => Prober.GetProb(allPossibleVertecies[vertex]));
                if (probes.Count == 0)
                {
                    ant.VisitedVetecies.Add(ant.VisitedVetecies.First());
                    return;
                }
                ant.VisitedVetecies.Add(Rules[ant.GetType()].Proccess(probes));
            }
        }

        protected void UpdateMinPath(IGraph graph)
        {
            var isChanged = false;
            foreach (var ant in Ants)
            {
                if (ant.VisitedVetecies.First() == ant.VisitedVetecies.Last() && ant.VisitedVetecies.Count != 1)
                {
                    var path = graph.CalculatePath(ant.VisitedVetecies);
                    if (path < MinPathLength)
                    {
                        MinPathLength = path;
                        Result = ant.VisitedVetecies;
                        CurrentIterationNoChanges = 0;
                        isChanged = true;
                    }
                }
                if (!isChanged)
                {
                    CurrentIterationNoChanges++;
                }
            }
        }
    }
}