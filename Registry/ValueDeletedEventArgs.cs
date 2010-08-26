using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// The EventArgs class for a RegistryValue deletion.
	/// </summary>
	public class ValueDeletedEventArgs : EventArgs
	{
		private string name;
		private object data;

		/// <summary>
		/// Initializes a new instance of the ValueDeletedEventArgs class.
		/// </summary>
		/// <param name="name">The name of the deleted RegistryValue.</param>
		/// <param name="data">The object contained in the deleted RegistryValue.</param>
		public ValueDeletedEventArgs(string name, object data)
		{
			this.name = name;
			this.data = data;
		}

		/// <summary>
		/// Gets the name of the deleted value.
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>
		/// Gets the data of the deleted value.
		/// </summary>
		public object Value
		{
			get
			{
				return this.data;
			}
		}
	}
}
