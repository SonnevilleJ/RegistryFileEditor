using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using Sonneville.Registry;

namespace Sonneville.RegistryFileEditor
{
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.ImageList listViewSmallImages;
		private System.Windows.Forms.ImageList listViewLargeImages;
		private System.Windows.Forms.ImageList treeViewImages;
		private System.Windows.Forms.StatusBar statusBar;
		private System.ComponentModel.IContainer components;

		#region Menu

		private System.Windows.Forms.MainMenu mainMenu;
		private System.Windows.Forms.MenuItem File;
		private System.Windows.Forms.MenuItem FileNew;
		private System.Windows.Forms.MenuItem FileOpen;
		private System.Windows.Forms.MenuItem FileClose;
		private System.Windows.Forms.MenuItem FileSave;
		private System.Windows.Forms.MenuItem FileSaveAs;
		private System.Windows.Forms.MenuItem FileSeperator;
		private System.Windows.Forms.MenuItem FileExit;
		private System.Windows.Forms.MenuItem Edit;
		private System.Windows.Forms.MenuItem EditCut;
		private System.Windows.Forms.MenuItem EditCopy;
		private System.Windows.Forms.MenuItem EditPaste;
		private System.Windows.Forms.MenuItem EditDelete;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem EditSelectAll;
		private System.Windows.Forms.MenuItem EditModify;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem View;
		private System.Windows.Forms.MenuItem ViewLargeIcons;
		private System.Windows.Forms.MenuItem ViewSmallIcons;
		private System.Windows.Forms.MenuItem ViewList;
		private System.Windows.Forms.MenuItem Help;
		private System.Windows.Forms.MenuItem HelpAbout;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.ContextMenu cmValue;
		private System.Windows.Forms.ContextMenu cmKey;
		private System.Windows.Forms.MenuItem ViewDetails;

		#region File
		private void File_Popup(object sender, System.EventArgs e)
		{
			if (((string)tabControl.SelectedTab.Tag) == "LocalReg")
			{
				FileClose.Enabled = false;
				FileSave.Enabled = false;
				FileSaveAs.Enabled = false;
			}
			else
			{
				FileClose.Enabled = true;
				FileSave.Enabled = true;
				FileSaveAs.Enabled = true;
			}
		}

		private void FileNew_Click(object sender, System.EventArgs e)
		{
			CreateNewTab(false);
		}

		private void FileExit_Click(object sender, System.EventArgs e)
		{
			Application.Exit();
		}

		private void FileClose_Click(object sender, System.EventArgs e)
		{
			this.tabControl.TabPages.Remove(this.tabControl.SelectedTab);
		}
		#endregion

		#region View
		private void ViewLargeIcons_Click(object sender, System.EventArgs e)
		{
			ChangeView(System.Windows.Forms.View.LargeIcon);
		}

		private void ViewSmallIcons_Click(object sender, System.EventArgs e)
		{
			ChangeView(System.Windows.Forms.View.SmallIcon);
		}

		private void ViewList_Click(object sender, System.EventArgs e)
		{
			ChangeView(System.Windows.Forms.View.List);
		}

		private void ViewDetails_Click(object sender, System.EventArgs e)
		{
			ChangeView(System.Windows.Forms.View.Details);
		}
		#endregion

		#endregion


