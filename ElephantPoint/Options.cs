using System;
using System.Collections.Generic;
using System.Reflection;

namespace ElephantPoint
{
	internal class Options
	{
		internal bool Help = false;
		internal bool b64 = false;
		internal int max_row = 50;
		internal string query_file = null;
		internal string token = null;
		internal string save_file = null;
		internal string file_url = null;
		internal string SPO_url = null;
		internal bool fql = false;
		internal string ref_filter = null;

		internal bool ParseArguments(string[] args)
		{
			FieldInfo[] fields = typeof(Options).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
			var parsedOptions = new HashSet<string>();

			foreach (string arg in args)
			{
				if (arg.StartsWith("/"))
				{
					var unknown = true;
					string[] argParts = arg.Split(new[] { ':' }, 2);
					string optionName = argParts[0].TrimStart('/').ToLower();

					foreach (FieldInfo field in fields)
					{
						if (optionName == field.Name.ToLower())
						{
							if (parsedOptions.Add(optionName))
							{
								if (argParts.Length == 1)
								{
									try
									{
										field.SetValue(this, true);
									}
									catch (ArgumentException)
									{
										Console.WriteLine($"[-] No value specified for '{optionName}'.");
										return false;
									}
								}
								else
								{
									string optionValue = argParts[1];

									try
									{
										field.SetValue(this, optionValue);
									}
									catch (ArgumentException)
									{
										try
										{
											field.SetValue(this, int.Parse(optionValue));
										}
										catch (FormatException)
										{
											Console.WriteLine($"[-] Invalid value specified for '{optionName}'.");
											return false;
										}
									}
								}

								unknown = false;
							}
							else
							{
								Console.WriteLine($"[-] '{optionName}' argument can only be specified once.");
								return false;
							}
						}
					}

					if (unknown)
					{
						Console.WriteLine($"[-] Unknown argument '{optionName}'.");
						return false;
					}
				}
			}

			return true;
		}
	}
}
