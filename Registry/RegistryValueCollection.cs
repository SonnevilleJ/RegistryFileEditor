using System;
using System.Collections;

namespace Sonneville.Registry
{
	/// <summary>
	/// A collection of RegistryValues.
	/// </summary>
	public class RegistryValueCollection : ICollection
	{
		private RegistryValue[] _list;
		private int _capacity;
		private int _size;
		private RegistryKey _parent;

		#region Constructors

		/// <summary>
		/// Creates an empty RegistryValueCollection.
		/// </summary>
		/// <param name="parent">The parent key of this RegistryValueCollection.</param>
		internal RegistryValueCollection(RegistryKey parent) : this(parent, 2)	// use default size of 2 elements
		{
		}

		/// <summary>
		/// Creates a RegistryValueCollection containing given values.
		/// </summary>
		/// <param name="parent">The parent key of this RegistryValueCollection.</param>
		/// <param name="values">A list of values to place in the RegistryValueCollection.</param>
		internal RegistryValueCollection(RegistryKey parent, params RegistryValue[] values) : this(parent, values.Length)
		{
			for(int i = 0; i < values.Length; i++)
			{
				RegistryValue temp = (RegistryValue)values[i];
				_list[i] = temp;
			}
		}

		/// <summary>
		/// Creates an empty RegistryValueCollection of a given size.
		/// </summary>
		/// <param name="parent">The parent key of this RegistryValueCollection.</param>
		/// <param name="capacity">The initial size of this RegistryValueCollection.</param>
		internal RegistryValueCollection(RegistryKey parent, int capacity)
		{
			_parent = parent;
			_list = new RegistryValue[capacity];
			_capacity = capacity;
			this.Add(new StringValue(_parent, "", null));
		}

		#endregion

		/// <summary>
		/// Adds a RegistryValue to the collection.
		/// </summary>
		/// <param name="value">The RegistryValue to add to the collection.</param>
		public void Add(RegistryValue value)
		{
			if(_size == _capacity)
			{
				// make room for new value
				ResizeArray(_capacity + 1);
			}
			_list[_size++] = value;
		}

		/// <summary>
		/// Specifies whether this RegistryValueCollection contains a RegistryValue of a given name.
		/// </summary>
		/// <param name="name">The name of the RegistryValue to search for.</param>
		/// <returns>True if a RegistryValue is found in the collection with the given name, otherwise false.</returns>
		public bool Contains(string name)
		{
			return this.IndexOf(name) == -1 ? false : true;
		}

		/// <summary>
		/// Specifies whether this RegistryValueCollection contains a given RegistryValue.
		/// </summary>
		/// <param name="value">The RegistryValue to search for.</param>
		/// <returns>True if the given RegistryValue is found in the collection, otherwise false.</returns>
		public bool Contains(RegistryValue value)
		{
			return this.IndexOf(value) == -1 ? false : true;
		}

		/// <summary>
		/// Gets the index of a RegistryValue in the collection with a given name.
		/// </summary>
		/// <param name="name">The name of the RegistryValue to search for.</param>
		/// <returns>An integer representing the index of the found RegistryValue, otherwise -1.</returns>
		public int IndexOf(string name)
		{
			for(int i = 0; i < _size; i++)
			{
				if(this[i].Name == name)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Gets the index of a given RegistryValue in the collection.
		/// </summary>
		/// <param name="value">The RegistryValue to search for.</param>
		/// <returns>An integer representing the index of the found RegistryValue, otherwise -1.</returns>
		public int IndexOf(RegistryValue value)
		{
			for(int i = 0; i < _size; i++)
			{
				if(this[i] == value)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Removes the RegistryValue at index from the collection.
		/// </summary>
		/// <param name="index">The index of the RegistryValue to remove from the collection.</param>
		private void Remove(int index)
		{
			if(index > -1)
			{
				for(; index < _size - 1; index++)
				{
					_list[index] = _list[index + 1];
				}
				_size--;
			}
		}

		/// <summary>
		/// Removes a given RegistryValue from the collection.
		/// </summary>
		/// <param name="value">The RegistryValue to remove from the collection.</param>
		public void Remove(RegistryValue value)
		{
			int index = this.IndexOf(value);
			Remove(index);
		}

		/// <summary>
		/// Removes the RegistryValue with a given name from the collection.
		/// </summary>
		/// <param name="name">The name of the RegistryValue to remove from the collection.</param>
		public void RemoveValue(string name)
		{
			int index = this.IndexOf(name);
			Remove(index);
		}

		/// <summary>
		/// Gets the RegistryValue stored at a given index in the collection.
		/// </summary>
		public RegistryValue this[int index]
		{
			get
			{
				return _list[index];
			}
		}

		/// <summary>
		/// Gets an array of RegistryValues stored in this RegistryValueCollection.
		/// </summary>
		/// <returns>An array of RegistryValues stored in this RegistryValueCollection.</returns>
		public RegistryValue[] ToArray()
		{
			return _list;
		}

		#region ICollection Members

		/// <summary>
		/// Returns if the current instance is synchronized (thread-safe).
		/// </summary>
		public bool IsSynchronized
		{
			get
			{
				// TODO:  Add RegistryValueCollection.IsSynchronized getter implementation
				return false;
			}
		}

		/// <summary>
		/// Gets the number of RegistryValues stored in this RegistryValueCollection.
		/// </summary>
		public int Count
		{
			get
			{
				return _size;
			}
		}

		/// <summary>
		/// Copies the RegistryValues stored in this RegistryValueCollection to a given array, starting at index.
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
		/// Copies the RegistryValues stored in this RegistryValueCollection to a given array, starting at index.
		/// </summary>
		/// <param name="array">The array of RegistryValues to copy elements to.</param>
		/// <param name="index">The index at which to begin copying to.</param>
		public void CopyTo(RegistryValue[] array, int index)
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
				// TODO:  Add RegistryValueCollection.SyncRoot getter implementation
				return null;
			}
		}

		#endregion

		#region IEnumerable Members

		/// <summary>
		/// Gets an enumerator for the collection.
		/// </summary>
		/// <returns>A RegistryValueEnumerator usable to move through the collection.</returns>
		public IEnumerator GetEnumerator()
		{
			return new RegistryValueEnumerator(this);
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
				RegistryValue[] temp = new RegistryValue[size];
				_list.CopyTo(temp, 0);
				_list = temp;
				_capacity = size;
			}
			else if(size < _capacity)
			{
				throw new ArgumentOutOfRangeException("size", size, "Size must be larger than current instance.");
			}
		}
	}

	/// <summary>
	/// An enumerator used to iterate through a RegistryValueCollection.
	/// </summary>
	internal class RegistryValueEnumerator : IEnumerator
	{
		private RegistryValueCollection _collection;
		private int _current = -1;

		internal RegistryValueEnumerator(RegistryValueCollection collection)
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
