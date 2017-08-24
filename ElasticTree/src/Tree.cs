using ElasticTree.src.DataClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticTree.src.Composite
{
    public class Tree
    {

        public Component RootNode { get; private set; }
        public List<Component> IncludedNodes { get; private set; }

        public Tree(Component root)
        {
            RootNode = root;
            IncludedNodes = new List<Component> { root };
        }



        public virtual void AddNode(Component parent, Component node)
        {
            /*
             *    Add new child to parent node.
             *    
             *    Update levels for all nested children of <node>, add them
             *    to IncludedNodes, if needed.
             *    Update parent to node
             */
            if ((node == null) || (parent == null))
                throw new NullReferenceException("Can't add null node in tree");

            if (IncludedNodes.IndexOf(parent) == -1)
                throw new InvalidOperationException("Parent node not exist in current tree");

            // add new child no parent node
            parent.AddChild(node);
            node.SetParent(parent);

            // update level of all nested children, add them to IncludeNodes(if not exists)
            Action<Component> UpdataLevelAddToIncludeNodes = (Component a) => 
            {
                a.Level = a.Parent.Level + 1;
                if (IncludedNodes.IndexOf(a) == -1)
                    IncludedNodes.Add(a);
            };

            ForEach(UpdataLevelAddToIncludeNodes, node);
        }

        public virtual void DeleteHard(Component node)
        {
            /*
             *    Delete node , all nested children will be lost
             */
            if (node == null) 
                throw new NullReferenceException("Deleted node can't be null");
            if (IncludedNodes.IndexOf(node) == -1)
                throw new InvalidOperationException("This node not exist in current tree");

            // remove child
            if (node == RootNode)
            {
                RootNode = null;
            }
            else
            {
                node.Parent.RemoveChild(node);
                node.RemoveParent();

                // update IncludedNodes
                Action<Component> UpdataIncludeNodes = (Component a) =>
                {
                    var index = IncludedNodes.IndexOf(a);
                    if (index != -1)
                        IncludedNodes.RemoveAt(index);
                };
                ForEach(UpdataIncludeNodes, node);
            }
        }

        public virtual void DeleteSafe(Component node)
        {
            /*
             *    Delete node , but his children go up for 1 level
             *    Levels of nested child will be updated
             */
            if (node == null)
                throw new NullReferenceException("Deleted node can't be null");
            if (node == RootNode)
                throw new InvalidOperationException("Can't delete root with this operation");
            if (IncludedNodes.IndexOf(node) == -1)
                throw new InvalidOperationException("This node not exist in current tree");


            var nodeParent = node.Parent;
            // remove dependency between node and parent
            nodeParent.RemoveChild(node);
            node.RemoveParent();

            // add all node children to his parent
            foreach (var child in node.Children)
            {
                AddNode(nodeParent, child);
            }
            // remove node from IncludedNodes collection
            IncludedNodes.Remove(node);
        }

        public virtual Component Find(Data data)
        {
            /*
             *   Find node that contain DataContainer - <data>
             */

            // <nodes> - collection with all nodes that contain <data>
            // start value roots children
            if (data == null)
                throw new ArgumentNullException("Finding node can't contain null data");

            var nodes = new List<Component>();
            nodes.Add(RootNode);
            
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].DataContainer.Equals(data))
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

        public void ForEach(Action<Component> action)
        {
            /*
             *    Iterate over all tree
             */
            ForEach(action, RootNode);
        }

        public void ForEach(Action<Component> action, Component start)
        {
            /*
             *    Iterate over all nodes(branches) that has started from <start> node
             */
            if (start == null)
                throw new ArgumentNullException("ForEach can't iterate from null node");
            var nodes = new List<Component>();
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

        public virtual void ReplaceChild(Component parent, Component oldChild, Component newChild)
        {
            /*
             *   Replace child <oldChild> in parent <parent> with new child <newChild>
             *   Level, IncludedNodes will be update
             *   Children from <oldChild> node will be lost
             *   oldChild parent - null
             */
            if ((parent == null) || (oldChild == null) || (newChild == null))
                throw new ArgumentNullException("Can't replace null node");
            if (!parent.HasChild(oldChild))
            {
                throw new InvalidOperationException("Can't replace, parent has not such child");
            }
            
            // change dependencies
            if(newChild.Parent != null)
                newChild.Parent.RemoveChild(newChild);
            newChild.SetParent(parent);
            parent.ReplaceChild(oldChild, newChild);
            oldChild.RemoveParent();


            // delete all nested children(from oldNode) in IncludedNodes
            Action<Component> DeleteOldNestedChild = (Component a) =>
            {
                var index = IncludedNodes.IndexOf(a);
                if (index != -1)
                    IncludedNodes.RemoveAt(index);
            };
            ForEach(DeleteOldNestedChild, oldChild);


            // update levels in nested nodes in newNode
            // update IncludedNodes with new element from newNode
            Action<Component> Update = (Component a) =>
            {
                a.Level = a.Parent.Level + 1;
                var index = IncludedNodes.IndexOf(a);
                if (index == -1)
                    IncludedNodes.Add(a);
            };
            ForEach(Update, newChild);
        }

        public void ReplaceNode(Component oldNode, Component newNode)
        {
            /*
             *    Replace old node with new node(parent of <oldNode> after that has no child <oldNode>
             *    and got <newNode>), all child of <oldNode> now child of newNode
             *    newNode can't has children
             */
            if ((oldNode == null) || (newNode == null))
                throw new ArgumentNullException("RaplaceNode can't work with null arguments");
            if (oldNode == RootNode)
            {
                throw new InvalidOperationException("Can't replace root with this operation");
            }
            if (newNode.Children.Count > 0)
                throw new InvalidOperationException("In replacing newNode can't has children");

            if (IncludedNodes.IndexOf(oldNode) == -1)
                throw new InvalidOperationException("Can't replace node, cuz oldNode not exist in current tree");


            // change dependency in parent
            oldNode.Parent.ReplaceChild(oldNode, newNode);

            // updata newNode
            newNode.SetParent(oldNode.Parent);
            newNode.Level = newNode.Parent.Level + 1;
            foreach (var child in oldNode.Children)
            {
                newNode.AddChild(child);
                child.SetParent(newNode);
            }

            // kill dependency in oldNode
            oldNode.RemoveParent();
            oldNode.Children.Clear();

            // update IncludedNodes
            IncludedNodes[IncludedNodes.IndexOf(oldNode)] = newNode;
        }

        public void ReplaceRoot(Component newRoot)
        {
            /*
             *    Replace root with new node.
             *    New root get all children from old root
             *    
             *    (!)New node might has no children
             */
            if (newRoot == null)
                throw new ArgumentNullException("RaplaceNode can't work with null arguments");
            if (newRoot.Children.Count > 0)
                throw new InvalidOperationException("In replacing newNode can't has children");

            // update new root
            newRoot.Parent = null;
            newRoot.Level = 0;

            // add children to newRoot
            foreach (var child in RootNode.Children)
            {
                newRoot.AddChild(child);
                child.SetParent(newRoot);
            }

            // kill oldRoot dependency
            RootNode.Children.Clear();

            // update InculedNodes
            IncludedNodes[IncludedNodes.IndexOf(RootNode)] = newRoot;


            RootNode = newRoot;

        }

        public IEnumerable<Component> Siblings(Component node)
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

        // TODO hash table
        // TODO swap branch
        // TODO swap node
        // TODO merge branch( add in replace node)

        public override string ToString()
        {
            return RootNode.ToString();
        }

        public void UpwardForEach(Action<Component> action, Component start)
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