		/// <summary>
		/// Creates a new main window for the application
		/// </summary>
		public MainForm()
		{
			this.Cursor = Cursors.AppStarting;
			InitializeComponent();
			statusBar.BringToFront();
			CreateNewTab(true);
			this.Cursor = Cursors.Default;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.File = new System.Windows.Forms.MenuItem();
			this.FileNew = new System.Windows.Forms.MenuItem();
			this.FileOpen = new System.Windows.Forms.MenuItem();
			this.FileClose = new System.Windows.Forms.MenuItem();
			this.FileSave = new System.Windows.Forms.MenuItem();
			this.FileSaveAs = new System.Windows.Forms.MenuItem();
			this.FileSeperator = new System.Windows.Forms.MenuItem();
			this.FileExit = new System.Windows.Forms.MenuItem();
			this.Edit = new System.Windows.Forms.MenuItem();
			this.EditModify = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.EditCut = new System.Windows.Forms.MenuItem();
			this.EditCopy = new System.Windows.Forms.MenuItem();
			this.EditPaste = new System.Windows.Forms.MenuItem();
			this.EditDelete = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.EditSelectAll = new System.Windows.Forms.MenuItem();
			this.View = new System.Windows.Forms.MenuItem();
			this.ViewLargeIcons = new System.Windows.Forms.MenuItem();
			this.ViewSmallIcons = new System.Windows.Forms.MenuItem();
			this.ViewList = new System.Windows.Forms.MenuItem();
			this.ViewDetails = new System.Windows.Forms.MenuItem();
			this.Help = new System.Windows.Forms.MenuItem();
			this.HelpAbout = new System.Windows.Forms.MenuItem();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.treeViewImages = new System.Windows.Forms.ImageList(this.components);
			this.listViewSmallImages = new System.Windows.Forms.ImageList(this.components);
			this.listViewLargeImages = new System.Windows.Forms.ImageList(this.components);
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.cmValue = new System.Windows.Forms.ContextMenu();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.cmKey = new System.Windows.Forms.ContextMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItem18 = new System.Windows.Forms.MenuItem();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.menuItem21 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.File,
																					 this.Edit,
																					 this.View,
																					 this.Help});
			// 
			// File
			// 
			this.File.Index = 0;
			this.File.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				 this.FileNew,
																				 this.FileOpen,
																				 this.FileClose,
																				 this.FileSave,
																				 this.FileSaveAs,
																				 this.FileSeperator,
																				 this.FileExit});
			this.File.Text = "&File";
			this.File.Popup += new System.EventHandler(this.File_Popup);
			// 
			// FileNew
			// 
			this.FileNew.Index = 0;
			this.FileNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.FileNew.Text = "&New";
			this.FileNew.Click += new System.EventHandler(this.FileNew_Click);
			// 
			// FileOpen
			// 
			this.FileOpen.Index = 1;
			this.FileOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.FileOpen.Text = "&Open...";
			this.FileOpen.Click += new System.EventHandler(this.FileOpen_Click);
			// 
			// FileClose
			// 
			this.FileClose.Index = 2;
			this.FileClose.Text = "&Close";
			this.FileClose.Click += new System.EventHandler(this.FileClose_Click);
			// 
			// FileSave
			// 
			this.FileSave.Enabled = false;
			this.FileSave.Index = 3;
			this.FileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.FileSave.Text = "&Save";
			// 
			// FileSaveAs
			// 
			this.FileSaveAs.Enabled = false;
			this.FileSaveAs.Index = 4;
			this.FileSaveAs.Text = "Save &As...";
			// 
			// FileSeperator
			// 
			this.FileSeperator.Index = 5;
			this.FileSeperator.Text = "-";
			// 
			// FileExit
			// 
			this.FileExit.Index = 6;
			this.FileExit.Text = "E&xit";
			this.FileExit.Click += new System.EventHandler(this.FileExit_Click);
			// 
			// Edit
			// 
			this.Edit.Index = 1;
			this.Edit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				 this.EditModify,
																				 this.menuItem8,
																				 this.EditCut,
																				 this.EditCopy,
																				 this.EditPaste,
																				 this.EditDelete,
																				 this.menuItem6,
																				 this.EditSelectAll});
			this.Edit.Text = "&Edit";
			// 
			// EditModify
			// 
			this.EditModify.Index = 0;
			this.EditModify.Text = "&Modify";
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "-";
			// 
			// EditCut
			// 
			this.EditCut.Index = 2;
			this.EditCut.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.EditCut.Text = "Cu&t";
			// 
			// EditCopy
			// 
			this.EditCopy.Index = 3;
			this.EditCopy.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.EditCopy.Text = "&Copy";
			// 
			// EditPaste
			// 
			this.EditPaste.Index = 4;
			this.EditPaste.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.EditPaste.Text = "&Paste";
			// 
			// EditDelete
			// 
			this.EditDelete.Index = 5;
			this.EditDelete.Shortcut = System.Windows.Forms.Shortcut.Del;
			this.EditDelete.Text = "&Delete";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 6;
			this.menuItem6.Text = "-";
			// 
			// EditSelectAll
			// 
			this.EditSelectAll.Index = 7;
			this.EditSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.EditSelectAll.Text = "Select &All";
			// 
			// View
			// 
			this.View.Index = 2;
			this.View.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				 this.ViewLargeIcons,
																				 this.ViewSmallIcons,
																				 this.ViewList,
																				 this.ViewDetails});
			this.View.Text = "&View";
			// 
			// ViewLargeIcons
			// 
			this.ViewLargeIcons.Index = 0;
			this.ViewLargeIcons.Text = "Large Icons";
			this.ViewLargeIcons.Click += new System.EventHandler(this.ViewLargeIcons_Click);
			// 
			// ViewSmallIcons
			// 
			this.ViewSmallIcons.Index = 1;
			this.ViewSmallIcons.Text = "&Small Icons";
			this.ViewSmallIcons.Click += new System.EventHandler(this.ViewSmallIcons_Click);
			// 
			// ViewList
			// 
			this.ViewList.Index = 2;
			this.ViewList.Text = "&List";
			this.ViewList.Click += new System.EventHandler(this.ViewList_Click);
			// 
			// ViewDetails
			// 
			this.ViewDetails.Index = 3;
			this.ViewDetails.Text = "&Details";
			this.ViewDetails.Click += new System.EventHandler(this.ViewDetails_Click);
			// 
			// Help
			// 
			this.Help.Index = 3;
			this.Help.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				 this.HelpAbout});
			this.Help.Text = "&Help";
			// 
			// HelpAbout
			// 
			this.HelpAbout.Index = 0;
			this.HelpAbout.Text = "&About";
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Location = new System.Drawing.Point(8, 8);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(560, 376);
			this.tabControl.TabIndex = 0;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "reg";
			this.saveFileDialog.Filter = "Registry Entry Files (*.reg)|*.reg";
			this.saveFileDialog.Title = "Save Registry File";
			// 
			// openFileDialog
			// 
			this.openFileDialog.DefaultExt = "reg";
			this.openFileDialog.Filter = "Registry Entry Files (*.reg)|*.reg";
			this.openFileDialog.Title = "Open Registry File";
			// 
			// treeViewImages
			// 
			this.treeViewImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.treeViewImages.ImageSize = new System.Drawing.Size(16, 16);
			this.treeViewImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeViewImages.ImageStream")));
			this.treeViewImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// listViewSmallImages
			// 
			this.listViewSmallImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.listViewSmallImages.ImageSize = new System.Drawing.Size(16, 16);
			this.listViewSmallImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("listViewSmallImages.ImageStream")));
			this.listViewSmallImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// listViewLargeImages
			// 
			this.listViewLargeImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.listViewLargeImages.ImageSize = new System.Drawing.Size(32, 32);
			this.listViewLargeImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("listViewLargeImages.ImageStream")));
			this.listViewLargeImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 392);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(576, 22);
			this.statusBar.TabIndex = 1;
			// 
			// cmValue
			// 
			this.cmValue.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.menuItem13,
																					this.menuItem14,
																					this.menuItem15,
																					this.menuItem16});
			// 
			// menuItem13
			// 
			this.menuItem13.DefaultItem = true;
			this.menuItem13.Index = 0;
			this.menuItem13.Text = "&Modify";
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 1;
			this.menuItem14.Text = "-";
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 2;
			this.menuItem15.Shortcut = System.Windows.Forms.Shortcut.Del;
			this.menuItem15.Text = "&Delete";
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 3;
			this.menuItem16.Shortcut = System.Windows.Forms.Shortcut.F2;
			this.menuItem16.Text = "&Rename";
			// 
			// cmKey
			// 
			this.cmKey.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																				  this.menuItem1,
																				  this.menuItem2,
																				  this.menuItem12,
																				  this.menuItem17,
																				  this.menuItem18,
																				  this.menuItem19,
																				  this.menuItem20,
																				  this.menuItem21});
			this.cmKey.Popup += new System.EventHandler(this.cmKey_Popup);
			// 
			// menuItem1
			// 
			this.menuItem1.DefaultItem = true;
			this.menuItem1.Index = 0;
			this.menuItem1.Text = "Expand";
			this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem3,
																					  this.menuItem4,
																					  this.menuItem5,
																					  this.menuItem7,
																					  this.menuItem9,
																					  this.menuItem10,
																					  this.menuItem11});
			this.menuItem2.Text = "&New";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 0;
			this.menuItem3.Text = "Key";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.Text = "-";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 2;
			this.menuItem5.Text = "&String Value";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 3;
			this.menuItem7.Text = "&Binary Value";
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 4;
			this.menuItem9.Text = "&DWORD (32-bit) Value";
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 5;
			this.menuItem10.Text = "&Multi-String Value";
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 6;
			this.menuItem11.Text = "&Expandable String Value";
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 2;
			this.menuItem12.Text = "&Find...";
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 3;
			this.menuItem17.Text = "-";
			// 
			// menuItem18
			// 
			this.menuItem18.Index = 4;
			this.menuItem18.Shortcut = System.Windows.Forms.Shortcut.Del;
			this.menuItem18.Text = "&Delete";
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 5;
			this.menuItem19.Shortcut = System.Windows.Forms.Shortcut.F2;
			this.menuItem19.Text = "&Rename";
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 6;
			this.menuItem20.Text = "-";
			// 
			// menuItem21
			// 
			this.menuItem21.Index = 7;
			this.menuItem21.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.menuItem21.Text = "&Copy Key Name";
			// 
			// MainForm
			// 
			this.AllowDrop = true;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(576, 414);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.tabControl);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "Registry File Editor";
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Creates a new tab with a blank, unbound registry file.
		/// </summary>
		private void CreateNewTab()
		{
			CreateNewTab(false);
		}

		/// <summary>
		/// Creates a new tab with a blank, unbound registry file.
		/// </summary>
		/// <param name="LocalReg">Specifies if this tab is a copy of the Local Registry.</param>
		private void CreateNewTab(bool LocalReg)
		{
			this.SuspendLayout();
			// prepare new tab
			TabPage TP;
			if(LocalReg)
			{
				TP = new TabPage("Local Registry");
				TP.Tag = "LocalReg";
			}
			else
			{
				TP = new TabPage("new file.reg");
			}

			// set up basic TreeView properties
			TreeView treeView = new TreeView();
			treeView.Name = "tv";
			treeView.Dock = DockStyle.Left;
			treeView.ImageList = treeViewImages;
			treeView.Width = 190;
			treeView.ContextMenu = cmKey;

			// delegate to update ListView contents when TreeView's selected node changes
			treeView.AfterSelect += new TreeViewEventHandler(treeView_AfterSelect);
			treeView.BeforeExpand += new TreeViewCancelEventHandler(treeView_BeforeExpand);

			// delegate to capture clicks on TreeView
			treeView.MouseUp +=new MouseEventHandler(treeView_MouseUp);
			treeView.KeyUp += new KeyEventHandler(treeView_KeyUp);

			// prepare inner controls for new tab
			ListView listView = new ListView();
			listView.Name = "lv";
			listView.View = System.Windows.Forms.View.Details;
			listView.Sorting = SortOrder.Ascending;
			listView.Dock = DockStyle.Fill;
			listView.SmallImageList = listViewSmallImages;
			listView.LargeImageList = listViewLargeImages;
			listView.Columns.Add("Name", 100, HorizontalAlignment.Left);
			listView.Columns.Add("Type", 85, HorizontalAlignment.Left);
			listView.Columns.Add("Data", 160, HorizontalAlignment.Left);

			// assign the ListView on the same TabPage to the TreeView's Tag property.
			// this ensures the delegates have access to the ListView to update values, etc.
			treeView.Tag = listView;

			Splitter splitter = new Splitter();
			splitter.Width = 5;
			splitter.Dock = DockStyle.Left;

			// add new controls to the TabPage
			TP.Controls.Add(splitter);
			TP.Controls.Add(treeView);
			TP.Controls.Add(listView);

			
			// finalize inner controls and select tab
			tabControl.TabPages.Add(TP);
			listView.BringToFront();
			ChangeView(System.Windows.Forms.View.Details);
			tabControl.SelectedTab = TP;
			ResetDefaultKeys(treeView);
			this.ResumeLayout(false);
		}

		/// <summary>
		/// Creates a new tab tied to a RegistryFile and populates it with data from the file.
		/// </summary>
		/// <param name="path">The full path to the Windows Registry File (*.reg) linked to the tab.</param>
		private void CreateNewTab(string path)
		{
			System.Windows.Forms.MessageBox.Show("Opening file: " + path);
		}

		private void ResetDefaultKeys(TreeView tv)
		{
			TreeNode myComputer = new TreeNode("My Computer", 0, 0);
			myComputer.Tag = "MyComputer";

			TreeNode HKCR = new TreeNode("HKEY_CLASSES_ROOT", 1, 2);
			TreeNode HKCU = new TreeNode("HKEY_CURRENT_USER", 1, 2);
			TreeNode HKLM = new TreeNode("HKEY_LOCAL_MACHINE", 1, 2);
			TreeNode HKU = new TreeNode("HKEY_USERS", 1, 2);
			TreeNode HKCC = new TreeNode("HKEY_CURRENT_CONFIG", 1, 2);

			myComputer.Nodes.Add(HKCR);
			myComputer.Nodes.Add(HKCU);
			myComputer.Nodes.Add(HKLM);
			myComputer.Nodes.Add(HKU);
			myComputer.Nodes.Add(HKCC);

			tv.Nodes.Clear();
			tv.Nodes.Add(myComputer);
			myComputer.Expand();

			RegistryKey rkHKCR = new RegistryKey(Microsoft.Win32.Registry.ClassesRoot);
			RegistryKey rkHKCU = new RegistryKey(Microsoft.Win32.Registry.CurrentUser);
			RegistryKey rkHKLM = new RegistryKey(Microsoft.Win32.Registry.LocalMachine);
			RegistryKey rkHKU = new RegistryKey(Microsoft.Win32.Registry.Users);
			RegistryKey rkHKCC = new RegistryKey(Microsoft.Win32.Registry.CurrentConfig);

			HKCR.Tag = rkHKCR;
			HKCU.Tag = rkHKCU;
			HKLM.Tag = rkHKLM;
			HKU.Tag = rkHKU;
			HKCC.Tag = rkHKCC;

			BuildTree(rkHKCR, HKCR, 0);
			BuildTree(rkHKCU, HKCU, 0);
			BuildTree(rkHKLM, HKLM, 0);
			BuildTree(rkHKU, HKU, 0);
			BuildTree(rkHKCC, HKCC, 0);
		}

		/// <summary>
		/// Creates TreeNodes for each subkey in a RegistryKey.
		/// </summary>
		/// <param name="key">The RegistryKey containing the subkeys to represent with TreeNodes.</param>
		/// <param name="node">The TreeNode to add the new TreeNodes to.</param>
		/// <param name="levels">How many node levels deep to build to the TreeNode structure.</param>
		/// recursive conditions for levels:
		/// -1: stop
		///  0: build subnodes for current level only - don't recurse
		/// >0: build subnodes for nodes levels deep
		private void BuildTree(RegistryKey key, TreeNode node, int levels)
		{
			if(levels > -1)
			{
				if(!key.HasBeenReadFromRegistry)
				{
					key.ReadFromRegistry(0);
				}
				foreach(RegistryKey subkey in key.Subkeys)
				{
					TreeNode subnode = new TreeNode(subkey.Name, 1, 2);
					subnode.Tag = subkey;
					BuildTree(subkey, subnode, levels - 1);
					node.Nodes.Add(subnode);
				}
			}
		}

		private void ChangeView(View view)
		{
			foreach(TabPage page in this.tabControl.Controls)
			{
				foreach(Control ctr in page.Controls)
				{
					if(ctr is ListView)
					{
						((ListView)ctr).View = view;
						break;
					}
				}
			}
			ViewLargeIcons.Checked = (view == System.Windows.Forms.View.LargeIcon);
			ViewSmallIcons.Checked = (view == System.Windows.Forms.View.SmallIcon);
			ViewList.Checked = (view == System.Windows.Forms.View.List);
			ViewDetails.Checked = (view == System.Windows.Forms.View.Details);
		}

		/// <summary>
		/// Delegate for expansion of a Node in a TreeView. Should populate all child nodes before expansion.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			TreeView tv = (TreeView)sender;
			ListView lv = (ListView)tv.Tag;
			if(e.Node.Tag as string == "MyComputer")
			{
				this.Cursor = Cursors.Default;
				return;
			}
			
			RegistryKey key = (RegistryKey)e.Node.Tag;
			for(int i = 0; i < key.Subkeys.Length; i++)
			{
				if(!key.Subkeys[i].HasBeenReadFromRegistry)
				{
					BuildTree(key.Subkeys[i], e.Node.Nodes[i], 0);
				}
			}
			this.Cursor = Cursors.Default;
		}

		/// <summary>
		/// Delegate for selection of a Node in a TreeView. Updates the corresponding ListView with data from the SelectedNode's corresponding RegistryKey.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			TreeView tv = (TreeView)sender;
			ListView lv = (ListView)tv.Tag;
			lv.Items.Clear();
			if(e.Node.Tag as string == "MyComputer")
			{
				this.Cursor = Cursors.Default;
				this.statusBar.Text = "My Computer";
				return;
			}
			RegistryKey key = (RegistryKey)e.Node.Tag;
			this.statusBar.Text = key.FullPath;
			foreach(RegistryValue val in key.Values)
			{
				if(val == null)
				{
					continue;
				}
				string[] properties = new string[3];
				int icon = 0;	// assume binary value icon
				switch(val.Type)
				{
					case RegistryValueType.None:
						properties[1] = "REG_NONE";
						break;
					case RegistryValueType.String:
						properties[1] = "REG_SZ";
						icon = 1;
						break;
					case RegistryValueType.ExpandableString:
						properties[1] = "REG_EXPAND_SZ";
						icon = 1;
						break;
					case RegistryValueType.Binary:
						properties[1] = "REG_BINARY";
						break;
					case RegistryValueType.Dword:
						properties[1] = "REG_DWORD";
						break;
					case RegistryValueType.Link:
						properties[1] = "REG_LINK";
						break;
					case RegistryValueType.MultiString:
						properties[1] = "REG_MULTI_SZ";
						icon = 1;
						break;
					case RegistryValueType.ResourceList:
						properties[1] = "REG_RESOURCE_LIST";
						break;
					default:
						properties[1] = val.Type.ToString();
						break;
				}
				if(val.Name == "")
				{
					properties[0] = "(Default)";
					if(((string)val.Value).Length == 0)
					{
						properties[2] = "(value not set)";
					}
					else
					{
						properties[2] = val.ValueString;
					}
				}
				else
				{
					properties[0] = val.Name;
				}
				ListViewItem item = new ListViewItem(properties, icon);
				lv.Items.Add(item);
			}
			this.Cursor = Cursors.Default;
		}

		private void FileOpen_Click(object sender, System.EventArgs e)
		{
			openFileDialog.ShowDialog();
			CreateNewTab(openFileDialog.FileName);
		}

		private void MainForm_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			Array a = (Array)e.Data.GetData(DataFormats.FileDrop);
			foreach(string file in a)
			{
				CreateNewTab(file);
			}
		}

		private void MainForm_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void menuItem1_Click(object sender, System.EventArgs e)
		{
			TreeView tv = GetCurrentTreeView();
			tv.SelectedNode.Toggle();
		}

		private TreeView GetCurrentTreeView()
		{
			System.Windows.Forms.Control.ControlCollection col = this.tabControl.SelectedTab.Controls;
			foreach(Control c in col)
			{
				if(c.Name == "tv")
				{
					return (TreeView)c;
				}
			}
			throw new ApplicationException("Control not found.");
		}

		private void treeView_MouseUp(object sender, MouseEventArgs e)
		{
			if(e.Button == MouseButtons.Right)
			{
				TreeView tv = (TreeView)sender;
				Point clickPoint = new Point(e.X, e.Y);
				TreeNode clickedNode = tv.GetNodeAt(clickPoint);
				if(clickedNode != null)
				{
					tv.SelectedNode = clickedNode;
				}
			}
		}

		private void cmKey_Popup(object sender, System.EventArgs e)
		{
			TreeView tv = (TreeView)((ContextMenu)sender).SourceControl;
			if(tv.SelectedNode.IsExpanded)
			{
				cmKey.MenuItems[0].Text = "Collapse";
			}
			else
			{
				cmKey.MenuItems[0].Text = "Expand";
			}
			Point showPoint = new Point(tv.SelectedNode.Bounds.Right, tv.SelectedNode.Bounds.Top);
			// Show context menu	
		}

		private void treeView_KeyUp(object sender, KeyEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine(e.KeyCode);
		}
	}
}