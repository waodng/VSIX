using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class ValidatorModel
    {
        public bool Checked { get; set; }

        public string PropertyName { get; set; }

        public string DisplayName { get; set; }

        public bool Nullable { get; set; }

        public int Minimal { get; set; }

        public int Maximum { get; set; }

        public string PropertyType { get; set; }
    }

}
