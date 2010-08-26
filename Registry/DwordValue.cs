using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// A 32-bit unsigned integer, stored in a RegistryKey.
	/// </summary>
	public sealed class DwordValue : RegistryValue
	{
		/// <summary>
		/// A DWORD Value in the Windows Registry.
		/// </summary>
		/// <param name="parent">The parent RegistryKey of this DWORD Value.</param>
		/// <param name="name">The name of this DWORD Value.</param>
		/// <param name="data">The 32-bit unsigned integer stored in this DWORD Value.</param>
		internal DwordValue(RegistryKey parent, string name, uint data) : base(parent, name, RegistryValueType.Dword)
		{
			UnsignedValue = data;
		}

		/// <summary>
		/// A DWORD Value in the Windows Registry.
		/// </summary>
		/// <param name="parent">The parent RegistryKey of this DWORD Value.</param>
		/// <param name="name">The name of this DWORD Value.</param>
		/// <param name="data">The 32-bit integer stored in this DWORD Value.</param>
		internal DwordValue(RegistryKey parent, string name, int data) : base(parent, name, RegistryValueType.Dword)
		{
			Value = data;
		}

		/// <summary>
		/// Gets or sets the 32-bit integer contained in this DWORD Value as an unsigned integer.
		/// </summary>
		[CLSCompliantAttribute(false)]
		public uint UnsignedValue
		{
			get
			{
				if(base.BinaryData.Length == 4)
				{
					uint val = 0;
					for (int i = 0; i < 4; i++)
					{
						val += Convert.ToUInt32(((double)BinaryData[i] * Math.Pow(256, i)));
					}
					return val;
				}
				else
				{
					throw new FormatException("Not a DWORD Value: more or less than 4 bytes of data.");
				}
			}
			set
			{
				base.BinaryData = new byte[4];
				for(int i = 0; i < 4; i++)
				{
					base.BinaryData[i] = (byte)(value & 255);
					value >>= 8;
				}
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
				this.Data = (int)value;
			}
		}

		/// <summary>
		/// Gets or sets the 32-bit integer contained in this DWORD Value.
		/// </summary>
		public int Data
		{
			get
			{
				if(base.BinaryData.Length == 4)
				{
					int value = 0;
					for(int i = 0; i < 4; i++)
					{
						value += Convert.ToInt32((double)BinaryData[i] * Math.Pow(256, i));
					}
					return value;
				}
				else
				{
					throw new FormatException("Not a DWORD Value: more or less than 4 bytes of data.");
				}
			}
			set
			{
				this.BinaryData = new byte[4];
				for(int i = 0; i < 4; i++)
				{
					base.BinaryData[i] = (byte)(value & 255);
					value >>= 8;
				}
			}
		}

		/// <summary>
		/// Gets a string representation of the value contained as a 32-bit hexadecimal number.
		/// </summary>
		public override string ValueString
		{
			get
			{
				System.Text.StringBuilder builder = new System.Text.StringBuilder("0x");
				uint val = this.UnsignedValue;
				string temp = val.ToString("x");
				for(int i = 0; i < 8 - temp.Length; i++)
				{
					builder.Append("0");
				}
				return builder.ToString() + temp + " (" + val + ")";
			}
		}


		/// <summary>
		/// Returns a System.String that represents the current Sonneville.Registry.DwordValue.
		/// </summary>
		/// <returns>A System.String that represents the current Sonneville.Registry.DwordValue.</returns>
		public override string ToString()
		{
			return this.FullPath + " = " + this.Data.ToString();
		}

		internal override string FormatForExport()
		{
			return ((this.Name.Length == 0) ? "@=dword:" : "\"" + this.Name + "\"=dword:") + this.Data.ToString("hhhhhhhh");
		}
		
		#region Cast and equality operators

		/// <summary>
		/// Determines whether the specified System.Object is equal to the current Sonneville.Registry.DwordValue.
		/// </summary>
		/// <param name="obj">The System.Object to compare to the current Sonneville.Registry.DwordValue.</param>
		/// <returns>True if the specified System.Object is equal to the current Sonneville.Registry.DwordValue; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			return (this == (RegistryValue)obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>A hash code for the current Sonneville.Registry.DwordValue.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
