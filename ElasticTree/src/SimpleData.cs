using ElasticTree.src.Composite;
using ElasticTree.src.DataClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticTree.src
{
    public class SimpleData : Data
    {
        public List<int> DataCollection { get; protected set; }

        public SimpleData(List<int> data)
        {
            DataCollection = data;
        }



        public override bool Equals(Data other)
        {
            if (!(other is SimpleData))
                return false;

            var instance = other as SimpleData;
            if (instance.DataCollection.Count != DataCollection.Count)
                return false;
            foreach(var e in instance.DataCollection)
            {
                if (DataCollection.IndexOf(e) == -1)
                    return false;
            }
            return true;
        }

        public override void SetData(Data data)
        {
            if (!(data is SimpleData))
                throw new InvalidOperationException("Can't set this data");
            DataCollection = (data as SimpleData).DataCollection;
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
