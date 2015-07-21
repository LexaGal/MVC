using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Algorithm.Models
{
    public class Matrix
    {
        public int[] DistGraph { get; set; }
        public int[] FlowGraph { get; set; }
        public int N { get; set; }

        public Matrix()
        {}

        public Matrix(int n)
        {
            N = n;
            DistGraph = new int[n];
            FlowGraph = new int[n];
        }
    }
}