using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using AntsAlg.AntsAlgorithm.Graph;
using AntsAlg.QapAlg;

namespace AntsAlg.AntsAlgorithm.Algorithm
{
    public class StandartAlgorithmBuilder
    {
        public IAlgorithm<T, IList<IVertex>> GetAlgorithm<T>(double alpha, double beta, int iterations,
            int iterationsWithoutChanges)
        {
            if (typeof (T) == typeof (QapGraph))
            {
                var algorithm = new QapAntAlgorithm((int) alpha, (int) beta, 10, iterationsWithoutChanges, iterations);
                return algorithm as IAlgorithm<T, IList<IVertex>>;
            }
            return null;
        }

        public QapAntAlgorithm GetAlgorithm<T>(Stream stream)
        {
            if (typeof(T) == typeof(QapGraph))
            {
                int phInc = 1;
                int extraPhInc = 2;
                int nAnts = 10;
                int iterations = 100;
                int iterationsWithoutChanges = 50;

                using (TextReader textReader = new StreamReader(stream))
                {
                    textReader.ReadLine();
                    string s = textReader.ReadLine();
                    if (s != null)
                    {
                        phInc = Convert.ToInt32(s);
                    }
                    textReader.ReadLine();
                    s = textReader.ReadLine();
                    if (s != null)
                    {
                        extraPhInc = Convert.ToInt32(s);
                    }
                    textReader.ReadLine();
                    s = textReader.ReadLine();
                    if (s != null)
                    {
                        nAnts = Convert.ToInt32(s);
                    }
                    textReader.ReadLine();
                    s = textReader.ReadLine();
                    if (s != null)
                    {
                        iterationsWithoutChanges = Convert.ToInt32(s);
                    }
                    textReader.ReadLine();
                    s = textReader.ReadLine();
                    if (s != null)
                    {
                        iterations = Convert.ToInt32(s);
                    }
                }
                QapAntAlgorithm algorithm = new QapAntAlgorithm(phInc, extraPhInc, nAnts, iterations, iterationsWithoutChanges);
                return algorithm;
            }
            return null;
        }

        public QapAntAlgorithm GetAlgorithm<T>(string path)
        {
            if (typeof(T) == typeof(QapGraph))
            {
                int phInc = 1;
                int extraPhInc = 2;
                int nAnts = 10;
                int iterations = 100;
                int iterationsWithoutChanges = 50;

                using (TextReader textReader = new StreamReader(path))
                {
                    textReader.ReadLine();
                    string s = textReader.ReadLine();
                    if (s != null)
                    {
                        phInc = Convert.ToInt32(s);
                    }
                    textReader.ReadLine();
                    s = textReader.ReadLine();
                    if (s != null)
                    {
                        extraPhInc = Convert.ToInt32(s);
                    }
                    textReader.ReadLine();
                    s = textReader.ReadLine();
                    if (s != null)
                    {
                        nAnts = Convert.ToInt32(s);
                    }
                    textReader.ReadLine();
                    s = textReader.ReadLine();
                    if (s != null)
                    {
                        iterationsWithoutChanges = Convert.ToInt32(s);
                    }
                    textReader.ReadLine();
                    s = textReader.ReadLine();
                    if (s != null)
                    {
                        iterations = Convert.ToInt32(s);
                    }
                }
                QapAntAlgorithm algorithm = new QapAntAlgorithm(phInc, extraPhInc, nAnts, iterations, iterationsWithoutChanges);
                return algorithm;
            }
            return null;
        }
    }
}
