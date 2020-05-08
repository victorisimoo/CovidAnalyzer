using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenerics.Interfaces {

    public abstract class IAVLStructure<T> {
        protected abstract void InsertValue(T value, Comparison<T> comparison);
        protected abstract T DeleteValue(T value, Comparison<T> comparison);
    }

    public abstract class IPriorityQueue<T> {
        protected abstract void Enqueue(T value, Comparison<T> comparison, Comparison<T> comparisonHour);
        protected abstract T Dequeue(Comparison<T> comparison, Comparison<T> comparisonHour);
        protected abstract T peek();
    }

}
