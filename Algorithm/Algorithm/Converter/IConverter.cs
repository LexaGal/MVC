using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithm.Converter
{
    interface IConverter
    {
        string GetStringView(string type, Object obj);
    }
}
