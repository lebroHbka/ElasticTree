using Trees.src.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trees.src.Tree
{
    public class Tree<T>
    {
        /*
         *      Tree that contain some datas and dependency between them.
         *      
         *      Parent can contain many children
         */
        public Node<T> RootNode { get; private set; }

        public Tree(Node<T> root)
        {
            RootNode = root;
        }



        public void AddNode(Node<T> parent, Node<T> node)
        {
            /*
             *    Add new child to parent node.
             *    
             *    Update levels for all nested children of <node>
             *    Update parent to node
             */
            if ((node == null) || (parent == null))
                throw new NullReferenceException("Can't add null node in tree");

            // add new child no parent node
            parent.Children.Add(node);
            node.Parent = parent;

            // update level of all nested children
            Action<Node<T>> UpdataLevelAddToIncludeNodes = (Node<T> a) => 
            {
                a.Level = a.Parent.Level + 1;
            };
            ForEach(UpdataLevelAddToIncludeNodes, node);
        }

        public void DeleteHard(Node<T> node)
        {
            /*
             *    Delete node , all nested children will be lost
             */
            if (node == null) 
                throw new NullReferenceException("Deleted node can't be null");

            // remove root
            if (node == RootNode)
            {
                RootNode = null;
            }
            else
            {
                node.Parent.Children.Remove(node);
                node.Parent = null;
            }
        }

        public void DeleteSafe(Node<T> node)
        {
            /*
             *    Delete node , but his children go up for 1 level
             *    Levels of nested child will be updated
             */
            if (node == null)
                throw new NullReferenceException("Deleted node can't be null");
            if (node == RootNode)
                throw new InvalidOperationException("Can't delete root with this operation");


            var parent = node.Parent;
            // remove dependency between node and parent
            parent.Children.Remove(node);
            node.Parent = null;

            // add all node children to his parent
            foreach (var child in node.Children)
            {
                AddNode(parent, child);
            }
        }

        public Node<T> Find(T data)
        {
            /*
             *   Find node that contain DataContainer - <data>
             */

            // <nodes> - collection with all nodes that contain <data>
            // start value roots children
            if (data == null)
                throw new ArgumentNullException("Finding node can't contain null data");

            var nodes = new List<Node<T>>();
            nodes.Add(RootNode);
            
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].Value.Equals(data))
                {
                    return nodes[i];
                }
                foreach (var child in nodes[i].Children)
                {
                    if (child != null)
                        nodes.Add(child);
                }
            }

            // if condition in cicle didn't work -> no such node -> return null
            return null;
        }  

        public void ForEach(Action<Node<T>> action)
        {
            /*
             *    Iterate over all tree
             */
            ForEach(action, RootNode);
        }

        public void ForEach(Action<Node<T>> action, Node<T> start)
        {
            /*
             *    Iterate over all nodes(branches) that has started from <start> node
             */
            if (start == null)
                throw new ArgumentNullException("ForEach can't iterate from null node");
            var nodes = new List<Node<T>>();
            action(start);
            foreach (var c in start.Children)
            {
                nodes.Add(c);
            }
            for (int i = 0; i < nodes.Count; i++)
            {
                action(nodes[i]);

                foreach (var child in nodes[i].Children)
                {
                    if (child != null)
                        nodes.Add(child);
                }
            }
        }

        public bool isEmpty()
        {
            /*
             *    Check has tree elements or no
             */
            return RootNode == null;
        }

        public void Replace(Node<T> old, Node<T> @new)
        {
            /*
             *   Replace <old> node with <new> node
             *   
             *   All <old> node branch is gone
             *   
             *   Level will be update
             */

            if ((old == null) || (@new == null))
                throw new ArgumentNullException("Can't replace null node");

            if (old.Parent == null)
            {
                RootNode = @new;
                @new.Parent = null;
            }
            else
            {
                // change dependencies
                if (@new.Parent != null)
                    @new.Parent.Children.Remove(@new);
                @new.Parent = old.Parent;
                old.Parent.Children[old.Parent.Children.IndexOf(old)] = @new;
                old.Parent = null;
            }

            // update levels in nested nodes in newNode
            Action<Node<T>> Update = (Node<T> a) =>
            {
                a.Level = (a.Parent != null) ? a.Parent.Level + 1 : 0;
            };
            ForEach(Update, @new);
        }

        public void ReplaceNode(Node<T> old, Node<T> @new)
        {
            /*
             *    Replace old node with new node, all child of <oldNode> now child of newNode
             *    (!)newNode can't has children
             */
            if ((old == null) || (@new == null))
                throw new ArgumentNullException("RaplaceNode can't work with null arguments");
            
            if (@new.Children.Count > 0)
                throw new InvalidOperationException("In replacing newNode can't has children");

            // delete child in new node parent
            if (@new.Parent != null)
                @new.Parent.Children.Remove(@new);

            if(old.Parent == null)
            {
                RootNode = @new;
                @new.Parent = null;
                @new.Level = 0;
            }
            else
            {
                // change dependency in oldNode parent
                old.Parent.Children[old.Parent.Children.IndexOf(old)] = @new;

                @new.Parent = old.Parent;
                @new.Level = @new.Parent.Level + 1;
            }
            old.Parent = null;
            
            foreach (var child in old.Children)
            {
                @new.Children.Add(child);
                child.Parent = @new;
            }
        }

        public IEnumerable<Node<T>> Siblings(Node<T> node)
        {
            /*
             *    Get all siblings(nodes that has same parent with node) of <node>
             */
            if (node == null)
                throw new ArgumentNullException("Null node has no siblings");

            if (node.Parent != null)
            {
                foreach (var parentChild in node.Parent.Children)
                {
                    if (parentChild != node)
                        yield return parentChild;
                }
            }
            else
                yield break;
        }

        public int Size()
        {
            /*
             *    Return nodes count
             */
            int nodeCount = 0;
            ForEach((Node<T> a) => { nodeCount++; });
            return nodeCount;
        }

        // TODO swap branch
        // TODO swap node
        // TODO merge branch( add in replace node)

        public override string ToString()
        {
            return RootNode.ToString();
        }

        public void UpwardForEach(Action<Node<T>> action, Node<T> start)
        {
            /*
             *    Iterate upside down, start from <start> node, and came up to root
             */
            if (start == null)
                throw new ArgumentNullException("UpwardForEach can't iterate from null node");
            var node = start;

            while (node != null)
            {
                action(node);
                node = node.Parent;
            }
        }

    }
}
