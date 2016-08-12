using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Channel.Mine.IMDB.Collections
{
    public partial class MediaCollection : ICollection<Entities.Media>
    {
        private Dictionary<String, Entities.Media> _collection;

        public MediaCollection() { this._collection = new Dictionary<String, Entities.Media>(); }

        public void Add(Entities.Media item) { this._collection.Add(item.ID, item); }

        public void Clear() { this._collection.Clear(); }

        public Boolean Contains(Entities.Media item) { return this._collection.ContainsKey(item.Title); }

        public Int32 Count { get { return this._collection.Count; } }

        public Boolean IsReadOnly { get { return false; } }

        public Boolean Remove(Entities.Media item) { return this._collection.Remove(item.ID); }

        public void CopyTo(Entities.Media[] array, Int32 arrayIndex) { throw new NotImplementedException(); }

        public IEnumerator<Entities.Media> GetEnumerator() { return this._collection.Values.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator() { return this._collection.Values.GetEnumerator(); }

        #region IDictionary

        public Entities.Media this[String key]
        {
            get
            {
                Entities.Media result = null;
                this._collection.TryGetValue(key, out result);
                return result;
            }
            set { this._collection[key] = value; }
        }

        /*
        public void Add(String key, Entities.Media value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public ICollection<string> Keys
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out Entities.Media value)
        {
            throw new NotImplementedException();
        }

        public ICollection<Entities.Media> Values
        {
            get { throw new NotImplementedException(); }
        }

        public void Add(KeyValuePair<string, Entities.Media> item)
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, Entities.Media> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, Entities.Media>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, Entities.Media> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<String, Entities.Media>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        */
        #endregion
    }
}
