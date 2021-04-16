using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Routindo.Plugins.SysPerf.UI.Models
{
    public class DriveModel
    {
        public DriveModel(string name, string label)
        {
            Name = name;
            Label = label;
        }
        public string Name { get; }
        public string Label { get; }

        public override string ToString()
        {
            return $"{Name} - ({Label})";
        }

        public override bool Equals(object obj)
        {
            var model = obj as DriveModel;
            if (model == null)
                return false;

            return String.Equals(model.Name, this.Name, StringComparison.CurrentCultureIgnoreCase);
        }

        protected bool Equals(DriveModel other)
        {
            return Name == other.Name && Label == other.Label;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Label);
        }
    }
}
