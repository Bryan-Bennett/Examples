using System;
using System.Collections;
using System.Collections.Generic;

namespace Examples.Collections
{
    /// <summary>
    /// The BijectiveDictionary{TKey, EKey} class is a dictionary that maintains 1:1 correspondence with another dictionary, its "inverse" BijectiveDictionary{EKey, TKey}.
    /// In other words, if X maps to Y then Y also maps to X.  Each bijective dictionary contains a reference to the other through their 'Inverse' property.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="EKey"></typeparam>
    /// <example>
    /// The following example demonstrates the behavior of the <see cref="BijectiveDictionary{TKey, EKey}"/>.
    /// <code>
    /// var bijective = new BijectiveDictionary{string, int}();
    /// bijective.Add("FooBar", 7);
    /// var v1 = bijective["FooBar"]; //v1 equals 7
    /// var v2 = bijective.Inverse[7]; //v2 equals "FooBar"
    /// bool kvpExists = bijective.Contains(new KeyValuePair{string, int}("FooBar", 3)); // kvpExists is false because the pair ("FooBar", 3) does not exist though ("FooBar", 7) does.
    /// v1.Clear(); //both bijective and its inverse (bijective.Inverse) are now empty
    /// </code>
    /// </example>
    public class BijectiveDictionary<TKey, EKey> : IDictionary<TKey, EKey>, IReadOnlyDictionary<TKey, EKey>
    {
        /// <summary>
        /// Gets or sets the value at the specified index.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public EKey this[TKey key]
        {
            get => Map[key];
            set
            {
                if (Map.ContainsKey(key))
                {
                    EKey oldValue = Map[key];
                    Inverse.InverseRemove(oldValue);
                    Map.Add(key, value);
                }
                else
                {
                    Add(key, value);
                }
            }
        }

        /// <summary>
        /// Gets a collection of keys in this BijectiveDictionary.
        /// </summary>
        public ICollection<TKey> Keys => Map.Keys;

        /// <summary>
        /// Gets a collection of values in this BijectiveDictionary.
        /// </summary>
        public ICollection<EKey> Values => Map.Values;

        /// <summary>
        /// Gets the current count of key/value pairs in this BijectiveDictionary.
        /// </summary>
        public int Count => Map.Count;

        /// <summary>
        /// BijectiveDictionaries are never read-only so this always returns false.  It can, however, be exposed as an IReadOnlyDictionary.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets the inverse <see cref="BijectiveDictionary{EKey, TKey}"/>.
        /// </summary>
        public BijectiveDictionary<EKey, TKey> Inverse { get; }

        private Dictionary<TKey, EKey> Map { get; } = new Dictionary<TKey, EKey>();

        /// <summary>
        /// Gets the collection of keys of this <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
        /// </summary>
        IEnumerable<TKey> IReadOnlyDictionary<TKey, EKey>.Keys => ((IReadOnlyDictionary<TKey, EKey>)Map).Keys;

        /// <summary>
        /// Gets the collection of values of this <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
        /// </summary>
        IEnumerable<EKey> IReadOnlyDictionary<TKey, EKey>.Values => ((IReadOnlyDictionary<TKey, EKey>)Map).Values;

        /// <summary>
        /// Creates a <see cref="BijectiveDictionary{TKey, EKey}"/> object.
        /// </summary>
        public BijectiveDictionary()
        {
            Inverse = new BijectiveDictionary<EKey, TKey>(this);
        }

        private BijectiveDictionary(BijectiveDictionary<EKey, TKey> inverse)
        {
            Inverse = inverse ?? throw new ArgumentNullException(nameof(inverse));
        }

        /// <summary>
        /// Adds the specified key and value to this <see cref="BijectiveDictionary{TKey, EKey}"/> and adds it to its inverse <see cref="BijectiveDictionary{EKey, TKey}"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, EKey value)
        {
            Map.Add(key, value);
            Inverse.InverseAdd(value, key);
        }

        /// <summary>
        /// Adds the specified key/value pair to this <see cref="BijectiveDictionary{TKey, EKey}"/> and also adds it to its inverse <see cref="BijectiveDictionary{EKey, TKey}"/>.
        /// </summary>
        /// <param name="item"></param>
        public void Add(KeyValuePair<TKey, EKey> item)
        {
            Map.Add(item.Key, item.Value);
            Inverse.InverseAdd(item.Value, item.Key);
        }

        /// <summary>
        /// Clears all elements in this <see cref="BijectiveDictionary{TKey, EKey}"/> and its inverse <see cref="BijectiveDictionary{EKey, TKey}"/>.
        /// </summary>
        public void Clear()
        {
            Map.Clear();
            Inverse.InverseClear();
        }

        /// <summary>
        /// Returns whether or not the exact specified key/value pair exists in this <see cref="BijectiveDictionary{TKey, EKey}"/>.  Note that this only
        /// returns true if this bijective dictionary contains the specified key, its inverse bijective dictionary contains the specified key using the value as the key,
        /// and that the specified key is equal to the value retrieved from the inverse bijective dictionary using the specified value as a key.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<TKey, EKey> item)
        {
            return (Map.ContainsKey(item.Key) && Inverse.InverseContainsKey(item.Value) && item.Key.Equals(Inverse[item.Value]));
        }

        /// <summary>
        /// Returns whether or not the exact specified key/value pair exists in this <see cref="BijectiveDictionary{TKey, EKey}"/>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return Map.ContainsKey(key);
        }

        /// <summary>
        /// Copies the elements of the <see cref="ICollection{KeyValuePair{TKey, EKey}}"/> to an System.Array, starting at a particular array index.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(KeyValuePair<TKey, EKey>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, EKey>>)Map).CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the specified key/value pair from this <see cref="BijectiveDictionary{TKey, EKey}"/> and returns whether or not it was removed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            if (Map.ContainsKey(key))
            {
                EKey value = Map[key];
                Map.Remove(key);
                Inverse.InverseRemove(value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes the specified key/value pair from this <see cref="BijectiveDictionary{TKey, EKey}"/> and returns whether or not it was removed.
        /// Note that the specified key must exist in this <see cref="BijectiveDictionary{TKey, EKey}"/>, the specified value must exist as a key
        /// in the inverse <see cref="BijectiveDictionary{EKey, TKey}"/>, and that the specified key must equal the value retrieved from the inverse.  If
        /// this is not the case, nothing is removed and this method returns false.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(KeyValuePair<TKey, EKey> item)
        {
            if (Map.ContainsKey(item.Key) && Inverse.InverseContainsKey(item.Value) && item.Key.Equals(Inverse[item.Value]))
            {
                return Remove(item.Key);
            }
            return false;
        }

        /// <summary>
        /// Tries to get the value using the specified key and returns whether or not the attempt was successful.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out EKey value)
        {
            return Map.TryGetValue(key, out value);
        }

        /// <summary>
        /// Gets the <see cref="IEnumerator{KeyValuePair{TKey, EKey}}"/> of this <see cref="BijectiveDictionary{TKey, EKey}"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, EKey>> GetEnumerator()
        {
            return Map.GetEnumerator();
        }

        /// <summary>
        /// Gets the <see cref="IEnumerator"/> of this <see cref="BijectiveDictionary{TKey, EKey}"/>.
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Map.GetEnumerator();
        }

        private void InverseAdd(TKey key, EKey value)
        {
            Map.Add(key, value);
        }

        private void InverseRemove(TKey key)
        {
            Map.Remove(key);
        }

        private bool InverseContainsKey(TKey key)
        {
            return Map.ContainsKey(key);
        }

        private void InverseClear()
        {
            Map.Clear();
        }
    }
}
