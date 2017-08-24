using ElasticTree.src.DataClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticTree.src.Composite

{
    public class Node : Component
    {
        public Node() { }

        public Node(Data data)
            :base(data)
        {
        }


        public override void AddChild(Component node)
        {
            /* 
             *    Add new node in children collection
             */
            if (node == null)
                throw new ArgumentNullException("Added child can't be null");
            Children.Add(node);
        } 

        public override bool HasChild(Component child)
        {
            /*
             *    Check has current node in children collection <child>, or no
             */
            foreach (var c in this.Children)
            {
                // compare links! be aware
                if (c == child)
                    return true;
            }
            return false;
        }

        public override void ReplaceChild(Component oldChild, Component newChild)
        {
            /*
             *   Replace old child with new child, if exist
             *   oldChild has same parent
             *   newChild after replace has old parent
             */
            if ((oldChild == null) || (newChild == null))
                throw new ArgumentNullException("Can't replace node that is null");
            for (int i = 0; i < Children.Count; i++)
            {
                // compare links! be aware
                if (Children[i] == oldChild)
                {
                    Children[i] = newChild;
                    break;
                }
            }
        }

        public override void RemoveChild(Component node)
        {
            // parent has no child -<node> any more
            // but <node> still has parent <this>
            if (node == null)
                throw new ArgumentNullException("Removed child can't be null");
            int i = 0;
            for (; i < Children.Count; i++)
            {
                // compare links! be aware
                if (Children[i] == node)
                {
                    break;
                }
            }
            if (i != Children.Count)
                Children.RemoveAt(i);
        }
    }
}
