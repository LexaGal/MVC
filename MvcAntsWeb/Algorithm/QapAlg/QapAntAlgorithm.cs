using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using AntsAlg.AntsAlgorithm.Graph;

namespace AntsAlg.QapAlg
{
    public class QapAntAlgorithm : IQapAntAlgorithm
    {
        private StringBuilder _resultBuilder;
        private QapAnt _currentAnt;
        private int _currentIteration;   
        private int _currentIterationNoChanges;
        private int _dislocationCoeff;
        private int _flag;

        public int PheromoneInc { get; set; }
        public int ExtraPheromoneInc { get; set; }
        public int MaxIterationsNoChanges { get; private set; }
        public int NAnts { get; private set; }
        public int MaxIterations { get; private set; }
        public QapGraph Graph { get; set; }        public IList<QapAnt> Ants { get; set; }
        public string Result { get; private set; }
        public QapAnt BestAnt { get; private set; }

        public QapAntAlgorithm(int pheromoneInc, int extraPheromoneInc, int nAnts, int maxIterations, int maxIterationsNoChanges)
        {
            NAnts = nAnts;
            PheromoneInc = pheromoneInc;
            ExtraPheromoneInc = extraPheromoneInc;
            MaxIterations = maxIterations;
            MaxIterationsNoChanges = maxIterationsNoChanges;
            BestAnt = new QapAnt {PathCost = int.MaxValue, VisitedVetecies = new List<IVertex>()};
        }

        public static object DeepObjectClone(object obj)
        {
            object objResult;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter  = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, obj);

