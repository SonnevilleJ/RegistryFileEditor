using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Sonneville.RegistryFileEditor
{
	/// <summary>
	/// The entry class for the application.
	/// </summary>
	public class RegistryFileEditor
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			// TODO: add in command-line support here, bypassing form creation
			Application.EnableVisualStyles();
			Application.DoEvents();
			Application.Run(new MainForm());
		}
	}
}