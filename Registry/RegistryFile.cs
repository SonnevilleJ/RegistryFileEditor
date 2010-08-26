using System;
using System.IO;

namespace Sonneville.Registry
{
	/// <summary>
	/// A .reg text file containing stored RegistryKeys and their RegistryValues.
	/// </summary>
	public class RegistryFile
	{
		/// <summary>
		/// An array of legal string Headers for RegistryFiles.
		/// </summary>
		public static string[] Headers
		{
			get
			{
				return RegistryFile.headers;
			}
		}

		/// <summary>
		/// The regular expression used to define a StringValue in a .reg file.
		/// </summary>
		public static string StringValue
		{
			get
			{
				return RegistryFile.stringValue;
			}
		}

		#region Internal Members

		/// <summary>
		/// String Array of valid headers for Windows Registry Files (*.reg)
		/// </summary>
		private static readonly string[] headers = {"Windows Registry Editor Version 5.00", "REGEDIT4"};

		/// <summary>
		/// Regex string to identify a StringValue within a Windows Registry File
		/// </summary>
		private static readonly string stringValue = @"\A\s*""[\w?\s?]*""=""[\w?\s?]*""\s*(;[\s\S]*)?\Z";

		/// <summary>
		/// Collection of RegistryKeys stored in the RegistryFile
		/// </summary>
		private RegistryKeyCollection _keys = new RegistryKeyCollection(this);

		/// <summary>
		/// Holds the full path filename of the Windows Registry File
		/// </summary>
		private string _filename;

		#endregion

		#region Constructors

		/// <summary>
		/// Opens or creates a new RegistryFile.
		/// </summary>
		public RegistryFile()
		{
			_filename = "";
		}

		/// <summary>
		/// Opens or creates a new RegistryFile.
		/// </summary>
		/// <param name="fileName">The full path and fileName of the .reg file to open or create.</param>
		public RegistryFile(string fileName)
		{
			_filename = fileName;
			ReadFromFile();
		}

		#endregion

		#region Read Methods

		/// <summary>
		/// Reads a RegistryFile from disk.
		/// </summary>
		public void ReadFromFile()
		{
			StreamReader reader = new StreamReader(_filename);
			string wholeFile = reader.ReadToEnd();
			reader.Close();
			wholeFile = wholeFile.Replace("\\\r\n", "");
			RegistryKey current = null;
			RegistryValue val = null;
			bool headerFound = false;
			string[] lines = wholeFile.Split('\r', '\n', ';');
			System.Collections.ArrayList heads = new System.Collections.ArrayList(RegistryFile.Headers);
			int i = 0;
			for(; i < lines.Length; i++)
			{
				lines[i] = lines[i].Trim();
				if(lines[i].StartsWith(";"))
				{
					// comment found
					continue;
				}
				if(heads.Contains(lines[i]))
				{
					headerFound = true;
					break;
				}
				if(lines[i] != "" || lines[i].StartsWith(";"))
				{
					throw new InvalidRegistryFileException("Value found before header.");
				}
			}
			if(!headerFound)
			{
				throw new InvalidRegistryFileException("A valid header was not found in the file.");
			}
			for(; i < lines.Length; i++)
			{
				lines[i] = lines[i].Trim();
				if(lines[i].StartsWith("[") && lines[i].EndsWith("]"))
				{
					if(current != null)
					{
						this.AddKey(current, false);
					}
					current = new RegistryKey(lines[i].Substring(1, lines[i].Length - 2));
				}
				else if(lines[i].StartsWith(";"))
				{
					continue;
				}
				else if(lines[i].StartsWith("@"))
				{
				}
				else if(lines[i].StartsWith("\""))
				{
					lines[i].Remove(0, 1);	// remove opening "
					string name = lines[i].Substring(0, lines[i].IndexOf("\""));
					lines[i].Remove(0, 2);
					if(lines[i].Substring(0, 1) == "\"")
					{
						string data = lines[i].Substring(1, lines[i].IndexOf("\"", 1));
						// string value found
					}
					else if(lines[i].Substring(0, 3).ToLower() == "hex")
					{
						// hex value found
					}
					else if(lines[i].Substring(0, 5).ToLower() == "dword")
					{
						// dword found
					}
					else
					{
						switch (lines[i].Substring(0, 6).ToLower())
						{
							case "hex(2)":
							{
								// expandable string
								break;
							}
							case "hex(7)":
							{
								// multi string
								break;
							}
							default:
							{
								// unsupported value
								break;
							}
						}
					}
				}
			}
		}

