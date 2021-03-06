﻿using System;
using System.Collections.Generic;
using CustomGenerics.Interfaces;
using System.Collections;

namespace CustomGenerics.Structures
{
    public class PriorityQueue<T> : IPriorityQueue<T>, IEnumerable<T> {

        //Priority Queue's methods
        public void EnqueuePatient(T value, Comparison<T> comparison, Comparison<T> comparisonHour) {
            Enqueue(value, comparison, comparisonHour);
        }
        public T DequeuePatient(Comparison<T> comparison, Comparison<T> comparisonHour) {
            return Dequeue(comparison, comparisonHour);
        }
        public T PeekPatient() {
            return peek();
        }
        protected override T Dequeue(Comparison<T> comparison, Comparison<T> comparisonHour) {
            if (root.valueNode != null) {
                T dequeueNode = root.valueNode;
                DeleteLastNode(root, level());
                DownChange(root, comparison, comparisonHour);
                return dequeueNode;
            }
            return default;

        }
        protected override void Enqueue(T value, Comparison<T> comparison, Comparison<T> comparisonHour) {
            AddNode(root, value, comparison, comparisonHour, level());
            levelCompleted = 0;
        }

        protected override T peek() {
            return root.valueNode;
        }

        Node<T> root = new Node<T>();
        public int levelCompleted = 0;
        public int numberNodes = 0;

        //Internal class Node
        public class Node<T> {
            public Node<T> leftNode;
            public Node<T> rightNode;
            public T valueNode;
            public int nodes;
            public int position = 0;
            public Node() { }
            public Node(T value) {
                this.valueNode = value;
                this.rightNode = null;
                this.leftNode = null;
                this.position = nodes;
            }
        }

