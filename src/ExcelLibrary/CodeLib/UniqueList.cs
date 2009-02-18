using System;
using System.Collections;
using System.Collections.Generic;

namespace QiHe.CodeLib
{
    public class UniqueList<T> : IList<T>
    {
        private IList<T> internalList;
        private IDictionary<T, int> internalLookup;

        public UniqueList()
        {
            this.internalList = new List<T>();
            this.internalLookup = new Dictionary<T, int>();
        }

        public UniqueList(int capacity)
        {
            this.internalList = new List<T>(capacity);
            this.internalLookup = new Dictionary<T, int>(capacity);
        }

        public int IndexOf(T item)
        {
            if (this.internalLookup.ContainsKey(item))
                return this.internalLookup[item];
            else
                return -1;
        }

        public void Insert(int index, T item)
        {
            if (internalLookup.ContainsKey(item))
                throw new ArgumentException("Duplicate item already exist in the list");

            this.internalList.Insert(index, item);
            this.internalLookup.Add(item, index);

            // require re-indexing
            for (int i = index; i < this.internalList.Count; i++)
            {
                T itemKey = this.internalList[i];
                this.internalLookup[itemKey] = i;
            }
        }

        public void RemoveAt(int index)
        {
            T item = this.internalList[index];
            this.internalList.RemoveAt(index);
            this.internalLookup.Remove(item);

            // require re-indexing
            for (int i = index; i < this.internalList.Count; i++)
            {
                T itemKey = this.internalList[i];
                this.internalLookup[itemKey] = i;
            }
        }

        public T this[int index]
        {
            get { return this.internalList[index]; }
            set { this.internalList[index] = value; }
        }

        public void Add(T item)
        {
            this.internalList.Add(item);
            if (!internalLookup.ContainsKey(item))
            {
                this.internalLookup.Add(item, internalList.Count - 1);
            }
        }

        public void Clear()
        {
            this.internalList.Clear();
            this.internalLookup.Clear();
        }

        public bool Contains(T item)
        {
            return this.internalLookup.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.internalList.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            if (this.internalLookup.ContainsKey(item))
            {
                int index = this.internalLookup[item];
                this.RemoveAt(index); // re-indexing implictly
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Count
        {
            get { return this.internalList.Count; }
        }

        public bool IsReadOnly
        {
            get { return this.internalList.IsReadOnly; }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return this.internalList.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return this.internalList.GetEnumerator();
        }
    }
}
