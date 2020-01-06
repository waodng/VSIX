using EnvDTE;
using EnvDTE80;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetMvcScaffolder
{
    public static class CustomCodeTypeExtensions
    {
        public static bool IsInterfaceType(this CodeType codeType)
        {
            return codeType is CodeInterface2;
        }
    }
}
