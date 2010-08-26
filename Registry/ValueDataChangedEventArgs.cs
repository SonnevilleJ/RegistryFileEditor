using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// The EventArgs class for a RegistryValue data change.
	/// </summary>
	public class ValueDataChangedEventArgs : EventArgs
	{
		private object oldData;
		private object newData;

		/// <summary>
		/// Initializes a new instance of the ValueDataChangedEventArgs class.
		/// </summary>
		/// <param name="oldData">The previous data contained in this RegistryValue.</param>
		/// <param name="newData">The new data contained in this RegistryValue.</param>
		public ValueDataChangedEventArgs(object oldData, object newData)
		{
			this.oldData = oldData;
			this.newData = newData;
		}

		/// <summary>
		/// Gets the previous data contained in this RegistryValue.
		/// </summary>
		public object OldData
		{
			get
			{
				return this.oldData;
			}
		}

		/// <summary>
		/// Gets the new data contained in this RegistryValue.
		/// </summary>
		public object NewData
		{
			get
			{
				return this.newData;
			}
		}
	}
}
