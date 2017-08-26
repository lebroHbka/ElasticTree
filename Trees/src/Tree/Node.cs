using Trees.src.Tree;
using System;
using System.Collections.Generic;
using System.Text;

namespace Trees.src.Tree

{
    public class Node<T> 
    {
        public T Value { get; }
        public Node<T> Parent { get; set; }
        public List<Node<T>> Children { get; set; } = new List<Node<T>>();
        public int Level { get; set; }


        public Node(T data)
        {
            Value = data;
        }



        public override bool Equals(object obj)
        {
            if (!(obj is Node<T>))
                return false;
            var data = (obj as Node<T>).Value;
            return Value.Equals(data);
        }

        public override string ToString()
        {
            return new string('-', Level * 2) + Value.ToString() + "\n";
        }
    }
}
