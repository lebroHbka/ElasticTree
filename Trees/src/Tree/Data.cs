using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees.src.Tree
{
    public class Data 
    {
        public List<int> DataCollection { get; set; }

        public Data(List<int> data)
        {
            DataCollection = data;
        }


        public bool Equals(Data other)
        {
            if (!(other is Data))
                return false;

            var instance = other as Data;
            if (instance.DataCollection.Count != DataCollection.Count)
                return false;
            foreach (var e in instance.DataCollection)
            {
                if (DataCollection.IndexOf(e) == -1)
                    return false;
            }
            return true;
        }

        
        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (var i in DataCollection)
            {
                str.Append(i + " ");
            }
            return str.ToString();
        }
    }
}
