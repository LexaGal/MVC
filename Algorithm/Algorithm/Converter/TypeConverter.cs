using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Algorithm.Authentication;
using Algorithm.DomainModels;
using Microsoft.Practices.ObjectBuilder2;

namespace Algorithm.Converter
{
    public static class TypeConverter
    {
        private static IDictionary<string, Func<object, string>> _typesDictionary;

        public static void Initialize()
        {
            _typesDictionary = new Dictionary<string, Func<object, string>>
            {
                {"Int32", o => ((int) o).ToString()},
                {typeof(Parameters).Name, o => ((Parameters) o).StringView},
                {typeof(DistMatrix).Name, o => ((DistMatrix) o).MatrixView},
                {typeof(FlowMatrix).Name, o => ((FlowMatrix) o).MatrixView},
                {typeof(ResultInfo).Name, o => ((ResultInfo) o).Result},
                {"String", o => o.ToString()},
                {typeof(RegisterModel).Name, o => ((RegisterModel) o).Data},
                {typeof(LoginModel).Name, o => ((LoginModel) o).Data},
                {typeof(User).Name, o => ((User) o).Data},
                {typeof(List<ResultInfo>).FullName,
                    o => string.Format("[{0}]", ((List<ResultInfo>) o).JoinStrings(", ", info => info.Result))}
            };
        }

        public static string GetStringView(string type, Object obj)
        {
            if (_typesDictionary != null && obj != null)
            {
                return _typesDictionary[type].Invoke(obj);
            }
            return null;
        }
    }
}