using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// A Windows Registry value in a RegistryKey.
	/// </summary>
	public abstract class RegistryValue
	{
		#region Internal Members

		/// <summary>
		/// The name of the RegistryValue.
		/// </summary>
		private string _name;
		/// <summary>
		/// The data this RegistryValue stores.
		/// </summary>
		private byte[] _data;
		/// <summary>
		/// The type of this RegistryValue.
		/// </summary>
		private readonly RegistryValueType _type;
		/// <summary>
		/// The RegistryKey which this RegistryValue is located in.
		/// </summary>
		private readonly RegistryKey _parent;
		/// <summary>
		/// Specifies if this RegistryValue is marked for deletion from the system registry (when saved to a RegistryFile).
		/// </summary>
		private bool _markedForDeletion;
		/// <summary>
		/// The HashCode of this instance.
		/// </summary>
		private readonly int _hashCode;

		#endregion

		#region Constructors

		/// <summary>
		/// A Registry Value in the Windows Registry.
		/// </summary>
		internal RegistryValue(RegistryKey parent, string name, RegistryValueType type)
		{
			if(parent == null)
			{
				throw new NullReferenceException("Parent RegistryKey cannot be null.");
			}
			_parent = parent;
			_name = name;
			_type = type;
			_hashCode = _parent.GetHashCode() ^ _name.GetHashCode() ^ _type.GetHashCode();
		}

		#endregion

		#region Delegates

		/// <summary>
		/// Handles a name change for a RegistryValue.
		/// </summary>
		public delegate void ValueNameChangedEventHandler(object sender, EventArgs e);
		/// <summary>
		/// Handles a change in a RegistryValue's contained data.
		/// </summary>
		public delegate void ValueDataChangedEventHandler(object sender, EventArgs e);
		/// <summary>
		/// Handles a deletion of this RegistryValue.
		/// </summary>
		public delegate void ValueDeletedEventHandler(object sender, EventArgs e);

		#endregion

		#region Events

		/// <summary>
		/// Occurs when a RegistryValue's name is changed.
		/// </summary>
		public event ValueNameChangedEventHandler NameChange;
		/// <summary>
		/// Occurs when a RegistryValue's contained data is changed.
		/// </summary>
		public event ValueDataChangedEventHandler DataChange;
		/// <summary>
		/// Occurs when a RegistryValue is deleted.
		/// </summary>
		public event ValueDeletedEventHandler ValueDeleted;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the name of the registry value in the Windows Registry.
		/// </summary>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if(NameChange != null && value != _name)
				{
					NameChange(this, new ValueNameChangedEventArgs(_name, value));
					_name = value;
				}
			}
		}

		/// <summary>
		/// Gets the full path and name of this RegistryValue.
		/// </summary>
		public string FullPath
		{
			get
			{
				return this.Parent.FullPath + "\\" + this.Name;
			}
		}

		/// <summary>
		/// Gets or sets the binary data of this RegistryValue.
		/// </summary>
		public byte[] BinaryData
		{
			get
			{
				return _data;
			}
			set
			{
				if(DataChange != null && value != _data)
				{
					DataChange(this, new ValueDataChangedEventArgs(_data, value));
				}
				_data = value;
			}
		}

		/// <summary>
		/// Gets or sets the data of this RegistryValue.
		/// </summary>
		public abstract object Value
		{
			get;
			set;
		}

		/// <summary>
		/// Gets a string representation of the value contained.
		/// </summary>
		public abstract string ValueString
		{
			get;
		}

		/// <summary>
		/// Specifies if this RegistryValue is marked for deletion from the Windows Registry (when saved to a RegistryFile).
		/// </summary>
		public bool MarkedForDeletion
		{
			get
			{
				return _markedForDeletion;
			}
			set
			{
				_markedForDeletion = value;
			}
		}

		/// <summary>
		/// Gets the type of this RegistryValue.
		/// </summary>
		public RegistryValueType Type
		{
			get
			{
				return _type;
			}
		}

		/// <summary>
		/// Gets the parent key of this RegistryValue.
		/// </summary>
		public RegistryKey Parent
		{
			get
			{
				return _parent;
			}
		}

		/// <summary>
		/// Deletes this RegistryValue.
		/// </summary>
		public void Delete()
		{
			if(ValueDeleted != null)
			{
				ValueDeleted(this, new ValueDeletedEventArgs(_name, _data));
			}
		}

		#endregion

		#region Equality operators

		/// <summary>
		/// Determines if two RegistryValues are the same.
		/// </summary>
		/// <param name="leftHandSide">The RegistryValue on the left-hand side of this operator.</param>
		/// <param name="rightHandSide">The RegistryValue on the right-hand side of this operator.</param>
		/// <returns>True if the two RegistryValues are the same, otherwise false.</returns>
		public static bool operator ==(RegistryValue leftHandSide, RegistryValue rightHandSide)
		{
			if(object.ReferenceEquals(leftHandSide, null) ^ object.ReferenceEquals(rightHandSide, null))
			{
				return false;
			}
			else if(object.ReferenceEquals(leftHandSide, null) && object.ReferenceEquals(rightHandSide, null))
			{
				return true;
			}
			else if(leftHandSide.Parent == rightHandSide.Parent &&
				leftHandSide.Name == rightHandSide.Name &&
				leftHandSide.Value == rightHandSide.Value)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Determines if two RegistryValues are not the same.
		/// </summary>
		/// <param name="leftHandSide">The RegistryValue on the left-hand side of this operator.</param>
		/// <param name="rightHandSide">The RegistryValue on the right-hand side of this operator.</param>
		/// <returns>True if the two RegistryValues are different, otherwise false.</returns>
		public static bool operator !=(RegistryValue leftHandSide, RegistryValue rightHandSide)
		{
			if(leftHandSide == null ^ rightHandSide == null)
			{
				return true;
			}
			else if(leftHandSide == null && rightHandSide == null)
			{
				return false;
			}
			else if(leftHandSide.Parent != rightHandSide.Parent || leftHandSide.Name != rightHandSide.Name || leftHandSide.Value != rightHandSide.Value)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified System.Object is equal to the current Sonneville.Registry.RegistryValue.
		/// </summary>
		/// <param name="obj">The System.Object to compare to the current Sonneville.Registry.RegistryValue.</param>
		/// <returns>True if the specified System.Object is equal to the current Sonneville.Registry.RegistryValue; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			if(this == (RegistryValue)obj)
			{
				return true;
			}
			else if (obj == null)
			{
				return false;
			}
			else
			{
			}
			return RegistryValue.Equals(this, obj);
		}

		/// <summary>
		/// Determines whether the two Sonneville.Registry.RegistryValue instances are equal.
		/// </summary>
		/// <param name="r1">The first Sonneville.Registry.RegistryValue to compare.</param>
		/// <param name="r2">The second Sonneville.Registry.RegistryValue to compare.</param>
		/// <returns>True if the first Sonneville.Registry.RegistryValue is equal to the second Sonneville.Registry.RegistryValue.</returns>
		public static bool Equals(RegistryValue r1, RegistryValue r2)
		{
			if (r1 == null && r2 == null)
			{
				return true;
			}
			else if (r1 == null || r2 == null)
			{
				return false;
			}
			else
			{
				return (r1._parent == r2._parent) && (r1._name == r2._name);
			}
		}

		/// <summary>
		/// Serves as a hash function, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>A hash code for the current Sonneville.Registry.RegistryValue.</returns>
		public override int GetHashCode()
		{
			return _hashCode;
		}

		#endregion

		internal abstract string FormatForExport();
	}
}
