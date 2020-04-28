using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Structures {
    public class Node <T> {

        private Node<T> leftNode, rightNode;
        private int height, bf;
        private T valueNode;

        public Node<T> addNodeElement(Node<T> root, Node<T> newNode, Comparison<T> comparison)
        {
            if (root == null)
            {
                newNode.bf = 0;
                newNode.height = 0;
                return newNode;
            }
            if (comparison.Invoke(newNode.getValue(), root.getValue()) > 0)
            {
                root.rightNode = rotate(addNodeElement(root.rightNode, newNode, comparison));

            }
            else
            {
                root.leftNode = rotate(addNodeElement(root.leftNode, newNode, comparison));
            }
            root = rotate(root);
            return root;
        }

        public Node<T> rotate(Node<T> nodeRotate)
        {
            if (nodeRotate == null) return nodeRotate;
            nodeRotate = updateHeight(nodeRotate);
            if (nodeRotate.bf < -1)
            {
                if (nodeRotate.rightNode.bf > 0)
                {
                    nodeRotate = rightLeft(nodeRotate);
                }
                else
                {
                    nodeRotate = left(nodeRotate);
                }
            }
            else if (nodeRotate.bf > 1)
            {
                if (nodeRotate.leftNode.bf < 0)
                {
                    nodeRotate = leftRight(nodeRotate);
                }
                else
                {
                    nodeRotate = right(nodeRotate);
                }
            }
            return nodeRotate;
        }

        private Node<T> left(Node<T> nodeRotate)
        {
            Node<T> newRoot = nodeRotate.rightNode;
            Node<T> temp = nodeRotate.rightNode.leftNode;
            nodeRotate.rightNode.leftNode = nodeRotate;
            nodeRotate.rightNode = temp;
            nodeRotate = updateHeight(nodeRotate);
            return newRoot;
        }

        private Node<T> right(Node<T> nodeRotate)
        {
            Node<T> newRoot = nodeRotate.leftNode;
            Node<T> temp = nodeRotate.leftNode.rightNode;
            nodeRotate.leftNode.rightNode = nodeRotate;
            nodeRotate.leftNode = temp;
            nodeRotate = updateHeight(nodeRotate);
            return newRoot;
        }

        private Node<T> leftRight(Node<T> nodeRotate)
        {
            nodeRotate.leftNode = left(nodeRotate.leftNode);
            nodeRotate = right(nodeRotate);
            return nodeRotate;
        }

        private Node<T> rightLeft(Node<T> nodeRotate)
        {
            nodeRotate.rightNode = right(nodeRotate.rightNode);
            nodeRotate = left(nodeRotate);
            return nodeRotate;
        }

        private Node<T> updateHeight(Node<T> nodeRotate)
        {
            int left, right;
            left = nodeRotate.leftNode != null ? nodeRotate.leftNode.height : -1;
            right = nodeRotate.rightNode != null ? nodeRotate.rightNode.height : -1;
            nodeRotate.bf = left - right;
            nodeRotate.height = (right > left ? right : left) + 1;
            return nodeRotate;
        }

        public Node<T> removeElement(Node<T> root, T value, Comparison<T> comparison)
        {
            if (comparison.Invoke(root.getValue(), value) == 0)
            {
                if (root.rightNode == null && root.leftNode == null)
                {
                    return null;
                }
                else if (root.rightNode == null)
                {
                    return rotate(root.leftNode);
                }
                else if (root.leftNode == null)
                {
                    return rotate(root.rightNode);
                }
                else
                {
                    Node<T> preNode = root.leftNode;
                    Node<T> predecessorNode;
                    if (preNode.rightNode == null)
                    {
                        predecessorNode = preNode;
                        predecessorNode.rightNode = root.rightNode;
                    }
                    else
                    {
                        while (preNode.rightNode.rightNode != null)
                        {
                            preNode = preNode.rightNode;
                        }
                        predecessorNode = preNode.rightNode;
                        preNode.rightNode = predecessorNode.leftNode;
                        predecessorNode.leftNode = root.leftNode;
                        predecessorNode.rightNode = root.rightNode;
                    }
                    return predecessorNode;
                }
            }
            else
            {
                if (comparison.Invoke(value, root.getValue()) > 0)
                {
                    root.rightNode = rotate(removeElement(root.rightNode, value, comparison));
                }
                else
                {
                    root.leftNode = rotate(removeElement(root.leftNode, value, comparison));
                }
                return rotate(root);
            }
        }

        public Node(T value)
        {
            setValue(value);
        }

        public Node() { }

        public void setValue(T value)
        {
            this.valueNode = value;
        }

        public T getValue()
        {
            return this.valueNode;
        }

        public Node<T> getLeftNode()
        {
            return leftNode;
        }

        public void setLeftNode(Node<T> leftNode)
        {
            this.leftNode = leftNode;
        }

        public Node<T> getRigthNode()
        {
            return rightNode;
        }

        public void setRightNode(Node<T> rightNode)
        {
            this.rightNode = rightNode;
        }

        public int getHeight()
        {
            return height;
        }

        public void setHeight(int height)
        {
            this.height = height;
        }

        public int getBf()
        {
            return bf;
        }

        public void setBf(int bf)
        {
            this.bf = bf;
        }

    }
}
