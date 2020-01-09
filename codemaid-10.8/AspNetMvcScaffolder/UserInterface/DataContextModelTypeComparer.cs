using System;
using System.Collections.Generic;

namespace AspNetMvcScaffolder.UserInterface
{
    internal class DataContextModelTypeComparer : Comparer<ModelType>
    {
        public override int Compare(ModelType x, ModelType y)
        {
            if (x == null && y == null)
            {
                return 0;
            }
            else if (x == null)
            {
                return -1;
            }
            else if (y == null)
            {
                return 1;
            }
            else
            {
                return StringComparer.CurrentCulture.Compare(x.ShortTypeName, y.ShortTypeName);
            }
        }
    }
}
