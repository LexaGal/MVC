using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Entities.DatabaseModels
{
    public class FlowMatrix
    {
        public FlowMatrix() {}

        public FlowMatrix(string text)
        {
            string[] strings = Regex.Split(text, @",\n");
            string num = Regex.Match(strings.First(), @"\d+").Value;
            N = Convert.ToInt32(num);
            Matrix = strings[1];
        }

        [Key]
        public int Id { get; set; }
        public string Matrix { get; set; }
        public int N { get; set; }

        public string MatrixView
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(String.Format("Flows Matrix: {0},\n", N));
                builder.Append(Matrix);
                return builder.ToString();
            }   
        }
    }
}