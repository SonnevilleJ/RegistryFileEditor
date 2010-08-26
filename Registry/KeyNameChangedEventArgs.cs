using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// The EventArgs class for a change in a RegistryKey's name.
	/// </summary>
	public class KeyNameChangedEventArgs : EventArgs
	{
		private string oldName;
        private string newName;
		
		/// <summary>
		/// The arguments for a RegistryKey.NameChanged event.
		/// </summary>
		/// <param name="oldName">The previous name of this RegistryKey.</param>
		/// <param name="newName">The new name for this RegistryKey.</param>
		public KeyNameChangedEventArgs(string oldName, string newName)
		{
			this.oldName = oldName;
			this.newName = newName;
		}

		/// <summary>
		/// Gets the previous name of this RegistryKey.
		/// </summary>
		public string OldName
		{
			get
			{
				return this.oldName;
			}
		}

		/// <summary>
		/// Gets the new name for this RegistryKey.
		/// </summary>
		public string NewName
		{
			get
			{
				return this.newName;
			}
		}
	}
}
