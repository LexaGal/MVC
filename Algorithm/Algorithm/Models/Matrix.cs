using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Algorithm.Models
{
    public class Matrix
    {
        public List<int> DistGraph { get; set; }
        public List<int> FlowGraph { get; set; }
        public int N { get; set; }

        public Matrix()
        {}

        public Matrix(int n)
        {
            N = n;
            DistGraph = new List<int>();
            FlowGraph = new List<int>();
        }
    }
}