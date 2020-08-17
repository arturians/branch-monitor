using System;
using System.IO;
using Newtonsoft.Json;

namespace BranchMonitor
{
	public static class SettingsLoader
	{
		public static Settings Load(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
				throw new InvalidOperationException("Not initialized Path to settings");
			return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));
		}
	}
}