﻿// #define USING_HASH_SET
// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==
/*============================================================
**
** Class:  SortedSet
**
** Purpose: A generic sorted set.
**
** Date:  August 15, 2008
** 
===========================================================*/

namespace ME.ECS.Collections {

    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using System.Collections;
    using System.Collections.Generic;
 
    unsafe internal class BitHelper {   // should not be serialized
 
        private const byte MarkedBitFlag = 1;
        private const byte IntSize = 32;
 
        // m_length of underlying int array (not logical bit array)
        private int m_length;
        
        // ptr to stack alloc'd array of ints
        [System.Security.SecurityCritical]
        private int* m_arrayPtr;
 
        // array of ints
        private int[] m_array;
 
        // whether to operate on stack alloc'd or heap alloc'd array 
        private bool useStackAlloc;
 
        /// <summary>
        /// Instantiates a BitHelper with a heap alloc'd array of ints
        /// </summary>
        /// <param name="bitArray">int array to hold bits</param>
        /// <param name="length">length of int array</param>
        // <SecurityKernel Critical="True" Ring="0">
        // <UsesUnsafeCode Name="Field: m_arrayPtr" />
        // <UsesUnsafeCode Name="Parameter bitArrayPtr of type: Int32*" />
        // </SecurityKernel>
        [System.Security.SecurityCritical]
        internal BitHelper(int* bitArrayPtr, int length) {
            this.m_arrayPtr = bitArrayPtr;
            this.m_length = length;
            useStackAlloc = true;
        }
 
        /// <summary>
        /// Instantiates a BitHelper with a heap alloc'd array of ints
        /// </summary>
        /// <param name="bitArray">int array to hold bits</param>
        /// <param name="length">length of int array</param>
        internal BitHelper(int[] bitArray, int length) {
            this.m_array = bitArray;
            this.m_length = length;
        }
 
        /// <summary>
        /// Mark bit at specified position
        /// </summary>
        /// <param name="bitPosition"></param>
        // <SecurityKernel Critical="True" Ring="0">
        // <UsesUnsafeCode Name="Field: m_arrayPtr" />
        // </SecurityKernel>
        [System.Security.SecurityCritical]
        internal unsafe void MarkBit(int bitPosition) {
            if (useStackAlloc) {
                int bitArrayIndex = bitPosition / IntSize;
                if (bitArrayIndex < m_length && bitArrayIndex >= 0) {
                    m_arrayPtr[bitArrayIndex] |= (MarkedBitFlag << (bitPosition % IntSize));
                }
            }
            else {
                int bitArrayIndex = bitPosition / IntSize;
                if (bitArrayIndex < m_length && bitArrayIndex >= 0) {
                    m_array[bitArrayIndex] |= (MarkedBitFlag << (bitPosition % IntSize));
                }
            }
        }
 
        /// <summary>
        /// Is bit at specified position marked?
        /// </summary>
        /// <param name="bitPosition"></param>
        /// <returns></returns>
        // <SecurityKernel Critical="True" Ring="0">
        // <UsesUnsafeCode Name="Field: m_arrayPtr" />
        // </SecurityKernel>
        [System.Security.SecurityCritical]
        internal unsafe bool IsMarked(int bitPosition) {
            if (useStackAlloc) {
                int bitArrayIndex = bitPosition / IntSize;
                if (bitArrayIndex < m_length && bitArrayIndex >= 0) {
                    return ((m_arrayPtr[bitArrayIndex] & (MarkedBitFlag << (bitPosition % IntSize))) != 0);
                }
                return false;
            }
            else {
                int bitArrayIndex = bitPosition / IntSize;
                if (bitArrayIndex < m_length && bitArrayIndex >= 0) {
                    return ((m_array[bitArrayIndex] & (MarkedBitFlag << (bitPosition % IntSize))) != 0);
                }
                return false;
            }
        }
 
        /// <summary>
        /// How many ints must be allocated to represent n bits. Returns (n+31)/32, but 
        /// avoids overflow
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        internal static int ToIntArrayLength(int n) {
            return n > 0 ? ((n - 1) / IntSize + 1) : 0;
        }
 
    }
 
    //
    // A binary search tree is a red-black tree if it satisfies the following red-black properties:
    // 1. Every node is either red or black
    // 2. Every leaf (nil node) is black
    // 3. If a node is red, then both its children are black
    // 4. Every simple path from a node to a descendant leaf contains the same number of black nodes
    // 
    // The basic idea of red-black tree is to represent 2-3-4 trees as standard BSTs but to add one extra bit of information  
    // per node to encode 3-nodes and 4-nodes. 
    // 4-nodes will be represented as:          B
    //                                                              R            R
    // 3 -node will be represented as:           B             or         B     
    //                                                              R          B               B       R
    // 
    // For a detailed description of the algorithm, take a look at "Algorithms" by Robert Sedgewick.
    //
 
    internal delegate bool TreeWalkPredicate<T>(SortedSetCopyable<T>.Node node);
 
    internal enum TreeRotation {
        LeftRotation = 1,
        RightRotation = 2,
        RightLeftRotation = 3,
        LeftRightRotation = 4,
    }
 