                memoryStream.Position = 0;
                objResult = binaryFormatter.Deserialize(memoryStream);
            }
            return objResult;
        }

        //==================== QAP Path Generating ================

        public double GetDislocationCoefficient()
        {
            int x10 = 12345;
            int x11 = 67890;
            int x20 = 24680;
            int x22 = 43210;
            const int m = 2147483647;
            const int m2 = 2145483479;
            const int a12 = 63308;
            const int q12 = 33921;
            const int r12 = 12979;
            const int a13 = -183326;
            const int q13 = 11714;
            const int r13 = 2883;
            const int a21 = 86098;
            const int q21 = 24919;
            const int r21 = 7417;
            const int a23 = -539608;
            const int q23 = 3976;
            const int r23 = 2071;
            const double coeff = 4.656612873077393e-10;

            int h = x10/q13;
            int p13 = (-1)*a13*(x10 - h*q13) - h*r13;
            h = x11/q12;
            int p12 = a12*(x11 - h*q12) - h*r12;

            if (p13 < 0)
            {
                p13 = p13 + m;
            }

            if (p12 < 0)
            {
                p12 = p12 + m;
            }

            var x12 = p12 - p13;

            if (x12 < 0)
            {
                x12 = x12 + m;
            }

            h = x20/q23;
            int p23 = (-1)*a23*(x20 - h*q23) - h*r23;
            h = x22/q21;
            int p21 = a21*(x22 - h*q21) - h*r21;

            if (p23 < 0)
            {
                p23 = p23 + m2;
            }
            if (p21 < 0)
            {
                p21 = p21 + m2;
            }

            x22 = p21 - p23;

            if (x22 < 0)
            {
                x22 = x22 + m2;
            }
            if (x12 < x22)
            {
                h = x12 - x22 + m;
            }
            else h = x12 - x22;
            
            return h*coeff;
        }

        public int GetPermutationalIndex(int low, int high)
        {
            int index = low + (int) ((high - low + 1)*GetDislocationCoefficient());
            return index;
        }

        public void GeneratePath(int[] path)
        {
            for (int i = 0; i < Ants.Count; i++)
            {
                path[i] = i;
            }

            for (int i = 0; i < Ants.Count - 1; i++)
            {
                int x = GetPermutationalIndex(i, Ants.Count - 1);
                int y = path[i];
                path[i] = path[x];
                path[x] = y;
            }
        }
        
        //=========================================================

        public void CreateAnts()
        {
            Ants = new List<QapAnt>();
            for (int i = 0; i < NAnts; i++)
            {
                IVertex t = Graph.Vertecies[i];
                Ants.Add(new QapAnt {VisitedVetecies = new List<IVertex>()});
                Ants.Last().VisitedVetecies.Add(t);
            }
        }

        public IList<IVertex> AntsTravel()
        {
            IList<IVertex> path = new List<IVertex>();
            for (int i = 0; i < Ants.Count; i++)
            {
                path.Add(new Vertex(i));
            }

            int[] nextI = new int[Ants.Count];
            int[] nextJ = new int[Ants.Count];
            int[] sumTrace = new int[Ants.Count];

            GeneratePath(nextI);
            GeneratePath(nextJ);

            for (int i = 0; i < Ants.Count; i++)
            {
                for (int j = 0; j < Ants.Count; j++)
                {
                    sumTrace[i] += (int)Graph.Edges[i, j].Mark.Pheromone.Value;
                }
            }

            for (int i = 0; i < Ants.Count; i++)
            {
                _dislocationCoeff = i;

                path[nextI[i]] = GetNextNode(new Tuple<int, int[], int[], int[]>(i, nextI, nextJ, sumTrace));

                for (int k = i; k < Ants.Count; k++)
                {
                    sumTrace[nextI[k]] -= (int)Graph.Edges[nextI[k], nextJ[_dislocationCoeff]].Mark.Pheromone.Value;
                }

                int y = nextJ[_dislocationCoeff];
                nextJ[_dislocationCoeff] = nextJ[i];
                nextJ[i] = y;
            }
            return path;
        }

        public IVertex GetNextNode(Tuple<int, int[], int[], int[]> tuple)
        {
            int index = 0;

            if (tuple != null)
            {
                int i = tuple.Item1;
                int[] nextI = tuple.Item2;
                int[] nextJ = tuple.Item3;
                int[] sumTrace = tuple.Item4;

                if (sumTrace != null && nextI != null && nextJ != null)
                {
                    int target = GetPermutationalIndex(0, sumTrace[nextI[i]] - 1);

                    double pheromone = Graph.Edges[nextI[i], nextJ[_dislocationCoeff]].Mark.Pheromone.Value;

                    while (pheromone < target)
                    {
                        _dislocationCoeff++;
                        pheromone += Graph.Edges[nextI[i], nextJ[_dislocationCoeff]].Mark.Pheromone.Value;
                    }
                    index = nextJ[_dislocationCoeff];
                }
            }
            return new Vertex(index);
        }

        public void LocalSearch(QapAnt ant)
        {
            // set of moves, numbered from 0 to index
            int[] move = new int[Ants.Count * (Ants.Count - 1) / 2];
            int nMoves = 0;

            for (int i = 0; i < Ants.Count - 1; i++)
            {
                for (int j = i + 1; j < Ants.Count; j++)
                {
                    move[nMoves++] = Ants.Count * i + j;
                }
            }

            bool isImproved = true;

            for (int scan = 0; scan < 2 && isImproved; scan++)
            {
                isImproved = false;

                for (int i = 0; i < nMoves - 1; i++)
                {
                    int x = GetPermutationalIndex(i + 1, nMoves - 1);
                    int y = move[i];
                    move[i] = move[x];
                    move[x] = y;
                }

                for (int i = 0; i < nMoves; i++)
                {
                    int r = move[i] / Ants.Count;
                    int s = move[i] % Ants.Count;
                    int moveCost = ComputeMoveCost(r, s, ant.VisitedVetecies);

                    if (moveCost < 0)
                    {
                        ant.PathCost += moveCost;

                        int y = ant.VisitedVetecies[r].Number;
                        ant.VisitedVetecies[r].Number = ant.VisitedVetecies[s].Number;
                        ant.VisitedVetecies[s].Number = y;

                        isImproved = true;
                    }
                }
            }
        }

        public int ComputeMoveCost(int r, int s, IList<IVertex> path)
        {
            int d = (int)((Graph.Edges[r, r].Mark.Path.Value
                           -
                           Graph.Edges[s, s].Mark.Path.Value)
                          *
                          (Graph.Flows[path[s].Number, path[s].Number]
                           -
                           Graph.Flows[path[r].Number, path[r].Number])
                          +
                          (Graph.Edges[r, s].Mark.Path.Value
                           -
                           Graph.Edges[s, r].Mark.Path.Value)
                          *
                          (Graph.Flows[path[s].Number, path[r].Number]
                           -
                           Graph.Flows[path[r].Number, path[s].Number]));

            for (int k = 0; k < Ants.Count; k++)
            {
                if (k != r && k != s)
                {
                    d += (int)((Graph.Edges[k, r].Mark.Path.Value
                                -
                                Graph.Edges[k, s].Mark.Path.Value)
                               *
                               (Graph.Flows[path[k].Number, path[s].Number]
                                -
                                Graph.Flows[path[k].Number, path[r].Number])
                               +
                               (Graph.Edges[r, k].Mark.Path.Value
                                -
                                Graph.Edges[s, k].Mark.Path.Value)
                               *
                               (Graph.Flows[path[s].Number, path[k].Number]
                                -
                                Graph.Flows[path[r].Number, path[k].Number]));

                }
            }
            return d;
        }     

        public int ComputePathCost(QapAnt ant)
        {
            int cost = 0;

            for (int i = 0; i < ant.VisitedVetecies.Count; i++)
            {
                for (int j = 0; j < ant.VisitedVetecies.Count; j++)
                {
                    cost += (int) (Graph.Edges[i, j].Mark.Path.Value *
                                   Graph.Flows[ant.VisitedVetecies[i].Number, ant.VisitedVetecies[j].Number]);
                }
            }
            ant.PathCost = cost;
            return cost;
        }

        public bool IsNewBestPath(QapAnt ant)
        {
            int bestOne = BestAnt.PathCost;
            int newOne = ant.PathCost;

            if (newOne < bestOne)
            {
                _flag = 2;
                BestAnt.VisitedVetecies = (IList<IVertex>)DeepObjectClone(ant.VisitedVetecies);
                BestAnt.PathCost = ant.PathCost;

                NewResultUpdate();
                
                return true;
            }
            if (newOne == bestOne)
            {
                _flag = 1;
            }
            else
            {
                _flag = 0;
            }
            return false;
        }

        public void UpdateGeneration()
        {
            if (_flag == 2)
            {
                PheromoneInc = 1;
                for (int i = 0; i < Ants.Count; i++)
                {
                    for (int j = i + 1; j < Ants.Count; j++)
                    {
                        Graph.Edges[i, j].Mark.Pheromone.Value = PheromoneInc;
                    }
                }
            }
            if (_flag == 1)
            {
                PheromoneInc++;
                for (int i = 0; i < Ants.Count; i++)
                {
                    for (int j = i + 1; j < Ants.Count; j++)
                    {
                        Graph.Edges[i, j].Mark.Pheromone.Value = PheromoneInc;
                    }
                }
            }
            if (_flag == 0)
            {
                for (int i = 1; i < Ants.Count; i++)
                {
                    Graph.Edges[i, _currentAnt.VisitedVetecies[i].Number].Mark.Pheromone.Value += PheromoneInc;
                    Graph.Edges[i, BestAnt.VisitedVetecies[i].Number].Mark.Pheromone.Value += PheromoneInc;
                }
            }
        }
      
        public bool IsFinished()
        {
            if (_currentIteration == MaxIterations || _currentIterationNoChanges == MaxIterationsNoChanges)
            {
                _resultBuilder.Append("End");
                Result = _resultBuilder.ToString();
                return true;
            }
            return false;
        }

        public void Run()
        {
            _currentAnt = new QapAnt();
            _currentIteration = 0;
            _currentIterationNoChanges = 0;
            BestAnt = new QapAnt {PathCost = Int32.MaxValue, VisitedVetecies = new List<IVertex>()};
            _resultBuilder = new StringBuilder();
            _resultBuilder.Append("Start\n");
            
            while (!IsFinished())
            {
                CreateAnts();

                IList<IVertex> newPath = AntsTravel();

                _currentAnt = new QapAnt {VisitedVetecies = newPath, PathCost = 0};
               
                ComputePathCost(_currentAnt);

                LocalSearch(_currentAnt);
                
                if (!IsNewBestPath(_currentAnt))
                {
                    _currentIterationNoChanges++;
                }
                else _currentIterationNoChanges = 0;

                UpdateGeneration();
                
                _currentIteration++;
                
                Ants.Clear();
            }
        }

        public void NewResultUpdate()
        {
            StringBuilder result = new StringBuilder();
            result.Append(String.Format("----------------------\nCost: {0}", BestAnt.PathCost));
           
            for (int i = 0; i < BestAnt.VisitedVetecies.Count - 1; i++)
            {
                result.Append(String.Format("\nIn location: {0} - ", i + 1))
                    .Append(String.Format("item: {0}", BestAnt.VisitedVetecies[i].Number + 1));
            }
            result.Append(String.Format("\nIn location: {0} - ", BestAnt.VisitedVetecies.Count))
                .Append(String.Format("item: {0}", BestAnt.VisitedVetecies.Last().Number + 1));

            result.Append(String.Format("\nIteration: {0}\n----------------------", _currentIteration + 1))
                .Append(Environment.NewLine);

            _resultBuilder.Append(result);
        }

        public IList<IVertex> Calculate(QapGraph input)
        {
            Graph = input;
            if (Graph != null)
            {
                Run();
            }
            if (BestAnt != null)
            {
                return BestAnt.VisitedVetecies;
            }
            return null;
        }
    }
}
