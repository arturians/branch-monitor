using System;
using System.Linq;

namespace BranchMonitor.GitCommands
{
	public static class ParserForCommandToGetMergedBranches
	{
		public static string[] GetBranches(string commandOutput)
		{
			return commandOutput
				.Split(new [] {'\n'}, StringSplitOptions.RemoveEmptyEntries)
				.Select(b => b.Trim())
				.Where(b => b != "origin/master" && b != "origin/HEAD -> origin/master")
				.Select(b => b.Replace("origin/", string.Empty))
				.ToArray();
		}
	}
}