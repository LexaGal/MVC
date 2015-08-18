using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Algorithm.DomainModels;

namespace Algorithm.Converter
{
    public class TypeConverter : IConverter
    {
        private readonly IDictionary<string, Func<object, string>> _typesDictionary;

        public TypeConverter()
        {
            _typesDictionary = new Dictionary<string, Func<object, string>>
            {
                {"Int32", o => ((int) o).ToString()},
                {"Parameters", o => ((Parameters) o).StringView},
                {"DistMatrix", o => ((DistMatrix) o).MatrixView},
                {"FlowMatrix", o => ((FlowMatrix) o).MatrixView},
                {"ResultInfo", o => ((ResultInfo) o).Result},
                {"String", o => o.ToString()}
            };
        }

        public string GetStringView(string type, Object obj)
        {
            if (_typesDictionary != null)
            {
                return _typesDictionary[type].Invoke(obj);
            }
            return null;
        }
    }
}