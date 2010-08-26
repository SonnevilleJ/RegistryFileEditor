using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// The EventArgs class for a subkey addition.
	/// </summary>
	public class SubkeyAddedEventArgs : EventArgs
	{
		private string name;

		/// <summary>
		/// Initializes a new instance of the SubkeyAddedEventArgs class
		/// </summary>
		/// <param name="name">The name of the new subkey.</param>
		public SubkeyAddedEventArgs(string name)
		{
			this.name = name;
		}

		/// <summary>
		/// Gets the name of the newly added subkey.
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