    [SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Justification = "by design name choice")]
    [DebuggerDisplay("Count = {Count}")]
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false)]
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
#if !FEATURE_NETCORE
    [Serializable]
    public class SortedSetCopyable<T> : ISet<T>, ICollection<T>, ICollection, ISerializable, IDeserializationCallback, IPoolableRecycle {
#else
    public class SortedSetCopyable<T> : ISet<T>, ICollection<T>, ICollection {
#endif //!FEATURE_NETCORE
        #region local variables/constants
        [ME.ECS.Serializer.SerializeField]
        Node root;
        [ME.ECS.Serializer.SerializeField]
        IComparer<T> comparer;
        [ME.ECS.Serializer.SerializeField]
        int count;
        [ME.ECS.Serializer.SerializeField]
        int version;
        private Object _syncRoot;
 
        private const String ComparerName = "Comparer";
        private const String CountName = "Count";
        private const String ItemsName = "Items";
        private const String VersionName = "Version";
        //needed for enumerator
        private const String TreeName = "Tree";
        private const String NodeValueName = "Item";
        private const String EnumStartName = "EnumStarted";        
        private const String ReverseName = "Reverse";
        private const String EnumVersionName = "EnumVersion";
 
#if !FEATURE_NETCORE
        //needed for TreeSubset
        private const String minName = "Min";
        private const String maxName = "Max";
        private const String lBoundActiveName = "lBoundActive";
        private const String uBoundActiveName = "uBoundActive";
 
        private SerializationInfo siInfo; //A temporary variable which we need during deserialization. 
#endif
        internal const int StackAllocThreshold = 100;
 
        #endregion
 
        
        public void OnRecycle() {

            this.count = default;
            this.version = default;
            this._syncRoot = default;
            this.RecycleNodes(ref this.root);
            
        }
        
        public void CopyFrom(SortedSetCopyable<T> other) {

            this.RecycleNodes(ref root);
            
            // these are explicit type checks in the mould of HashSet. It would have worked better
            // with something like an ISorted<T> (we could make this work for SortedList.Keys etc)
            SortedSetCopyable<T> baseSortedSet = other as SortedSetCopyable<T>;
            //SortedSetCopyable<T> baseTreeSubSet = other as TreeSubSet;
            //if (baseSortedSet != null && baseTreeSubSet == null && AreComparersEqual(this, baseSortedSet)) {
                //breadth first traversal to recreate nodes
                if (baseSortedSet.Count == 0) {
                    count = 0;
                    version = 0;
                    root = null;
                    return;
                }
 
 
                //pre order way to replicate nodes
                Stack<Node> theirStack = PoolStack<SortedSetCopyable<T>.Node>.Spawn(2 * log2(baseSortedSet.Count) + 2);
                Stack<Node> myStack = PoolStack<SortedSetCopyable<T>.Node>.Spawn(2 * log2(baseSortedSet.Count) + 2);
                Node theirCurrent = baseSortedSet.root;
                Node myCurrent = (theirCurrent != null ? new SortedSetCopyable<T>.Node(theirCurrent.Item, theirCurrent.IsRed) : null);
                root = myCurrent;
                while (theirCurrent != null) {
                    theirStack.Push(theirCurrent);
                    myStack.Push(myCurrent);
                    myCurrent.Left = (theirCurrent.Left != null ? SortedSetCopyable<T>.Node.Spawn(theirCurrent.Left.Item, theirCurrent.Left.IsRed) : null);
                    theirCurrent = theirCurrent.Left;
                    myCurrent = myCurrent.Left;
                }
                while (theirStack.Count != 0) {
                    theirCurrent = theirStack.Pop();
                    myCurrent = myStack.Pop();
                    Node theirRight = theirCurrent.Right;
                    Node myRight = null;
                    if (theirRight != null) {
                        myRight = SortedSetCopyable<T>.Node.Spawn(theirRight.Item, theirRight.IsRed);
                    }
                    myCurrent.Right = myRight;
 
                    while (theirRight != null) {
                        theirStack.Push(theirRight);
                        myStack.Push(myRight);
                        myRight.Left = (theirRight.Left != null ? SortedSetCopyable<T>.Node.Spawn(theirRight.Left.Item, theirRight.Left.IsRed) : null);
                        theirRight = theirRight.Left;
                        myRight = myRight.Left;
                    }
                }
                PoolStack<SortedSetCopyable<T>.Node>.Recycle(ref myStack);
                PoolStack<SortedSetCopyable<T>.Node>.Recycle(ref theirStack);
                count = baseSortedSet.count;
                version = 0;
            //}
            
            //this.RecycleNodes(ref this.root);
            //this.CopyNodes(ref this.root, other.root);
            
            this.count = other.count;
            this.version = other.version;
            this._syncRoot = other._syncRoot;

        }

        private void RecycleNodes(ref Node node) {

            if (node == null) return;
            
            var current = node.Left;
            while (current != null) {

                var next = current.Left;
                PoolClass<Node>.Recycle(ref current);
                current = next;
                
            }
            
            current = node.Right;
            while (current != null) {

                var next = current.Right;
                PoolClass<Node>.Recycle(ref current);
                current = next;
                
            }
            
            PoolClass<Node>.Recycle(ref node);
            
        }

        /*private void CopyNodes(ref Node root, Node otherRoot) {

            if (otherRoot == null) {

                root = null;
                return;
                
            }
            
            root = PoolClass<Node>.Spawn();
            root.Item = otherRoot.Item;
            root.IsRed = otherRoot.IsRed;
            
            var current = otherRoot.Left;
            var rootNode = root;
            while (current != null) {

                var next = current.Left;
                {
                    var node = PoolClass<Node>.Spawn();
                    node.Item = current.Item;
                    node.IsRed = current.IsRed;
                    rootNode.Left = node;
                    rootNode = node;
                }
                current = next;
                
            }
            
            current = otherRoot.Right;
            rootNode = root;
            while (current != null) {

                var next = current.Right;
                {
                    var node = PoolClass<Node>.Spawn();
                    node.Item = current.Item;
                    node.IsRed = current.IsRed;
                    rootNode.Right = node;
                    rootNode = node;
                }
                current = next;
                
            }
            
        }*/
        
        #region Constructors
        public SortedSetCopyable() {
            this.comparer = Comparer<T>.Default;
 
        }
 
        public SortedSetCopyable(IComparer<T> comparer) {
            if (comparer == null) {
                this.comparer = Comparer<T>.Default;
            } else {
                this.comparer = comparer;
            }
        }
 
 
        public SortedSetCopyable(IEnumerable<T> collection) : this(collection, Comparer<T>.Default) { }
 
        public SortedSetCopyable(IEnumerable<T> collection, IComparer<T> comparer)
            : this(comparer) {
 
            if (collection == null) {
                throw new ArgumentNullException("collection");
            }
 
            // these are explicit type checks in the mould of HashSet. It would have worked better
            // with something like an ISorted<T> (we could make this work for SortedList.Keys etc)
            SortedSetCopyable<T> baseSortedSet = collection as SortedSetCopyable<T>;
            SortedSetCopyable<T> baseTreeSubSet = collection as TreeSubSet;
            if (baseSortedSet != null && baseTreeSubSet == null && AreComparersEqual(this, baseSortedSet)) {
                //breadth first traversal to recreate nodes
                if (baseSortedSet.Count == 0) {
                    count = 0;
                    version = 0;
                    root = null;
                    return;
                }
 
 
                //pre order way to replicate nodes
                Stack<Node> theirStack = new Stack<SortedSetCopyable<T>.Node>(2 * log2(baseSortedSet.Count) + 2);
                Stack<Node> myStack = new Stack<SortedSetCopyable<T>.Node>(2 * log2(baseSortedSet.Count) + 2);
                Node theirCurrent = baseSortedSet.root;
                Node myCurrent = (theirCurrent != null ? new SortedSetCopyable<T>.Node(theirCurrent.Item, theirCurrent.IsRed) : null);
                root = myCurrent;
                while (theirCurrent != null) {
                    theirStack.Push(theirCurrent);
                    myStack.Push(myCurrent);
                    myCurrent.Left = (theirCurrent.Left != null ? new SortedSetCopyable<T>.Node(theirCurrent.Left.Item, theirCurrent.Left.IsRed) : null);
                    theirCurrent = theirCurrent.Left;
                    myCurrent = myCurrent.Left;
                }
                while (theirStack.Count != 0) {
                    theirCurrent = theirStack.Pop();
                    myCurrent = myStack.Pop();
                    Node theirRight = theirCurrent.Right;
                    Node myRight = null;
                    if (theirRight != null) {
                        myRight = new SortedSetCopyable<T>.Node(theirRight.Item, theirRight.IsRed);
                    }
                    myCurrent.Right = myRight;
 
                    while (theirRight != null) {
                        theirStack.Push(theirRight);
                        myStack.Push(myRight);
                        myRight.Left = (theirRight.Left != null ? new SortedSetCopyable<T>.Node(theirRight.Left.Item, theirRight.Left.IsRed) : null);
                        theirRight = theirRight.Left;
                        myRight = myRight.Left;
                    }
                }
                count = baseSortedSet.count;
                version = 0;
            } else {     //As it stands, you're doing an NlogN sort of the collection
 
                List<T> els = new List<T>(collection);
                els.Sort(this.comparer);
                for (int i = 1; i < els.Count; i++) {
                    if (comparer.Compare(els[i], els[i - 1]) == 0) {
                        els.RemoveAt(i);
                        i--;
                    }
                }
                root = ConstructRootFromSortedArray(els.ToArray(), 0, els.Count - 1, null);
                count = els.Count;
                version = 0;
            }
        }
 
 
#if !FEATURE_NETCORE
        protected SortedSetCopyable(SerializationInfo info, StreamingContext context) {
            siInfo = info;
        }
#endif
        #endregion
 
        #region Bulk Operation Helpers
        private void AddAllElements(IEnumerable<T> collection) {
 
            foreach (T item in collection) {
                if (!this.Contains(item))
                    Add(item);
            }
        }
 
        private void RemoveAllElements(IEnumerable<T> collection) {
            T min = this.Min;
            T max = this.Max;
            foreach (T item in collection) {
                if (!(comparer.Compare(item, min) < 0 || comparer.Compare(item, max) > 0) && this.Contains(item))
                    this.Remove(item);
            }
        }
 
        private bool ContainsAllElements(IEnumerable<T> collection) {
            foreach (T item in collection) {
                if (!this.Contains(item)) {
                    return false;
                }
            }
            return true;
        }
 
        //
        // Do a in order walk on tree and calls the delegate for each node.
        // If the action delegate returns false, stop the walk.
        // 
        // Return true if the entire tree has been walked. 
        // Otherwise returns false.
        //
        internal bool InOrderTreeWalk(TreeWalkPredicate<T> action) {
            return InOrderTreeWalk(action, false);
        }
 
        // Allows for the change in traversal direction. Reverse visits nodes in descending order 
        internal virtual bool InOrderTreeWalk(TreeWalkPredicate<T> action, bool reverse) {
            if (root == null) {
                return true;
            }
 
            // The maximum height of a red-black tree is 2*lg(n+1).
            // See page 264 of "Introduction to algorithms" by Thomas H. Cormen
            // note: this should be logbase2, but since the stack grows itself, we 
            // don't want the extra cost
            Stack<Node> stack = new Stack<Node>(2 * (int)(SortedSetCopyable<T>.log2(Count + 1)));
            Node current = root;
            while (current != null) {
                stack.Push(current);
                current = (reverse ? current.Right : current.Left);
            }
            while (stack.Count != 0) {
                current = stack.Pop();
                if (!action(current)) {
                    return false;
                }
 
                Node node = (reverse ? current.Left : current.Right);
                while (node != null) {
                    stack.Push(node);
                    node = (reverse ? node.Right : node.Left);
                }
            }
            return true;
        }
 
        //
        // Do a left to right breadth first walk on tree and 
        // calls the delegate for each node.
        // If the action delegate returns false, stop the walk.
        // 
        // Return true if the entire tree has been walked. 
        // Otherwise returns false.
        //
        internal virtual bool BreadthFirstTreeWalk(TreeWalkPredicate<T> action) {
            if (root == null) {
                return true;
            }
 
            List<Node> processQueue = new List<Node>();
            processQueue.Add(root);
            Node current;
 
            while (processQueue.Count != 0) {
                current = processQueue[0];
                processQueue.RemoveAt(0);
                if (!action(current)) {
                    return false;
                }
                if (current.Left != null) {
                    processQueue.Add(current.Left);
                }
                if (current.Right != null) {
                    processQueue.Add(current.Right);
                }
            }
            return true;
        }
        #endregion
 
        #region Properties
        public int Count {
            get {
                VersionCheck();
                return count;
            }
        }
 
        public IComparer<T> Comparer {
            get {
                return comparer;
            }
        }
 
        bool ICollection<T>.IsReadOnly {
            get {
                return false;
            }
        }
 
        bool ICollection.IsSynchronized {
            get {
                return false;
            }
        }
 
        object ICollection.SyncRoot {
            get {
                if (_syncRoot == null) {
                    System.Threading.Interlocked.CompareExchange(ref _syncRoot, new Object(), null);
                }
                return _syncRoot;
            }
        }
        #endregion
 
        #region Subclass helpers
 
        //virtual function for subclass that needs to update count
        internal virtual void VersionCheck() { }
 
 
        //virtual function for subclass that needs to do range checks
        internal virtual bool IsWithinRange(T item) {
            return true;
 
        }
        #endregion
 
        #region ICollection<T> Members
        /// <summary>
        /// Add the value ITEM to the tree, returns true if added, false if duplicate 
        /// </summary>
        /// <param name="item">item to be added</param> 
        public bool Add(T item) {
            return AddIfNotPresent(item);
        }
 
        void ICollection<T>.Add(T item) {
            AddIfNotPresent(item);
        }
 
 
        /// <summary>
        /// Adds ITEM to the tree if not already present. Returns TRUE if value was successfully added         
        /// or FALSE if it is a duplicate
        /// </summary>        
        internal virtual bool AddIfNotPresent(T item) {
            if (root == null) {   // empty tree
                root = Node.Spawn(item, false);
                count = 1;
                version++;
                return true;
            }
 
            //
            // Search for a node at bottom to insert the new node. 
            // If we can guanratee the node we found is not a 4-node, it would be easy to do insertion.
            // We split 4-nodes along the search path.
            // 
            Node current = root;
            Node parent = null;
            Node grandParent = null;
            Node greatGrandParent = null;
 
            //even if we don't actually add to the set, we may be altering its structure (by doing rotations
            //and such). so update version to disable any enumerators/subsets working on it
            version++;
 
 
            int order = 0;
            while (current != null) {
                order = comparer.Compare(item, current.Item);
                if (order == 0) {
                    // We could have changed root node to red during the search process.
                    // We need to set it to black before we return.
                    root.IsRed = false;
                    return false;
                }
 
                // split a 4-node into two 2-nodes                
                if (Is4Node(current)) {
                    Split4Node(current);
                    // We could have introduced two consecutive red nodes after split. Fix that by rotation.
                    if (IsRed(parent)) {
                        InsertionBalance(current, ref parent, grandParent, greatGrandParent);
                    }
                }
                greatGrandParent = grandParent;
                grandParent = parent;
                parent = current;
                current = (order < 0) ? current.Left : current.Right;
            }
 
            Debug.Assert(parent != null, "Parent node cannot be null here!");
            // ready to insert the new node
            Node node = Node.Spawn(item);
            if (order > 0) {
                parent.Right = node;
            } else {
                parent.Left = node;
            }
 
            // the new node will be red, so we will need to adjust the colors if parent node is also red
            if (parent.IsRed) {
                InsertionBalance(node, ref parent, grandParent, greatGrandParent);
            }
 
            // Root node is always black
            root.IsRed = false;
            ++count;
            return true;
        }
 
        /// <summary>
        /// Remove the T ITEM from this SortedSet. Returns true if successfully removed.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item) {
            return this.DoRemove(item); // hack so it can be made non-virtual
        }
 
        internal virtual bool DoRemove(T item) {
 
            if (root == null) {
                return false;
            }
 
 
            // Search for a node and then find its succesor. 
            // Then copy the item from the succesor to the matching node and delete the successor. 
            // If a node doesn't have a successor, we can replace it with its left child (if not empty.) 
            // or delete the matching node.
            // 
            // In top-down implementation, it is important to make sure the node to be deleted is not a 2-node.
            // Following code will make sure the node on the path is not a 2 Node. 
 
            //even if we don't actually remove from the set, we may be altering its structure (by doing rotations
            //and such). so update version to disable any enumerators/subsets working on it
            version++;
 
            Node current = root;
            Node parent = null;
            Node grandParent = null;
            Node match = null;
            Node parentOfMatch = null;
            bool foundMatch = false;
            while (current != null) {
                if (Is2Node(current)) { // fix up 2-Node
                    if (parent == null) {   // current is root. Mark it as red
                        current.IsRed = true;
                    } else {
                        Node sibling = GetSibling(current, parent);
                        if (sibling.IsRed) {
                            // If parent is a 3-node, flip the orientation of the red link. 
                            // We can acheive this by a single rotation        
                            // This case is converted to one of other cased below.
                            Debug.Assert(!parent.IsRed, "parent must be a black node!");
                            if (parent.Right == sibling) {
                                RotateLeft(parent);
                            } else {
                                RotateRight(parent);
                            }
 
                            parent.IsRed = true;
                            sibling.IsRed = false;    // parent's color
                            // sibling becomes child of grandParent or root after rotation. Update link from grandParent or root
                            ReplaceChildOfNodeOrRoot(grandParent, parent, sibling);
                            // sibling will become grandParent of current node 
                            grandParent = sibling;
                            if (parent == match) {
                                parentOfMatch = sibling;
                            }
 
                            // update sibling, this is necessary for following processing
                            sibling = (parent.Left == current) ? parent.Right : parent.Left;
                        }
                        Debug.Assert(sibling != null && sibling.IsRed == false, "sibling must not be null and it must be black!");
 
                        if (Is2Node(sibling)) {
                            Merge2Nodes(parent, current, sibling);
                        } else {
                            // current is a 2-node and sibling is either a 3-node or a 4-node.
                            // We can change the color of current to red by some rotation.
                            TreeRotation rotation = RotationNeeded(parent, current, sibling);
                            Node newGrandParent = null;
                            switch (rotation) {
                                case TreeRotation.RightRotation:
                                    Debug.Assert(parent.Left == sibling, "sibling must be left child of parent!");
                                    Debug.Assert(sibling.Left.IsRed, "Left child of sibling must be red!");
                                    sibling.Left.IsRed = false;
                                    newGrandParent = RotateRight(parent);
                                    break;
                                case TreeRotation.LeftRotation:
                                    Debug.Assert(parent.Right == sibling, "sibling must be left child of parent!");
                                    Debug.Assert(sibling.Right.IsRed, "Right child of sibling must be red!");
                                    sibling.Right.IsRed = false;
                                    newGrandParent = RotateLeft(parent);
                                    break;
 
                                case TreeRotation.RightLeftRotation:
                                    Debug.Assert(parent.Right == sibling, "sibling must be left child of parent!");
                                    Debug.Assert(sibling.Left.IsRed, "Left child of sibling must be red!");
                                    newGrandParent = RotateRightLeft(parent);
                                    break;
 
                                case TreeRotation.LeftRightRotation:
                                    Debug.Assert(parent.Left == sibling, "sibling must be left child of parent!");
                                    Debug.Assert(sibling.Right.IsRed, "Right child of sibling must be red!");
                                    newGrandParent = RotateLeftRight(parent);
                                    break;
                            }
 
                            newGrandParent.IsRed = parent.IsRed;
                            parent.IsRed = false;
                            current.IsRed = true;
                            ReplaceChildOfNodeOrRoot(grandParent, parent, newGrandParent);
                            if (parent == match) {
                                parentOfMatch = newGrandParent;
                            }
                            grandParent = newGrandParent;
                        }
                    }
                }
 
                // we don't need to compare any more once we found the match
                int order = foundMatch ? -1 : comparer.Compare(item, current.Item);
                if (order == 0) {
                    // save the matching node
                    foundMatch = true;
                    match = current;
                    parentOfMatch = parent;
                }
 
                grandParent = parent;
                parent = current;
 
                if (order < 0) {
                    current = current.Left;
                } else {
                    current = current.Right;       // continue the search in  right sub tree after we find a match
                }
            }
 
            // move successor to the matching node position and replace links
            if (match != null) {
                ReplaceNode(match, parentOfMatch, parent, grandParent);
                --count;
            }
 
            if (root != null) {
                root.IsRed = false;
            }
            return foundMatch;
        }
 
        public virtual void Clear() {
            PoolClass<Node>.Recycle(ref root);
            root = null;
            count = 0;
            ++version;
        }
 
 
        public virtual bool Contains(T item) {
 
            return FindNode(item) != null;
        }
 
 
 
 
        public void CopyTo(T[] array) { CopyTo(array, 0, Count); }
 
        public void CopyTo(T[] array, int index) { CopyTo(array, index, Count); }
 
        public void CopyTo(T[] array, int index, int count) {
            // will array, starting at arrayIndex, be able to hold elements? Note: not
            // checking arrayIndex >= array.Length (consistency with list of allowing
            // count of 0; subsequent check takes care of the rest)
            //upper bound
            count += index;
 
            Stack<Node> stack = PoolStack<Node>.Spawn(2 * (int)(SortedSetCopyable<T>.log2(Count + 1)));
            Node current = root;
            while (current != null) {
                stack.Push(current);
                current = current.Left;
            }
            while (stack.Count != 0) {
                current = stack.Pop();
                
                if (index >= count) {
                    return;
                }

                array[index++] = current.Item;

                Node node = current.Right;
                while (node != null) {
                    stack.Push(node);
                    node = node.Left;
                }
            }
            PoolStack<Node>.Recycle(ref stack);
            
            /*InOrderTreeWalk(delegate(Node node) {
                if (index >= count) {
                    return false;
                } else {
                    array[index++] = node.Item;
                    return true;
                }
            });*/
        }
 
        void ICollection.CopyTo(Array array, int index) {
            T[] tarray = array as T[];
            if (tarray != null) {
                CopyTo(tarray, index);
            } else {
                object[] objects = array as object[];
                InOrderTreeWalk(delegate(Node node) { objects[index++] = node.Item; return true; });
            }
        }
 
        #endregion
 
        #region IEnumerable<T> members
        public Enumerator GetEnumerator() {
            return new Enumerator(this);
        }
 
 
 
 
        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return new Enumerator(this);
        }
 
        IEnumerator IEnumerable.GetEnumerator() {
            return new Enumerator(this);
        }
        #endregion
 
        #region Tree Specific Operations
 
        private static Node GetSibling(Node node, Node parent) {
            if (parent.Left == node) {
                return parent.Right;
            }
            return parent.Left;
        }
 
        // After calling InsertionBalance, we need to make sure current and parent up-to-date.
        // It doesn't matter if we keep grandParent and greatGrantParent up-to-date 
        // because we won't need to split again in the next node.
        // By the time we need to split again, everything will be correctly set.
        //  
        private void InsertionBalance(Node current, ref Node parent, Node grandParent, Node greatGrandParent) {
            Debug.Assert(grandParent != null, "Grand parent cannot be null here!");
            bool parentIsOnRight = (grandParent.Right == parent);
            bool currentIsOnRight = (parent.Right == current);
 
            Node newChildOfGreatGrandParent;
            if (parentIsOnRight == currentIsOnRight) { // same orientation, single rotation
                newChildOfGreatGrandParent = currentIsOnRight ? RotateLeft(grandParent) : RotateRight(grandParent);
            } else {  // different orientaton, double rotation
                newChildOfGreatGrandParent = currentIsOnRight ? RotateLeftRight(grandParent) : RotateRightLeft(grandParent);
                // current node now becomes the child of greatgrandparent 
                parent = greatGrandParent;
            }
            // grand parent will become a child of either parent of current.
            grandParent.IsRed = true;
            newChildOfGreatGrandParent.IsRed = false;
 
            ReplaceChildOfNodeOrRoot(greatGrandParent, grandParent, newChildOfGreatGrandParent);
        }
 
        private static bool Is2Node(Node node) {
            Debug.Assert(node != null, "node cannot be null!");
            return IsBlack(node) && IsNullOrBlack(node.Left) && IsNullOrBlack(node.Right);
        }
 
        private static bool Is4Node(Node node) {
            return IsRed(node.Left) && IsRed(node.Right);
        }
 
        private static bool IsBlack(Node node) {
            return (node != null && !node.IsRed);
        }
 
        private static bool IsNullOrBlack(Node node) {
            return (node == null || !node.IsRed);
        }
 
        private static bool IsRed(Node node) {
            return (node != null && node.IsRed);
        }
 
        private static void Merge2Nodes(Node parent, Node child1, Node child2) {
            Debug.Assert(IsRed(parent), "parent must be be red");
            // combing two 2-nodes into a 4-node
            parent.IsRed = false;
            child1.IsRed = true;
            child2.IsRed = true;
        }
 
        // Replace the child of a parent node. 
        // If the parent node is null, replace the root.        
        private void ReplaceChildOfNodeOrRoot(Node parent, Node child, Node newChild) {
            if (parent != null) {
                if (parent.Left == child) {
                    parent.Left = newChild;
                } else {
                    parent.Right = newChild;
                }
            } else {
                root = newChild;
            }
        }
 
        // Replace the matching node with its succesor.
        private void ReplaceNode(Node match, Node parentOfMatch, Node succesor, Node parentOfSuccesor) {
            if (succesor == match) {  // this node has no successor, should only happen if right child of matching node is null.
                Debug.Assert(match.Right == null, "Right child must be null!");
                succesor = match.Left;
            } else {
                Debug.Assert(parentOfSuccesor != null, "parent of successor cannot be null!");
                Debug.Assert(succesor.Left == null, "Left child of succesor must be null!");
                Debug.Assert((succesor.Right == null && succesor.IsRed) || (succesor.Right.IsRed && !succesor.IsRed), "Succesor must be in valid state");
                if (succesor.Right != null) {
                    succesor.Right.IsRed = false;
                }
 
                if (parentOfSuccesor != match) {   // detach succesor from its parent and set its right child
                    parentOfSuccesor.Left = succesor.Right;
                    succesor.Right = match.Right;
                }
 
                succesor.Left = match.Left;
            }
 
            if (succesor != null) {
                succesor.IsRed = match.IsRed;
            }
 
            ReplaceChildOfNodeOrRoot(parentOfMatch, match, succesor);
 
        }
 
        internal virtual Node FindNode(T item) {
            Node current = root;
            while (current != null) {
                int order = comparer.Compare(item, current.Item);
                if (order == 0) {
                    return current;
                } else {
                    current = (order < 0) ? current.Left : current.Right;
                }
            }
 
            return null;
        }
 
        //used for bithelpers. Note that this implementation is completely different 
        //from the Subset's. The two should not be mixed. This indexes as if the tree were an array.
        //http://en.wikipedia.org/wiki/Binary_Tree#Methods_for_storing_binary_trees
        internal virtual int InternalIndexOf(T item) {
            Node current = root;
            int count = 0;
            while (current != null) {
                int order = comparer.Compare(item, current.Item);
                if (order == 0) {
                    return count;
                } else {
                    current = (order < 0) ? current.Left : current.Right;
                    count = (order < 0) ? (2 * count + 1) : (2 * count + 2);
                }
            }
            return -1;
        }
 
 
 
        internal Node FindRange(T from, T to) {
            return FindRange(from, to, true, true);
        }
        internal Node FindRange(T from, T to, bool lowerBoundActive, bool upperBoundActive) {
            Node current = root;
            while (current != null) {
                if (lowerBoundActive && comparer.Compare(from, current.Item) > 0) {
                    current = current.Right;
                } else {
                    if (upperBoundActive && comparer.Compare(to, current.Item) < 0) {
                        current = current.Left;
                    } else {
                        return current;
                    }
                }
            }
 
            return null;
        }
 
        internal void UpdateVersion() {
            ++version;
        }
 
 
        private static Node RotateLeft(Node node) {
            Node x = node.Right;
            node.Right = x.Left;
            x.Left = node;
            return x;
        }
 
        private static Node RotateLeftRight(Node node) {
            Node child = node.Left;
            Node grandChild = child.Right;
 
            node.Left = grandChild.Right;
            grandChild.Right = node;
            child.Right = grandChild.Left;
            grandChild.Left = child;
            return grandChild;
        }
 
        private static Node RotateRight(Node node) {
            Node x = node.Left;
            node.Left = x.Right;
            x.Right = node;
            return x;
        }
 
        private static Node RotateRightLeft(Node node) {
            Node child = node.Right;
            Node grandChild = child.Left;
 
            node.Right = grandChild.Left;
            grandChild.Left = node;
            child.Left = grandChild.Right;
            grandChild.Right = child;
            return grandChild;
        }
        /// <summary>
        /// Testing counter that can track rotations
        /// </summary>
 
 
        private static TreeRotation RotationNeeded(Node parent, Node current, Node sibling) {
            Debug.Assert(IsRed(sibling.Left) || IsRed(sibling.Right), "sibling must have at least one red child");
            if (IsRed(sibling.Left)) {
                if (parent.Left == current) {
                    return TreeRotation.RightLeftRotation;
                }
                return TreeRotation.RightRotation;
            } else {
                if (parent.Left == current) {
                    return TreeRotation.LeftRotation;
                }
                return TreeRotation.LeftRightRotation;
            }
        }
 
        /// <summary>
        /// Used for deep equality of SortedSet testing
        /// </summary>
        /// <returns></returns>
        public static IEqualityComparer<SortedSetCopyable<T>> CreateSetComparer() {
            return new SortedSetEqualityComparer<T>();
        }
 
        /// <summary>
        /// Create a new set comparer for this set, where this set's members' equality is defined by the
        /// memberEqualityComparer. Note that this equality comparer's definition of equality must be the
        /// same as this set's Comparer's definition of equality
        /// </summary>                
        public static IEqualityComparer<SortedSetCopyable<T>> CreateSetComparer(IEqualityComparer<T> memberEqualityComparer) {
            return new SortedSetEqualityComparer<T>(memberEqualityComparer);
        }
 
 
        /// <summary>
        /// Decides whether these sets are the same, given the comparer. If the EC's are the same, we can
        /// just use SetEquals, but if they aren't then we have to manually check with the given comparer
        /// </summary>        
        internal static bool SortedSetEquals(SortedSetCopyable<T> set1, SortedSetCopyable<T> set2, IComparer<T> comparer) {
            // handle null cases first
            if (set1 == null) {
                return (set2 == null);
            } else if (set2 == null) {
                // set1 != null
                return false;
            }
 
            if (AreComparersEqual(set1, set2)) {
                if (set1.Count != set2.Count)
                    return false;
 
                return set1.SetEquals(set2);
            } else {
                bool found = false;
                foreach (T item1 in set1) {
                    found = false;
                    foreach (T item2 in set2) {
                        if (comparer.Compare(item1, item2) == 0) {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        return false;
                }
                return true;
            }
 
        }
 
 
        //This is a little frustrating because we can't support more sorted structures
        private static bool AreComparersEqual(SortedSetCopyable<T> set1, SortedSetCopyable<T> set2) {
            return set1.Comparer.Equals(set2.Comparer);
        }
 
 
        private static void Split4Node(Node node) {
            node.IsRed = true;
            node.Left.IsRed = false;
            node.Right.IsRed = false;
        }
 
        /// <summary>
        /// Copies this to an array. Used for DebugView
        /// </summary>
        /// <returns></returns>
        internal T[] ToArray() {
            T[] newArray = new T[Count];
            CopyTo(newArray);
            return newArray;
        }
 
 
        #endregion
 
        #region ISet Members
 
        /// <summary>
        /// Transform this set into its union with the IEnumerable OTHER            
        ///Attempts to insert each element and rejects it if it exists.          
        /// NOTE: The caller object is important as UnionWith uses the Comparator 
        ///associated with THIS to check equality                                
        /// Throws ArgumentNullException if OTHER is null                         
        /// </summary>
        /// <param name="other"></param>
        public void UnionWith(IEnumerable<T> other) {
            if (other == null) {
                throw new ArgumentNullException("other");
            }
 
            SortedSetCopyable<T> s = other as SortedSetCopyable<T>;
            TreeSubSet t = this as TreeSubSet;
 
            if (t != null)
                VersionCheck();
 
            if (s != null && t == null && this.count == 0) {
                SortedSetCopyable<T> dummy = new SortedSetCopyable<T>(s, this.comparer);
                this.root = dummy.root;
                this.count = dummy.count;
                this.version++;
                return;
            }
 
 
            if (s != null && t == null && AreComparersEqual(this, s) && (s.Count > this.Count / 2)) { //this actually hurts if N is much greater than M the /2 is arbitrary
                //first do a merge sort to an array.
                T[] merged = new T[s.Count + this.Count];
                int c = 0;
                Enumerator mine = this.GetEnumerator();
                Enumerator theirs = s.GetEnumerator();
                bool mineEnded = !mine.MoveNext(), theirsEnded = !theirs.MoveNext();
                while (!mineEnded && !theirsEnded) {
                    int comp = Comparer.Compare(mine.Current, theirs.Current);
                    if (comp < 0) {
                        merged[c++] = mine.Current;
                        mineEnded = !mine.MoveNext();
                    } else if (comp == 0) {
                        merged[c++] = theirs.Current;
                        mineEnded = !mine.MoveNext();
                        theirsEnded = !theirs.MoveNext();
                    } else {
                        merged[c++] = theirs.Current;
                        theirsEnded = !theirs.MoveNext();
                    }
                }
 
                if (!mineEnded || !theirsEnded) {
                    Enumerator remaining = (mineEnded ? theirs : mine);
                    do {
                        merged[c++] = remaining.Current;
                    } while (remaining.MoveNext());
                }
 
                //now merged has all c elements
 
                //safe to gc the root, we  have all the elements
                root = null;
 
 
                root = SortedSetCopyable<T>.ConstructRootFromSortedArray(merged, 0, c - 1, null);
                count = c;
                version++;
            } else {
                AddAllElements(other);
            }
        }
 
 
        private static Node ConstructRootFromSortedArray(T[] arr, int startIndex, int endIndex, Node redNode) {
 
 
 
            //what does this do?
            //you're given a sorted array... say 1 2 3 4 5 6 
            //2 cases:
            //    If there are odd # of elements, pick the middle element (in this case 4), and compute
            //    its left and right branches
            //    If there are even # of elements, pick the left middle element, save the right middle element
            //    and call the function on the rest
            //    1 2 3 4 5 6 -> pick 3, save 4 and call the fn on 1,2 and 5,6
            //    now add 4 as a red node to the lowest element on the right branch
            //             3                       3
            //         1       5       ->     1        5
            //           2       6             2     4   6            
            //    As we're adding to the leftmost of the right branch, nesting will not hurt the red-black properties
            //    Leaf nodes are red if they have no sibling (if there are 2 nodes or if a node trickles
            //    down to the bottom
 
 
            //the iterative way to do this ends up wasting more space than it saves in stack frames (at
            //least in what i tried)
            //so we're doing this recursively
            //base cases are described below
            int size = endIndex - startIndex + 1;
            if (size == 0) {
                return null;
            }
            Node root = null;
            if (size == 1) {
                root =  Node.Spawn(arr[startIndex], false);
                if (redNode != null) {
                    root.Left = redNode;
                }
            } else if (size == 2) {
                root = Node.Spawn(arr[startIndex], false);
                root.Right = Node.Spawn(arr[endIndex], false);
                root.Right.IsRed = true;
                if (redNode != null) {
                    root.Left = redNode;
                }
            } else if (size == 3) {
                root = Node.Spawn(arr[startIndex + 1], false);
                root.Left = Node.Spawn(arr[startIndex], false);
                root.Right = Node.Spawn(arr[endIndex], false);
                if (redNode != null) {
                    root.Left.Left = redNode;
 
                }
            } else {
                int midpt = ((startIndex + endIndex) / 2);
                root = Node.Spawn(arr[midpt], false);
                root.Left = ConstructRootFromSortedArray(arr, startIndex, midpt - 1, redNode);
                if (size % 2 == 0) {
                    root.Right = ConstructRootFromSortedArray(arr, midpt + 2, endIndex, Node.Spawn(arr[midpt + 1], true));
                } else {
                    root.Right = ConstructRootFromSortedArray(arr, midpt + 1, endIndex, null);
                }
            }
            return root;
 
        }
 
 
        /// <summary>
        /// Transform this set into its intersection with the IEnumerable OTHER     
        /// NOTE: The caller object is important as IntersectionWith uses the        
        /// comparator associated with THIS to check equality                        
        /// Throws ArgumentNullException if OTHER is null                         
        /// </summary>
        /// <param name="other"></param>   
        public virtual void IntersectWith(IEnumerable<T> other) {
            if (other == null) {
                throw new ArgumentNullException("other");
            }
 
            if (Count == 0)
                return;
 
            //HashSet<T> optimizations can't be done until equality comparers and comparers are related
 
            //Technically, this would work as well with an ISorted<T>
            SortedSetCopyable<T> s = other as SortedSetCopyable<T>;
            TreeSubSet t = this as TreeSubSet;
            if (t != null)
                VersionCheck();
            //only let this happen if i am also a SortedSet, not a SubSet
            if (s != null && t == null && AreComparersEqual(this, s)) {
 
 
                //first do a merge sort to an array.
                T[] merged = new T[this.Count];
                int c = 0;
                Enumerator mine = this.GetEnumerator();
                Enumerator theirs = s.GetEnumerator();
                bool mineEnded = !mine.MoveNext(), theirsEnded = !theirs.MoveNext();
                T max = Max;
                T min = Min;
 
                while (!mineEnded && !theirsEnded && Comparer.Compare(theirs.Current, max) <= 0) {
                    int comp = Comparer.Compare(mine.Current, theirs.Current);
                    if (comp < 0) {
                        mineEnded = !mine.MoveNext();
                    } else if (comp == 0) {
                        merged[c++] = theirs.Current;
                        mineEnded = !mine.MoveNext();
                        theirsEnded = !theirs.MoveNext();
                    } else {
                        theirsEnded = !theirs.MoveNext();
                    }
                }
 
                //now merged has all c elements
 
                //safe to gc the root, we  have all the elements
                root = null;
 
                root = SortedSetCopyable<T>.ConstructRootFromSortedArray(merged, 0, c - 1, null);
                count = c;
                version++;
            } else {
                IntersectWithEnumerable(other);
            }
        }
 
        internal virtual void IntersectWithEnumerable(IEnumerable<T> other) {
            //
            List<T> toSave = new List<T>(this.Count);
            foreach (T item in other) {
                if (this.Contains(item)) {
                    toSave.Add(item);
                    this.Remove(item);
                }
            }
            this.Clear();
            AddAllElements(toSave);
 
        }
 
 
 
        /// <summary>
        ///  Transform this set into its complement with the IEnumerable OTHER       
        ///  NOTE: The caller object is important as ExceptWith uses the        
        ///  comparator associated with THIS to check equality                        
        ///  Throws ArgumentNullException if OTHER is null                           
        /// </summary>
        /// <param name="other"></param>
        public void ExceptWith(IEnumerable<T> other) {
            if (other == null) {
                throw new ArgumentNullException("other");
            }
 
            if (count == 0)
                return;
 
            if (other == this) {
                this.Clear();
                return;
            }
 
            SortedSetCopyable<T> asSorted = other as SortedSetCopyable<T>;
 
            if (asSorted != null && AreComparersEqual(this, asSorted)) {
                //outside range, no point doing anything               
                if (!(comparer.Compare(asSorted.Max, this.Min) < 0 || comparer.Compare(asSorted.Min, this.Max) > 0)) {
                    T min = this.Min;
                    T max = this.Max;
                    foreach (T item in other) {
                        if (comparer.Compare(item, min) < 0)
                            continue;
                        if (comparer.Compare(item, max) > 0)
                            break;
                        Remove(item);
                    }
                }
 
            } else {
                RemoveAllElements(other);
            }
        }
 
        /// <summary>
        ///  Transform this set so it contains elements in THIS or OTHER but not both
        ///  NOTE: The caller object is important as SymmetricExceptWith uses the        
        ///  comparator associated with THIS to check equality                        
        ///  Throws ArgumentNullException if OTHER is null                           
        /// </summary>
        /// <param name="other"></param>
        public void SymmetricExceptWith(IEnumerable<T> other) {
            if (other == null) {
                throw new ArgumentNullException("other");
            }
 
            if (this.Count == 0) {
                this.UnionWith(other);
                return;
            }
 
            if (other == this) {
                this.Clear();
                return;
            }
 
 
            SortedSetCopyable<T> asSorted = other as SortedSetCopyable<T>;
 
#if USING_HASH_SET
            HashSet<T> asHash = other as HashSet<T>;
#endif
            if (asSorted != null && AreComparersEqual(this, asSorted)) {
                SymmetricExceptWithSameEC(asSorted);
            }
#if USING_HASH_SET
 else if (asHash != null && this.comparer.Equals(Comparer<T>.Default) && asHash.Comparer.Equals(EqualityComparer<T>.Default)) {
                SymmetricExceptWithSameEC(asHash);
            }
#endif
 else {
                //need perf improvement on this
                T[] elements = (new List<T>(other)).ToArray();
                Array.Sort(elements, this.Comparer);
                SymmetricExceptWithSameEC(elements);
            }
        }
 
        //OTHER must be a set
        internal void SymmetricExceptWithSameEC(ISet<T> other) {
            foreach (T item in other) {
                //yes, it is classier to say
                //if (!this.Remove(item))this.Add(item);
                //but this ends up saving on rotations                    
                if (this.Contains(item)) {
                    this.Remove(item);
                } else {
                    this.Add(item);
                }
            }
        }
 
        //OTHER must be a sorted array
        internal void SymmetricExceptWithSameEC(T[] other) {
            if (other.Length == 0) {
                return;
            }
            T last = other[0];
            for (int i = 0; i < other.Length; i++) {
                while (i < other.Length && i != 0 && comparer.Compare(other[i], last) == 0)
                    i++;
                if (i >= other.Length)
                    break;
                if (this.Contains(other[i])) {
                    this.Remove(other[i]);
                } else {
                    this.Add(other[i]);
                }
                last = other[i];
            }
        }
 
 
        /// <summary>
        /// Checks whether this Tree is a subset of the IEnumerable other
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [System.Security.SecuritySafeCritical]
        public bool IsSubsetOf(IEnumerable<T> other) {
            if (other == null) {
                throw new ArgumentNullException("other");
            }
 
            if (Count == 0)
                return true;
 
 
            SortedSetCopyable<T> asSorted = other as SortedSetCopyable<T>;
            if (asSorted != null && AreComparersEqual(this, asSorted)) {
                if (this.Count > asSorted.Count)
                    return false;
                return IsSubsetOfSortedSetWithSameEC(asSorted);
            } else {
                //worst case: mark every element in my set and see if i've counted all
                //O(MlogN)
 
                ElementCount result = CheckUniqueAndUnfoundElements(other, false);
                return (result.uniqueCount == Count && result.unfoundCount >= 0);
            }
        }
 
        private bool IsSubsetOfSortedSetWithSameEC(SortedSetCopyable<T> asSorted) {
            SortedSetCopyable<T> prunedOther = asSorted.GetViewBetween(this.Min, this.Max);
            foreach (T item in this) {
                if (!prunedOther.Contains(item))
                    return false;
            }
            return true;
 
        }
 
 
        /// <summary>
        /// Checks whether this Tree is a proper subset of the IEnumerable other
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [System.Security.SecuritySafeCritical]
        public bool IsProperSubsetOf(IEnumerable<T> other) {
            if (other == null) {
                throw new ArgumentNullException("other");
            }
 
            if ((other as ICollection) != null) {
                if (Count == 0)
                    return (other as ICollection).Count > 0;
            }
 
 
#if USING_HASH_SET
            //do it one way for HashSets
            HashSet<T> asHash = other as HashSet<T>;
            if (asHash != null && comparer.Equals(Comparer<T>.Default) && asHash.Comparer.Equals(EqualityComparer<T>.Default)) {
                return asHash.IsProperSupersetOf(this);
            }
#endif
            //another for sorted sets with the same comparer
            SortedSetCopyable<T> asSorted = other as SortedSetCopyable<T>;
            if (asSorted != null && AreComparersEqual(this, asSorted)) {
                if (this.Count >= asSorted.Count)
                    return false;
                return IsSubsetOfSortedSetWithSameEC(asSorted);
            }
 
 
            //worst case: mark every element in my set and see if i've counted all
            //O(MlogN).
            ElementCount result = CheckUniqueAndUnfoundElements(other, false);
            return (result.uniqueCount == Count && result.unfoundCount > 0);
        }
 
 
        /// <summary>
        /// Checks whether this Tree is a super set of the IEnumerable other
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsSupersetOf(IEnumerable<T> other) {
            if (other == null) {
                throw new ArgumentNullException("other");
            }
 
            if ((other as ICollection) != null && (other as ICollection).Count == 0)
                return true;
 
            //do it one way for HashSets
#if USING_HASH_SET
            HashSet<T> asHash = other as HashSet<T>;
            if (asHash != null && comparer.Equals(Comparer<T>.Default) && asHash.Comparer.Equals(EqualityComparer<T>.Default)) {
                return asHash.IsSubsetOf(this);
            }
#endif
            //another for sorted sets with the same comparer
            SortedSetCopyable<T> asSorted = other as SortedSetCopyable<T>;
            if (asSorted != null && AreComparersEqual(this, asSorted)) {
                if (this.Count < asSorted.Count)
                    return false;
                SortedSetCopyable<T> pruned = GetViewBetween(asSorted.Min, asSorted.Max);
                foreach (T item in asSorted) {
                    if (!pruned.Contains(item))
                        return false;
                }
                return true;
            }
            //and a third for everything else
            return ContainsAllElements(other);
        }
 
        /// <summary>
        /// Checks whether this Tree is a proper super set of the IEnumerable other
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [System.Security.SecuritySafeCritical]
        public bool IsProperSupersetOf(IEnumerable<T> other) {
            if (other == null) {
                throw new ArgumentNullException("other");
            }
 
            if (Count == 0)
                return false;
 
            if ((other as ICollection) != null && (other as ICollection).Count == 0)
                return true;
 
#if USING_HASH_SET
            //do it one way for HashSets
 
            HashSet<T> asHash = other as HashSet<T>;
            if (asHash != null && comparer.Equals(Comparer<T>.Default) && asHash.Comparer.Equals(EqualityComparer<T>.Default)) {
                return asHash.IsProperSubsetOf(this);
            }
#endif
            //another way for sorted sets
            SortedSetCopyable<T> asSorted = other as SortedSetCopyable<T>;
            if (asSorted != null && AreComparersEqual(asSorted, this)) {
                if (asSorted.Count >= this.Count)
                    return false;
                SortedSetCopyable<T> pruned = GetViewBetween(asSorted.Min, asSorted.Max);
                foreach (T item in asSorted) {
                    if (!pruned.Contains(item))
                        return false;
                }
                return true;
            }
 
 
            //worst case: mark every element in my set and see if i've counted all
            //O(MlogN)
            //slight optimization, put it into a HashSet and then check can do it in O(N+M)
            //but slower in better cases + wastes space
            ElementCount result = CheckUniqueAndUnfoundElements(other, true);
            return (result.uniqueCount < Count && result.unfoundCount == 0);
        }
 
 
 
        /// <summary>
        /// Checks whether this Tree has all elements in common with IEnumerable other
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        [System.Security.SecuritySafeCritical]
        public bool SetEquals(IEnumerable<T> other) {
            if (other == null) {
                throw new ArgumentNullException("other");
            }
 
#if USING_HASH_SET
            HashSet<T> asHash = other as HashSet<T>;
            if (asHash != null && comparer.Equals(Comparer<T>.Default) && asHash.Comparer.Equals(EqualityComparer<T>.Default)) {
                return asHash.SetEquals(this);
            }
#endif
            SortedSetCopyable<T> asSorted = other as SortedSetCopyable<T>;
            if (asSorted != null && AreComparersEqual(this, asSorted)) {
                IEnumerator<T> mine = this.GetEnumerator();
                IEnumerator<T> theirs = asSorted.GetEnumerator();
                bool mineEnded = !mine.MoveNext();
                bool theirsEnded = !theirs.MoveNext();
                while (!mineEnded && !theirsEnded) {
                    if (Comparer.Compare(mine.Current, theirs.Current) != 0) {
                        return false;
                    }
                    mineEnded = !mine.MoveNext();
                    theirsEnded = !theirs.MoveNext();
                }
                return mineEnded && theirsEnded;
            }
 
            //worst case: mark every element in my set and see if i've counted all
            //O(N) by size of other            
            ElementCount result = CheckUniqueAndUnfoundElements(other, true);
            return (result.uniqueCount == Count && result.unfoundCount == 0);
        }
 
 
 
        /// <summary>
        /// Checks whether this Tree has any elements in common with IEnumerable other
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Overlaps(IEnumerable<T> other) {
            if (other == null) {
                throw new ArgumentNullException("other");
            }
 
            if (this.Count == 0)
                return false;
 
            if ((other as ICollection<T> != null) && (other as ICollection<T>).Count == 0)
                return false;
 
            SortedSetCopyable<T> asSorted = other as SortedSetCopyable<T>;
            if (asSorted != null && AreComparersEqual(this, asSorted) && (comparer.Compare(Min, asSorted.Max) > 0 || comparer.Compare(Max, asSorted.Min) < 0)) {
                return false;
            }
#if USING_HASH_SET
            HashSet<T> asHash = other as HashSet<T>;
            if (asHash != null && comparer.Equals(Comparer<T>.Default) && asHash.Comparer.Equals(EqualityComparer<T>.Default)) {
                return asHash.Overlaps(this);
            }
#endif
            foreach (T item in other) {
                if (this.Contains(item)) {
                    return true;
                }
            }
            return false;
        }
 
        /// <summary>
        /// This works similar to HashSet's CheckUniqueAndUnfound (description below), except that the bit
        /// array maps differently than in the HashSet. We can only use this for the bulk boolean checks.
        /// 
        /// Determines counts that can be used to determine equality, subset, and superset. This
        /// is only used when other is an IEnumerable and not a HashSet. If other is a HashSet
        /// these properties can be checked faster without use of marking because we can assume 
        /// other has no duplicates.
        /// 
        /// The following count checks are performed by callers:
        /// 1. Equals: checks if unfoundCount = 0 and uniqueFoundCount = Count; i.e. everything 
        /// in other is in this and everything in this is in other
        /// 2. Subset: checks if unfoundCount >= 0 and uniqueFoundCount = Count; i.e. other may
        /// have elements not in this and everything in this is in other
        /// 3. Proper subset: checks if unfoundCount > 0 and uniqueFoundCount = Count; i.e
        /// other must have at least one element not in this and everything in this is in other
        /// 4. Proper superset: checks if unfound count = 0 and uniqueFoundCount strictly less
        /// than Count; i.e. everything in other was in this and this had at least one element
        /// not contained in other.
        /// 
        /// An earlier implementation used delegates to perform these checks rather than returning
        /// an ElementCount struct; however this was changed due to the perf overhead of delegates.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="returnIfUnfound">Allows us to finish faster for equals and proper superset
        /// because unfoundCount must be 0.</param>
        /// <returns></returns>
        // <SecurityKernel Critical="True" Ring="0">
        // <UsesUnsafeCode Name="Local bitArrayPtr of type: Int32*" />
        // <ReferencesCritical Name="Method: BitHelper..ctor(System.Int32*,System.Int32)" Ring="1" />
        // <ReferencesCritical Name="Method: BitHelper.IsMarked(System.Int32):System.Boolean" Ring="1" />
        // <ReferencesCritical Name="Method: BitHelper.MarkBit(System.Int32):System.Void" Ring="1" />
        // </SecurityKernel>
        [System.Security.SecurityCritical]
        private unsafe ElementCount CheckUniqueAndUnfoundElements(IEnumerable<T> other, bool returnIfUnfound) {
            ElementCount result;
 
            // need special case in case this has no elements. 
            if (Count == 0) {
                int numElementsInOther = 0;
                foreach (T item in other) {
                    numElementsInOther++;
                    // break right away, all we want to know is whether other has 0 or 1 elements
                    break;
                }
                result.uniqueCount = 0;
                result.unfoundCount = numElementsInOther;
                return result;
            }
 
 
            int originalLastIndex = Count;
            int intArrayLength = BitHelper.ToIntArrayLength(originalLastIndex);
 
            BitHelper bitHelper;
            if (intArrayLength <= StackAllocThreshold) {
                int* bitArrayPtr = stackalloc int[intArrayLength];
                bitHelper = new BitHelper(bitArrayPtr, intArrayLength);
            } else {
                int[] bitArray = new int[intArrayLength];
                bitHelper = new BitHelper(bitArray, intArrayLength);
            }
 
            // count of items in other not found in this
            int unfoundCount = 0;
            // count of unique items in other found in this
            int uniqueFoundCount = 0;
 
            foreach (T item in other) {
                int index = InternalIndexOf(item);
                if (index >= 0) {
                    if (!bitHelper.IsMarked(index)) {
                        // item hasn't been seen yet
                        bitHelper.MarkBit(index);
                        uniqueFoundCount++;
                    }
                } else {
                    unfoundCount++;
                    if (returnIfUnfound) {
                        break;
                    }
                }
            }
 
            result.uniqueCount = uniqueFoundCount;
            result.unfoundCount = unfoundCount;
            return result;
        }
        public int RemoveWhere(Predicate<T> match) {
            if (match == null) {
                throw new ArgumentNullException("match");
            }
            List<T> matches = new List<T>(this.Count);
 
            BreadthFirstTreeWalk(delegate(Node n) {
                if (match(n.Item)) {
                    matches.Add(n.Item);
                }
                return true;
            });
            // reverse breadth first to (try to) incur low cost
            int actuallyRemoved = 0;
            for (int i = matches.Count - 1; i >= 0; i--) {
                if (this.Remove(matches[i])) {
                    actuallyRemoved++;
                }
            }
 
            return actuallyRemoved;
 
        }
 
 
        #endregion
 
        #region ISorted Members
 
 
        public T Min {
            get {
                T ret = default(T);
                InOrderTreeWalk(delegate(SortedSetCopyable<T>.Node n) { ret = n.Item; return false; });
                return ret;
            }
        }
 
        public T Max {
            get {
                T ret = default(T);
                InOrderTreeWalk(delegate(SortedSetCopyable<T>.Node n) { ret = n.Item; return false; }, true);
                return ret;
            }
        }
 
        public IEnumerable<T> Reverse() {
            Enumerator e = new Enumerator(this, true);
            while (e.MoveNext()) {
                yield return e.Current;
            }
        }
 
 
        /// <summary>
        /// Returns a subset of this tree ranging from values lBound to uBound
        /// Any changes made to the subset reflect in the actual tree
        /// </summary>
        /// <param name="lowVestalue">Lowest Value allowed in the subset</param>
        /// <param name="highestValue">Highest Value allowed in the subset</param>        
        public virtual SortedSetCopyable<T> GetViewBetween(T lowerValue, T upperValue) {
            if (Comparer.Compare(lowerValue, upperValue) > 0) {
                throw new ArgumentException("lowerBound is greater than upperBound");
            }
            return new TreeSubSet(this, lowerValue, upperValue, true, true);
        }
 
#if DEBUG

        /// <summary>
        /// debug status to be checked whenever any operation is called
        /// </summary>
        /// <returns></returns>
        internal virtual bool versionUpToDate() {
            return true;
        }
#endif
 
 
        /// <summary>
        /// This class represents a subset view into the tree. Any changes to this view
        /// are reflected in the actual tree. Uses the Comparator of the underlying tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>   
#if !FEATURE_NETCORE
        [Serializable]
        internal sealed class TreeSubSet : SortedSetCopyable<T>, ISerializable, IDeserializationCallback {
#else
        internal sealed class TreeSubSet : SortedSetCopyable<T> {
#endif
            SortedSetCopyable<T> underlying;
            T min, max;
            //these exist for unbounded collections
            //for instance, you could allow this subset to be defined for i>10. The set will throw if
            //anything <=10 is added, but there is no upperbound. These features Head(), Tail(), were punted
            //in the spec, and are not available, but the framework is there to make them available at some point.
            bool lBoundActive, uBoundActive;
            //used to see if the count is out of date            
 
 
#if DEBUG
            internal override bool versionUpToDate() {
                return (this.version == underlying.version);
            }
#endif
 
            public TreeSubSet(SortedSetCopyable<T> Underlying, T Min, T Max, bool lowerBoundActive, bool upperBoundActive)
                : base(Underlying.Comparer) {
                underlying = Underlying;
                min = Min;
                max = Max;
                lBoundActive = lowerBoundActive;
                uBoundActive = upperBoundActive;
                root = underlying.FindRange(min, max, lBoundActive, uBoundActive); // root is first element within range                                
                count = 0;
                version = -1;
                VersionCheckImpl();
            }
 
#if !FEATURE_NETCORE
            /// <summary>
            /// For serialization and deserialization
            /// </summary>
            private TreeSubSet() {
                comparer = null;
            }
 
 
            [SuppressMessage("Microsoft.Usage", "CA2236:CallBaseClassMethodsOnISerializableTypes", Justification = "special case TreeSubSet serialization")]
            private TreeSubSet(SerializationInfo info, StreamingContext context) {
                siInfo = info;
                OnDeserializationImpl(info);
            }
#endif // !FEATURE_NETCORE
 
            /// <summary>
            /// Additions to this tree need to be added to the underlying tree as well
            /// </summary>           
 
            internal override bool AddIfNotPresent(T item) {
 
                bool ret = underlying.AddIfNotPresent(item);
                VersionCheck();
#if DEBUG
                Debug.Assert(this.versionUpToDate() && this.root == this.underlying.FindRange(min, max));
#endif
 
                return ret;
            }
 
 
            public override bool Contains(T item) {
                VersionCheck();
#if DEBUG
                Debug.Assert(this.versionUpToDate() && this.root == this.underlying.FindRange(min, max));
#endif
                return base.Contains(item);
            }
 
            internal override bool DoRemove(T item) { // todo: uppercase this and others
 
                if (!IsWithinRange(item)) {
                    return false;
                }
 
                bool ret = underlying.Remove(item);
                VersionCheck();
#if DEBUG
                Debug.Assert(this.versionUpToDate() && this.root == this.underlying.FindRange(min, max));
#endif
                return ret;
            }
 
            public override void Clear() {
 
 
                if (count == 0) {
                    return;
                }
 
                List<T> toRemove = new List<T>();
                BreadthFirstTreeWalk(delegate(Node n) { toRemove.Add(n.Item); return true; });
                while (toRemove.Count != 0) {
                    underlying.Remove(toRemove[toRemove.Count - 1]);
                    toRemove.RemoveAt(toRemove.Count - 1);
                }
                root = null;
                count = 0;
                version = underlying.version;
            }
 
 
            internal override bool IsWithinRange(T item) {
 
                int comp = (lBoundActive ? Comparer.Compare(min, item) : -1);
                if (comp > 0) {
                    return false;
                }
                comp = (uBoundActive ? Comparer.Compare(max, item) : 1);
                if (comp < 0) {
                    return false;
                }
                return true;
            }
 
            internal override bool InOrderTreeWalk(TreeWalkPredicate<T> action, Boolean reverse) {
                VersionCheck();
 
                if (root == null) {
                    return true;
                }
 
                // The maximum height of a red-black tree is 2*lg(n+1).
                // See page 264 of "Introduction to algorithms" by Thomas H. Cormen
                Stack<Node> stack = new Stack<Node>(2 * (int)SortedSetCopyable<T>.log2(count + 1)); //this is not exactly right if count is out of date, but the stack can grow
                Node current = root;
                while (current != null) {
                    if (IsWithinRange(current.Item)) {
                        stack.Push(current);
                        current = (reverse ? current.Right : current.Left);
                    } else if (lBoundActive && Comparer.Compare(min, current.Item) > 0) {
                        current = current.Right;
                    } else {
                        current = current.Left;
                    }
                }
 
                while (stack.Count != 0) {
                    current = stack.Pop();
                    if (!action(current)) {
                        return false;
                    }
 
                    Node node = (reverse ? current.Left : current.Right);
                    while (node != null) {
                        if (IsWithinRange(node.Item)) {
                            stack.Push(node);
                            node = (reverse ? node.Right : node.Left);
                        } else if (lBoundActive && Comparer.Compare(min, node.Item) > 0) {
                            node = node.Right;
                        } else {
                            node = node.Left;
                        }
                    }
                }
                return true;
            }
 
            internal override bool BreadthFirstTreeWalk(TreeWalkPredicate<T> action) {
                VersionCheck();
 
                if (root == null) {
                    return true;
                }
 
                List<Node> processQueue = new List<Node>();
                processQueue.Add(root);
                Node current;
 
                while (processQueue.Count != 0) {
                    current = processQueue[0];
                    processQueue.RemoveAt(0);
                    if (IsWithinRange(current.Item) && !action(current)) {
                        return false;
                    }
                    if (current.Left != null && (!lBoundActive || Comparer.Compare(min, current.Item) < 0)) {
                        processQueue.Add(current.Left);
                    }
                    if (current.Right != null && (!uBoundActive || Comparer.Compare(max, current.Item) > 0)) {
                        processQueue.Add(current.Right);
                    }
 
                }
                return true;
            }
 
            internal override SortedSetCopyable<T>.Node FindNode(T item) {
 
                if (!IsWithinRange(item)) {
                    return null;
                }
                VersionCheck();
#if DEBUG
                Debug.Assert(this.versionUpToDate() && this.root == this.underlying.FindRange(min, max));
#endif
                return base.FindNode(item);
            }
 
            //this does indexing in an inefficient way compared to the actual sortedset, but it saves a
            //lot of space
            internal override int InternalIndexOf(T item) {
                int count = -1;
                foreach (T i in this) {
                    count++;
                    if (Comparer.Compare(item, i) == 0)
                        return count;
                }
#if DEBUG
                Debug.Assert(this.versionUpToDate() && this.root == this.underlying.FindRange(min, max));
#endif
                return -1;
            }
            /// <summary>
            /// checks whether this subset is out of date. updates if necessary.
            /// </summary>
            internal override void VersionCheck() {
                VersionCheckImpl();
            }
 
            private void VersionCheckImpl() {
                Debug.Assert(underlying != null, "Underlying set no longer exists");
                if (this.version != underlying.version) {
                    this.root = underlying.FindRange(min, max, lBoundActive, uBoundActive);
                    this.version = underlying.version;
                    count = 0;
                    InOrderTreeWalk(delegate(Node n) { count++; return true; });
                }
            }
 
 
 
            //This passes functionality down to the underlying tree, clipping edges if necessary
            //There's nothing gained by having a nested subset. May as well draw it from the base
            //Cannot increase the bounds of the subset, can only decrease it
            public override SortedSetCopyable<T> GetViewBetween(T lowerValue, T upperValue) {
 
                if (lBoundActive && Comparer.Compare(min, lowerValue) > 0) {
                    //lBound = min;
                    throw new ArgumentOutOfRangeException("lowerValue");
                }
                if (uBoundActive && Comparer.Compare(max, upperValue) < 0) {
                    //uBound = max;
                    throw new ArgumentOutOfRangeException("upperValue");
                }
                TreeSubSet ret = (TreeSubSet)underlying.GetViewBetween(lowerValue, upperValue);
                return ret;
            }
 
            internal override void IntersectWithEnumerable(IEnumerable<T> other) {
 
                List<T> toSave = new List<T>(this.Count);
                foreach (T item in other) {
                    if (this.Contains(item)) {
                        toSave.Add(item);
                        this.Remove(item);
                    }
                }
                this.Clear();
                this.AddAllElements(toSave);
#if DEBUG
                Debug.Assert(this.versionUpToDate() && this.root == this.underlying.FindRange(min, max));
#endif
            }
 
#if !FEATURE_NETCORE
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
                GetObjectData(info, context);
            }
 
            protected override void GetObjectData(SerializationInfo info, StreamingContext context) {
                info.AddValue(maxName, max, typeof(T));
                info.AddValue(minName, min, typeof(T));
                info.AddValue(lBoundActiveName, lBoundActive);
                info.AddValue(uBoundActiveName, uBoundActive);
                base.GetObjectData(info, context);
            }
 
            void IDeserializationCallback.OnDeserialization(Object sender) {
                //don't do anything here as its already been done by the constructor
                //OnDeserialization(sender);
 
            }
 
            protected override void OnDeserialization(Object sender) {
                OnDeserializationImpl(sender);
            }
 
            private void OnDeserializationImpl(Object sender) {
                comparer = (IComparer<T>)siInfo.GetValue(ComparerName, typeof(IComparer<T>));
                int savedCount = siInfo.GetInt32(CountName);
                max = (T)siInfo.GetValue(maxName, typeof(T));
                min = (T)siInfo.GetValue(minName, typeof(T));
                lBoundActive = siInfo.GetBoolean(lBoundActiveName);
                uBoundActive = siInfo.GetBoolean(uBoundActiveName);
                underlying = new SortedSetCopyable<T>();
 
                if (savedCount != 0) {
                    T[] items = (T[])siInfo.GetValue(ItemsName, typeof(T[]));
 
                    for (int i = 0; i < items.Length; i++) {
                        underlying.Add(items[i]);
                    }
                }
                underlying.version = siInfo.GetInt32(VersionName);
                count = underlying.count;
                version = underlying.version - 1;
                VersionCheck(); //this should update the count to be right and update root to be right
 
                siInfo = null;
 
            }
#endif // !FEATURE_NETCORE
 
 
 
 
        }
 
 
        #endregion
 
        #region Serialization methods
 
#if !FEATURE_NETCORE
        // LinkDemand here is unnecessary as this is a methodimpl and linkdemand from the interface should suffice
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            GetObjectData(info, context);
        }
 
        protected virtual void GetObjectData(SerializationInfo info, StreamingContext context) {
            
            info.AddValue(CountName, count); //This is the length of the bucket array.
            info.AddValue(ComparerName, comparer, typeof(IComparer<T>));
            info.AddValue(VersionName, version);
            
            if (root != null) {
                T[] items = new T[Count];
                CopyTo(items, 0);
                info.AddValue(ItemsName, items, typeof(T[]));
            }
        }
 
        void IDeserializationCallback.OnDeserialization(Object sender) {
            OnDeserialization(sender);
        }
 
        protected virtual void OnDeserialization(Object sender) {
            if (comparer != null) {
                return; //Somebody had a dependency on this class and fixed us up before the ObjectManager got to it.
            }
 
            comparer = (IComparer<T>)siInfo.GetValue(ComparerName, typeof(IComparer<T>));
            int savedCount = siInfo.GetInt32(CountName);
 
            if (savedCount != 0) {
                T[] items = (T[])siInfo.GetValue(ItemsName, typeof(T[]));
 
                for (int i = 0; i < items.Length; i++) {
                    Add(items[i]);
                }
            }
 
            version = siInfo.GetInt32(VersionName);
            siInfo = null;
        }
#endif //!FEATURE_NETCORE
        #endregion
 
        #region Helper Classes
        internal class Node : IPoolableRecycle {
            public bool IsRed;
            public T Item;
            public Node Left;
            public Node Right;

            public static Node Spawn(T item) {

                var instance = PoolClass<Node>.Spawn();
                instance.Item = item;
                instance.IsRed = true;
                return instance;
                
            }

            public static Node Spawn(T item, bool isRed) {

                var instance = PoolClass<Node>.Spawn();
                instance.Item = item;
                instance.IsRed = isRed;
                return instance;
                
            }

            public void OnRecycle() {

                this.IsRed = default;
                this.Item = default;
                this.Left = default;
                this.Right = default;
                
            }

            public Node() {}
            
            public Node(T item) {
                // The default color will be red, we never need to create a black node directly.                
                this.Item = item;
                IsRed = true;
            }
 
            public Node(T item, bool isRed) {
                // The default color will be red, we never need to create a black node directly.                
                this.Item = item;
                this.IsRed = isRed;
            }
        }
 
        [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes", Justification = "not an expected scenario")]
#if !FEATURE_NETCORE
        [Serializable]
        public struct Enumerator : IEnumerator<T>, IEnumerator, ISerializable, IDeserializationCallback {
#else
        public struct Enumerator : IEnumerator<T>, IEnumerator {
#endif
            private SortedSetCopyable<T> tree;
            private int version;
 
 
            private Stack<SortedSetCopyable<T>.Node> stack;
            private SortedSetCopyable<T>.Node current;
            static SortedSetCopyable<T>.Node dummyNode = new SortedSetCopyable<T>.Node(default(T));
 
            private bool reverse;
 
#if !FEATURE_NETCORE
            private SerializationInfo siInfo;
#endif
            internal Enumerator(SortedSetCopyable<T> set) {
                tree = set;
                //this is a hack to make sure that the underlying subset has not been changed since
                //
                tree.VersionCheck();
 
                version = tree.version;
 
                // 2lg(n + 1) is the maximum height
                stack = PoolStack<SortedSetCopyable<T>.Node>.Spawn(2 * (int)SortedSetCopyable<T>.log2(set.Count + 1));
                current = null;
                reverse = false;
#if !FEATURE_NETCORE
                siInfo = null;
#endif
                Intialize();
            }
 
            internal Enumerator(SortedSetCopyable<T> set, bool reverse) {
                tree = set;
                //this is a hack to make sure that the underlying subset has not been changed since
                //
                tree.VersionCheck();
                version = tree.version;
 
                // 2lg(n + 1) is the maximum height
                stack = PoolStack<SortedSetCopyable<T>.Node>.Spawn(2 * (int)SortedSetCopyable<T>.log2(set.Count + 1));
                current = null;
                this.reverse = reverse;
#if !FEATURE_NETCORE
                siInfo = null;
#endif
                Intialize();
 
            }
 
#if !FEATURE_NETCORE
            private Enumerator(SerializationInfo info, StreamingContext context) {
                tree = null;
                version = -1;
                current = null;
                reverse = false;
                stack = null;
                this.siInfo = info;
            }
            
            void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
                GetObjectData(info, context);
            }
 
            private void GetObjectData(SerializationInfo info, StreamingContext context) {
                info.AddValue(TreeName, tree, typeof(SortedSetCopyable<T>));
                info.AddValue(EnumVersionName, version);
                info.AddValue(ReverseName, reverse);
                info.AddValue(EnumStartName, !NotStartedOrEnded);
                info.AddValue(NodeValueName, (current == null ? dummyNode.Item : current.Item), typeof(T));
            }
 
            void IDeserializationCallback.OnDeserialization(Object sender) {
                OnDeserialization(sender);
            }
 
            private void OnDeserialization(Object sender) {
                tree = (SortedSetCopyable<T>)siInfo.GetValue(TreeName, typeof(SortedSetCopyable<T>));
                version = siInfo.GetInt32(EnumVersionName);
                reverse = siInfo.GetBoolean(ReverseName);
                bool EnumStarted = siInfo.GetBoolean(EnumStartName);
                stack = new Stack<SortedSetCopyable<T>.Node>(2 * (int)SortedSetCopyable<T>.log2(tree.Count + 1));
                current = null;
                if (EnumStarted) {
                    T item = (T)siInfo.GetValue(NodeValueName, typeof(T));
                    Intialize();
                    //go until it reaches the value we want
                    while (this.MoveNext()) {
                        if (tree.Comparer.Compare(this.Current, item) == 0)
                            break;
                    }
                }
 
 
            }
#endif //!FEATURE_NETCORE
 
 
            private void Intialize() {
 
                current = null;
                SortedSetCopyable<T>.Node node = tree.root;
                Node next = null, other = null;
                while (node != null) {
                    next = (reverse ? node.Right : node.Left);
                    other = (reverse ? node.Left : node.Right);
                    if (tree.IsWithinRange(node.Item)) {
                        stack.Push(node);
                        node = next;
                    } else if (next == null || !tree.IsWithinRange(next.Item)) {
                        node = other;
                    } else {
                        node = next;
                    }
                }
            }
 
            public bool MoveNext() {
 
                //this is a hack to make sure that the underlying subset has not been changed since
                //
                tree.VersionCheck();
 
                if (stack.Count == 0) {
                    current = null;
                    return false;
                }
 
                current = stack.Pop();
                SortedSetCopyable<T>.Node node = (reverse ? current.Left : current.Right);
                Node next = null, other = null;
                while (node != null) {
                    next = (reverse ? node.Right : node.Left);
                    other = (reverse ? node.Left : node.Right);
                    if (tree.IsWithinRange(node.Item)) {
                        stack.Push(node);
                        node = next;
                    } else if (other == null || !tree.IsWithinRange(other.Item)) {
                        node = next;
                    } else {
                        node = other;
                    }
                }
                return true;
            }
 
            public void Dispose() {
                
                PoolStack<Node>.Recycle(ref stack);
                
            }
 
            public T Current {
                get {
                    if (current != null) {
                        return current.Item;
                    }
                    return default(T);
                }
            }
 
            object IEnumerator.Current {
                get {
                    return current.Item;
                }
            }
 
            internal bool NotStartedOrEnded {
                get {
                    return current == null;
                }
            }
 
            internal void Reset() {
                stack.Clear();
                Intialize();
            }
 
            void IEnumerator.Reset() {
                Reset();
            }
 
 
 
 
        }
 
 
 
        internal struct ElementCount {
            internal int uniqueCount;
            internal int unfoundCount;
        }
        #endregion
 
        #region misc
 
        /// <summary>
        /// Searches the set for a given value and returns the equal value it finds, if any.
        /// </summary>
        /// <param name="equalValue">The value to search for.</param>
        /// <param name="actualValue">The value from the set that the search found, or the default value of <typeparamref name="T"/> when the search yielded no match.</param>
        /// <returns>A value indicating whether the search was successful.</returns>
        /// <remarks>
        /// This can be useful when you want to reuse a previously stored reference instead of 
        /// a newly constructed one (so that more sharing of references can occur) or to look up
        /// a value that has more complete data than the value you currently have, although their
        /// comparer functions indicate they are equal.
        /// </remarks>
        public bool TryGetValue(T equalValue, out T actualValue) {
            Node node = FindNode(equalValue);
            if (node != null) {
                actualValue = node.Item;
                return true;
            }
            actualValue = default(T);
            return false;
        }
 
        // used for set checking operations (using enumerables) that rely on counting
        private static int log2(int value) {
            //Contract.Requires(value>0)
            int c = 0;
            while (value > 0) {
                c++;
                value >>= 1;
            }
            return c;
        }
        #endregion
 
 
    }
 
    /// <summary>
    /// A class that generates an IEqualityComparer for this SortedSet. Requires that the definition of
    /// equality defined by the IComparer for this SortedSet be consistent with the default IEqualityComparer
    /// for the type T. If not, such an IEqualityComparer should be provided through the constructor.
    /// </summary>    
    internal class SortedSetEqualityComparer<T> : IEqualityComparer<SortedSetCopyable<T>> {
        private IComparer<T> comparer;
        private IEqualityComparer<T> e_comparer;
 
        public SortedSetEqualityComparer() : this(null, null) { }
 
        public SortedSetEqualityComparer(IComparer<T> comparer) : this(comparer, null) { }
 
        public SortedSetEqualityComparer(IEqualityComparer<T> memberEqualityComparer) : this(null, memberEqualityComparer) { }
 
        /// <summary>
        /// Create a new SetEqualityComparer, given a comparer for member order and another for member equality (these
        /// must be consistent in their definition of equality)
        /// </summary>        
        public SortedSetEqualityComparer(IComparer<T> comparer, IEqualityComparer<T> memberEqualityComparer) {
            if (comparer == null)
                this.comparer = Comparer<T>.Default;
            else
                this.comparer = comparer;
            if (memberEqualityComparer == null)
                e_comparer = EqualityComparer<T>.Default;
            else
                e_comparer = memberEqualityComparer;
        }
 
 
        // using comparer to keep equals properties in tact; don't want to choose one of the comparers
        public bool Equals(SortedSetCopyable<T> x, SortedSetCopyable<T> y) {
            return SortedSetCopyable<T>.SortedSetEquals(x, y, comparer);
        }
        //IMPORTANT: this part uses the fact that GetHashCode() is consistent with the notion of equality in
        //the set
        public int GetHashCode(SortedSetCopyable<T> obj) {
            int hashCode = 0;
            if (obj != null) {
                foreach (T t in obj) {
                    hashCode = hashCode ^ (e_comparer.GetHashCode(t) & 0x7FFFFFFF);
                }
            } // else returns hashcode of 0 for null HashSets
            return hashCode;
        }
 
        // Equals method for the comparer itself. 
        public override bool Equals(Object obj) {
            SortedSetEqualityComparer<T> comparer = obj as SortedSetEqualityComparer<T>;
            if (comparer == null) {
                return false;
            }
            return (this.comparer == comparer.comparer);
        }
 
        public override int GetHashCode() {
            return comparer.GetHashCode() ^ e_comparer.GetHashCode();
        }
 
 
    }
 
}