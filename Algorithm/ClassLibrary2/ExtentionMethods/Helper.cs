using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities.DatabaseModels;
using WebGrease.Css.Extensions;

namespace Aop.ExtentionMethods
{
    public static class Helper
    {
        public static IList<List<T>> SplitIntoLists<T>(this IList<T> list, int n)
        {
            return list.Select((x, i) => new {Index = i, Value = x})
                .GroupBy(x => x.Index/n)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        public static IList<T> ToList<T>(this T[,] t, int n)
        {
            IList<T> lineInts = new List<T>();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    var elem = t[i, j];
                    lineInts.Add(elem);
                }
            }
            return lineInts;
        }

        public static string JoinStrings(this IList<ResultInfo> list, string s)
        {
            StringBuilder builder = new StringBuilder();
            list.ForEach(i => builder.AppendFormat("[{0}]{1}", i.Result, s));
            return builder.ToString();
        }
    }
}