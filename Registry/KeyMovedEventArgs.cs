using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// The EventArgs class for a moved RegistryKey.
	/// </summary>
	public class KeyMovedEventArgs : EventArgs
	{
		RegistryKey oldParent;
		RegistryKey newParent;
		Microsoft.Win32.RegistryHive oldHive;
		Microsoft.Win32.RegistryHive newHive;

		/// <summary>
		/// Initializes a new instance of the KeyMovedEventArgs class.
		/// </summary>
		/// <param name="oldHive">The previous registry hive of the moved RegistryKey.</param>
		/// <param name="newHive">The new registry hive of the moved RegistryKey.</param>
		/// <param name="oldKey">The previous parent key of the moved RegistryKey.</param>
		/// <param name="newKey">The new parent key of the moved RegistryKey.</param>
		public KeyMovedEventArgs(Microsoft.Win32.RegistryHive oldHive, Microsoft.Win32.RegistryHive newHive, RegistryKey oldKey, RegistryKey newKey)
		{
			this.oldHive = oldHive;
			this.newHive = newHive;
			this.oldParent = oldKey;
			this.newParent = newKey;
		}

		/// <summary>
		/// Gets the previous registry hive of the moved RegistryKey.
		/// </summary>
		public Microsoft.Win32.RegistryHive OldHive
		{
			get
			{
				return this.oldHive;
			}
		}

		/// <summary>
		/// Gets the new registry hive of the moved RegistryKey.
		/// </summary>
		public Microsoft.Win32.RegistryHive NewHive
		{
			get
			{
				return this.newHive;
			}
		}

		/// <summary>
		/// Gets the previous parent key of the moved RegistryKey.
		/// </summary>
		public RegistryKey OldParent
		{
			get
			{
				return this.oldParent;
			}
		}

		/// <summary>
		/// Gets the new parent key of the moved RegistryKey.
		/// </summary>
		public RegistryKey NewParent
		{
			get
			{
				return this.newParent;
			}
		}
	}
}
