using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Algorithm.DomainModels;

namespace Algorithm.Models
{
    public class DatabaseViewModel
    {
        public IEnumerable<Parameters> Parameters { get; set; }
        public IEnumerable<DistMatrix> DistMatrices { get; set; }
        public IEnumerable<FlowMatrix> FlowMatrices { get; set; }
    }
}