		#endregion

		#region Old Read Methods
//
//		/// <summary>
//		/// Reads a RegistryFile into memory from disk.
//		/// </summary>
//		/// <exception cref="InvalidRegistryFileException">The file is not a legal RegistryFile.</exception>
//		public void OldRead(string pointless)
//		{
//			StreamReader reader = new StreamReader(_filename);
//			{
//				System.Collections.ArrayList headersList = new System.Collections.ArrayList(RegistryFile.Headers);
//				bool headerFound = false;
//				string first;
//				while(!headerFound && (first = reader.ReadLine()) != null)
//				{
//					// Loops through the file one line at a time to find a header
//					// Header should be on first line, but not necessary
//					headerFound = headersList.Contains(first);
//				}
//				if(!headerFound)
//				{
//					throw new InvalidRegistryFileException();
//				}
//				// It is now safe to assume the header has been found
//				// and this is a legal registry file
//			}
//			// Now loop through the rest of the file.
//			// When a new key is discovered, add it to any parent key
//			// and call ReadValues() which adds values until a new key is discovered
//
//			string line;
//			while((line = reader.ReadLine()) != null)
//			{
//				if(line.StartsWith("[") && line.EndsWith("]"))
//				{
//					RegistryKey newKey = new RegistryKey(line.Substring(1, line.Length - 2));
//					OldReadValues(newKey, reader);
//					_keys.Add(newKey);
//				}
//			}
//		}
//
//		private void OldReadValues(RegistryKey parent, StreamReader reader)
//		{
//			string line;
//			line = reader.ReadLine();
//			int index;
//			if((index = line.LastIndexOf("dword:")) != -1)
//			{
//				// new dword found
//			}
//			else if((index = line.LastIndexOf("=hex(")) != -1)
//			{
//				string type = line.Substring(index, line.IndexOf(")", index));
//				switch (type)
//				{
//					case "1":
//						// new string found
//						// program should never encounter this,
//						// but it is valid syntax for the .reg file
//						break;
//					case "2":
//						// new expand found
//						break;
//					case "3":
//						// new binary found
//						string name = line.Substring(1, line.IndexOf("\"=hex:") - 1);
//						string data = line.Substring(name.Length + 6);
//						break;
//					case "4":
//						// new dword found
//						// again, program should never encouter this,
//						// but is still valid syntax
//						break;
//					case "5":
//						// new dword (big endian) found
//						break;
//					case "6":
//						// new link found
//						// currently, I have no idea what this is
//						// and I'd rather not mess with it ;)
//						break;
//					case "7":
//						// new multistring found
//						break;
//					case "8":
//						// new resource list found
//						// I don't know what this is either
//						break;
//					case "9":
//						// new full resource descriptor found
//						// I don't know what this is either
//						break;
//					default:
//						throw new NotSupportedException("Value type not supported: hex(" + type + ")");
//				}
//			}
//			else if(line.LastIndexOf("=hex:") != -1)
//			{
//				// new binary found
//			}
//			else
//			{
//				// new string found
//			}
//		}
//
		#endregion

		#region Write Methods

