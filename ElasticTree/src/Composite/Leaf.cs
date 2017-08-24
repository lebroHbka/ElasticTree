using ElasticTree.src.DataClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticTree.src.Composite
{
    public class Leaf : Component
    {
        public Leaf(Data data)
            :base(data)
        {
        }

        public override void AddChild(Component node)
        {
            throw new InvalidOperationException("Can't add child to leaf");
        }

        public override void ReplaceChild(Component oldChild, Component newChild)
        {
            throw new InvalidOperationException("Leaf has no child");
        }

        public override bool HasChild(Component child)
        {
            return false;
        }

        public override void RemoveChild(Component node)
        {
            throw new InvalidOperationException("Leaf has no child");
        }
    }
}
