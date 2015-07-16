using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Algorithm.Models
{
    public class Matrix
    {
        public List<int> Graph { get; set; }
        public int N { get; set; }
        public int M { get; set; }

        public Matrix()
        {}

        public Matrix(int n, int m)
        {
            N = n;
            M = m;
            Graph = new List<int>();
        }
    }
}