        //Add element in the priority queue
        public void AddNode(Node<T> root, T value, Comparison<T> ComparePriority, Comparison<T> CompareHour, int initialLevel) {
            if (root.valueNode == null) {
                root.valueNode = value;
                root.leftNode = new Node<T>();
                root.rightNode = new Node<T>();
                numberNodes++;
                root.position = (numberNodes);
            } 
            else if (root.leftNode.valueNode == null && root.rightNode.valueNode == null) {
                AddNode(root.leftNode, value, ComparePriority, CompareHour, initialLevel);
            }
            else if (root.leftNode.valueNode != null && root.rightNode.valueNode == null) {
                AddNode(root.rightNode, value, ComparePriority, CompareHour, initialLevel);
            }
            else {
                if (initialLevel > 1) {
                    if (((numberNodes + 1) / Convert.ToInt32(Math.Pow(2, (initialLevel - 1))) % 2 == 0)) {
                        AddNode(root.leftNode, value, ComparePriority, CompareHour, --initialLevel);
                    }
                    else if (((numberNodes + 1) / Convert.ToInt32(Math.Pow(2, (initialLevel - 1))) % 2 == 1)) {
                        AddNode(root.rightNode, value, ComparePriority, CompareHour, --initialLevel);
                    }
                } else if (nodeHasChild(root) == 1) {
                    if (root.rightNode.valueNode == null) {
                        AddNode(root.leftNode, value, ComparePriority, CompareHour, initialLevel);
                    } else {
                        AddNode(root.rightNode, value, ComparePriority, CompareHour, initialLevel);
                    }
                }
            }
            if (root.leftNode.valueNode != null || root.rightNode.valueNode != null) {
                T val = root.valueNode;
                UpChange(root, val, ComparePriority, CompareHour);
            }
        }
        //Delete the element with most priority
        public void DeleteLastNode(Node<T> lastNode, int initialLevel) {
            if (lastNode.position == numberNodes) {
                if (numberNodes == 1) {
                    root = new Node<T>();
                } else {
                    root.valueNode = lastNode.valueNode;
                    numberNodes--;
                }
            }
            else if ((numberNodes / Convert.ToInt32(Math.Pow(2, (initialLevel - 1)))) % 2 == 0) {
                if (lastNode.leftNode.position == numberNodes) {
                    numberNodes--;
                    root.valueNode = lastNode.leftNode.valueNode;
                    lastNode.leftNode = new Node<T>();
                }
                else {
                    --initialLevel;
                    DeleteLastNode(lastNode.leftNode, initialLevel);
                }
            }
            else if ((numberNodes / Convert.ToInt32(Math.Pow(2, (initialLevel - 1)))) % 2 == 1) {
                if (lastNode.rightNode.position == numberNodes) {
                    numberNodes--;
                    root.valueNode = lastNode.rightNode.valueNode;
                    lastNode.rightNode = new Node<T>();
                } else {
                    --initialLevel;
                    DeleteLastNode(lastNode.rightNode, initialLevel);
                }
            }
        }
        //Verify if the node has children
        public int nodeHasChild(Node<T> node) {
            if (node.leftNode.valueNode != null && node.rightNode.valueNode != null) {
                return 1;
            } else {
                return 0;
            }
        }
        //Level of the last element inserted
        public int level() {
            int total = Convert.ToInt32(Math.Pow(2, levelCompleted));
            while (numberNodes >= total && numberNodes >= (total + Convert.ToInt32(Math.Pow(2, levelCompleted + 1)))) {
                total += Convert.ToInt32(Math.Pow(2, levelCompleted + 1));
                levelCompleted++;
            }
            return (levelCompleted + 1);
        }
        //Upward ordering for insertion
        public void UpChange(Node<T> nodeChange, T original, Comparison<T> ComparePriority, Comparison<T> CompareHour) {
            if (nodeHasChild(nodeChange) == 1) {
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) == 0 &&
                    CompareHour.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) > 0) {
                    nodeChange.valueNode = nodeChange.leftNode.valueNode;
                    nodeChange.leftNode.valueNode = original;
                }
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.rightNode.valueNode) == 0 &&
                    CompareHour.Invoke(nodeChange.valueNode, nodeChange.rightNode.valueNode) > 0) {
                    nodeChange.valueNode = nodeChange.rightNode.valueNode;
                    nodeChange.rightNode.valueNode = original;
                }
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) > 0) {
                    nodeChange.valueNode = nodeChange.leftNode.valueNode;
                    nodeChange.leftNode.valueNode = original;
                }
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.rightNode.valueNode) > 0) {
                    nodeChange.valueNode = nodeChange.rightNode.valueNode;
                    nodeChange.rightNode.valueNode = original;
                }
            }
            else if (nodeChange.rightNode.valueNode == null) {
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) == 0 &&
                    CompareHour.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) > 0) {
                    nodeChange.valueNode = nodeChange.leftNode.valueNode;
                    nodeChange.leftNode.valueNode = original;
                }
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) > 0) {
                    nodeChange.valueNode = nodeChange.leftNode.valueNode;
                    nodeChange.leftNode.valueNode = original;
                }
            }
        }
        //Downward ordering for delete
        public void DownChange(Node<T> nodeChange, Comparison<T> ComparePriority, Comparison<T> CompareHour) {
            if (nodeHasChild(nodeChange) == 1) {
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.rightNode.valueNode) == 0 &&
                    CompareHour.Invoke(nodeChange.valueNode, nodeChange.rightNode.valueNode) > 0) {
                    T auxValue = nodeChange.valueNode;
                    nodeChange.valueNode = nodeChange.rightNode.valueNode;
                    nodeChange.rightNode.valueNode = auxValue;
                    DownChange(nodeChange.rightNode, ComparePriority, CompareHour);
                }
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) == 0 &&
                    CompareHour.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) > 0) {
                    T auxValue = nodeChange.valueNode;
                    nodeChange.valueNode = nodeChange.leftNode.valueNode;
                    nodeChange.leftNode.valueNode = auxValue;
                    DownChange(nodeChange.leftNode, ComparePriority, CompareHour);
                }
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.rightNode.valueNode) > 0) {
                    T auxValue = nodeChange.valueNode;
                    nodeChange.valueNode = nodeChange.rightNode.valueNode;
                    nodeChange.rightNode.valueNode = auxValue;
                    DownChange(nodeChange.rightNode, ComparePriority, CompareHour);
                }
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) > 0) {
                    T auxValue = nodeChange.valueNode;
                    nodeChange.valueNode = nodeChange.leftNode.valueNode;
                    nodeChange.leftNode.valueNode = auxValue;
                    DownChange(nodeChange.leftNode, ComparePriority, CompareHour);
                }
            }
            else if (nodeChange.rightNode.valueNode == null && nodeChange.leftNode.valueNode != null) {
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) == 0 &&
                    CompareHour.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) > 0) {
                    T auxValue = nodeChange.valueNode;
                    nodeChange.valueNode = nodeChange.leftNode.valueNode;
                    nodeChange.leftNode.valueNode = auxValue;
                    DownChange(nodeChange.leftNode, ComparePriority, CompareHour);
                }
                if (ComparePriority.Invoke(nodeChange.valueNode, nodeChange.leftNode.valueNode) > 0) {
                    T auxValue = nodeChange.valueNode;
                    nodeChange.valueNode = nodeChange.leftNode.valueNode;
                    nodeChange.leftNode.valueNode = auxValue;
                    DownChange(nodeChange.leftNode, ComparePriority, CompareHour);
                }
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
