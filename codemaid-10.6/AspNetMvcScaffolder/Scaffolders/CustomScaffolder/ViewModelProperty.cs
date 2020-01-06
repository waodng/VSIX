using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetMvcScaffolder
{
    [Serializable]
    public class ViewModelProperty
    {
        private bool _checked=true;
        public bool Checked {
            get{
        return this._checked;
        } set{
        this._checked=value;
        }}

        public string EntityPropertyName { get; set; }

        public string ViewModelPropertyName { get; set; }

        public string ViewModelDisplayName { get; set; }

        private bool _nullable=true;
         public bool Nullable {
            get{
                return this._nullable;
        } set{
            this._nullable = value;
        }}

        public int? Minimal { get; set; }

        public int? Maximum { get; set; }

        public string PropertyType { get; set; }
    }
}
