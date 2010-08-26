using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// The EventArgs class for a RegsitryValue name change.
	/// </summary>
	public class ValueNameChangedEventArgs : EventArgs
	{
		private string oldName;
		private string newName;

		/// <summary>
		/// Initializes a new instance of the ValueNameChangedEventArgs class.
		/// </summary>
		/// <param name="oldName">The previous name of this RegistryValue.</param>
		/// <param name="newName">The new name for this RegistryValue.</param>
		public ValueNameChangedEventArgs(string oldName, string newName)
		{
			this.oldName = oldName;
			this.newName = newName;
		}

		/// <summary>
		/// Gets the previous name of this RegistryValue.
		/// </summary>
		public string OldName
		{
			get
			{
				return this.oldName;
			}
		}

		/// <summary>
		/// Gets the new name for this RegistryValue.
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
