using System;
using System.Globalization;

namespace Sonneville.Registry
{
	/// <summary>
	/// A string, stored in a RegistryKey.
	/// </summary>
	public class StringValue : RegistryValue
	{
		private bool _expandable = false;

		/// <summary>
		/// A String Value in the Windows Registry.
		/// </summary>
		/// <param name="parent">The parent RegistryKey of this String Value.</param>
		/// <param name="name">The name of this String Value.</param>
		/// <param name="data">The string stored by this String Value.</param>
		internal StringValue(RegistryKey parent, string name, string data) : base(parent, name, RegistryValueType.String)
		{
			if(data != null)
			{
				Value = data;
			}
			else
			{
				base.BinaryData = new byte[2];
			}
		}

		/// <summary>
		/// A String Value in the Windows Registry.
		/// </summary>
		protected StringValue(RegistryKey parent, string name, RegistryValueType type) : base(parent, name, type)
		{
		}

		/// <summary>
		/// A String Value in the Windows Registry.
		/// </summary>
		protected StringValue(RegistryKey parent, string name, string data, RegistryValueType type) : base(parent, name, type)
		{
			if(data != null)
			{
				base.BinaryData = new byte[data.Length * 2];
				Value = data;
			}
			else
			{
				base.BinaryData = new byte[2];
			}
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
				this.Data = (string)value;
			}
		}

		/// <summary>
		/// Gets or sets the string contained in this String Value.
		/// </summary>
		public string Data
		{
			get
			{
				System.Text.StringBuilder builder = new System.Text.StringBuilder(base.BinaryData.Length / 2);
				short character = 0;
				for(int i = 0; i < base.BinaryData.Length - 2; i++)
				{
					switch(i % 2)
					{
						case 0:
							character += Convert.ToInt16(base.BinaryData[i]);
							builder.Append((char)character);
							break;
						case 1:
							character = Convert.ToInt16(base.BinaryData[i]);
							character <<= 8;
							break;
					}
				}
				builder.Replace(((char)0).ToString(), "\r\n");
				return builder.ToString();
			}
			set
			{
				if(value != null)
				{
					int end = value.Length;
					this.BinaryData = new byte[(end + 1) * 2];
					for(int i = 0; i < end; i++)
					{
						// checks for newlines
						// these must be replaced in the registry by zeros
						if(value[i] == '\n')
						{
							base.BinaryData[i * 2] = (byte)0;
							base.BinaryData[i * 2 + 1] = (byte)0;
						}
						else
						{
							this.BinaryData[i * 2] = (byte)(((int)value[i]) % 256);
							this.BinaryData[i * 2 + 1] = (byte)(((int)value[i]) / 256);
						}
					}
					this.BinaryData[end * 2] = (byte)0;
					this.BinaryData[end * 2 + 1] = (byte)0;
				}
				else
				{
					this.BinaryData = null;
				}
			}
		}

		/// <summary>
		/// Gets a string representation of the value contained.
		/// </summary>
		public override string ValueString
		{
			get
			{
				return this.Data;
			}
		}

		/// <summary>
		/// Gets a boolean value that specifies whether or not this StringValue is expandable.
		/// </summary>
		public bool IsExpandable
		{
			get
			{
				return _expandable;
			}
		}

		/// <summary>
		/// Gets the value of the environment variable stored in this expandable StringValue.
		/// </summary>
		public string EnvironmentValue
		{
			get
			{
				return Environment.GetEnvironmentVariable(this.EnvironmentVariable);
			}
		}

		/// <summary>
		/// Gets the environment variable stored in this expandable StringValue.
		/// </summary>
		public string EnvironmentVariable
		{
			get
			{
				if(this.IsExpandable)
				{
					string data = this.Data;
					foreach(string key in Environment.GetEnvironmentVariables())
					{
						if(data.IndexOf("%" + key + "%") != -1)
						{
							return key;
						}
					}
					throw new ApplicationException("Could not find a suitable key in the list of environment variables.");
				}
				else
				{
					throw new FormatException("Not an expandable StringValue.");
				}
			}
		}

		/// <summary>
		/// Returns a System.String that represents the current StringValue.
		/// </summary>
		/// <returns>A System.String that represents the current StringValue.</returns>
		public override string ToString()
		{
			return this.FullPath + " = " + this.Data;
		}

		internal override string FormatForExport()
		{
			return ((this.Name.Length == 0) ? "@=\"" : "\"" + this.Name + "\"=\"") + this.Data + "\"";
		}

		#region Cast and equality operators

		/// <summary>
		/// Determines whether the specified System.Object is equal to the current Sonneville.Registry.StringValue.
		/// </summary>
		/// <param name="obj">The System.Object to compare to the current Sonneville.Registry.StringValue.</param>
		/// <returns>True if the specified System.Object is equal to the current Sonneville.Registry.StringValue; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			return (this == (RegistryValue)obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>A hash code for the current Sonneville.Registry.StringValue.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
