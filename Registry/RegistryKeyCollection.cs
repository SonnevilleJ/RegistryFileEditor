using System;
using System.Collections;

namespace Sonneville.Registry
{
	/// <summary>
	/// A collection of RegistryKeys.
	/// </summary>
	public sealed class RegistryKeyCollection : ICollection
	{
		private RegistryKey[] _list;
		private int _capacity;
		private int _size;
		private RegistryKey _parent;

		/// <summary>
		/// Initializes a new instance of the RegistryKeyCollection class.
		/// </summary>
		/// <param name="parent">The parent key of this RegistryKeyCollection.</param>
		internal RegistryKeyCollection(RegistryKey parent) : this(parent, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the RegistryKeyCollection class that is empty and has the specified initial capacity.
		/// </summary>
		/// <param name="parent">The parent key of this RegistryKeyCollection.</param>
		/// <param name="capacity">The number of elements that the new list is initially capable of storing.</param>
		internal RegistryKeyCollection(RegistryKey parent, int capacity)
		{
			_parent = parent;
			_list = new RegistryKey[capacity];
			_capacity = capacity;
		}

		/// <summary>
		/// Initializes a new instance of the RegistryKeyCollection class which includes the RegistryKeys in the specified array.
		/// </summary>
		/// <param name="parent">The parent key of this RegistryKeyCollection.</param>
		/// <param name="array">An array of RegistryKeys to build a RegistryKeyCollection from.</param>
		public RegistryKeyCollection(RegistryKey parent, RegistryKey[] array)
		{
			_parent = parent;
			this.Absorb(array);
		}

		/// <summary>
		/// Absorbs all RegistryKeys in the array into the RegistryKeyCollection.
		/// </summary>
		/// <param name="array">The one-dimensional array of RegistryKeys to absorb into this RegistryKeyCollection.</param>
		public void Absorb(RegistryKey[] array)
		{
			ResizeArray(_capacity + array.Length);
			foreach(RegistryKey key in array)
			{
				this.Add(key);
			}
		}

		/// <summary>
		/// Adds a RegistryKey to the collection.
		/// </summary>
		/// <param name="key">The RegistryKey object to add to the end of the RegistryKeyCollection.</param>
		public void Add(RegistryKey key)
		{
			if(_size == _capacity)
			{
				// make room for new key
				ResizeArray(_capacity + 1);
			}
			_list[_size++] = key;
		}

		/// <summary>
		/// Determines whether a RegistryKey is in the RegistryKeyCollection.
		/// </summary>
		/// <param name="fullName">The full name of the RegistryKey to locate in the RegistryKeyCollection.</param>
		/// <returns>True if there exists a RegistryKey with the specified full name in the RegistryKeyCollection.</returns>
		public bool Contains(string fullName)
		{
			foreach(RegistryKey key in this)
			{
				if(key.FullPath == fullName)
				{
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Determines whether a RegistryKey is in the RegistryKeyCollection.
		/// </summary>
		/// <param name="key">The RegistryKey to locate in the RegistryKeyCollection.</param>
		/// <returns>True if the RegistryKey exists in the RegistryKeyCollection.</returns>
		public bool Contains(RegistryKey key)
		{
			return IndexOf(key) == -1 ? false : true;
		}

		/// <summary>
		/// Deletes a RegistryKey from this RegistryKeyCollection.
		/// </summary>
		/// <param name="key">The RegistryKey to delete from this RegistryKeyCollection.</param>
		public void Delete(RegistryKey key)
		{
			if(key == null)
			{
				throw new NullReferenceException();
			}
			Remove(key);
			key.Delete();
		}

		/// <summary>
		/// Gets the index of a given RegistryKey in the collection.
		/// </summary>
		/// <param name="key">The RegistryKey to search for.</param>
		/// <returns>An integer representing the index of the found RegistryKey, otherwise -1.</returns>
		public int IndexOf(RegistryKey key)
		{
			for(int i = 0; i < this.Count; i++)
			{
				if(this[i] == key)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the RegistryKey stored at a given index in the collection.
		/// </summary>
		public RegistryKey this[int index]
		{
			get
			{
				return (RegistryKey)_list[index];
			}
//			set
//			{
//				_list[index] = (object)value;
//			}
		}

		/// <summary>
		/// Copies the RegistryKeys in this RegistryKeyCollection to a new RegistryKey array.
		/// </summary>
		/// <returns>An array of RegistryKeys contained in this RegistryKeyCollection.</returns>
		public RegistryKey[] ToArray()
		{
			return _list;
			//
			//			RegistryKey[] keys = new RegistryKey[this.List.Count];
			//			for(int i = 0; i < this.List.Count; i++)
			//			{
			//				keys[i] = (RegistryKey)this.List[i];
			//			}
			//			return keys;
		}

		/// <summary>
		/// Removes the specified RegistryKey from the RegistryKeyCollection.
		/// </summary>
		/// <param name="key">The RegistryKey to remove from the RegistryKeyCollection.</param>
		public void Remove(RegistryKey key)
		{
			this.Remove(key.Name);
		}

		/// <summary>
		/// Removes a RegistryKey with the specified full name from the RegistryKeyCollection.
		/// </summary>
		/// <param name="fullName">The full name of the RegistryKey to remove from the RegistryKeyCollection.</param>
		public void Remove(string fullName)
		{
			foreach(RegistryKey key in this)
			{
				if(key.FullPath == fullName)
				{
					this.Remove(key);
				}
			}
		}

		/// <summary>
		/// Explicitly converts a RegistryKeyCollection to an array of RegistryKeys.
		/// </summary>
		/// <param name="collection">The RegistryKeyCollection instance to convert.</param>
		/// <returns>An array of RegistryKeys contained in the specified RegistryKeyCollection.</returns>
		public static explicit operator RegistryKey[](RegistryKeyCollection collection)
		{
			if(collection == null)
			{
				throw new NullReferenceException();
			}
			return collection.ToArray();
		}

		#region ICollection Members

		/// <summary>
		/// Returns if the current instance is synchronized (thread-safe).
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				// TODO:  Add RegistryKeyCollection.IsSynchronized getter implementation
				return false;
			}
		}

		/// <summary>
		/// Gets the number of RegistryKeys stored in this RegistryKeyCollection.
		/// </summary>
		public int Count
		{
			get
			{
				return _size;
			}
		}

		/// <summary>
		/// Copies the RegistryKeys stored in this RegistryKeyCollection to a given array, starting at index.
		/// </summary>
		/// <param name="array">The System.Array to copy elements to.</param>
		/// <param name="index">The index at which to begin copying to.</param>
		public void CopyTo(Array array, int index)
		{
			if(array == null)
			{
				throw new NullReferenceException();
			}
			if(array.Length - index < _size)
			{
				throw new ArgumentOutOfRangeException("array", array, "Given array is too small.");
			}
			else
			{
				for(int i = index; i < _size; i++)
				{
					array.SetValue(this[i], i);
				}
			}
		}

		/// <summary>
		/// Copies the RegistryKeys stored in this RegistryKeyCollection to a given array, starting at index.
		/// </summary>
		/// <param name="array">The array of RegistryKey to copy elements to.</param>
		/// <param name="index">The index at which to begin copying to.</param>
		public void CopyTo(RegistryKey[] array, int index)
		{
			CopyTo(array, index);
		}

		/// <summary>
		/// Gets an object used to synchronize access to the collection.
		/// </summary>
		public object SyncRoot
		{
			get
			{
				// TODO:  Add RegistryKeyCollection.SyncRoot getter implementation
				return null;
			}
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Gets an enumerator for the collection.
		/// </summary>
		/// <returns>A RegistryKeyEnumerator usable to move through the collection.</returns>
		public IEnumerator GetEnumerator()
		{
			return new RegistryKeyEnumerator(this);
		}

		#endregion

		/// <summary>
		/// Resizes the internal array to a given size.
		/// </summary>
		/// <param name="size">The size to restructure the internal array to.</param>
		private void ResizeArray(int size)
		{
			if(size > _capacity)
			{
				RegistryKey[] temp = new RegistryKey[size];
				_list.CopyTo(temp, 0);
				_list = temp;
				_capacity = size;
			}
			else if(size < _capacity)
			{
				throw new ArgumentOutOfRangeException("size", size, "Size must be larger than current instance.");
			}
		}

		internal class RegistryKeyEnumerator : IEnumerator
		{
			private RegistryKeyCollection _collection;
			private int _current = -1;

			internal RegistryKeyEnumerator(RegistryKeyCollection collection)
			{
				_collection = collection;
				if(_collection.Count == 0)
				{
					_current = -1;
				}
				else
				{
					_current = 0;
				}
			}

			#region IEnumerator Members

			public virtual void Reset()
			{
				_current = -1;
			}

			public virtual object Current
			{
				get
				{
					if(_current > -1)
					{
						return _collection[_current];
					}
					else
					{
						throw new InvalidOperationException("Invalid operation: Enumerator must be moved before calling Current.");
					}
				}
			}

			public virtual bool MoveNext()
			{
				if(_current < 0)
				{
					return false;
				}
				_current++;
				if(_current == _collection.Count)
				{
					_current = -1;
				}
				return true;
			}

			#endregion

		}

	}
}