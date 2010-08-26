using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// An array of strings, stored in a RegistryKey.
	/// </summary>
	public sealed class MultiStringValue : Sonneville.Registry.StringValue
	{
		/// <summary>
		/// A Multi-String value in the Windows Registry.
		/// </summary>
		/// <param name="parent">The parent RegistryKey of this Multi-String Value.</param>
		/// <param name="name">The name of this Multi-String Value.</param>
		/// <param name="data">An array of strings contained in this Mulit-String Value.</param>
		internal MultiStringValue(RegistryKey parent, string name, string[] data) : base(parent, name, RegistryValueType.MultiString)
		{
			Value = data;
		}

		/// <summary>
		/// The value stored in this RegistryValue. 
		/// </summary>
		public override object Value
		{
			get
			{
				return (object)this.Data;
			}
			set
			{
				this.Data = (string[])value;
			}
		}

		/// <summary>
		/// Gets or sets the array of strings contained in this Multi-String Value.
		/// </summary>
		public new string[] Data
		{
			get
			{
				string[] temp = base.Data.Split('\n');
				for(int i = 0; i < temp.Length; i++)
				{
					temp[i] = temp[i].Replace("\r", "");
				}
				return temp;
			}
			set
			{
				if(value != null)
				{
					System.Text.StringBuilder builder = new System.Text.StringBuilder();
					for(int i = 0; i < value.Length; i++)
					{
						builder.Append(value[i] + "\n");
					}
					base.Value = builder.ToString();
				}
				else
				{
					base.Value = ("\n").ToString();
				}
			}
		}

		/// <summary>
		/// Gets a string representation of the value contained as an array of strings.
		/// </summary>
		public override string ValueString
		{
			get
			{
				string[] strings = this.Data;
				System.Text.StringBuilder builder = new System.Text.StringBuilder(strings[0]);
				for(int i = 1; i < strings.Length; i++)
				{
					builder.Append(" " + strings[i]);
				}
				return builder.ToString();
			}
		}


		/// <summary>
		/// Returns a System.String that represents the current MultiStringValue.
		/// </summary>
		/// <returns>A System.String that represents the current MultiStringValue.</returns>
		public override string ToString()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder(this.Data[0]);
			for(int i = 1; i < this.Data.Length - 1; i++)
			{
				builder.Append(" " + this.Data[i]);
			}
			return this.FullPath + " = " + builder.ToString();
		}

		internal override string FormatForExport()
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			for(int i = 0; i < this.Data.Length; i++)
			{
				builder.Append(this.Data[i] + "\r\n");
			}
			int[] bytes = new int[builder.Length * 2];
			for(int i = 0; i < bytes.Length; i += 2)
			{
				if(builder[i] == '\r' || builder[i] == '\n')
				{
					bytes[i * 2] = 0;
					bytes[i * 2 + 1] = 0;
				}
				else
				{
					bytes[i * 2] = (int)builder[i];
					bytes[i * 2 + 1] = 0;
				}
			}
			string name = ((this.Name.Length == 0) ? "@=hex(7):" : "\"" + this.Name + "\"=hex(7):");
			int total = name.Length;
			builder = new System.Text.StringBuilder();
			for(int i = 0; i < bytes.Length - 1; i++)
			{
				if(total > 77)
				{
					builder.Append("\\\r\n  ");
					total = 2;
				}
				builder.Append(bytes[i].ToString("hh") + ",");
				total += 3;
			}
			builder.Append(bytes[bytes.Length].ToString("hh"));
			return name + builder.ToString();
		}

		#region Cast and equality operators

		/// <summary>
		/// Determines whether the specified System.Object is equal to the current Sonneville.Registry.MultiStringValue.
		/// </summary>
		/// <param name="obj">The System.Object to compare to the current Sonneville.Registry.MultiStringValue.</param>
		/// <returns>True if the specified System.Object is equal to the current Sonneville.Registry.MultiStringValue; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			return (this == (RegistryValue)obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>A hash code for the current Sonneville.Registry.MultiStringValue.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