		private void WriteKey(RegistryKey key, StreamWriter writer)
		{
			writer.WriteLine("[" + key.FullPath + "]");
			foreach(RegistryValue value in key.Values)
			{
				switch(value.Type)
				{
					case RegistryValueType.Binary:
						writer.Write(BuildValueString((BinaryValue)value));
						break;
					case RegistryValueType.Dword:
						writer.Write(BuildValueString((DwordValue)value));
						break;
					case RegistryValueType.ExpandableString:
						writer.Write(BuildValueString((StringValue)value));
						break;
					case RegistryValueType.MultiString:
						writer.Write(BuildValueString((MultiStringValue)value));
						break;
					case RegistryValueType.String:
						writer.Write(BuildValueString((StringValue)value));
						break;
				}
			}
		}

		private string BuildValueString(BinaryValue value)
		{
			string name = "";
			if(value.Name != "")
			{
				name = "\"" + value.Name + "\"=hex:";
			}
			else
			{
				name = "@=hex:";
			}
			string data = ConvertBytesToAscii((byte[])value.Value, name.Length);
			return name + data + "\r\n";
		}

		private string BuildValueString(DwordValue value)
		{
			string name = "\"" + value.Name + "\"=";
			string data = "";
			uint temp = value.UnsignedValue;
			for(int i = 7; i >= 0; i--)
			{
				switch (temp / (uint)Math.Pow(16, i))
				{
					case 0:
						data += "0";
						break;
					case 1:
						data += "1";
						temp -= (uint)Math.Pow(16, i);
						break;
					case 2:
						data += "2";
						temp -= 2 * (uint)Math.Pow(16, i);
						break;
					case 3:
						data += "3";
						temp -= 3 * (uint)Math.Pow(16, i);
						break;
					case 4:
						data += "4";
						temp -= 4 * (uint)Math.Pow(16, i);
						break;
					case 5:
						data += "5";
						temp -= 5 * (uint)Math.Pow(16, i);
						break;
					case 6:
						data += "6";
						temp -= 6 * (uint)Math.Pow(16, i);
						break;
					case 7:
						data += "7";
						temp -= 7 * (uint)Math.Pow(16, i);
						break;
					case 8:
						data += "8";
						temp -= 8 * (uint)Math.Pow(16, i);
						break;
					case 9:
						data += "9";
						temp -= 9 * (uint)Math.Pow(16, i);
						break;
					case 10:
						data += "a";
						temp -= 10 * (uint)Math.Pow(16, i);
						break;
					case 11:
						data += "b";
						temp -= 11 * (uint)Math.Pow(16, i);
						break;
					case 12:
						data += "c";
						temp -= 12 * (uint)Math.Pow(16, i);
						break;
					case 13:
						data += "d";
						temp -= 13 * (uint)Math.Pow(16, i);
						break;
					case 14:
						data += "e";
						temp -= 14 * (uint)Math.Pow(16, i);
						break;
					case 15:
						data += "f";
						temp -= 15 * (uint)Math.Pow(16, i);
						break;
				}
			}			
			return name + "dword:" + data + "\r\n";
		}

//		private string BuildValueString(ExpandableStringValue value)
//		{
//			string name = "";
//			if(value.Name != "")
//			{
//				name = "\"" + value.Name + "\"=hex(2):";
//			}
//			else
//			{
//				name = "@=hex(2):";
//			}
//			string data = ConvertBytesToAscii(ConvertStringToBytes(value.Value), name.Length);
//			return name + data + "\r\n";
//		}
//
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private string BuildValueString(MultiStringValue value)
		{
			string[] Value = (string[])value.Value;
			string name = "";
			if(value.Name != "")
			{
				name = "\"" + value.Name + "\"=hex(7):";
			}
			else
			{
				name = "@=hex(7):";
			}
			int totalLength = 0;
			foreach(string str in (Value))
			{
				totalLength += (str.Length * 2) + 2;
			}
			totalLength += 2;
			byte[] bytes = new byte[totalLength];
			int length = 0;
			for(int i = 0; i < Value.Length; i++)
			{
				ConvertStringToBytes(Value[i]).CopyTo(bytes, length);
				length += Value[i].Length * 2;
				bytes[length] = (byte)0;
				bytes[length + 1] = (byte)0;
				length += 2;
			}
			string data = ConvertBytesToAscii(bytes, name.Length);
			return name + data + "\r\n";
		}

