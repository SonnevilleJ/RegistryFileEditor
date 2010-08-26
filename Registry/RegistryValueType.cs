using System;

namespace Sonneville.Registry
{
	/// <summary>
	/// The type of RegistryValue.
	/// </summary>
	public enum RegistryValueType
	{
		/// <summary>
		/// No specific value type.
		/// </summary>
		None = 0x0,
		/// <summary>
		/// A string of characters.
		/// </summary>
		String = 0x1,
		/// <summary>
		/// A string containing a variable to be replaced when called by an applicaton.
		/// </summary>
		ExpandableString = 0x2,
		/// <summary>
		/// A value who's binary contents can be edited.
		/// </summary>
		Binary = 0x3,
		/// <summary>
		/// A 32-bit (hexadecimal) number.
		/// </summary>
		Dword = 0x4,
		/// <summary>
		/// A 32-bit (hexadecimal) number in big-endian format.
		/// </summary>
		DwordBigEndian = 0x5,
		/// <summary>
		/// Reserved for system use.
		/// </summary>
		Link = 0x6,
		/// <summary>
		/// An array of string values.
		/// </summary>
		MultiString = 0x7,
		/// <summary>
		/// Reserved for operating system use.
		/// </summary>
		ResourceList = 0x8
	}
}