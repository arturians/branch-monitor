using System;
using System.Linq;

namespace BranchMonitor.GitCommands
{
	public static class ParserForCommandToGetAllRemoteBranches
	{
		public static string[] GetBranches(string output)
		{
			return output
				.Split(new[]{'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
				.Select(r => r.Trim())
				.Select(r => r.Substring(0, r.IndexOf(' ')))
				.Where(b => b != "origin/master" && b != "origin/HEAD")
				.ToArray();
		}
	}
}