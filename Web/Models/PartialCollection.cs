using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Marina.Store.Web.Models
{
    public class PartialCollection<T> : ICollection<T>
    {
        private readonly ICollection<T> _collection = new Collection<T>();
        private readonly int _total;

        public PartialCollection(int total, IEnumerable<T> items)
        {
            _total = total;
            foreach (var item in items)
            {
                _collection.Add(item);
            }
        }

        public int TotalCount { get { return _total; } }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _collection.Add(item);
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
           _collection.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return _collection.Remove(item);
        }

        public int Count
        {
            get { return _collection.Count;  }
        }

        public bool IsReadOnly
        {
            get { return _collection.IsReadOnly; }
        }
    }
}