		private string BuildValueString(StringValue value)
		{
			string name = "";
			if(value.Name != "")
			{
				name = "\"" + value.Name + "\"=";
			}
			else
			{
				name = "@=";
			}
			return name + "\"" + value.Value + "\"" + "\r\n";
		}

		private string ConvertBytesToAscii(byte[] data, int length)
		{
			string val = "";
			for(int i = 0; i < data.Length; i++)
			{
				string temp = ((int)(data[i])).ToString("x");
				val += ((temp.Length == 1) ? "0" + temp : temp) + ",";
				if(val.Length - val.LastIndexOf("\r\n") > 77 - length)
				{
					val += "\\\r\n  ";
					length = 0;	// loop catches first line and stops correcting for name
				}
			}
			return val.Substring(0, val.Length - 1);
		}

		private byte[] ConvertStringToBytes(string data)
		{
			data += "\r\n";
			byte[] val = new byte[data.Length * 2];
			for(int i = 0; i < val.Length; i += 2)
			{
				switch(data[i / 2])
				{
					case '\r':
					case '\n':
						val[i] = (byte)0;
						break;
					default:
						val[i] = (byte)(int)data[i / 2];
						break;
				}
				val[i + 1] = 0;
			}
			return val;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Gets the fileName of this RegistryFile.
		/// </summary>
		public string FileName
		{
			get
			{
				return _filename;
			}
		}

		/// <summary>
		/// Writes the RegistryFile to disk.
		/// </summary>
		/// <exception cref="InvalidOperationException">RegistryFile has no fileName set.</exception>
		public void Save()
		{
			if(_filename != "" && _filename != null)
			{
				Save(_filename);
			}
			else
			{
				throw new InvalidOperationException("Cannot save a RegistryFile with no fileName.");
			}
		}

		/// <summary>
		/// Writes the RegistryFile to disk with the specified fileName.
		/// </summary>
		/// <param name="fileName">The path and fileName of the file to save to.</param>
		public void Save(string fileName)
		{
			_filename = fileName;
			StreamWriter writer = new StreamWriter(fileName);
			writer.WriteLine(RegistryFile.Headers[0] + "\r\n");
			foreach(RegistryKey key in _keys)
			{
				WriteKey(key, writer);
				writer.WriteLine();
			}
			writer.Flush();
			writer.Close();
		}

		/// <summary>
		/// Gets an array of RegistryKeys stored in this RegistryFile.
		/// </summary>
		public RegistryKey[] Keys
		{
			get
			{
				return _keys.ToArray();
			}
		}

		/// <summary>
		/// Adds a RegistryKey into this RegistryFile.
		/// </summary>
		/// <param name="key">The name of the RegistryKey to add to this RegistryFile.</param>
		/// <param name="addSubKeys">True if the entire chain of subkeys is to be added.</param>
		public void AddKey(RegistryKey key, bool addSubKeys)
		{
			_keys.Add(key);
			if(addSubKeys)
			{
				foreach(RegistryKey subkey in key.Subkeys)
				{
					this.AddKey(subkey, true);
				}
			}
		}

		/// <summary>
		/// Removes a RegistryKey from this RegistryFile.
		/// </summary>
		/// <param name="key">The RegistryKey to remove from this RegistryFile.</param>
		/// <param name="removeSubKeys">True if the entire chain of subkeys is to be removed.</param>
		public void RemoveKey(RegistryKey key, bool removeSubKeys)
		{
			_keys.Remove(key);
			if(removeSubKeys)
			{
				foreach(RegistryKey subkey in key.Subkeys)
				{
					this.RemoveKey(subkey, true);
				}
			}
		}

		#endregion
	}
}
