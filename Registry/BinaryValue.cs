using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// An array of bytes, stored in a RegistryKey.
	/// </summary>
	public sealed class BinaryValue : Sonneville.Registry.RegistryValue
	{
		internal BinaryValue(RegistryKey parent, string name, byte[] data) : base(parent, name, RegistryValueType.Binary)
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
				this.Data = (byte[])value;
			}
		}

		/// <summary>
		/// The array of bytes contained in this BinaryValue.
		/// </summary>
		public byte[] Data
		{
			get
			{
				return BinaryData;
			}
			set
			{
				BinaryData = value;
			}
		}

		/// <summary>
		/// Gets a string representation of the value contained as an array of hexadecimal bytes.
		/// </summary>
		public override string ValueString
		{
			get
			{
				byte[] val = this.Data;
				if(val.Length > 0)
				{
					System.Text.StringBuilder builder = new System.Text.StringBuilder(val.Length * 3);
					string temp = ((int)this.BinaryData[0]).ToString("x");
					if(temp.Length < 2)
					{
						temp = "0" + temp;
					}
					builder.Append(temp);
					for(int i = 1; i < val.Length; i++)
					{
						builder.Append(" ");
						temp = ((int)this.BinaryData[i]).ToString("x");
						for(int j = temp.Length; j < 2; j++)
						{
							temp = "0" + temp;
						}
						builder.Append(temp);
					}
					return builder.ToString();
				}
				return "(zero-length binary value)";
			}
		}


		/// <summary>
		/// Returns a string that represents the current BinaryValue.
		/// </summary>
		/// <returns>A string that represents the current BinaryValue.</returns>
		public override string ToString()
		{
			return ((MultiStringValue)this).ToString();
		}

		internal override string FormatForExport()
		{
			string name;
			if(this.Name.Length == 0)
			{
				name = "@=hex:";
			}
			else
			{
				name = "\"" + this.Name + "\"=hex:";
			}
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			int total = name.Length;
			int count = this.Data.Length;
			for(int i = 0; i < count - 1; i++)
			{
				if(total > 77)
				{
					builder.Append("\\\r\n  ");
					total = 2;
				}
				builder.Append(this.Data[i].ToString("hh") + ",");
				total += 3;
			}
			builder.Append(this.Data[count].ToString("hh"));
			return name + builder.ToString();
		}

		#region Cast and equality operators

		/// <summary>
		/// Explicitly casts a BinaryValue to a StringValue.
		/// </summary>
		/// <param name="value">The BinaryValue to cast to a StringValue.</param>
		/// <returns>A StringValue with data taken from the BinaryValue.</returns>
		public static explicit operator StringValue(BinaryValue value)
		{
			if(value == null)
			{
				throw new NullReferenceException();
			}
			byte[] bytes = value.Data;
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			for(int i = 1; i < bytes.Length; i += 2)
			{
				uint character = 0;
				character += (uint)bytes[i - 1];
				character = character << 8;
				character += (uint)bytes[i];
				builder.Append((char)character);
			}
			return new StringValue(value.Parent, value.Name, builder.ToString());
		}

		/// <summary>
		/// Explicitly casts a BinaryValue to a DwordValue.
		/// </summary>
		/// <param name="value">The BinaryValue to cast to a DwordValue.</param>
		/// <returns>A DwordValue with data taken from the BinaryValue.</returns>
		public static explicit operator DwordValue(BinaryValue value)
		{
			if(value == null)
			{
				throw new NullReferenceException();
			}
			if(value.Data.Length == 4)
			{
				uint number = 0;
				for(int i = 0; i < 4; i++)
				{
					number += System.Convert.ToUInt32(value.Data[i].ToString(), 16);
					number = number << 8;
				}
				return new DwordValue(value.Parent, value.Name, number);
			}
			else
			{
				throw new FormatException("Excessive or insufficient binary data for DWORD value.");
			}
		}

		/// <summary>
		/// Explicitly casts a BinaryValue to a MultiStringValue.
		/// </summary>
		/// <param name="value">The BinaryValue to cast to a MultiStringValue.</param>
		/// <returns>A MultiStringValue with data taken from the BinaryValue</returns>
		public static explicit operator MultiStringValue(BinaryValue value)
		{
			if(value == null)
			{
				throw new NullReferenceException();
			}
			byte[] bytes = value.Data;
			System.Collections.ArrayList strings = new System.Collections.ArrayList();
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			for(int i = 1; i < bytes.Length; i += 2)
			{
				uint character = 0;
				character += (uint)bytes[i - 1];
				character = character << 8;
				character += (uint)bytes[i];
				if(character == 0)
				{
					strings.Add(builder.ToString());
					builder = new System.Text.StringBuilder();
				}
				else
				{
					builder.Append((char)character);
				}
			}
			return new MultiStringValue(value.Parent, value.Name, (string[])strings.ToArray());
		}

		/// <summary>
		/// Determines whether the specified System.Object is equal to the current Sonneville.Registry.BinaryValue.
		/// </summary>
		/// <param name="obj">The System.Object to compare to the current Sonneville.Registry.BinaryValue.</param>
		/// <returns>True if the specified System.Object is equal to the current Sonneville.Registry.BinaryValue; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			return (this == (RegistryValue)obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>A hash code for the current Sonneville.Registry.BinaryValue.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
