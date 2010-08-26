using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// The EventArgs class for a new RegistryValue.
	/// </summary>
	public class ValueAddedEventArgs : EventArgs
	{
		private string name;
		private RegistryValueType type;
		private object data;

		/// <summary>
		/// Initializes a new instance of the ValueAddedEventArgs class.
		/// </summary>
		/// <param name="name">The name of the new RegistryValue.</param>
		/// <param name="type">The RegistryValueType of the new RegistryValue.</param>
		/// <param name="data">The data contained in the new RegistryValue.</param>
		public ValueAddedEventArgs(string name, RegistryValueType type, object data)
		{
			this.name = name;
			this.type = type;
			this.data = data;
		}

		/// <summary>
		/// Gets the name of the new RegistryValue.
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>
		/// Gets the type of the new RegistryValue.
		/// </summary>
		public RegistryValueType Type
		{
			get
			{
				return this.type;
			}
		}

		/// <summary>
		/// Gets the data contained in the new RegistryValue.
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
