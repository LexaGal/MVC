using System;
using System.Collections.Generic;
using ClassLibrary1.DatabaseModels;

namespace ClassLibrary3.Converting
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
                {typeof(RegisterModel).Name, o => ((RegisterModel) o).s},
                {typeof(LoginModel).Name, o => ((LoginModel) o).Data},
                {typeof(User).Name, o => ((User) o).StringView},
                {typeof(List<ResultInfo>).FullName,
                    o => string.Format("[{0}]", ((List<ResultInfo>) o).JoinStrings(", ", info => info))}
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