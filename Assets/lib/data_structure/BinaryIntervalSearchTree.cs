using System;
using System.Collections;
using System.Collections.Generic;

namespace Sesim.DataStructures
{
    /// <summary>
    /// An implementation of Red-Black tree for finding which interval a point lives in.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    public class BinaryIntervalSearchTree<TKey, TVal> where TKey : IComparable<TKey>
    {
        BistNode<TKey, TVal> root;

        public TVal this[TKey key] { get => root.Find(key).value.Value; }

        public void Add(TKey key, TVal val) { throw new NotImplementedException(); }
    }

    /// <summary>
    /// A node in a Binary Interval Search Tree, which implements Red-Black tree
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TVal"></typeparam>
    public class BistNode<TKey, TVal> where TKey : IComparable<TKey>
    {
        public bool isRed;
        public KeyValuePair<TKey, TVal> value;
        public BistNode<TKey, TVal> parent;
        public BistNode<TKey, TVal> left;
        public BistNode<TKey, TVal> right;

        public BistNode(TKey key, TVal value)
        : this(new KeyValuePair<TKey, TVal>(key, value))
        { }

        public BistNode<TKey, TVal> sibling
        {
            get => (this.parent.left == this) ? this.parent.right : this.parent.left;
        }

        public bool isLeftSibling { get => this.parent.left == this; }

        public BistNode(KeyValuePair<TKey, TVal> kvp)
        {
            this.value = kvp;
            this.isRed = true;
            this.parent = null;
            this.left = null;
            this.right = null;
        }

        public BistNode<TKey, TVal> Find(TKey key)
        {
            if (key.CompareTo(this.value.Key) == 0)
                return this;
            else if (this.left != null && key.CompareTo(this.value.Key) < 0)
                return this.left.Find(key);
            else if (this.right != null)
                return this.right.Find(key);
            else return null;
        }

        public void Add(BistNode<TKey, TVal> node)
        {

        }

        protected void RawAdd(BistNode<TKey, TVal> node)
        {
            node.parent = this;
            if (node.value.Key.CompareTo(this.value.Key) <= 0)
                this.left = node;
            else
                this.right = node;

            node.InsertEnsureProperty();
        }

        public void InsertEnsureProperty()
        {
            if (this.parent == null)
                // case 1
                this.isRed = false;
            else if (!this.parent.isRed)
            {
                var uncle = this.parent.sibling;
                if (uncle != null && uncle.isRed == true)
                {
                    // case 3
                    this.parent.isRed = false;
                    uncle.isRed = false;
                    this.parent.parent.isRed = true;
                    this.parent.parent.InsertEnsureProperty();
                }
                else
                {
                    if (isLeftSibling && !parent.isLeftSibling)
                        this.parent.RotateRight();
                    else if (!isLeftSibling && parent.isLeftSibling)
                        this.parent.RotateLeft();
                    this.parent.isRed = false;
                    this.parent.parent.isRed = true;
                    if (isLeftSibling)
                        this.parent.parent.RotateRight();
                    else
                        this.parent.parent.RotateLeft();
                }
            }
        }

        /// <summary>
        /// Rotate right with **this.left** being pivot
        /// </summary>
        public void RotateRight()
        {
            var parent = this.parent;
            var ils = this.isLeftSibling;
            var origLeftSibling = this.left;
            this.left = this.left.right;
            this.left.parent = this;
            origLeftSibling.right = this;
            this.parent = origLeftSibling;
            origLeftSibling.parent = parent;
            if (ils) parent.left = origLeftSibling;
            else parent.right = origLeftSibling;
        }

        /// <summary>
        /// Rotate left with **this.right** being pivot
        /// </summary>
        public void RotateLeft()
        {
            var parent = this.parent;
            var ils = this.isLeftSibling;
            var origRightSibling = this.right;
            this.right = this.right.left;
            this.right.parent = this;
            origRightSibling.left = this;
            this.parent = origRightSibling;
            origRightSibling.parent = parent;
            if (ils) parent.left = origRightSibling;
            else parent.right = origRightSibling;
        }

        public static implicit operator KeyValuePair<TKey, TVal>(BistNode<TKey, TVal> n)
        {
            return n.value;
        }

        public static implicit operator BistNode<TKey, TVal>(KeyValuePair<TKey, TVal> kvp)
        {
            return new BistNode<TKey, TVal>(kvp);
        }
    }
}
