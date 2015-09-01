using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AntsAlg.AntsAlgorithm.Graph;
using Path = AntsAlg.AntsAlgorithm.Algorithm.Path;

namespace AntsAlg.QapAlg
{
    public class QapGraph : Graph
    {
        public double[,] Flows { get; protected set; }

        public QapGraph()
        {}

        public QapGraph(double[,] array, double[,] flows)
            : base(array)
        {
            Flows = flows;
        }

        public void SetGraphMatrices(int nAnts, int[] dists, int[] flows)
        {
            Edges = new IEdge[nAnts, nAnts];
            
            Vertecies = new List<IVertex>();
            for (var i = 0; i < nAnts; i++)
            {
                Vertecies.Add(new Vertex(i));
            }
            
            for (int i = 0; i < dists.Length; i++)
            {
                var elem = dists[i];
                Edges[i/nAnts, i%nAnts] = new Edge(new Mark(new Path(elem)));
                EdgesList.Add(Edges[i/nAnts, i%nAnts]);
            }

            for (int i = 0; i < flows.Length; i++)
            {
                var elem = flows[i];
                Flows[i/nAnts, i%nAnts] = elem;
            }
        }

        public QapGraph Load(Stream stream, int nAnts)
        {
            Edges = new IEdge[nAnts, nAnts];
            Flows = new double[nAnts, nAnts];

            using (TextReader textReader = new StreamReader(stream))
            {
                textReader.ReadLine();
                string s = textReader.ReadLine();
                int i = 0;
                while (!String.IsNullOrEmpty(s))
                {
                    string[] strings = Regex.Split(s, @"[ \t]+");

                    for (int j = 0; j < strings.Length; j++)
                    {
                        Edges[i, j] = new Edge(new Mark(new Path(int.Parse(strings[j]))));
                    }
                    i++;
                    s = textReader.ReadLine();

                    if (s != null && s.Contains("Flows Matrix:"))
                    {
                        s = textReader.ReadLine();
                        i = 0;
                        while (!String.IsNullOrEmpty(s))
                        {
                            strings = Regex.Split(s, @"[ \t]+");

                            for (int j = 0; j < strings.Length; j++)
                            {
                                Flows[i, j] = int.Parse(strings[j]);
                            }
                            i++;
                            s = textReader.ReadLine();
                        }
                    }
                }
            }
            Vertecies = new List<IVertex>();
            for (var i = 0; i < nAnts; i++)
            {
                Vertecies.Add(new Vertex(i));
            }
            return this;
        }
    }
}
