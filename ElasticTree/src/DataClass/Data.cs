using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticTree.src.DataClass
{
    public abstract class Data
    {
        // Equal two instance or not
        public abstract bool Equals(Data other);

        // Replace current data with other
        public abstract void SetData(Data other);
    }
}
