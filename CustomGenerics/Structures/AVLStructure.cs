using CustomGenerics.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CustomGenerics.Structures {
    public class AVLStructure<T> : IAVLStructure<T>, IEnumerable<T> {

        private Node<T> root = new Node<T>();
        private int size;

        public void addElement(T newElement, Comparison<T> comparison) {
            InsertValue(newElement, comparison);
        }

        public void deleteElement(T deleteElement, Comparison<T> comparison){
            DeleteValue(deleteElement, comparison);
        }

        public T searchValue(T searchElement, Comparison<T> comparison) {
            return search(root, searchElement, comparison);
        }

        public IEnumerator<T> GetEnumerator() {
            throw new NotImplementedException();
        }

        protected override T DeleteValue(T value, Comparison<T> comparison) {
            if (!contains(root, value, comparison)) {
                return default(T);
            }
            root = root.rotate(root.removeElement(root, value, comparison));
            size--;
            return value;
        }
        private bool contains(Node<T> root, T value, Comparison<T> comparison){
            if (root == null) return false;
            if (comparison.Invoke(root.getValue(), value) == 0){
                return true;
            }else{
                if (contains(root.getRigthNode(), value, comparison)) { return true; }
                else if (contains(root.getLeftNode(), value, comparison)) { return true; }
                return false;
            }
        }

        private T search(Node<T> root, T value, Comparison<T> comparison) {
            if (root == null) return default(T);
            if (comparison.Invoke(root.getValue(), value) == 0) {
                return root.getValue();
            } else {
                if (comparison.Invoke(root.getRigthNode().getValue(), value) == 0) {
                    search(root.getRigthNode(), value, comparison);
                }
                else if (comparison.Invoke(root.getRigthNode().getValue(), value) > 0) {
                    search(root.getLeftNode(), value, comparison);
                }
                if (comparison.Invoke(root.getLeftNode().getValue(), value) == 0) {
                    search(root.getLeftNode(), value, comparison);
                }
                else if (comparison.Invoke(root.getLeftNode().getValue(), value) < 0) {
                    search(root.getRigthNode(), value, comparison);
                }
                return default(T);
            }
        }

        protected override void InsertValue(T value, Comparison<T> comparison) {
            Node<T> newNode = new Node<T>(value);
            if (size == 0) {
                root = root.addNodeElement(null, newNode, comparison);
            } else {
                root = root.addNodeElement(root, newNode, comparison);
            }
            size++;
        }

        public bool isEmpty() {
            if (size == 0) return true;
            return false;
        }

        public void clearTree() {
            size = 0;
            root = null;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
    }
}
