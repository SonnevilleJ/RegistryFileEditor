using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// A key in the Windows Registry.
	/// </summary>
	public class RegistryKey
	{
		#region Internal Members

		private Microsoft.Win32.RegistryHive _win32Hive;
		private Microsoft.Win32.RegistryKey _win32Key;
		private string _name;
		private RegistryKey _parent;
		private bool _markedForDeletion;
		private bool _linkedToRegistry;
		private readonly bool _isHive;
		private bool _read;
		private RegistryKeyCollection _subkeys;
		private RegistryValueCollection _values;

		private static string ConvertHiveToString(Microsoft.Win32.RegistryHive hive)
		{
			switch(hive)
			{
				case Microsoft.Win32.RegistryHive.ClassesRoot:
					return "HKEY_CLASSES_ROOT";
				case Microsoft.Win32.RegistryHive.CurrentConfig:
					return "HKEY_CURRENT_CONFIG";
				case Microsoft.Win32.RegistryHive.CurrentUser:
					return "HKEY_CURRENT_USER";
				case Microsoft.Win32.RegistryHive.DynData:
					return "HKEY_DYNAMIC_DATA";
				case Microsoft.Win32.RegistryHive.LocalMachine:
					return "HKEY_LOCAL_MACHINE";
				case Microsoft.Win32.RegistryHive.PerformanceData:
					return "HKEY_PERFORMANCE_DATA";
				case Microsoft.Win32.RegistryHive.Users:
					return "HKEY_USERS";
				default:
					throw new ArgumentException("Unsupported registry hive", "hive");
			}
		}

		private static Microsoft.Win32.RegistryHive ConvertStringToHive(string hive)
		{
			switch(hive.ToUpper())
			{
				case "HKEY_LOCAL_MACHINE":
					return Microsoft.Win32.RegistryHive.LocalMachine;
				case "HKEY_CURRENT_USER":
					return Microsoft.Win32.RegistryHive.CurrentUser;
				case "HKEY_CLASSES_ROOT":
					return Microsoft.Win32.RegistryHive.ClassesRoot;
				case "HKEY_USERS":
					return Microsoft.Win32.RegistryHive.Users;
				case "HKEY_CURRENT_CONFIG":
					return Microsoft.Win32.RegistryHive.CurrentConfig;
				case "HKEY_DYN_DATA":
					return Microsoft.Win32.RegistryHive.DynData;
				case "HKEY_PERFORMANCE_DATA":
					return Microsoft.Win32.RegistryHive.PerformanceData;
				default:
					throw new FormatException("Invalid registry Win32Hive.");
			}
		}

		#endregion

		#region Internal Event EventHandlers

		/// <summary>
		/// Handles deletions of Keys in this RegistryKey.
		/// </summary>
		/// <param name="sender">The deleted RegistryKey.</param>
		/// <param name="e">The KeyDeletedEventArgs instance containing the details of the name change.</param>
		private void subkey_Deleted(object sender, EventArgs e)
		{
			_subkeys.Remove((RegistryKey)sender);
		}

		/// <summary>
		/// Handles deletions of values in this RegistryKey.
		/// </summary>
		/// <param name="sender">The deleted value.</param>
		/// <param name="e">The ValueDeletedEventArgs instance containing the details of the deletion.</param>
		private void value_ValueDeleted(object sender, EventArgs e)
		{
			_values.Remove((RegistryValue)sender);
		}

		/// <summary>
		/// Handles changes in parent keys for subkeys in this RegistryKey.
		/// </summary>
		/// <param name="sender">The moved subkey.</param>
		/// <param name="e">The KeyMovedEventArgs instance containing the details of the move.</param>
		private void subkey_Moved(object sender, EventArgs e)
		{
			_subkeys.Remove((RegistryKey)sender);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Creates a RegistryKey with the full location of the key.
		/// </summary>
		/// <param name="fullName">The path and name of this registry key in the Windows Registry.</param>
		public RegistryKey(string fullName) : this(fullName, false)
		{
		}

		/// <summary>
		/// Creates a RegistryKey with the full location of the key, and optionally links the key to the Windows Registry.
		/// </summary>
		/// <param name="fullName">The path and name of this registry key in the Windows Registry.</param>
		/// <param name="linkedToRegistry">Specifies if this registry key is to be linked to the Windows Registry.</param>
		public RegistryKey(string fullName, bool linkedToRegistry)
		{
			if(fullName == null || fullName.Length == 0)
			{
				throw new ArgumentException("Argument \"fullName\" cannot be null or \"\".", fullName);
			}
			int marker = fullName.IndexOf("\\", 0);
			int lastMarker = fullName.LastIndexOf("\\", 0);
			if(marker != -1)
			{
				// TODO: Finish fullpath constructor
				// currently works for hives and top-level keys (handled without entering this block)
				_win32Hive = RegistryKey.ConvertStringToHive(fullName.Substring(0, marker));
				_name = fullName.Substring(lastMarker + 1);
			}
			else
			{
				_win32Hive = RegistryKey.ConvertStringToHive(fullName);
				_win32Key = GetWin32Key(_win32Hive);
				_name = fullName.Substring(lastMarker + 1);
				_isHive = true;
			}
			_subkeys = new RegistryKeyCollection(this);
			_values = new RegistryValueCollection(this);
			this.LinkedToRegistry = linkedToRegistry;
		}

		/// <summary>
		/// Creates a RegistryKey with the specified Win32Hive, branch, and name.
		/// </summary>
		/// <param name="win32Hive">The registry Win32Hive in which this key resides.</param>
		/// <param name="name">The name of this registry key.</param>
		public RegistryKey(Microsoft.Win32.RegistryHive win32Hive, string name) : this(win32Hive, name, false)
		{
		}

		/// <summary>
		/// Creates a RegistryKey with the specified Win32Hive, branch, and name, and optionally links this registry key to the Windows Registry.
		/// </summary>
		/// <param name="win32Hive">The registry Win32Hive in which this key resides.</param>
		/// <param name="name">The name of this registryKey.</param>
		/// <param name="linkedToRegistry">Specifies if this registry key is to be linked to the Windows Registry.</param>
		public RegistryKey(Microsoft.Win32.RegistryHive win32Hive, string name, bool linkedToRegistry)
		{
			_win32Hive = win32Hive;
			_win32Key = GetWin32Key(_win32Hive);
			_name = name;
			_subkeys = new RegistryKeyCollection(this);
			_values = new RegistryValueCollection(this);
			this.LinkedToRegistry = linkedToRegistry;
		}

		/// <summary>
		/// Creates a Sonneville.Registry.RegistryKey representing a Microsoft.Win32.RegistryKey.
		/// </summary>
		/// <param name="registryKey">The Microsoft.Win32.RegistryKey to represent.</param>
		/// <exception cref="InvalidOperationException">The specified Microsoft.Win32.RegistryKey does not exist.</exception>
		/// <exception cref="System.IO.IOException">A general error reading the Windows Registry.</exception>
		public RegistryKey(Microsoft.Win32.RegistryKey registryKey) : this(registryKey, false)
		{
		}

		/// <summary>
		/// Creates a Sonneville.Registry.RegistryKey representing a Microsoft.Win32.RegistryKey.
		/// </summary>
		/// <param name="registryKey">The Microsoft.Win32.RegistryKey to represent.</param>
		/// <param name="linkedToRegistry">Specifies if this registry key is to be linked to the Windows Registry.</param>
		public RegistryKey(Microsoft.Win32.RegistryKey registryKey, bool linkedToRegistry) : this(registryKey.Name, linkedToRegistry)
		{
		}

		/// <summary>
		/// Creates a RegistryKey designated as a RegistryHive.
		/// </summary>
		/// <param name="hive">The Registry Hive to represent.</param>
		private RegistryKey(Microsoft.Win32.RegistryHive hive) : this(hive, false)
		{
		}

		/// <summary>
		/// Creates a RegistryKey designated as a RegistryHive.
		/// </summary>
		/// <param name="hive">The Registry Hive to represent.</param>
		/// <param name="linkedToRegistry">Specifies if this registry key is to be linked to the Windows Registry.</param>
		private RegistryKey(Microsoft.Win32.RegistryHive hive, bool linkedToRegistry)
		{
			_isHive = true;
			_name = ConvertHiveToString(hive);
			_win32Hive = hive;
			_win32Key = GetWin32Key(_win32Hive);
			_subkeys = new RegistryKeyCollection(this);
			_values = new RegistryValueCollection(this);
			this.LinkedToRegistry = linkedToRegistry;
		}

		/// <summary>
		/// Creates a RegistryKey as a child of the parent RegistryKey.
		/// </summary>
		/// <param name="parent">The parent RegistryKey of this child RegistryKey.</param>
		/// <param name="name">The name of this child RegistryKey.</param>
		private RegistryKey(RegistryKey parent, string name) : this(parent, name, false)
		{
		}

		/// <summary>
		/// Creates a RegistryKey as a child of the parent RegistryKey.
		/// </summary>
		/// <param name="parent">The parent RegistryKey of this child RegistryKey.</param>
		/// <param name="name">The name of this child RegistryKey.</param>
		/// <param name="linkedToRegistry">Specifies if this registry key is to be linked to the Windows Registry.</param>
		private RegistryKey(RegistryKey parent, string name, bool linkedToRegistry)
		{
			_parent = parent;
			_isHive = false;
			_name = name;
			_win32Hive = _parent.Win32Hive;
			try
			{
				_win32Key = _parent._win32Key.OpenSubKey(name, false);
			}
			catch(System.Security.SecurityException)
			{
				_win32Key = null;
			}
			_subkeys = new RegistryKeyCollection(this);
			_values = new RegistryValueCollection(this);
			this.LinkedToRegistry = linkedToRegistry;
		}
		/// <summary>
		/// Gets a RegistryKey that serves as a registry hive.
		/// </summary>
		/// <param name="hive">The registry hive to create.</param>
		/// <returns>A RegistryKey that serves as a registry hive.</returns>
		public static RegistryKey GetHive(Microsoft.Win32.RegistryHive hive)
		{
			return RegistryKey.GetHive(hive, false);
		}

		/// <summary>
		/// Gets a RegistryKey that serves as a registry hive, and optionally links this registry key to the Windows Registry.
		/// </summary>
		/// <param name="hive">The registry hive to create.</param>
		/// <param name="linkedToRegistry">Specifies if this registry key is to be linked to the Windows Registry.</param>
		/// <returns>A RegistryKey that serves as a registry hive.</returns>
		public static RegistryKey GetHive(Microsoft.Win32.RegistryHive hive, bool linkedToRegistry)
		{
			return new RegistryKey(hive, linkedToRegistry);
		}

		private static Microsoft.Win32.RegistryKey GetWin32Key(Microsoft.Win32.RegistryHive hive)
		{
			switch(hive)
			{
				case Microsoft.Win32.RegistryHive.ClassesRoot:
					return Microsoft.Win32.Registry.ClassesRoot;
				case Microsoft.Win32.RegistryHive.CurrentUser:
					return Microsoft.Win32.Registry.CurrentUser;
				case Microsoft.Win32.RegistryHive.LocalMachine:
					return Microsoft.Win32.Registry.LocalMachine;
				case Microsoft.Win32.RegistryHive.Users:
					return Microsoft.Win32.Registry.Users;
				case Microsoft.Win32.RegistryHive.CurrentConfig:
					return Microsoft.Win32.Registry.CurrentConfig;
				case Microsoft.Win32.RegistryHive.DynData:
					return Microsoft.Win32.Registry.DynData;
				case Microsoft.Win32.RegistryHive.PerformanceData:
					return Microsoft.Win32.Registry.PerformanceData;
				default:
					throw new NotSupportedException("Win32 Registry Hive \"" + hive.ToString() + "\" not supported.");
			}
		}

		#endregion

		#region Delegates
		/// <summary>
		/// Handles a name change for a RegistryKey.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A KeyNameChangedEventArgs object that contains the event data.</param>
		public delegate void KeyNameChangedEventHandler(object sender, EventArgs e);
		/// <summary>
		/// Handles a subkey addition for a RegistryKey.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A SubkeyAddedEventArgs object that contains the event data.</param>
		public delegate void SubkeyAddedEventHandler(object sender, EventArgs e);
		/// <summary>
		/// Handles a deletion of a subkey.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A KeyDeletedEventArgs object that contains the event data.</param>
		public delegate void KeyDeletedEventHandler(object sender, EventArgs e);
		/// <summary>
		/// Handles a change in parent for a RegistryKey.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A KeyMovedEventArgs object that contains the event data.</param>
		public delegate void SubkeyMovedEventHandler(object sender, EventArgs e);
		/// <summary>
		/// Handles a value addition for a RegistryKey.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">A ValueAddedEventArgs object that contains the event data.</param>
		public delegate void ValueAddedEventHandler(object sender, EventArgs e);
		#endregion

		#region Events
		/// <summary>
		/// Occurs when a RegistryKey's name is changed.
		/// </summary>
		public event KeyNameChangedEventHandler NameChanged;
		/// <summary>
		/// Occurs when a RegistryKey is added as a subkey (child) to a parent RegistryKey.
		/// </summary>
		public event SubkeyAddedEventHandler SubkeyAdded;
		/// <summary>
		/// Occurs when a RegistryKey is deleted.
		/// </summary>
		public event KeyDeletedEventHandler Deleted;
		/// <summary>
		/// Occurs when a RegistryKey is moved to a different location.
		/// </summary>
		public event SubkeyMovedEventHandler Moved;
		/// <summary>
		/// Occurs when a RegistryValue is created in a RegistryKey.
		/// </summary>
		public event ValueAddedEventHandler ValueAdded;
		#endregion

		#region Naming Members

		/// <summary>
		/// The name of this registry key that identifies it in the parent key.
		/// </summary>
		/// <exception cref="FormatException">The specified name for this RegistryKey is blank.</exception>
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if(value == null || value.Length == 0)
				{
					throw new FormatException("Cannot rename key " + this.Name + ": The specified key name is empty.");
				}
				if(_isHive)
				{
					throw new InvalidOperationException("Cannot rename a registry hive");
				}
				string oldName = _name;
				_name = value;
				_win32Key = this.ToWin32Key();
				if(NameChanged != null)
				{
					NameChanged(this, new KeyNameChangedEventArgs(oldName, _name));
				}
			}
		}

		/// <summary>
		/// Gets the Win32 registry Win32Hive this key is located in.
		/// </summary>
		public Microsoft.Win32.RegistryHive Win32Hive
		{
			get
			{
				return _win32Hive;
			}
		}

		/// <summary>
		/// Gets the Win32Key copy of this RegistryKey.
		/// </summary>
		public Microsoft.Win32.RegistryKey Win32Key
		{
			get
			{
				return _win32Key;
			}
		}

		/// <summary>
		/// Gets the registry hive this key is located in as a string.
		/// </summary>
		public string Hive
		{
			get
			{
				return ConvertHiveToString(_win32Hive);
			}
		}

		// HKLM\Software\Microsoft\Windows\CurrentVersion
		// should return "Software\\Microsoft\\Windows"
		/// <summary>
		/// Gets the location of this RegistryKey in its Win32Hive.
		/// </summary>
		public string Branch
		{
			get
			{
				if(this._isHive)
				{
					return "";
				}
				else if(_parent._isHive)
				{
					return "";
				}
				return _parent.Branch + _parent.Name + "\\";
			}
		}

		/// <summary>
		/// Gets the full path and name of this RegistryKey.
		/// </summary>
		public string FullPath
		{
			get
			{
				if(_isHive)
				{
					return this.Name;
				}
				else if(_parent._isHive)
				{
					return this.Hive + "\\" + this.Name;
				}
				return this.Hive + "\\" + this.Branch + this.Name;
			}
		}

		/// <summary>
		/// Gets the parent key of this RegistryKey.
		/// </summary>
		private RegistryKey Parent
		{
			get
			{
				return _parent;
			}
		}

		#endregion

		#region Registry Interaction

		/// <summary>
		/// Removes this RegistryKey from the Windows Registry and deletes the RegistryKey.
		/// </summary>
		public void RemoveFromRegistry()
		{
			Microsoft.Win32.RegistryKey regKey = this.Parent.Win32Key;
			regKey.DeleteSubKeyTree(this.Name);
			this.Delete();
		}

		/// <summary>
		/// Specifies if this RegistryKey's subkeys and values have been read from the Registry.
		/// </summary>
		public bool HasBeenReadFromRegistry
		{
			get
			{
				return _read;
			}
		}

		/// <summary>
		/// Specifies if this RegistryKey is marked for deletion from the system registry (when saved to a RegistryFile).
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
		/// Imports the RegistryKey into the Windows Registry.
		/// </summary>
		public void Import()
		{
			Microsoft.Win32.RegistryKey regKey = this.ToWin32Key();
			foreach(RegistryValue regValue in this.Values)
			{
				regKey.SetValue(regValue.Name, regValue.Value);
			}
			foreach(RegistryKey subKey in this.Subkeys)
			{
				subKey.Import();
			}
		}

		/// <summary>
		/// Converts a Sonneville.Registry.RegistryKey to a Microsoft.Win32.RegistryKey.
		/// </summary>
		/// <returns>An array of Microsoft.Win32.RegistryKeys.</returns>
		public Microsoft.Win32.RegistryKey ToWin32Key()
		{
			if(_win32Key != null)
			{
				return _win32Key;
			}
			this._win32Key = _parent._win32Key.OpenSubKey(this.Name, true);
			return _win32Key;
		}

		private static Microsoft.Win32.RegistryKey GetSubkey(Microsoft.Win32.RegistryKey key, string subkeys)
		{
			if(subkeys.StartsWith("\\"))
			{
				subkeys = subkeys.Remove(0, 1);
			}
			int end = subkeys.IndexOf("\\");
			if(end < 0)
			{
				end = subkeys.Length;
				return key.OpenSubKey(subkeys);
			}
			return GetSubkey(key.OpenSubKey(subkeys.Substring(0, end)), subkeys.Substring(end));
		}

		/// <summary>
		/// Gets a string representing the full path of this Registry Key.
		/// </summary>
		/// <returns>A string representing the full path of this Registry Key.</returns>
		public override string ToString()
		{
			return this.FullPath;
		}

		#endregion

		#region Value Access

		/// <summary>
		/// Gets an array of subkeys contained in this RegistryKey.
		/// </summary>
		public RegistryKey[] Subkeys
		{
			get
			{
				return _subkeys.ToArray();
			}
		}

		/// <summary>
		/// Deletes all RegistryKeys contained as subkeys in this RegistryKey.
		/// </summary>
		public void DeleteAllSubkeys()
		{
			_subkeys = new RegistryKeyCollection(this);
		}

		/// <summary>
		/// Gets an array of values contained in this RegistryKey.
		/// </summary>
		public RegistryValue[] Values
		{
			get
			{
				return _values.ToArray();
			}
		}

		/// <summary>
		/// Deletes all RegistryValues contained in this RegistryKey.
		/// </summary>
		public void DeleteAllValues()
		{
			_values = new RegistryValueCollection(this);
		}

		/// <summary>
		/// Gets the default value of this RegistryKey.
		/// </summary>
		public RegistryValue DefaultValue
		{
			get
			{
				foreach(RegistryValue val in this.Values)
				{
					if(val.Name.Length == 0)
					{
						return val;
					}
				}
				throw new InvalidOperationException("RegistryKey does not contain a default value");
			}
		}

		/// <summary>
		/// Adds a RegistryKey as a subkey to this RegistryKey.
		/// </summary>
		/// <param name="subkey">The RegistryKey to add as a subkey.</param>
		public void AddKey(RegistryKey subkey)
		{
			if(subkey == null)
			{
				throw new NullReferenceException();
			}
//			if(_isHive)
//			{
//				throw new InvalidOperationException("Cannot add a subkey to a registry hive");
//			}
			if(SubkeyAdded != null)
			{
				SubkeyAdded(this, new SubkeyAddedEventArgs(subkey.Name));
			}
			_subkeys.Add(subkey);
			subkey.Deleted += new KeyDeletedEventHandler(subkey_Deleted);
			subkey.Moved += new SubkeyMovedEventHandler(subkey_Moved);
		}

		/// <summary>
		/// Adds a registry value to this RegistryKey.
		/// </summary>
		/// <param name="name">The name of the value to add.</param>
		/// <param name="type">The RegistryValueType type of the value to add.</param>
		/// <param name="data">The binary data (represented as a string) to add.</param>
		/// <exception cref="NotSupportedException">The specified RegistryValueType is unsupported by this version of the Registry API.</exception>
		public void AddValue(string name, RegistryValueType type, object data)
		{
			if(_isHive)
			{
				throw new InvalidOperationException("Cannot add a value to a registry hive");
			}
			if(name == null || name.Length == 0)
			{
				_values.RemoveValue("");
			}
			RegistryValue value;
			switch(type)
			{
				case RegistryValueType.ExpandableString:
				case RegistryValueType.String:
					value = new StringValue(this, name, (string)data);
					break;
				case RegistryValueType.Dword:
					value = new DwordValue(this, name, (uint)data);
					break;
				case RegistryValueType.MultiString:
					value = new MultiStringValue(this, name, (string[])data);
					break;
				case RegistryValueType.Binary:
					value = new BinaryValue(this, name, (byte[])data);
					break;
				default:
					throw new NotSupportedException("RegistryValueType not supported.");
			}
			_values.Add(value);
			value.ValueDeleted += new Sonneville.Registry.RegistryValue.ValueDeletedEventHandler(value_ValueDeleted);
			if(ValueAdded != null)
			{
				ValueAdded(this, new ValueAddedEventArgs(name, type, data));
			}
		}

		/// <summary>
		/// Adds a registry value to this RegistryKey.
		/// </summary>
		/// <param name="name">The name of the value to add.</param>
		/// <param name="data">The binary data (represented as a string) to add.</param>
		/// <exception cref="NotSupportedException">The specified RegistryValueType is unsupported by this version of the Registry API.</exception>
		public void AddValue(string name, object data)
		{
			if(_isHive)
			{
				throw new InvalidOperationException("Cannot add a value to a registry hive");
			}
			if(name == null || name.Length == 0)
			{
				_values.RemoveValue("");
			}
			RegistryValue value = null;
			byte[] bData = data as byte[];
			string[] saData = data as string[];
			string sData = data as string;
			if(bData != null)
			{
				value = new BinaryValue(this, name, bData);
			}
			else if(saData != null)
			{
				value = new MultiStringValue(this, name, saData);
			}
			else if(sData != null)
			{
				value = new StringValue(this, name, sData);
			}
			else if(data is int)
			{
				// must test for int AFTER string,
				// because int is not reference type;
				// can't use is on value types.
				value = new DwordValue(this, name, (int)data);
			}
			else
			{
				value = new StringValue(this, name, sData);
			}
			_values.Add(value);
			value.ValueDeleted += new Sonneville.Registry.RegistryValue.ValueDeletedEventHandler(value_ValueDeleted);
			if(ValueAdded != null)
			{
				ValueAdded(this, new ValueAddedEventArgs(value.Name, value.Type, value.Value));
			}
		}

		/// <summary>
		/// Deletes this RegistryKey and all its child keys and values.
		/// </summary>
		public void Delete()
		{
			if(this.Deleted != null)
			{
				this.Deleted(this, new KeyDeletedEventArgs(this.Name));
			}
			System.Threading.Thread t1 = new System.Threading.Thread(new System.Threading.ThreadStart(this.DoDelete));
			t1.Start();
		}

		private void DoDelete()
		{
			foreach(RegistryKey subkey in this.Subkeys)
			{
				subkey.DoDelete();
			}
			foreach(RegistryValue value in this.Values)
			{
				value.Delete();
			}
		}

		/// <summary>
		/// Removes the specified RegistryKey from this RegistryKey's list of subkeys.
		/// </summary>
		/// <param name="subkey">The RegistryKey to abandon.</param>
		/// <exception cref="Exception">The specified subkey was not found in this RegistryKey.</exception>
		public bool DeleteSubkey(RegistryKey subkey)
		{
			if(_isHive)
			{
				throw new InvalidOperationException("Cannot delete a default registry key");
			}
			if(_subkeys.Contains(subkey))
			{
				_subkeys.Delete(subkey);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Removes the specified RegistryValue from this RegistryKey's list of values.
		/// </summary>
		/// <param name="name">The name of the RegistryValue to delete.</param>
		/// <exception cref="System.Exception">Thrown when a RegistryValue with the specified name is not found in this RegistryKey.</exception>
		public bool DeleteValue(string name)
		{
			if(_values.Contains(name))
			{
				_values.RemoveValue(name);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Abandons this key's current parent key and makes this RegistryKey a child key under a new parent key.
		/// </summary>
		/// <param name="newParent">The new parent key of this RegistryKey.</param>
		/// <exception cref="Exception">The new parent of this RegistryKey is itself.</exception>
		public void Move(RegistryKey newParent)
		{
			if(newParent == null)
			{
				throw new NullReferenceException();
			}
			// just in case someone thinks they're funny...
			if(newParent.Equals(this))
			{
				throw new InvalidOperationException("Cannot add RegistryKey as a child to itself");
			}
			else
			{
				if(Moved != null)
				{
					Moved(this, new KeyMovedEventArgs(_win32Hive, newParent._win32Hive, _parent, newParent));
				}
				newParent.AddKey(this);
			}
		}

		/// <summary>
		/// Specifies if this RegistryKey is linked to the system registry. If true, all changes to this key will be reflected in the system registry.
		/// </summary>
		public bool LinkedToRegistry
		{
			get
			{
				return _linkedToRegistry;
			}
			set
			{
				if(value)
				{
					ReadFromRegistry(int.MaxValue);
				}
				_linkedToRegistry = value;
			}
		}

		#endregion

		#region Cast and equality operators

		/// <summary>
		/// Determines if two RegistryKeys are the same.
		/// </summary>
		/// <param name="leftHandSide">The RegistryKey on the left-hand side of this operator.</param>
		/// <param name="rightHandSide">The RegistryKey on the right-hand side of this operator.</param>
		/// <returns>True if the two RegistryKeys are the same, otherwise false.</returns>
		public static bool operator ==(RegistryKey leftHandSide, RegistryKey rightHandSide)
		{
			if(object.ReferenceEquals(leftHandSide, null) ^ object.ReferenceEquals(rightHandSide, null))
			{
				return false;
			}
			else if(object.ReferenceEquals(leftHandSide, null) && object.ReferenceEquals(rightHandSide, null))
			{
				return true;
			}
			else
			{
				return leftHandSide.FullPath == rightHandSide.FullPath;
			}
		}

		/// <summary>
		/// Determines if two RegistryKeys are not the same.
		/// </summary>
		/// <param name="leftHandSide">The RegistryKey on the left-hand side of this operator.</param>
		/// <param name="rightHandSide">The RegistryKey on the right-hand side of this operator.</param>
		/// <returns>True if the two RegistryKeys are different, otherwise false.</returns>
		public static bool operator !=(RegistryKey leftHandSide, RegistryKey rightHandSide)
		{
			return !(leftHandSide == rightHandSide);
		}

		/// <summary>
		/// Determines whether the specified System.Object is equal to the current Sonneville.Registry.RegistryKey.
		/// </summary>
		/// <param name="obj">The System.Object to compare to the current Sonneville.Registry.RegistryKey.</param>
		/// <returns>True if the specified System.Object is equal to the current Sonneville.Registry.RegistryKey; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			return (this == (RegistryKey)obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>A hash code for the current Sonneville.Registry.RegistryKey.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion

		/// <summary>
		/// Reads the RegistryKey and its subkeys from the Windows Registry.
		/// </summary>
		public void ReadFromRegistry()
		{
			ReadFromRegistry(0);
		}

		/// <summary>
		/// Reads the subkeys and values from the Windows Registry.
		/// </summary>
		/// <param name="levels">The number of sublevels to be read from the registry.</param>
		public void ReadFromRegistry(int levels)
		{
			if(levels >= 0)
			{
				this.DeleteAllSubkeys();
				this.DeleteAllValues();
				RegistryKey child = null;
				try
				{
					foreach(string subkey in this.ToWin32Key().GetSubKeyNames())
					{
						child = new RegistryKey(this, subkey);
						this.AddKey(child);
						child.ReadFromRegistry(levels - 1);
					}
					foreach(string valName in _win32Key.GetValueNames())
					{
						this.AddValue(valName, _win32Key.GetValue(valName));
					}
				}
				catch(System.Security.SecurityException)
				{
				}
				_read = true;
			}
		}
	}
}
