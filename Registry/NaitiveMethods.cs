using System;
using System.Runtime.InteropServices;

namespace Sonneville.Registry
{
	internal sealed class NativeMethods
	{
		private NativeMethods()
		{
		}
		
		[DllImport("advapi32.dll", CharSet=CharSet.Auto)]
		internal static extern int RegQueryValueEx(IntPtr hKey, string lpValueName, int[] lpReserved, ref int lpType, [Out] byte[] lpData, ref int lpcbData);
	}
}
