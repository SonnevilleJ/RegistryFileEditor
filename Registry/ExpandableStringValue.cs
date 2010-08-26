using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// A string containing unexpanded references to environment variables, stored in a RegistryKey.
	/// </summary>
	public sealed class ExpandableStringValue : Sonneville.Registry.StringValue
	{
		/// <summary>
		/// An Expandable String Value in the Windows Registry.
		/// </summary>
		/// <param name="parent">The parent RegistryKey of this Expandable String Value.</param>
		/// <param name="name">The name of this Expandable String Value.</param>
		/// <param name="unexpandedData">The expandable string contained in this Expandable String Value.</param>
		internal ExpandableStringValue(RegistryKey parent, string name, string unexpandedData) : base(parent, name, unexpandedData)
		{
			this.UnexpandedData = unexpandedData;
		}

		/// <summary>
		/// The expanded string stored in this ExpandableStringValue.
		/// </summary>
		public string ExpandedValue
		{
			get
			{
				string data = this.Value;
				bool unexpanded = false;
				foreach(string key in Environment.GetEnvironmentVariables())
				{
					string value = System.Environment.GetEnvironmentVariable(key);
					if(value != null)
					{
						data = data.Replace(value, "%" + key + "%");
						unexpanded = true;
					}
				}
				if(!unexpanded)
				{
					throw new FormatException("Value cannot be unexpanded; no suitable environment variable found.");
				}
				return data;
			}
			set
			{
				int start = value.IndexOf("%");
				string key = value.Substring(start + 1, value.IndexOf("%", start + 1));
				string expanded = Environment.GetEnvironmentVariable(key);
				if(expanded != null)
				{
					value = value.Replace("%" + key + "%", expanded);
				}
				else
				{
					throw new FormatException("Environment variable not found; cannot unexpand variable.");
				}
				base.Value = value;
			}
		}

		/// <summary>
		/// Returns a System.String that represents the current ExpandableStringValue.
		/// </summary>
		/// <returns>A System.String that represents the current ExpandableStringValue.</returns>
		public override string ToString()
		{
			return this.FullPath + " = " + this.Value;
		}

		internal override string FormatForExport()
		{
			string name = ((this.Name == "") ? "@=hex(2):" : "\"" + this.Name + "\"=hex(2):");
			byte[] bytes = new byte[this.Value.Length * 2];
			for(int i = 0; i < this.Value.Length; i++)
			{
				bytes[i * 2] = (byte)(int)this.Value[i];
				bytes[i * 2 + 1] = 0;
			}
			int total = name.Length;
			string binary = "";
			for(int i = 0; i < bytes.Length - 1; i++)
			{
				if(total > 77)
				{
					binary += "\\\r\n  ";
					total = 2;
				}
				binary += bytes[i].ToString("hh") + ",";
				total += 3;
			}
			binary += bytes[bytes.Length].ToString("hh");
			return name + binary;
		}

		#region Cast and equality operators

		/// <summary>
		/// Determines whether the specified System.Object is equal to the current Sonneville.Registry.ExpandableStringValue.
		/// </summary>
		/// <param name="obj">The System.Object to compare to the current Sonneville.Registry.ExpandableStringValue.</param>
		/// <returns>True if the specified System.Object is equal to the current Sonneville.Registry.ExpandableStringValue; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			return (this == (RegistryValue)obj);
		}

		/// <summary>
		/// Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.
		/// </summary>
		/// <returns>A hash code for the current Sonneville.Registry.ExpandableStringValue.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion
	}
}
