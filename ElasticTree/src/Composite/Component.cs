using ElasticTree.src.DataClass;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ElasticTree.src.Composite
{
    public abstract class Component
    {
        public virtual List<Component> Children { get; set; } = new List<Component>();
        public virtual Data DataContainer { get; }
        public virtual int Level { get; set; }
        public virtual Component Parent { get; set; }

        public Component() { }

        public Component(Data data)
        {
            DataContainer = data;
        }
        
        // Child methods
        public abstract void AddChild(Component node);

        public abstract bool HasChild(Component node);

        public abstract void ReplaceChild(Component oldChild, Component newChild);

        public abstract void RemoveChild(Component node);


        // Parent methods
        public virtual void SetParent(Component newParent)
        {
            // change parent for newParent
            if (newParent == null)
                throw new ArgumentNullException("Replaced parent can't be null");
            Parent = newParent;
        }

        public virtual void RemoveParent()
        {
            // this node now has no parent
            // but previos parent has child <node>
            
            Parent = null;
        }


        // Data container methods

        public override bool Equals(object obj)
        {
            if (!(obj is Component))
                return false;
            var data = (obj as Component).DataContainer;
            return DataContainer.Equals(data);
        }

        public virtual void SetData(Data data)
        {
            DataContainer.SetData(data);
        }



        public override string ToString()
        {
            var str = new StringBuilder();

            str.Append(new string('-', Level * 2) + DataContainer.ToString() + "\n");

            foreach (var c in Children)
            {
                if (c != null)
                    str.Append(c.ToString());
            }
            return str.ToString();
        } 
    }
}
