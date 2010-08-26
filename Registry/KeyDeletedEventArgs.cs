using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// The EventArgs class for a deleted RegistryKey.
	/// </summary>
	public class KeyDeletedEventArgs : EventArgs
	{
		private string name;

		/// <summary>
		/// Initializes a new instance of the KeyDeletedEventArgs class.
		/// </summary>
		/// <param name="name">The name of the deleted RegistryKey.</param>
		public KeyDeletedEventArgs(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Gets the name of the deleted RegistryKey.
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
		}
	}
}
