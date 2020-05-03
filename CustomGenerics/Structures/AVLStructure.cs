using CustomGenerics.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CustomGenerics.Structures {
    public class AVLStructure<T> : IAVLStructure<T>, IEnumerable<T> {

        private Node<T> root = new Node<T>();
        List<T> findNode = new List<T>();
        private int size;

        public void addElement(T newElement, Comparison<T> comparison) {
            InsertValue(newElement, comparison);
        }

        public void deleteElement(T deleteElement, Comparison<T> comparison){
            DeleteValue(deleteElement, comparison);
        }

        public List<T> searchValue(T searchElement, Comparison<T> comparisonName) {
            findNode.Clear();
            var found = search(root, searchElement, comparisonName);
            return found;
            
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
        private bool contains(Node<T> root, T value, Comparison<T> comparison)
        {
            if (root == null) return false;
            if (comparison.Invoke(root.getValue(), value) == 0){
                return true;
            }else{
                if (contains(root.getRigthNode(), value, comparison)) { return true; }
                else if (contains(root.getLeftNode(), value, comparison)) { return true; }
                return false;
            }
        }

        
        private List<T> search(Node<T> root, T value, Comparison<T> comparisonName)
        {
            if (root == null) return default;
            if (comparisonName.Invoke(root.getValue(), value) == 0) { findNode.Add(root.getValue()); }
            if (contains(root.getRigthNode(), value, comparisonName)) { search(root.getRigthNode(), value, comparisonName); }
            if (contains(root.getLeftNode(), value, comparisonName)) { search(root.getLeftNode(), value, comparisonName); }
            return findNode;
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
